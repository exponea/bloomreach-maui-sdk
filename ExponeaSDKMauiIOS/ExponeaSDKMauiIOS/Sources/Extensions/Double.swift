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
        } else {
            assertionFailure("Nil value")
            return 0
        }
    }
}
