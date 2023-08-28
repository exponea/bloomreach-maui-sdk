//
//  Int.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

extension Int {
    static func assertValueFromDict(data: [String: Any], key: String) -> Int {
        if let value = data[key] as? Int {
            return value
        } else {
            assertionFailure("Nil value")
            return 0
        }
    }
}
