//
//  TestUtils.swift
//  BloomreachSDKMauiIOSTests
//
//  Created by Adam Mihalik on 29/09/2023.
//

import Foundation

struct TestUtils {
    static func readTestFileConf(_ fileName: String) -> [String: Any] {
        let mainBundle = Bundle(identifier: "com.bloomreach.maui.ios.BloomreachSDKMauiIOSTest.BloomreachSDKMauiIOSTests")
        guard let path = mainBundle?.path(forResource: "Jsons/\(fileName)", ofType: "json") else { return [:] }
        guard let data = try? Data(contentsOf: URL(fileURLWithPath: path), options: .mappedIfSafe) else { return [:] }
        guard let jsonResult = try? JSONSerialization.jsonObject(with: data, options: .mutableLeaves) as? NSDictionary else { return [:] }
        return jsonResult as! [String: Any]
    }

    static func readTestFile(_ fileName: String) -> String {
        let mainBundle = Bundle(identifier: "com.bloomreach.maui.ios.BloomreachSDKMauiIOSTest.BloomreachSDKMauiIOSTests")!
        let path = mainBundle.path(forResource: "Jsons/\(fileName)", ofType: "json")!
        let data = try? Data(contentsOf: URL(fileURLWithPath: path), options: .mappedIfSafe)
        return String(data: data!, encoding: .utf8)!
    }
}
