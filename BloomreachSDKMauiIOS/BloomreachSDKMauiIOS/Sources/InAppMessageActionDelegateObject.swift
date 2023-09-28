//
//  InAppMessageActionDelegateObject.swift
//  BloomreachSDKMauiIOS
//
//  Created by Michal Severín on 20.09.2023.
//

import ExponeaSDK

final class InAppMessageActionDelegateObject: InAppMessageActionDelegate {

    let overrideDefaultBehavior: Bool
    let trackActions: Bool
    let completion: TypeBlock<MethodResult>?

    init(overrideDefaultBehavior: Bool, trackActions: Bool, completion: TypeBlock<MethodResult>?) {
        self.overrideDefaultBehavior = overrideDefaultBehavior
        self.trackActions = trackActions
        self.completion = completion
    }

    func inAppMessageAction(with message: ExponeaSDK.InAppMessage, button: ExponeaSDK.InAppMessageButton?, interaction: Bool) {
        var data: [String: Any] = [:]
        var messageData: [String: Any] = [:]

        messageData["id"] = message.id
        messageData["name"] = message.name
        messageData["rawMessageType"] = message.rawMessageType
        messageData["rawFrequency"] = message.rawFrequency
        messageData["variantId"] = message.variantId
        messageData["variantName"] = message.variantName
        messageData["eventType"] = message.trigger.eventType
        messageData["priority"] = message.priority
        messageData["delayMS"] = message.delayMS
        messageData["timeoutMS"] = message.timeoutMS
        messageData["payloadHtml"] = message.payloadHtml
        messageData["isHtml"] = message.isHtml
        messageData["rawHasTrackingConsent"] = message.hasTrackingConsent
        messageData["consentCategoryTracking"] = message.consentCategoryTracking

        data["message"] = messageData.jsonData
        data["buttonText"] = button?.text
        data["buttonLink"] = button?.url
        data["isUserInteraction"] = interaction

        completion?(.success(data.jsonData))
    }
}
