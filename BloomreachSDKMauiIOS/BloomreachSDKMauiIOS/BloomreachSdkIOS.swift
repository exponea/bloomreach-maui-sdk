//
//  BloomreachSDK.swift
//  BloomreachSDKMauiIOS
//
//  Created by Adam Mihalik on 14/08/2023.
//

import ExponeaSDK
import ExponeaSDK_Notifications

// This protocol is used queried using reflection by native iOS SDK to see if it's run by Maui SDK
@objc(IsBloomreachMauiSDK)
protocol IsBloomreachMauiSDK {
}
@objc(BloomreachMauiVersion)
public class BloomreachMauiVersion: NSObject, ExponeaVersionProvider {
    required public override init() { }
    public func getVersion() -> String {
        "0.2.0"
    }
}

@objc(AuthorizationProviderType)
public protocol AuthorizationProviderType {
    init()
    func getAuthorizationToken() -> String?
}

@objc(MauiAuthorizationProvider)
public class MauiAuthorizationProvider: NSObject, AuthorizationProviderType {
    required public override init() { }
    public func getAuthorizationToken() -> String? {
        ""
    }
}

@objc(BloomreachSdkIOS)
public class BloomreachSdkIOS: NSObject, BloomreachInvokable {

    @objc
    public static var instance = BloomreachSdkIOS()

    var notificationService: ExponeaNotificationService?

    public var exponeaSDK: ExponeaType = Exponea.shared

    public func setupExponeaSDK(type: ExponeaType) {
        self.exponeaSDK = type
    }

    @objc
    public func invokeMethod(method: String?, params: String?) -> MethodResult {
        return parse(method: method, params: params)
    }

    @objc
    public func invokeMethodAsync(
        method: String?,
        params: String?,
        done: @escaping (MethodResult) -> Void
    ) {
        return parseAsync(method: method, params: params, done: done)
    }

    @objc
    public func invokeMethodForUI(method: String?, params: String?) -> MethodResultForUI {
        switch method {
        case "getAppInboxButton":
            return MethodResultForUI.success(Exponea.shared.getAppInboxButton())
        default:
            return MethodResultForUI.unsupportedMethod(method ?? "nil")
        }
    }

    @objc
    public func handleRemoteMessage(
        appGroup: String,
        notificationRequest: UNNotificationRequest,
        handler: @escaping (UNNotificationContent) -> Void
    ) -> MethodResult {
        if !ExponeaSDK.Exponea.isExponeaNotification(userInfo: notificationRequest.content.userInfo) {
            return .failure("Notification is non-Bloomreach, skipping")
        }
        notificationService?.serviceExtensionTimeWillExpire()   // end previous if exists
        notificationService = ExponeaNotificationService(appGroup: appGroup)
        notificationService?.process(request: notificationRequest, contentHandler: handler)
        return .success("true")
    }

    @objc
    public func handleRemoteMessageContent(
        notification: UNNotification,
        context: NSExtensionContext?,
        controller: UIViewController
    ) -> MethodResult {
        let notificationContentService = ExponeaNotificationContentService()
        notificationContentService.didReceive(notification, context: context, viewController: controller)
        return .success("true")
    }
}

class MauiInAppDelegate: InAppMessageActionDelegate {

    public let overrideDefaultBehavior: Bool
    public let trackActions: Bool
    private let handler: (InAppMessage, InAppMessageButton?, Bool) -> Void

    init(
        overrideDefaultBehavior: Bool,
        trackActions: Bool,
        handler: @escaping (InAppMessage, InAppMessageButton?, Bool) -> Void
    ) {
        self.overrideDefaultBehavior = overrideDefaultBehavior
        self.trackActions = trackActions
        self.handler = handler
    }

    public func inAppMessageAction(with message: InAppMessage, button: InAppMessageButton?, interaction: Bool) {
        handler(message, button, interaction)
    }
}
