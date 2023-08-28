//
//  ExponeaSDK.swift
//  ExponeaSDKMauiIOS
//
//  Created by Adam Mihalik on 14/08/2023.
//

import ExponeaSDK

// This protocol is used queried using reflection by native iOS SDK to see if it's run by Maui SDK
@objc(IsExponeaMauiSDK)
protocol IsExponeaMauiSDK {
}
@objc(ExponeaMauiVersion)
public class ExponeaMauiVersion: NSObject, ExponeaVersionProvider {
    required public override init() { }
    public func getVersion() -> String {
        "1.0.0"
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

@objc(ExponeaSDK)
public class ExponeaSDK: NSObject, ExponeaInvokable {
    
    @objc
    public static let instance = ExponeaSDK()

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
        switch method {
        case "fetchAppInbox":
            Exponea.shared.fetchAppInbox { messages in
                guard let messagesJson = try? JSONSerialization.data(withJSONObject: messages),
                      let messagesString = String(data: messagesJson, encoding: .utf8) else {
                    done(MethodResult.failure("Unable to serialize AppInbox messages"))
                    return
                }
                done(MethodResult.success(messagesString))
            }
        case "setInAppMessageDelegate":
            guard let params = params,
                  let paramsData = params.data(using: .utf8),
                  let delegateSetup = try? JSONSerialization.jsonObject(
                    with: paramsData,
                    options: []
                  ) as? [String: Bool?] else {
                done(MethodResult.failure("Unable to register InAppMessage delegate for given input"))
                return
            }
            let overrideDefaultBehavior = (delegateSetup["overrideDefaultBehavior"] ?? false) ?? false
            let trackActions = (delegateSetup["trackActions"] ?? true) ?? true
            Exponea.shared.inAppMessagesDelegate = MauiInAppDelegate(
                overrideDefaultBehavior: overrideDefaultBehavior,
                trackActions: trackActions,
                handler: { message, button, interaction in
                    var result = [
                        "message": message,
                        "interaction": interaction
                    ]
                    if let button = button {
                        result["button"] = button
                    }
                    guard let resultJson = try? JSONSerialization.data(withJSONObject: result),
                          let resultString = String(data: resultJson, encoding: .utf8) else {
                        done(MethodResult.failure("Unable to serialize InApp action data"))
                        return
                    }
                    done(MethodResult.success(resultString))
                }
            )
        default:
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
