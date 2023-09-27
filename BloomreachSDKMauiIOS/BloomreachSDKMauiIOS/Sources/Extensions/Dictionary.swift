//
//  Dictionary.swift
//  BloomreachSDKMauiIOS
//
//  Created by Michal Severín on 18.09.2023.
//

import Foundation

extension Dictionary {
    var jsonData: String? {
        var toReturn: [String: Any] = [:]
        for value in self {
            let stringKey: String
            if let key = value.key as? String {
                stringKey = key
            } else if let key = value.key as? Int {
                stringKey = String(key)
            } else if let key = value.key as? Double {
                stringKey = String(key)
            } else if let key = value.key as? NSNumber {
                stringKey = key.stringValue
            } else {
                stringKey = "\(value.key)"
            }
            toReturn[stringKey] = value.value
        }
        if let data = try? JSONSerialization.data(withJSONObject: toReturn, options: []) {
            let json = String(data: data, encoding: .utf8)
            return json
        }
        return nil
    }
}
