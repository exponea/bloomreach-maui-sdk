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
        default:
            self = .unsupported
        }
    }
}
