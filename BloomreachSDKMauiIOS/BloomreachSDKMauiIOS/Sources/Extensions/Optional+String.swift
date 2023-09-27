//
//  Optional+String.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import Foundation

extension Optional where Wrapped == String {
    var json: [String : Any] {
        guard let confParams = self else {
            return [:]
        }
        return confParams.json
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
