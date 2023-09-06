//
//  Double.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import Foundation

extension Double {
    static func assertValueFromDict(data: [String: Any], key: String) -> Double {
        if let value = data[key] as? Double {
            return value
        } else if let value = data[key] as? Int {
            return Double(value)
        } else {
            return 0
        }
    }
}
