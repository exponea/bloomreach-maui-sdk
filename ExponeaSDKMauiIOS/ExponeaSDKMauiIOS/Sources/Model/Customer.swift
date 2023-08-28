//
//  Customer.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK

struct Customer {

    let customerIds: [String: String]?
    let properties: [String: JSONConvertible]
    let timestamp: Double?
    
    init(data: [String: Any]) {
        customerIds = data["customerIds"] as? [String: String]
        properties = data["properties"] as? [String: JSONConvertible] ?? [:]
        timestamp = data["timestamp"] as? Double
    }
}
