//
//  AppInboxTests.swift
//  BloomreachSDKMauiIOSTests
//
//  Created by Adam Mihalik on 29/09/2023.
//

import XCTest
import Foundation
import Nimble
import Quick
import ExponeaSDK
import ExponeaSDK_Notifications

@testable import BloomreachSDKMauiIOS

class AppInboxTests: QuickSpec {
    override func spec() {
        var exponeaMock = MockExponea()
        beforeEach {
            // reset all
            exponeaMock = MockExponea()
            // set mock to use
            BloomreachSdkIOS.instance.setupExponeaSDK(type: exponeaMock)
        }
        it("GetAppInboxButton_Valid") {
            exponeaMock.registerResponse("getAppInboxButton", UIButton())
            let result = BloomreachSdkIOS.instance.invokeMethodForUI(method: "GetAppInboxButton", params: nil)
            exponeaMock.verifyMethodCalled("getAppInboxButton")
            expect(result.success).to(beTrue())
            expect(result.data).toNot(beNil())
        }

        it("TrackAppInboxClick_App_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClick",
                params: TestUtils.readTestFile("TrackAppInboxClick_App_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClick")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClick")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.app))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClick")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClick_Browser_Push") {
            let expectedMessage = self.buildAppInboxMessage("push")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClick",
                params: TestUtils.readTestFile("TrackAppInboxClick_Browser_Push")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClick")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClick")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.browser))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClick")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClick_Deeplink_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClick",
                params: TestUtils.readTestFile("TrackAppInboxClick_Deeplink_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClick")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClick")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.deeplink))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClick")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClick_NoAction_Unknown") {
            let expectedMessage = self.buildAppInboxMessage("unknown")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClick",
                params: TestUtils.readTestFile("TrackAppInboxClick_NoAction_Unknown")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClick")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClick")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.noAction))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClick")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClickWithoutTrackingConsent_App_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClickWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxClickWithoutTrackingConsent_App_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClickWithoutTrackingConsent")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.app))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClickWithoutTrackingConsent_Browser_Push") {
            let expectedMessage = self.buildAppInboxMessage("push")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClickWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxClickWithoutTrackingConsent_Browser_Push")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClickWithoutTrackingConsent")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.browser))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClickWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClickWithoutTrackingConsent")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.deeplink))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown") {
            let expectedMessage = self.buildAppInboxMessage("unknown")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxClickWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxClickWithoutTrackingConsent")
            expect(result.success).to(beTrue())
            let capturedAction = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[0] as! MessageItemAction
            expect(capturedAction.title).to(equal("Action title"))
            expect(capturedAction.type).to(equal(MessageItemActionType.noAction))
            expect(capturedAction.url).to(equal("https://example.com"))
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxClickWithoutTrackingConsent")[1] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpened_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpened",
                params: TestUtils.readTestFile("TrackAppInboxOpened_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpened")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpened")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpened_Push") {
            let expectedMessage = self.buildAppInboxMessage("push")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpened",
                params: TestUtils.readTestFile("TrackAppInboxOpened_Push")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpened")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpened")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpened_Unknown") {
            let expectedMessage = self.buildAppInboxMessage("unknown")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpened",
                params: TestUtils.readTestFile("TrackAppInboxOpened_Unknown")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpened")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpened")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpenedWithoutTrackingConsent_Html") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpenedWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Html")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpenedWithoutTrackingConsent")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpenedWithoutTrackingConsent")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpenedWithoutTrackingConsent_Push") {
            let expectedMessage = self.buildAppInboxMessage("push")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpenedWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Push")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpenedWithoutTrackingConsent")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpenedWithoutTrackingConsent")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("TrackAppInboxOpenedWithoutTrackingConsent_Unknown") {
            let expectedMessage = self.buildAppInboxMessage("unknown")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            let result = BloomreachSdkIOS.instance.invokeMethod(
                method: "TrackAppInboxOpenedWithoutTrackingConsent",
                params: TestUtils.readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Unknown")
            )
            exponeaMock.verifyMethodCalled("trackAppInboxOpenedWithoutTrackingConsent")
            let capturedMessage = exponeaMock.getCapturedParams("trackAppInboxOpenedWithoutTrackingConsent")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

        it("FetchAppInbox_Empty") {
            let expectedAppInbox: [MessageItem] = []
            exponeaMock.registerResponse("fetchAppInbox", ExponeaSDK.Result.success(expectedAppInbox))
            var result: MethodResult?
            BloomreachSdkIOS.instance.invokeMethodAsync(method: "FetchAppInbox", params: nil) { it in
                result = it
            }
            exponeaMock.verifyMethodCalled("fetchAppInbox")
            guard let result = result else {
                fail("MethodResult is NIL")
                return
            }
            expect(result.success).to(beTrue())
            expect(result.data).to(equal(TestUtils.readTestFile("FetchAppInbox_Empty")))
        }

        it("FetchAppInbox_Multiple") {
            let expectedAppInbox: [MessageItem] = [
                self.buildAppInboxMessage("html", "12345"),
                self.buildAppInboxMessage("html", "67890")
            ]
            exponeaMock.registerResponse("fetchAppInbox", ExponeaSDK.Result.success(expectedAppInbox))
            var result: MethodResult?
            BloomreachSdkIOS.instance.invokeMethodAsync(method: "FetchAppInbox", params: nil) { it in
                result = it
            }
            exponeaMock.verifyMethodCalled("fetchAppInbox")
            guard let result = result else {
                fail("MethodResult is NIL")
                return
            }
            expect(result.success).to(beTrue())
            expect(result.data.json as NSDictionary).to(equal(TestUtils.readTestFile("FetchAppInbox_Multiple").json as NSDictionary))
        }

        it("FetchAppInbox_Single") {
            let expectedAppInbox: [MessageItem] = [
                self.buildAppInboxMessage("html", "12345")
            ]
            exponeaMock.registerResponse("fetchAppInbox", ExponeaSDK.Result.success(expectedAppInbox))
            var result: MethodResult?
            BloomreachSdkIOS.instance.invokeMethodAsync(method: "FetchAppInbox", params: nil) { it in
                result = it
            }
            exponeaMock.verifyMethodCalled("fetchAppInbox")
            guard let result = result else {
                fail("MethodResult is NIL")
                return
            }
            expect(result.success).to(beTrue())
            expect(result.data.json as NSDictionary).to(equal(TestUtils.readTestFile("FetchAppInbox_Single").json as NSDictionary))
        }

        it("FetchAppInboxItem_Valid") {
            let expectedMessage = self.buildAppInboxMessage("html")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            var result: MethodResult?
            BloomreachSdkIOS.instance.invokeMethodAsync(method: "FetchAppInboxItem", params: "12345") { it in
                result = it
            }
            exponeaMock.verifyMethodCalled("fetchAppInboxItem")
            guard let result = result else {
                fail("MethodResult is NIL")
                return
            }
            expect(result.success).to(beTrue())
            expect(result.data.json as NSDictionary).to(equal(TestUtils.readTestFile("FetchAppInboxItem_Valid").json as NSDictionary))
        }

        it("MarkAppInboxAsRead") {
            let expectedMessage = self.buildAppInboxMessage("html", "abcd")
            exponeaMock.registerResponse("fetchAppInboxItem", ExponeaSDK.Result.success(expectedMessage))
            exponeaMock.registerResponse("markAppInboxAsRead", true)
            var result: MethodResult?
            BloomreachSdkIOS.instance.invokeMethodAsync(method: "MarkAppInboxAsRead", params: "abcd") { it in
                result = it
            }
            exponeaMock.verifyMethodCalled("markAppInboxAsRead")
            guard let result = result else {
                fail("MethodResult is NIL")
                return
            }
            expect(result.success).to(beTrue())
            let capturedMessageId = exponeaMock.getCapturedParams("fetchAppInboxItem")[0] as! String
            expect(capturedMessageId).to(equal("abcd"))
            let capturedMessage = exponeaMock.getCapturedParams("markAppInboxAsRead")[0] as! MessageItem
            expect(capturedMessage.id).to(equal(expectedMessage.id))
            expect(capturedMessage.type).to(equal(expectedMessage.type))
            expect(capturedMessage.read).to(equal(expectedMessage.read))
            expect(capturedMessage.receivedTime).to(equal(expectedMessage.receivedTime))
            expect(capturedMessage.rawContent?.count).to(equal(2))
            expect(capturedMessage.rawContent).to(equal(expectedMessage.rawContent))
        }

    }

    func buildAppInboxMessage(_ messageType: String, _ id: String = "12345") -> MessageItem {
        return MessageItem(
            id: id,
            type: messageType,
            read: true,
            rawReceivedTime: 10.0,
            rawContent: [
                "prop1": "val1".jsonValue,
                "prop2": 2.0.jsonValue
            ]
        )
    }
}
