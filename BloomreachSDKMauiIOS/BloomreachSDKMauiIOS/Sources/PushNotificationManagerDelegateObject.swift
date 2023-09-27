//
//  PushNotificationManagerDelegateObject.swift
//  BloomreachSDKMauiIOS
//
//  Created by Michal Severín on 18.09.2023.
//

import ExponeaSDK

final class PushNotificationManagerDelegateObject: PushNotificationManagerDelegate {

    let completion: TypeBlock<MethodResult>?
    
    init(completion: TypeBlock<MethodResult>? = nil) {
        self.completion = completion
    }
    
    func pushNotificationOpened(with action: ExponeaSDK.ExponeaNotificationActionType, value: String?, extraData: [AnyHashable : Any]?) {
        var data: [String: Any] = [:]
        data["action"] = action.rawValue
        data["value"] = value
        data["data"] = extraData
        completion?(.success(data.jsonData))
    }
    
    func silentPushNotificationReceived(extraData: [AnyHashable : Any]?) {
        completion?(.success(extraData?.jsonData))
    }
}
