//
//  Array+JSON.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 02.09.2023.
//

import Foundation

extension Array {
    var json: String? {
        guard let messagesJson = try? JSONSerialization.data(withJSONObject: self),
              let messagesString = String(data: messagesJson, encoding: .utf8) else {
            return nil
        }
        return messagesString
    }
}
