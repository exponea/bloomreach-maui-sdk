//
//  ExponeaError.swift
//  ExponeaSDKMauiIOS
//
//  Created by Adam Mihalik on 14/08/2023.
//

import Foundation

public enum ExponeaError : LocalizedError {
    case unsupportedMethod(String)
    case common(String)
    
    public var errorDescription: String? {
        switch self {
        case .unsupportedMethod(let method):
            return "Method \(method) is currently unsupported"
        case .common(let errorMessage):
            return errorMessage
        }
    }
}
