//
//  ExponeaAsyncMethodType.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 02.09.2023.
//

import ExponeaSDK

public enum ExponeaAsyncMethodType {
    case flushData(data: TypeBlock<MethodResult>?)
    case unsupported
    
    init(method: String?, params: String?, asyncBlock: TypeBlock<MethodResult>? = nil) {
        guard let methodName = method else {
            self = .unsupported
            return
        }
        switch methodName.lowercased() {
        case "flushdata":
            self = .flushData(data: asyncBlock)
        default:
            self = .unsupported
        }
    }
}
