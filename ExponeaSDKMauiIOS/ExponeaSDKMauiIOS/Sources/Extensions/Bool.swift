//
//  Bool.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import Foundation

extension Bool {
    static func assertValueFromDict(data: [String: Any], key: String) -> Bool {
        if let value = data[key] as? Bool {
            return value
        } else {
            return false
        }
    }
}
