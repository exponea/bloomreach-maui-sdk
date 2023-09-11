//
//  ExponeaSDKMauiIOSTests.swift
//  ExponeaSDKMauiIOSTests
//
//  Created by Ankmara on 30.08.2023.
//

import XCTest
import Foundation
import Nimble
import Quick
import ExponeaSDK
import ExponeaSDK_Notifications

@testable import ExponeaSDKMauiIOS

class ExponeaSDKMauiIOSTests: QuickSpec {
    enum JSONType {
        case configuration
        
        var name: String {
            switch self {
            case .configuration:
                return "Configure_Configuration_Variant_1"
            }
        }
    }

    override  func spec() {
        func getConfFrom(type: JSONType) -> [String : Any] {
            let mainBundle = Bundle(identifier: "com.exponea.maui.ios.ExponeaSDKMauiIOSTest.ExponeaSDKMauiIOSTests")
            guard let path = mainBundle?.path(forResource: "Jsons/\(type.name)", ofType: "json") else { return [:] }
            guard let data = try? Data(contentsOf: URL(fileURLWithPath: path), options: .mappedIfSafe) else { return [:] }
            guard let jsonResult = try? JSONSerialization.jsonObject(with: data, options: .mutableLeaves) as? NSDictionary else { return [:] }
            return jsonResult as! [String : Any]
        }

        it("assert bool - true") {
            let data: [String: Any] = ["isOk": true]
            let isOk: Bool = .assertValueFromDict(data: data, key: "isOk")
            expect(isOk).to(beTrue())
        }

        it("assert bool - false") {
            let data: [String: Any] = ["isOk": ""]
            let isOk: Bool = .assertValueFromDict(data: data, key: "isOk")
            expect(isOk).to(beFalse())
        }

        it("array json") {
            let array: [String] = [
                "key1",
                "key2",
                "key3"
            ]
            let json = array.json
            let result = json?.contains("key1") == true && json?.contains("key2") == true && json?.contains("key3") == true
            expect(result).to(beTrue())
        }

        it("should setup simplest configuration") {
            let exponea = Exponea.shared
            let configData: [String: Any] = getConfFrom(type: .configuration)
            let success = ExponeaSDK.instance.configure(data: configData).success
            expect(success).to(beTrue())
            expect(exponea.configuration!.projectToken).to(equal("projToken"))
            expect(exponea.configuration!.baseUrl).to(equal("https://url.com"))
            expect(exponea.configuration!.defaultProperties).to(beNil())
            expect(exponea.configuration!.automaticPushNotificationTracking).to(equal(true))
            expect(exponea.configuration!.tokenTrackFrequency).to(equal(.daily))
            guard case .immediate = exponea.flushingMode else {
                XCTFail("Incorect flushing mode")
                return
            }
        }
 
        it("setDefaultProperties") {
            let exponea = Exponea.shared
            let configData: [String: Any] = [
                "appGroup": "appGroup",
                "allowDefaultCustomerProperties": true,
                "automaticPushNotificationTracking": true,
                "automaticSessionTracking": true,
                "baseUrl": "https://api.exponea.com",
                "flushEventMaxRetries": 20,
                "projectToken": "projectToken",
                "requirePushAuthorization": false,
                "sessionTimeout": 10,
                "token": "token",
                "tokenTrackFrequency": "daily"
            ]
            let success = ExponeaSDK.instance.configure(data: configData).success
            expect(success).to(beTrue())
            let isSuccess = ExponeaSDK.instance.setDefaultProperties(data: [
                "prop1": "value1",
                "prop2": "value2"
            ]).success
            Exponea.shared.configuration?.saveToUserDefaults()
            expect(isSuccess).to(beTrue())
            let savedProps = Exponea.shared.configuration?.defaultProperties ?? [:]
            expect(savedProps["prop1"] as? String).to(equal("value1"))
        }
        
        it("flushing") {
            let success = ExponeaSDK.instance.setFlushMode(value: "manual").success
            expect(success).to(beTrue())
            let flushValue = ExponeaSDK.instance.getFlushMode().data
            expect(flushValue).to(equal("manual"))
        }
        
        it("session tracking") {
            let configData: [String: Any] = [
                "appGroup": "appGroup",
                "allowDefaultCustomerProperties": true,
                "automaticPushNotificationTracking": true,
                "automaticSessionTracking": true,
                "baseUrl": "https://api.exponea.com",
                "flushEventMaxRetries": 20,
                "projectToken": "projectToken",
                "requirePushAuthorization": false,
                "sessionTimeout": 10,
                "token": "token",
                "tokenTrackFrequency": "daily"
            ]
            let _ = ExponeaSDK.instance.configure(data: configData)
            let successSet = ExponeaSDK.instance.setAutomaticSessionTracking(value: true, timeout: 10).success
            expect(successSet).to(beTrue())
            let isAutomatic = Exponea.shared.configuration?.automaticSessionTracking == true
            expect(isAutomatic).to(beTrue())
        }
        
        it("log level") {
            let success = ExponeaSDK.instance.setLogLevel(level: 1).success
            expect(success).to(beTrue())
//            let value = Exponea.logger.logLevel == .error
//            expect(value).to(beTrue())
        }

        it("timeout") {
            let configData: [String: Any] = [
                "appGroup": "appGroup",
                "allowDefaultCustomerProperties": true,
                "automaticPushNotificationTracking": true,
                "automaticSessionTracking": true,
                "baseUrl": "https://api.exponea.com",
                "flushEventMaxRetries": 20,
                "projectToken": "projectToken",
                "requirePushAuthorization": false,
                "sessionTimeout": 10,
                "token": "token",
                "tokenTrackFrequency": "daily"
            ]
            let _ = ExponeaSDK.instance.configure(data: configData)
            if let value = Exponea.shared.configuration?.sessionTimeout {
                expect(value).to(equal(10))
            }
        }
        
        it("save mode") {
            let success = ExponeaSDK.instance.setSafeMode(value: true).success
            expect(success).to(beTrue())
            expect(Exponea.shared.safeModeEnabled).to(beTrue())
        }
        
        it("Should not crash tracking payment") {
            let exponea = Exponea.shared
            expect { exponea.trackPayment(properties: [:], timestamp: nil) }.toNot( throwError() )
        }
        
        it("Should not crash tracking event") {
            let exponea = Exponea.shared
            expect { exponea.trackEvent(properties: [:], timestamp: nil, eventType: nil) }.toNot( throwError() )
        }
        
        it("Should not crash tracking session") {
            let exponea = Exponea.shared
            expect(exponea.trackSessionStart()).toNot( throwError() )
            expect(exponea.trackSessionEnd()).toNot( throwError() )
        }
    }
}

