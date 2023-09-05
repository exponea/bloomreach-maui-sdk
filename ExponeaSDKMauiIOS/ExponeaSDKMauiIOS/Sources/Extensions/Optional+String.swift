//
//  Optional+String.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import Foundation

extension Optional where Wrapped == String {
    var json: [String : Any] {
        guard let confParams = self,
              let confParamsData = confParams.data(using: .utf8),
              let confMap = try? JSONSerialization.jsonObject(
                with: confParamsData,
                options: []
              ) as? [String: Any] else {
            return [:]
        }
        return confMap
    }
}

extension Optional where Wrapped == String {
    func assertValue() -> String {
        if let self {
            return self
        } else {
            return ""
        }
    }
}
