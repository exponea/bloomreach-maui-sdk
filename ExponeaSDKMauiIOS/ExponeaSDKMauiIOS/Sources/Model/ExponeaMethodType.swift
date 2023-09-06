//
//  ExponeaMethodType.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK

public enum ExponeaMethodType {
    case anonymize
    case configure(data: [String: Any])
    case flushData
    case getConfiguration
    case getCustomerCookie
    case getCheckPushSetup
    case getDefaultProperties
    case getFlushMode
    case getFlushPeriod
    case getLogLevel
    case getSessionTimeout
    case getTokenTrackFrequency
    case identifyCustomer(data: [String: Any])
    case isAutomaticSessionTracking
    case isAutoPushNotification
    case isConfigured
    case isSafeMode
    case setAutomaticSessionTracking(data: [String: Any])
    case setCheckPushSetup(data: [String: Any])
    case setDefaultProperties(data: [String: Any])
    case setFlushMode(data: [String: Any])
    case setLogLevel(data: [String: Any])
    case setSafeMode(data: [String: Any])
    case trackPaymentEvent(data: [String: Any])
    case trackEvent(data: [String: Any])
    case trackSessionEnd
    case trackSessionStart
    case unsupported

    init(method: String?, params: String?) {
        guard let methodName = method else {
            self = .unsupported
            return
        }
        switch methodName.lowercased() {
        case "trackpaymentevent":
            self = .trackPaymentEvent(data: params.json)
        case "trackevent":
            self = .trackEvent(data: params.json)
        case "tracksessionend":
            self = .trackSessionEnd
        case "tracksessionstart":
            self = .trackSessionStart
        case "anonymize":
            self = .anonymize
        case "configure":
            self = .configure(data: params.json)
        case "flushdata":
            self = .flushData
        case "getconfiguration":
            self = .getConfiguration
        case "getcustomercookie":
            self = .getCustomerCookie
        case "getcheckpushsetup":
            self = .getCheckPushSetup
        case "getdefaultproperties":
            self = .getDefaultProperties
        case "getflushmode":
            self = .getFlushMode
        case "getflushperiod":
            self = .getFlushPeriod
        case "getloglevel":
            self = .getLogLevel
        case "getsessiontimeout":
            self = .getSessionTimeout
        case "gettokentrackfrequency":
            self = .getTokenTrackFrequency
        case "identifycustomer":
            self = .identifyCustomer(data: params.json)
        case "isautomaticsessiontracking":
            self = .isAutomaticSessionTracking
        case "isautopushnotification":
            self = .isAutoPushNotification
        case "isconfigured":
            self = .isConfigured
        case "issafemode":
            self = .isSafeMode
        case "setautomaticsessiontracking":
            self = .setAutomaticSessionTracking(data: params.json)
        case "setcheckpushsetup":
            self = .setCheckPushSetup(data: params.json)
        case "setdefaultproperties":
            self = .setDefaultProperties(data: params.json)
        case "setflushmode":
            self = .setFlushMode(data: params.json)
        case "setloglevel":
            self = .setLogLevel(data: params.json)
        case "setsafemode":
            self = .setSafeMode(data: params.json)
        default:
            self = .unsupported
        }
    }
}
