//
//  String.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import Foundation

extension String {
    static func assertValueFromDict(data: [String: Any], key: String) -> String {
        (data[key] as? String).assertValue()
    }
    var json: [String: Any] {
        guard let confParamsData = self.data(using: .utf8),
              let confMap = try? JSONSerialization.jsonObject(
                with: confParamsData,
                options: []
              ) as? [String: Any] else {
            return [:]
        }
        return confMap
    }
}
