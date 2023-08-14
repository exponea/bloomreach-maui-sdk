//
//  StaticInAppContentBlockView.swift
//  ExponeaSDK
//
//  Created by Ankmara on 25.06.2023.
//  Copyright © 2023 Exponea. All rights reserved.
//

import UIKit
import WebKit

public final class StaticInAppContentBlockView: UIView, WKNavigationDelegate {

    // MARK: - Properties
    public var refresh: EmptyBlock?
    private lazy var webview: WKWebView = {
        let userScript: WKUserScript = .init(source: inAppContentBlocksManager.disableZoomSource, injectionTime: .atDocumentEnd, forMainFrameOnly: true)
        let newWebview = WKWebView(frame: .init(x: 0, y: 0, width: UIScreen.main.bounds.size.width, height: 0))
        newWebview.scrollView.showsVerticalScrollIndicator = false
        newWebview.scrollView.bounces = false
        newWebview.backgroundColor = .clear
        newWebview.isOpaque = false
        newWebview.translatesAutoresizingMaskIntoConstraints = false
        let configuration = newWebview.configuration
        configuration.userContentController.addUserScript(userScript)
        if let contentRuleList = inAppContentBlocksManager.contentRuleList {
            configuration.userContentController.add(contentRuleList)
        }
        return newWebview
    }()

    private let placeholder: String
    private lazy var inAppContentBlocksManager = InAppContentBlocksManager.manager
    private lazy var calculator: WKWebViewHeightCalculator = .init()
    private var html: String = ""
    private var height: NSLayoutConstraint?

    public init(placeholder: String) {
        self.placeholder = placeholder
        super.init(frame: .zero)

        webview.navigationDelegate = self
        calculator.heightUpdate = { [weak self] height in
            guard let self = self, height.height > 0 else { return }
            self.replacePlaceholder(inputView: self, loadedInAppContentBlocksView: self.webview, height: height.height - 15)
        }
        getContent()
    }

    public func reload() {
        getContent()
    }

    required init?(coder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }

    private func getContent() {
        guard !placeholder.isEmpty else {
            replacePlaceholder(inputView: self, loadedInAppContentBlocksView: .init(frame: .zero), height: 0)
            return
        }
        let data = inAppContentBlocksManager.prepareInAppContentBlocksStaticView(placeholderId: placeholder)
        webview.tag = data.tag
        if data.html.isEmpty {
            inAppContentBlocksManager.refreshStaticViewContent(staticQueueData: .init(tag: data.tag, placeholderId: placeholder) {
                self.webview.tag = $0.tag
                self.loadContent(html: $0.html)
            })
        } else {
            loadContent(html: data.html)
        }
    }

    private func loadContent(html: String) {
        guard !html.isEmpty else {
            replacePlaceholder(inputView: self, loadedInAppContentBlocksView: .init(frame: .zero), height: 0)
            return
        }
        self.html = html
        calculator.loadHtml(placedholderId: placeholder, html: html)
    }

    private func replacePlaceholder(inputView: UIView, loadedInAppContentBlocksView: UIView, height: CGFloat) {
        onMain {
            let duration: TimeInterval = 0.3
            loadedInAppContentBlocksView.alpha = 0
            UIView.animate(withDuration: duration) {
                loadedInAppContentBlocksView.alpha = 0
            } completion: { [weak self] isDone in
                guard let self else { return }
                if isDone {
                    loadedInAppContentBlocksView.constraints.forEach { cons in
                        self.removeConstraint(cons)
                    }
                    loadedInAppContentBlocksView.removeFromSuperview()
                    inputView.addSubview(loadedInAppContentBlocksView)
                    loadedInAppContentBlocksView.topAnchor.constraint(equalTo: inputView.topAnchor, constant: 5).isActive = true
                    loadedInAppContentBlocksView.leadingAnchor.constraint(equalTo: inputView.leadingAnchor, constant: 5).isActive = true
                    loadedInAppContentBlocksView.trailingAnchor.constraint(equalTo: inputView.trailingAnchor, constant: -5).isActive = true
                    if self.height != nil {
                        self.height?.constant = height
                    } else {
                        self.height = loadedInAppContentBlocksView.heightAnchor.constraint(equalToConstant: height)
                        self.height?.isActive = true
                    }
                    loadedInAppContentBlocksView.bottomAnchor.constraint(equalTo: inputView.bottomAnchor, constant: -5).isActive = true
                    loadedInAppContentBlocksView.sizeToFit()
                    loadedInAppContentBlocksView.layoutIfNeeded()
                    UIView.animate(withDuration: duration) {
                        loadedInAppContentBlocksView.alpha = 1
                    }
                    self.webview.loadHTMLString(self.html, baseURL: nil)
                }
            }
        }
    }

    public func webView(
        _ webView: WKWebView,
        decidePolicyFor navigationAction: WKNavigationAction,
        decisionHandler: @escaping (WKNavigationActionPolicy) -> Void
    ) {
        let result = inAppContentBlocksManager.inAppContentBlocksPlaceholders.first(where: { $0.tags?.contains(webView.tag) == true })
        let webAction: WebActionManager = .init {
            let indexOfPlaceholder: Int = self.inAppContentBlocksManager.inAppContentBlocksPlaceholders.firstIndex(where: { $0.id == result?.id ?? "" }) ?? 0
            let currentDisplay = self.inAppContentBlocksManager.inAppContentBlocksPlaceholders[indexOfPlaceholder].displayState
            self.inAppContentBlocksManager.inAppContentBlocksPlaceholders[indexOfPlaceholder].displayState = .init(displayed: currentDisplay?.displayed, interacted: Date())
            if let message = result {
                Exponea.shared.trackInAppContentBlocksClose(message: message, isUserInteraction: true)
            }
            self.reload()
        } onActionCallback: { action in
            let indexOfPlaceholder: Int = self.inAppContentBlocksManager.inAppContentBlocksPlaceholders.firstIndex(where: { $0.id == result?.id ?? "" }) ?? 0
            let currentDisplay = self.inAppContentBlocksManager.inAppContentBlocksPlaceholders[indexOfPlaceholder].displayState
            self.inAppContentBlocksManager.inAppContentBlocksPlaceholders[indexOfPlaceholder].displayState = .init(displayed: currentDisplay?.displayed, interacted: Date())
            if let message = result {
                Exponea.shared.trackInAppContentBlocksClick(message: message, buttonText: action.buttonText, buttonLink: action.actionUrl)
            }
            self.inAppContentBlocksManager.urlOpener.openBrowserLink(action.actionUrl)
            self.reload()
        } onErrorCallback: { error in
            Exponea.logger.log(.error, message: "WebActionManager error \(error.localizedDescription)")
        }
        webAction.htmlPayload = result?.personalizedMessage?.htmlPayload
        let handled = webAction.handleActionClick(navigationAction.request.url)
        if handled {
            Exponea.logger.log(.verbose, message: "[HTML] Action \(navigationAction.request.url?.absoluteString ?? "Invalid") has been handled")
            decisionHandler(.cancel)
        } else {
            Exponea.logger.log(.verbose, message: "[HTML] Action \(navigationAction.request.url?.absoluteString ?? "Invalid") has not been handled, continue")
            decisionHandler(.allow)
        }
    }
}
