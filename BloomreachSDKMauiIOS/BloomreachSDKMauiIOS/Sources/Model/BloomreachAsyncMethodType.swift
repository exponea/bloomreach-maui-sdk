//
//  BloomreachAsyncMethodType.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 02.09.2023.
//

import ExponeaSDK

public enum BloomreachAsyncMethodType {
    case flushData(data: TypeBlock<MethodResult>?)
    case setReceivedPushCallback(data: TypeBlock<MethodResult>?)
    case setOpenedPushCallback(data: TypeBlock<MethodResult>?)
    case requestPushAuthorization(completion: TypeBlock<MethodResult>?)
    case setInAppMessageActionCallback(data: [String: Any], completion: TypeBlock<MethodResult>?)
    case fetchAppInbox(completion: TypeBlock<MethodResult>?)
    case fetchAppInboxItem(data: String?, completion: TypeBlock<MethodResult>?)
    case markAppInboxAsRead(data: String?, completion: TypeBlock<MethodResult>?)
    case fetchConsents(completion: TypeBlock<MethodResult>?)
    case fetchRecommendation(data: [String: Any], completion: TypeBlock<MethodResult>?)
    case unsupported

    init(method: String?, params: String?, asyncBlock: TypeBlock<MethodResult>? = nil) {
        guard let methodName = method else {
            self = .unsupported
            return
        }
        switch methodName.lowercased() {
        case "flushdata":
            self = .flushData(data: asyncBlock)
        case "setreceivedpushcallback":
            self = .setReceivedPushCallback(data: asyncBlock)
        case "setopenedpushcallback":
            self = .setOpenedPushCallback(data: asyncBlock)
        case "requestpushauthorization":
            self = .requestPushAuthorization(completion: asyncBlock)
        case "setinappmessageactioncallback":
            self = .setInAppMessageActionCallback(data: params.json, completion: asyncBlock)
        case "fetchappinbox":
            self = .fetchAppInbox(completion: asyncBlock)
        case "fetchappinboxitem":
            self = .fetchAppInboxItem(data: params, completion: asyncBlock)
        case "markappinboxasread":
            self = .markAppInboxAsRead(data: params, completion: asyncBlock)
        case "fetchconsents":
            self = .fetchConsents(completion: asyncBlock)
        case "fetchrecommendation":
            self = .fetchRecommendation(data: params.json, completion: asyncBlock)
        default:
            self = .unsupported
        }
    }
}
