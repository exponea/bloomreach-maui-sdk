//
//  BloomreachInvokable+AppInbox.swift
//  BloomreachSDKMauiIOS
//
//  Created by Adam Mihalik on 29/09/2023.
//

import Foundation
import ExponeaSDK

// MARK: - AppInbox

extension MessageItem {
    var json: [String: Any] {
        let mauiTypeValue: String
        switch self.type {
        case "push": mauiTypeValue = "Push"
        case "html": mauiTypeValue = "Html"
        default: mauiTypeValue = "Unknown"
        }
        let rawContent = self.rawContent?.mapValues { $0.rawValue } ?? [:]
        return [
            "id": self.id,
            "type": mauiTypeValue,
            "isRead": self.read,
            "receivedTime": Double(self.receivedTime.timeIntervalSince1970),
            "content": rawContent
        ]
    }
}

public extension BloomreachInvokable {
    func fetchAppInbox(completion: TypeBlock<MethodResult>?) {
        exponeaSDK.fetchAppInbox { appInboxResult in
            guard let appInbox = appInboxResult.value else {
                completion?(.failure(
                    appInboxResult.error?.localizedDescription ?? "AppInbox messages load failed, see logs"
                ))
                return
            }
            let mauiResult = appInbox.map { message in message.json }
            completion?(.success(mauiResult.json))
        }
    }

    func fetchAppInboxItem(messageId: String?, completion: TypeBlock<MethodResult>?) {
        guard let messageId = messageId else {
            completion?(.failure("Property 'messageId' is required."))
            return
        }
        exponeaSDK.fetchAppInboxItem(messageId) { messageResult in
            guard let message = messageResult.value else {
                completion?(.failure(
                    messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs"
                ))
                return
            }
            let mauiResult = message.json
            completion?(.success(mauiResult.jsonData))
        }
    }

    func markAppInboxAsRead(messageId: String?, completion: TypeBlock<MethodResult>?) {
        guard let messageId = messageId else {
            completion?(.failure("Property 'messageId' is required."))
            return
        }
        // we need load message to ensure fresh assignments
        exponeaSDK.fetchAppInboxItem(messageId) { messageResult in
            guard let message = messageResult.value else {
                completion?(.failure(
                    messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs"
                ))
                return
            }
            exponeaSDK.markAppInboxAsRead(message) { marked in
                completion?(marked ? .success("true") : .failure("AppInbox message was not marked as read"))
            }
        }
    }

    func setAppInboxProvider(data: [String: Any]) -> MethodResult {
        let style: AppInboxStyle
        do {
            style = try AppInboxStyleParser(data as NSDictionary).parse()
        } catch let error {
            return .failure(error.localizedDescription)
        }
        exponeaSDK.appInboxProvider = StyledAppInboxProvider(style)
        return .success(nil)
    }

    func trackAppInboxClick(data: [String: Any]) -> MethodResult {
        let dataParsing = parseAppInboxActionAndMessage(data)
        guard let action = dataParsing.action,
              let message = dataParsing.message else {
            return dataParsing.error ?? .failure("Tracking of AppInbox click got invalid data, see logs")
        }
        // we need load message to ensure fresh assignments
        exponeaSDK.fetchAppInboxItem(message.id) { messageResult in
            guard let message = messageResult.value else {
                Exponea.logger.log(.error, message: messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs")
                return
            }
            exponeaSDK.trackAppInboxClick(action: action, message: message)
        }
        return .success(nil)
    }

    func trackAppInboxClickWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        let dataParsing = parseAppInboxActionAndMessage(data)
        guard let action = dataParsing.action,
              let message = dataParsing.message else {
            return dataParsing.error ?? .failure("Tracking of AppInbox click got invalid data, see logs")
        }
        // we need load message to ensure fresh assignments
        exponeaSDK.fetchAppInboxItem(message.id) { messageResult in
            guard let message = messageResult.value else {
                Exponea.logger.log(.error, message: messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs")
                return
            }
            exponeaSDK.trackAppInboxClickWithoutTrackingConsent(action: action, message: message)
        }
        return .success(nil)
    }

    func trackAppInboxOpened(data: [String: Any]) -> MethodResult {
        let dataParsing = parseAppInboxMessage(data)
        guard let message = dataParsing.message else {
            return dataParsing.error ?? .failure("Tracking of AppInbox open got invalid data, see logs")
        }
        // we need load message to ensure fresh assignments
        exponeaSDK.fetchAppInboxItem(message.id) { messageResult in
            guard let message = messageResult.value else {
                Exponea.logger.log(.error, message: messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs")
                return
            }
            exponeaSDK.trackAppInboxOpened(message: message)
        }
        return .success(nil)
    }

    func trackAppInboxOpenedWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        let dataParsing = parseAppInboxMessage(data)
        guard let message = dataParsing.message else {
            return dataParsing.error ?? .failure("Tracking of AppInbox open got invalid data, see logs")
        }
        // we need load message to ensure fresh assignments
        exponeaSDK.fetchAppInboxItem(message.id) { messageResult in
            guard let message = messageResult.value else {
                Exponea.logger.log(.error, message: messageResult.error?.localizedDescription ?? "AppInbox message not found, see logs")
                return
            }
            exponeaSDK.trackAppInboxOpenedWithoutTrackingConsent(message: message)
        }
        return .success(nil)
    }

    // swiftlint:disable large_tuple
    private func parseAppInboxActionAndMessage(_ source: [String: Any]) -> (action: MessageItemAction?, message: MessageItem?, error: MethodResult?) {
        guard let actionData = source["action"] as? [String: Any] else {
            return (nil, nil, .failure("Property 'action' is required."))
        }
        guard let actionTypeMaui = actionData["type"] as? String,
              let actionType = parseAppInboxActionType(actionTypeMaui) else {
            return (nil, nil, .failure("Invalid value '\(String(describing: actionData["type"]))' for 'action.type'."))
        }
        let action = MessageItemAction(
            action: actionType.rawValue,
            title: actionData["title"] as? String,
            url: actionData["url"] as? String
        )
        guard let messageData = source["message"] as? [String: Any] else {
            return (nil, nil, .failure("Property 'message' is required."))
        }
        let messageParsing = parseAppInboxMessage(messageData)
        return (action, messageParsing.message, messageParsing.error)
    }

    private func parseAppInboxMessage(_ source: [String: Any]) -> (message: MessageItem?, error: MethodResult?) {
        guard let messageId = source["id"] as? String else {
            return (nil, .failure("Invalid value '\(String(describing: source["id"]))' for 'message.id'."))
        }
        guard let messageTypeMaui = source["type"] as? String else {
            return (nil, .failure("Invalid value '\(String(describing: source["type"]))' for 'message.type'."))
        }
        let message = MessageItem(
            id: messageId,
            type: parseAppInboxMessageType(messageTypeMaui),
            read: source["isRead"] as? Bool ?? false,
            rawReceivedTime: source["receivedTime"] as? Double,
            rawContent: source["content"] as? [String: JSONValue] ?? [:]
        )
        return (message, nil)
    }

    private func parseAppInboxMessageType(_ source: String) -> String {
        switch source {
        case "Push": return "push"
        case "Html": return "html"
        default: return "unknown"
        }
    }

    private func parseAppInboxActionType(_ source: String) -> MessageItemActionType? {
        switch source {
        case "App": return MessageItemActionType.app
        case "Browser": return MessageItemActionType.browser
        case "Deeplink": return MessageItemActionType.deeplink
        case "NoAction": return MessageItemActionType.noAction
        default: return nil
        }
    }
}
