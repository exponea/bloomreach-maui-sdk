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
        "0.0.1"
    }
}

@objc(AuthorizationProviderType)
public protocol AuthorizationProviderType {
    init()
    func getAuthorizationToken() -> String?
}

@objc(MauiAuthorizationProvider)
public class MauiAuthorizationProvider : NSObject, AuthorizationProviderType {
    required public override init() { }
    public func getAuthorizationToken() -> String? {
        ""
    }
}

@objc(BloomreachSdkIOS)
public class BloomreachSdkIOS: NSObject, BloomreachInvokable {
    
    @objc
    public static var instance = BloomreachSdkIOS()
    
    var notificationService: ExponeaNotificationService? = nil

    public var exponeaSDK: ExponeaType = Exponea.shared

    public func setupExponeaSDK(type: ExponeaType) {
        self.exponeaSDK = type
    }

    @objc
    public func invokeMethod(method: String?, params: String?) -> MethodResult {
        do {
            return parse(method: method, params: params)
        } catch let error {
            return MethodResult.failure("Method \(method ?? "nil") failed: \(error)")
        }
    }
    
    @objc
    public func invokeMethodAsync(
        method: String?,
        params: String?,
        done: @escaping (MethodResult)->()
    ) {
        do {
            return parseAsync(method: method, params: params, done: done)
        } catch let error {
            return done(MethodResult.unsupportedMethod(method ?? "nil"))
        }
    }
    
    @objc
    public func invokeMethodForUI(method: String?, params: String?) -> MethodResultForUI {
        do {
            switch method {
            case "getAppInboxButton":
                return MethodResultForUI.success(Exponea.shared.getAppInboxButton())
            default:
                return MethodResultForUI.unsupportedMethod(method ?? "nil")
            }
        } catch let error {
            return MethodResultForUI.failure("Method \(method ?? "nil") failed: \(error)")
        }
    }
    
    @objc
    public func handleRemoteMessage(
        appGroup: String,
        notificationRequest: UNNotificationRequest,
        handler: @escaping (UNNotificationContent) -> Void
    ) -> MethodResult {
        if (!ExponeaSDK.Exponea.isExponeaNotification(userInfo: notificationRequest.content.userInfo)) {
            Exponea.logger.log(ExponeaSDK.LogLevel.error, message: "APNS-BR Notification is non-Bloomreach, skipping")
            return .failure("Notification is non-Bloomreach, skipping")
        }
        notificationService?.serviceExtensionTimeWillExpire()   // end previous if exists
        notificationService = ExponeaNotificationService(appGroup: appGroup)
        Exponea.logger.log(ExponeaSDK.LogLevel.error, message: "APNS-BR Notification is processing")
        notificationService?.process(request: notificationRequest, contentHandler: handler)
        Exponea.logger.log(ExponeaSDK.LogLevel.error, message: "APNS-BR Notification was processed")
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
    private let handler: (InAppMessage, InAppMessageButton?, Bool) -> ()
    
    init(
        overrideDefaultBehavior: Bool,
        trackActions: Bool,
        handler: @escaping (InAppMessage, InAppMessageButton?, Bool) -> ()
    ) {
        self.overrideDefaultBehavior = overrideDefaultBehavior
        self.trackActions = trackActions
        self.handler = handler
    }
    
    public func inAppMessageAction(with message: InAppMessage, button: InAppMessageButton?, interaction: Bool) {
        handler(message, button, interaction)
    }
}
