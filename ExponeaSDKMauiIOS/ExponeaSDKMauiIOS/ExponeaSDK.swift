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
public class ExponeaSDK : NSObject {

    @objc
    public static let instance = ExponeaSDK()

    @objc
    public func invokeMethod(method: String?, params: String?) -> MethodResult {
        do {
            switch method {
            case "greetings":
                return sayHello()
            case "configureWithResult":
                return invokeInit2(params)
            default:
                return MethodResult.unsupportedMethod(method ?? "nil")
            }
        } catch let error {
            return MethodResult.failure("Method \(method ?? "nil") failed: \(error)")
        }
    }

    private func invokeInit2(_ confParams: String?) -> MethodResult {
        guard let confParams = confParams,
              let confParamsData = confParams.data(using: .utf8),
              let confMap = try? JSONSerialization.jsonObject(
                with: confParamsData,
                options: []
              ) as? [String: Any?] else {
            return MethodResult.failure("Unable to init SDK with empty configuration input")
        }
        guard let conf = try? Configuration(
            projectToken: confMap["projectToken"] as? String,
            authorization: Authorization.token(confMap["projectToken"] as? String ?? ""),
            baseUrl: confMap["baseUrl"] as? String
        ) else {
            return MethodResult.failure("Unable to build configuration from params")
        }
        Exponea.shared.configure(with: conf)
        return MethodResult.success("")
    }

    private func sayHello() -> MethodResult {
        return MethodResult.success("Hello from iOS")
    }
}
