//
//  String.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

extension String {
    static func assertValueFromDict(data: [String: Any], key: String) -> String {
        (data[key] as? String).assertValue()
    }
}
