//
//  InAppMessagePresenterType.swift
//  ExponeaSDK
//
//  Created by Panaxeo on 05/12/2019.
//  Copyright © 2019 Exponea. All rights reserved.
//

import Foundation

protocol InAppMessagePresenterType {
    func presentInAppMessage(
        messageType: InAppMessageType,
        payload: InAppMessagePayload?,
        payloadHtml: String?,
        delay: TimeInterval,
        timeout: TimeInterval?,
        imageData: Data?,
        actionCallback: @escaping (InAppMessagePayloadButton) -> Void,
        dismissCallback: @escaping TypeBlock<Bool>,
        presentedCallback: ((InAppMessageView?, String?) -> Void)?
    )
}
