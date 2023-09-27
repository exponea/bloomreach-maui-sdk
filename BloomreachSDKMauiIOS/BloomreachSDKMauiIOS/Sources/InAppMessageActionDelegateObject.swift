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
        var payloadMessageData: [String: Any] = [:]
        var buttonData: [String: Any] = [:]
        var buttonArray: [[String: Any]] = []
        var dateFilterData: [String: Any] = [:]
        
        dateFilterData["enabled"] = message.dateFilter.enabled
        dateFilterData["startDate"] = message.dateFilter.startDate
        dateFilterData["endDate"] = message.dateFilter.endDate

        if let buttons = message.payload?.buttons {
            for button in buttons {
                var newButton: [String: Any] = [:]
                newButton["buttonText"] = button.buttonText
                newButton["rawButtonType"] = button.rawButtonType
                newButton["buttonType"] = button.buttonType.rawValue
                newButton["buttonLink"] = button.buttonLink
                newButton["buttonTextColor"] = button.buttonTextColor
                newButton["buttonBackgroundColor"] = button.buttonBackgroundColor
                buttonArray.append(newButton)
            }
        }
        buttonData["data"] = buttonArray
        
        payloadMessageData["imageUrl"] = message.payload?.imageUrl
        payloadMessageData["title"] = message.payload?.title
        payloadMessageData["titleTextColor"] = message.payload?.titleTextColor
        payloadMessageData["titleTextSize"] = message.payload?.titleTextSize
        payloadMessageData["bodyText"] = message.payload?.bodyText
        payloadMessageData["bodyTextColor"] = message.payload?.bodyTextColor
        payloadMessageData["bodyTextSize"] = message.payload?.bodyTextSize
        payloadMessageData["backgroundColor"] = message.payload?.backgroundColor
        payloadMessageData["closeButtonColor"] = message.payload?.closeButtonColor
        payloadMessageData["messagePosition"] = message.payload?.messagePosition
        payloadMessageData["textPosition"] = message.payload?.textPosition
        payloadMessageData["textOverImage"] = message.payload?.textOverImage
        payloadMessageData["id"] = message.payload?.imageUrl

        messageData["id"] = message.id
        messageData["rawMessageType"] = message.rawMessageType
        messageData["rawFrequency"] = message.rawFrequency
        messageData["payload"] = payloadMessageData
        messageData["variantId"] = message.variantId
        messageData["variantName"] = message.variantName
        messageData["trigger"] = message.trigger.eventType
        messageData["dateFilter"] = dateFilterData
        messageData["priority"] = message.priority
        messageData["delayMS"] = message.delayMS
        messageData["timeoutMS"] = message.timeoutMS
        messageData["payloadHtml"] = message.payloadHtml
        messageData["isHtml"] = message.isHtml
        messageData["hasTrackingConsent"] = message.hasTrackingConsent
        messageData["consentCategoryTracking"] = message.consentCategoryTracking

        data["payload"] = payloadMessageData
        data["message"] = messageData
        data["buttons"] = buttonData
        data["filter"] = dateFilterData
        data["interaction"] = interaction
        
        completion?(.success(data.jsonData))
    }
}
