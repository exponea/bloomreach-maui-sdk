//
//  MethodResult.swift
//  ExponeaSDKMauiIOS
//
//  Created by Adam Mihalik on 14/08/2023.
//

import Foundation

@objc(MethodResult)
public class MethodResult : NSObject {
    @objc
    public let success: Bool
    @objc
    public let data: String
    @objc
    public let error: String
    
    init(success: Bool, data: String, error: String) {
        self.success = success
        self.data = data
        self.error = error
    }
    
    public static func success(_ data: String?) -> MethodResult {
        return MethodResult(success: true, data: data ?? "", error: "")
    }
    
    public static func failure(_ message: String) -> MethodResult {
        return MethodResult(success: false, data: "", error: message)
    }
    
    public static func unsupportedMethod(_ method: String) -> MethodResult {
        return .failure("Method \(method) is currently unsupported")
    }
}
