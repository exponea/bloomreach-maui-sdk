//
//  BloomreachSDKMauiIOSTests.swift
//  BloomreachSDKMauiIOSTests
//
//  Created by Ankmara on 30.08.2023.
//

import XCTest
import Foundation
import Nimble
import Quick
import ExponeaSDK
import ExponeaSDK_Notifications

@testable import BloomreachSDKMauiIOS

class BloomreachSDKMauiIOSTests: QuickSpec {
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
        func getConfFrom(type: JSONType) -> [String: Any] {
            return readTestFile(fileName: type.name)
        }

        func readTestFile(fileName: String) -> [String: Any] {
            let mainBundle = Bundle(identifier: "com.bloomreach.maui.ios.BloomreachSDKMauiIOSTest.BloomreachSDKMauiIOSTests")
            guard let path = mainBundle?.path(forResource: "Jsons/\(fileName)", ofType: "json") else { return [:] }
            guard let data = try? Data(contentsOf: URL(fileURLWithPath: path), options: .mappedIfSafe) else { return [:] }
            guard let jsonResult = try? JSONSerialization.jsonObject(with: data, options: .mutableLeaves) as? NSDictionary else { return [:] }
            return jsonResult as! [String: Any]
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
            let success = BloomreachSdkIOS.instance.configure(data: configData).success
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
            let success = BloomreachSdkIOS.instance.configure(data: configData).success
            expect(success).to(beTrue())
            let isSuccess = BloomreachSdkIOS.instance.setDefaultProperties(data: [
                "prop1": "value1",
                "prop2": "value2"
            ]).success
            Exponea.shared.configuration?.saveToUserDefaults()
            expect(isSuccess).to(beTrue())
            let savedProps = Exponea.shared.configuration?.defaultProperties ?? [:]
            expect(savedProps["prop1"] as? String).to(equal("value1"))
        }

        it("flushing") {
            let success = BloomreachSdkIOS.instance.setFlushMode(value: "manual").success
            expect(success).to(beTrue())
            let flushValue = BloomreachSdkIOS.instance.getFlushMode().data
            expect(flushValue).to(equal("manual"))
        }

        it("session tracking") {
            BloomreachSdkIOS.instance.setupExponeaSDK(type: Exponea.shared)
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
            _ = BloomreachSdkIOS.instance.configure(data: configData)
            let successSet = BloomreachSdkIOS.instance.setAutomaticSessionTracking(value: true).success
            expect(successSet).to(beTrue())
            let isAutomatic = Exponea.shared.configuration?.automaticSessionTracking == true
            expect(isAutomatic).to(beTrue())
        }

        it("log level") {
            let success = BloomreachSdkIOS.instance.setLogLevel(level: "error").success
            expect(success).to(beTrue())
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
            _ = BloomreachSdkIOS.instance.configure(data: configData)
            if let value = Exponea.shared.configuration?.sessionTimeout {
                expect(value).to(equal(10))
            }
        }

        it("save mode") {
            BloomreachSdkIOS.instance.setupExponeaSDK(type: Exponea.shared)
            let success = BloomreachSdkIOS.instance.setSafeMode(value: true).success
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

        it("InAppMessageActionDelegateObject") {
            var data: [String: Any] = [:]
            data["id"] = "id"
            data["isHtml"] = true
            data["variantId"] = 1
            let message: InAppMessage = .init(data: data)
            expect(message.id).to(equal("id"))
            expect(message.isHtml).to(beTrue())
            expect(message.variantId).to(equal(1))
        }

        it("InAppMessageActionDelegateObject") {
            var exponeaMock = MockExponea()
            BloomreachSdkIOS.instance.setupExponeaSDK(type: exponeaMock)
            let data: [String: Any] = [
                "overrideDefaultBehavior": true,
                "trackActions": false
            ]
            BloomreachSdkIOS.instance.setInAppMessageActionCallback(data: data) { _ in }
            expect(exponeaMock.inAppMessagesDelegate.overrideDefaultBehavior).to(beTrue())
            expect(exponeaMock.inAppMessagesDelegate.trackActions).to(beFalse())
        }

        it("InApp click") {
            BloomreachSdkIOS.instance.setupExponeaSDK(type: MockExponea())
            var message: [String: Any] = [:]
            message["id"] = "id"
            message["isHtml"] = true
            message["variantId"] = 1
            let data: [String: Any] = [
                "message": message.jsonData as Any,
                "buttonText": "Button text",
                "buttonLink": "Button link"
            ]
            _ = BloomreachSdkIOS.instance.trackInAppMessageClick(data: data)
            let exponea = (BloomreachSdkIOS.instance.exponeaSDK as! MockExponea)
            let found = exponea.calls.first(where: { $0.name == "trackInAppMessageClick" })
            let msg = (found?.params[0] as! InAppMessage)
            expect(msg.id).to(equal("id"))
            expect(msg.isHtml).to(beTrue())
            expect(msg.variantId).to(equal(1))
            expect((found?.params[1] as! String)).to(equal("Button text"))
            expect((found?.params[2] as! String)).to(equal("Button link"))
        }

        it("InApp close") {
            BloomreachSdkIOS.instance.setupExponeaSDK(type: MockExponea())
            var message: [String: Any] = [:]
            message["id"] = "id"
            message["isHtml"] = true
            message["variantId"] = 1
            let data: [String: Any] = [
                "message": message.jsonData as Any,
                "isUserInteraction": false
            ]
            _ = BloomreachSdkIOS.instance.trackInAppMessageClose(data: data)
            let exponea = (BloomreachSdkIOS.instance.exponeaSDK as! MockExponea)
            let found = exponea.calls.first(where: { $0.name == "trackInAppMessageClose" })
            let msg = (found?.params[0] as! InAppMessage)
            expect(msg.id).to(equal("id"))
            expect(msg.isHtml).to(beTrue())
            expect(msg.variantId).to(equal(1))
            expect((found?.params[1] as! Bool)).to(beFalse())
        }

        it("InApp trackInAppMessageCloseWithoutTrackingConsent") {
            BloomreachSdkIOS.instance.setupExponeaSDK(type: MockExponea())
            var message: [String: Any] = [:]
            message["id"] = "id"
            message["isHtml"] = true
            message["variantId"] = 1
            let data: [String: Any] = [
                "message": message.jsonData as Any,
                "isUserInteraction": true
            ]
            _ = BloomreachSdkIOS.instance.trackInAppMessageCloseWithoutTrackingConsent(data: data)
            let exponea = (BloomreachSdkIOS.instance.exponeaSDK as! MockExponea)
            let found = exponea.calls.first(where: { $0.name == "trackInAppMessageCloseWithoutTrackingConsent" })
            let msg = (found?.params[0] as! InAppMessage)
            expect(msg.id).to(equal("id"))
            expect(msg.isHtml).to(beTrue())
            expect(msg.variantId).to(equal(1))
            expect((found?.params[1] as! Bool)).to(beTrue())
        }
    }
}
