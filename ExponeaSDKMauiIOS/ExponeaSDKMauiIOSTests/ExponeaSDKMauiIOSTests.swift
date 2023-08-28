//
//  ExponeaSDKMauiIOSTests.swift
//  ExponeaSDKMauiIOSTests
//
//  Created by Ankmara on 30.08.2023.
//

import Foundation
import Quick
import Nimble

@testable import ExponeaSDKMauiIOS

class ExponeaSDKMauiIOSTests: QuickSpec {
    override class func spec() {

        it("assert bool") {
            let data: [String: Any] = ["isOk": true]
            let isOk: Bool = .assertValueFromDict(data: data, key: "isOk")
            expect(isOk).to(beTrue())
        }

        it("parse archived json") {
            let data: [String: Any] = [
                :
            ]
        }

        it("identifyCustomer") {
            let data: [String: Any] = [
                "customerIds": ["id": "idvalue"],
                "properties": ["isGold": "yes"],
                "timestamp": 100
            ]
            ExponeaSDK.instance.identifyCustomer(data: data)
            let customernfo = ExponeaSDK.instance.getDefaultProperties()
            print(customernfo.data)
        }

        it("getDefaultProperties") {

        }
    }
}
