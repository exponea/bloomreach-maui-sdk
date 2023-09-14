//
//  ExponeaMethodType.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK

public enum ExponeaMethodType {
    case anonymize(data: [String: Any])
    case configure(data: [String: Any])
    case flushData
    case getConfiguration
    case getCustomerCookie
    case getCheckPushSetup
    case getDefaultProperties
    case getFlushMode
    case getFlushPeriod
    case setFlushPeriod(data: Int64?)
    case getLogLevel
    case getSessionTimeout
    case setSessionTimeout(data: Int64?)
    case getTokenTrackFrequency
    case identifyCustomer(data: [String: Any])
    case isAutomaticSessionTracking
    case isAutoPushNotification
    case isConfigured
    case isSafeMode
    case setAutomaticSessionTracking(data: Bool)
    case setCheckPushSetup(data: Bool)
    case setDefaultProperties(data: [String: Any])
    case setFlushMode(data: String?)
    case setLogLevel(data: String?)
    case setSafeMode(data: Bool)
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
            self = .anonymize(data: params.json)
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
        case "setflushperiod":
            self = .setFlushPeriod(data: params == nil ? nil : Int64(params!))
        case "getloglevel":
            self = .getLogLevel
        case "getsessiontimeout":
            self = .getSessionTimeout
        case "setsessiontimeout":
            self = .setSessionTimeout(data: params == nil ? nil : Int64(params!))
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
            self = .setAutomaticSessionTracking(data: JsonDataParser.parseBoolean(params))
        case "setcheckpushsetup":
            self = .setCheckPushSetup(data: JsonDataParser.parseBoolean(params))
        case "setdefaultproperties":
            self = .setDefaultProperties(data: params.json)
        case "setflushmode":
            self = .setFlushMode(data: params)
        case "setloglevel":
            self = .setLogLevel(data: params)
        case "setsafemode":
            self = .setSafeMode(data: JsonDataParser.parseBoolean(params))
        default:
            self = .unsupported
        }
    }
}
