//
//  ExponeaAsyncMethodType.swift
//  ExponeaSDKMauiIOS
//
//  Created by Michal Severín on 02.09.2023.
//

import ExponeaSDK

public enum ExponeaAsyncMethodType {
    case fetchConsent(data: TypeBlock<MethodResult>?)
    case unsupported
    
    init(method: String?, params: String?, asyncBlock: TypeBlock<MethodResult>? = nil) {
        guard let methodName = method else {
            self = .unsupported
            return
        }
        switch methodName.lowercased() {
        case "fetchconsents":
            self = .fetchConsent(data: asyncBlock)
        default:
            self = .unsupported
        }
    }
}
