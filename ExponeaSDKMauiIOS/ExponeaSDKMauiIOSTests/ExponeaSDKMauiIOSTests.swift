//
//  ExponeaSDKMauiIOSTests.swift
//  ExponeaSDKMauiIOSTests
//
//  Created by Ankmara on 30.08.2023.
//

import XCTest
import Foundation
import Quick
import Nimble
import ExponeaSDK
import NimbleObjectiveC

@testable import ExponeaSDKMauiIOS

class ExponeaSDKMauiIOSTests: QuickSpec {
    override class func spec() {
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
            expect(exponea.configuration!.projectToken).to(equal("projectToken"))
            expect(exponea.configuration!.baseUrl).to(equal(Constants.Repository.baseUrl))
            expect(exponea.configuration!.defaultProperties).to(beNil())
            expect(exponea.configuration!.sessionTimeout).to(equal(10.0))
            expect(exponea.configuration!.automaticSessionTracking).to(equal(true))
            expect(exponea.configuration!.automaticPushNotificationTracking).to(equal(true))
            expect(exponea.configuration!.tokenTrackFrequency).to(equal(.daily))
            expect(exponea.configuration!.appGroup).to(equal("appGroup"))
            expect(exponea.configuration!.flushEventMaxRetries).to(equal(20))
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
            let isAutomatic = ExponeaSDK.instance.isAutomaticSessionTracking().data == "true"
            expect(isAutomatic).to(beTrue())
            let isAutomaticSDK = Exponea.shared.configuration?.automaticSessionTracking == true
            expect(isAutomaticSDK).to(beTrue())
        }
        
        it("log level") {
            let success = ExponeaSDK.instance.setLogLevel(level: 1).success
            expect(success).to(beTrue())
            let value = Exponea.logger.logLevel == .error
            expect(value).to(beTrue())
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
            let value = ExponeaSDK.instance.getSessionTimeout().data
            expect(value).to(equal("10.0"))
        }
        
        it("save mode") {
            let success = ExponeaSDK.instance.setSafeMode(value: true).success
            expect(success).to(beTrue())
            expect(Exponea.shared.safeModeEnabled).to(beTrue())
        }
        
        it("Should not crash tracking payment") {
            var exponea = Exponea.shared
            expect { exponea.trackPayment(properties: [:], timestamp: nil) }.toNot( throwError() )
        }
        
        it("Should not crash tracking event") {
            var exponea = Exponea.shared
            expect { exponea.trackEvent(properties: [:], timestamp: nil, eventType: nil) }.toNot( throwError() )
        }
        
        it("Should not crash tracking session") {
            var exponea = Exponea.shared
            expect(exponea.trackSessionStart()).toNot( throwError() )
            expect(exponea.trackSessionEnd()).toNot( throwError() )
        }
    }
}

