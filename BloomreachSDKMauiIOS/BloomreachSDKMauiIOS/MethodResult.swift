//
//  MethodResult.swift
//  BloomreachSDKMauiIOS
//
//  Created by Adam Mihalik on 14/08/2023.
//

import Foundation
import UIKit

@objc(MethodResult)
public class MethodResult : NSObject {
    @objc
    public let success: Bool
    @objc
    public let data: String
    @objc
    public let error: String
    
    @objc
    public init(
        success: Bool,
        data: String,
        error: String
    ) {
        self.success = success
        self.data = data
        self.error = error
    }
    
    public static func success(_ data: String?) -> MethodResult {
        .init(success: true, data: data ?? "", error: "")
    }
    
    public static func failure(_ message: String) -> MethodResult {
        MethodResult(success: false, data: "", error: message)
    }
    
    public static func unsupportedMethod(_ method: String) -> MethodResult {
        .failure("Method \(method) is currently unsupported")
    }
}

@objc(MethodResultForUI)
public class MethodResultForUI : NSObject {
    @objc
    public let success: Bool
    @objc
    public let data: UIView?
    @objc
    public let error: String
    
    @objc
    public init(
        success: Bool,
        data: UIView?,
        error: String
    ) {
        self.success = success
        self.data = data
        self.error = error
    }
    
    public static func success(_ data: UIView) -> MethodResultForUI {
        return MethodResultForUI(success: true, data: data, error: "")
    }
    
    public static func failure(_ message: String) -> MethodResultForUI {
        return MethodResultForUI(success: false, data: nil, error: message)
    }
    
    public static func unsupportedMethod(_ method: String) -> MethodResultForUI {
        return .failure("Method \(method) is currently unsupported")
    }
}
