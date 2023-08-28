//
//  Configuration.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK

struct Configuration {
    
    let appGroup: String?
    let advancedAuthEnabled: Bool
    let allowDefaultCustomerProperties: Bool
    let automaticPushNotificationTracking: Bool
    let automaticSessionTracking: Bool
    let baseUrl: String
    let flushEventMaxRetries: Int
    let projectToken: String
    let requirePushAuthorization: Bool
    let sessionTimeout: Double
    let token: String
    var tokenTrackFrequency: TokenTrackFrequency = .onTokenChange
    
    init(data: [String: Any]) {
        appGroup = data["appGroup"] as? String
        advancedAuthEnabled = .assertValueFromDict(data: data, key: "advancedAuthEnabled")
        allowDefaultCustomerProperties = .assertValueFromDict(data: data, key: "allowDefaultCustomerProperties")
        automaticPushNotificationTracking = .assertValueFromDict(data: data, key: "automaticPushNotificationTracking")
        automaticSessionTracking = .assertValueFromDict(data: data, key: "automaticSessionTracking")
        baseUrl = .assertValueFromDict(data: data, key: "baseUrl")
        flushEventMaxRetries = data["flushEventMaxRetries"] as? Int ?? 0
        projectToken = .assertValueFromDict(data: data, key: "projectToken")
        requirePushAuthorization = .assertValueFromDict(data: data, key: "requirePushAuthorization")
        sessionTimeout = .assertValueFromDict(data: data, key: "sessionTimeout")
        token = .assertValueFromDict(data: data, key: "token")
        if let tokenTrackFrequency = data["tokenTrackFrequency"] as? String {
            switch tokenTrackFrequency {
            case "onTokenChange":
                self.tokenTrackFrequency = .onTokenChange
            case "daily":
                self.tokenTrackFrequency = .daily
            case "everyLaunch":
                self.tokenTrackFrequency = .everyLaunch
            default:
                assertionFailure("No token track frequency set")
            }
        }
    }
}
