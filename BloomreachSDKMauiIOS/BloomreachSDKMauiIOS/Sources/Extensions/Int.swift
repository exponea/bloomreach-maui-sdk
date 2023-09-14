//
//  Int.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

extension Int {
    static func assertValueFromDict(data: [String: Any], key: String) -> Int {
        if let value = data[key] as? Int {
            return value
        } else {
            return 0
        }
    }
}
