//
//  JsonDataParser.swift
//  BloomreachSDKMauiIOS
//
//  Created by Adam Mihalik on 13/09/2023.
//

import Foundation

import ExponeaSDK

struct JsonDataParser {

    static func parseBoolean(_ source: String?) -> Bool {
        return Bool(source?.lowercased() ?? "false") ?? false
    }

    static func parse(dictionary: [String: Any]) -> [String: JSONConvertible] {
        var data: [String: JSONConvertible] = [:]
        dictionary.forEach { key, value in
            data[key] = parseValue(value: value)
        }
        return data
    }

    static func parseArray(array: NSArray) -> [JSONConvertible] {
        return array.map { parseValue(value: $0) }
    }

    static func parseValue(value: Any) -> JSONConvertible {
        if let dictionary = value as? [String: Any] {
            return parse(dictionary: dictionary)
        } else if let array = value as? NSArray {
            return parseArray(array: array)
        } else if let number = value as? NSNumber {
            if number === kCFBooleanFalse {
                return false
            } else if number === kCFBooleanTrue {
                return true
            } else {
                return number.doubleValue
            }
        } else if let string = value as? NSString {
            return string
        }
        return NSNull()
    }
}
