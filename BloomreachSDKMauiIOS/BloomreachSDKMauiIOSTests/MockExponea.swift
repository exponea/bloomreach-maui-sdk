//
//  MockExponea.swift
//  BloomreachSDKMauiIOSTests
//
//  Created by Michal Severín on 20.09.2023.
//

import ExponeaSDK
import Foundation

class MockExponea: ExponeaType {

    var inAppContentBlocksManager: ExponeaSDK.InAppContentBlocksManagerType?

    func configure(
        _ projectSettings: ExponeaSDK.Exponea.ProjectSettings,
        pushNotificationTracking: ExponeaSDK.Exponea.PushNotificationTracking,
        automaticSessionTracking: ExponeaSDK.Exponea.AutomaticSessionTracking,
        defaultProperties: [String: ExponeaSDK.JSONConvertible]?,
        inAppContentBlocksPlaceholders: [String]?,
        flushingSetup: ExponeaSDK.Exponea.FlushingSetup,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) { }

    func configure(
        projectToken: String,
        authorization: ExponeaSDK.Authorization,
        baseUrl: String?, appGroup: String?,
        defaultProperties: [String: ExponeaSDK.JSONConvertible]?,
        inAppContentBlocksPlaceholders: [String]?,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) { }

    func configure(
        projectToken: String,
        projectMapping: [ExponeaSDK.EventType: [ExponeaSDK.ExponeaProject]],
        authorization: ExponeaSDK.Authorization,
        baseUrl: String?,
        appGroup: String?,
        defaultProperties: [String: ExponeaSDK.JSONConvertible]?,
        inAppContentBlocksPlaceholders: [String]?,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) { }

    var appInboxProvider: ExponeaSDK.AppInboxProvider = DefaultAppInboxProvider()

    init() {}

    struct Call {
        let name: String
        let params: [Any?]
    }

    var calls: [Call] = []

    var isConfiguredValue: Bool = false
    var isConfigured: Bool {
        get {
            calls.append(Call(name: "isConfigured:get", params: []))
            return isConfiguredValue
        }
        set {
            calls.append(Call(name: "isConfigured:set", params: [newValue]))
            isConfiguredValue = newValue
        }
    }

    var configurationValue: ExponeaSDK.Configuration?
    var configuration: ExponeaSDK.Configuration? {
        get {
            calls.append(Call(name: "configuration:get", params: []))
            return configurationValue
        }
        set {
            calls.append(Call(name: "configuration:set", params: [newValue]))
            configurationValue = newValue
        }
    }

    var customerCookieValue: String?
    var customerCookie: String? {
        get {
            calls.append(Call(name: "customerCookie:get", params: []))
            return customerCookieValue
        }
        set {
            calls.append(Call(name: "customerCookie:set", params: [newValue]))
            customerCookieValue = newValue
        }
    }

    var flushingModeValue: FlushingMode = .automatic
    var flushingMode: FlushingMode {
        get {
            calls.append(Call(name: "flushingMode:get", params: []))
            return flushingModeValue
        }
        set {
            calls.append(Call(name: "flushingMode:set", params: [newValue]))
            flushingModeValue = newValue
        }
    }

    var pushNotificationsDelegateValue: PushNotificationManagerDelegate?
    var pushNotificationsDelegate: PushNotificationManagerDelegate? {
        get {
            calls.append(Call(name: "pushNotificationsDelegate:get", params: []))
            return pushNotificationsDelegateValue
        }
        set {
            calls.append(Call(name: "pushNotificationsDelegate:set", params: [newValue]))
            pushNotificationsDelegateValue = newValue
        }
    }

    var inAppMessagesDelegateValue: InAppMessageActionDelegate = TestDefaultInAppDelegate()

    var inAppMessagesDelegate: InAppMessageActionDelegate {
        get {
            calls.append(Call(name: "inAppMessagesDelegate:get", params: []))
            return inAppMessagesDelegateValue
        }
        set {
            calls.append(Call(name: "inAppMessagesDelegate:set", params: [newValue]))
            inAppMessagesDelegateValue = newValue
        }
    }

    var safeModeEnabledValue: Bool = false
    var safeModeEnabled: Bool {
        get { safeModeEnabledValue }
        set { safeModeEnabledValue = newValue }
    }

    var checkPushSetupValue: Bool = false
    var checkPushSetup: Bool {
        get {
            calls.append(Call(name: "checkPushSetup:get", params: []))
            return checkPushSetupValue
        }
        set {
            calls.append(Call(name: "checkPushSetup:set", params: [newValue]))
            checkPushSetupValue = newValue
        }
    }

    var defaultPropertiesValue: [String: JSONConvertible]?
    var defaultProperties: [String: JSONConvertible]? {
        get {
            calls.append(Call(name: "defaultProperties:get", params: []))
            return defaultPropertiesValue
        }
        set {
            calls.append(Call(name: "defaultProperties:set", params: [newValue]))
            defaultPropertiesValue = newValue
        }
    }

    func configure(
        _ projectSettings: Exponea.ProjectSettings,
        pushNotificationTracking: Exponea.PushNotificationTracking,
        automaticSessionTracking: Exponea.AutomaticSessionTracking,
        defaultProperties: [String: JSONConvertible]?,
        flushingSetup: Exponea.FlushingSetup,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) {
        calls.append(
            Call(
                name: "configure",
                params: [
                    pushNotificationTracking,
                    automaticSessionTracking,
                    defaultProperties,
                    flushingSetup,
                    allowDefaultCustomerProperties,
                    advancedAuthEnabled
                ]
            )
        )
    }

    func configure(
        projectToken: String,
        authorization: Authorization,
        baseUrl: String?,
        appGroup: String?,
        defaultProperties: [String: JSONConvertible]?,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) {
        fatalError("Not implemented")
    }

    func configure(
        projectToken: String,
        projectMapping: [EventType: [ExponeaProject]],
        authorization: Authorization,
        baseUrl: String?,
        appGroup: String?,
        defaultProperties: [String: JSONConvertible]?,
        allowDefaultCustomerProperties: Bool?,
        advancedAuthEnabled: Bool?
    ) {
        fatalError("Not implemented")
    }

    func configure(plistName: String) {
        fatalError("Not implemented")
    }

    func trackEvent(properties: [String: JSONConvertible], timestamp: Double?, eventType: String?) {
        calls.append(Call(name: "trackEvent", params: [properties, timestamp, eventType]))
    }

    func trackCampaignClick(url: URL, timestamp: Double?) {
        fatalError("Not implemented")
    }

    func trackPayment(properties: [String: JSONConvertible], timestamp: Double?) {
        fatalError("Not implemented")
    }

    func identifyCustomer(
        customerIds: [String: String]?,
        properties: [String: JSONConvertible],
        timestamp: Double?
    ) {
        calls.append(Call(name: "identifyCustomer", params: [customerIds, properties]))
    }

    func flushData() {
        calls.append(Call(name: "flushData", params: []))
    }

    func flushData(completion: ((FlushResult) -> Void)?) {
        fatalError("Not implemented")
    }

    func trackPushToken(_ token: Data) {
        fatalError("Not implemented")
    }

    func trackPushToken(_ token: String?) {
        fatalError("Not implemented")
    }

    func handlePushNotificationToken(deviceToken: Data) {
        fatalError("Not implemented")
    }

    func trackPushOpened(with userInfo: [AnyHashable: Any]) {
        fatalError("Not implemented")
    }

    func handlePushNotificationOpened(response: UNNotificationResponse) {
        fatalError("Not implemented")
    }

    func handlePushNotificationOpened(userInfo: [AnyHashable: Any], actionIdentifier: String?) {
        fatalError("Not implemented")
    }

    func trackSessionStart() {
        calls.append(Call(name: "trackSessionStart", params: []))
    }

    func trackSessionEnd() {
        calls.append(Call(name: "trackSessionEnd", params: []))
    }

    func fetchRecommendation<T>(
        with options: RecommendationOptions,
        completion: @escaping (Result<RecommendationResponse<T>>) -> Void
    ) where T: RecommendationUserData {
        calls.append(Call(name: "fetchRecommendation", params: [options, completion]))
    }

    func fetchConsents(completion: @escaping (Result<ConsentsResponse>) -> Void) {
        calls.append(Call(name: "fetchConsents", params: [completion]))
    }

    func anonymize() {
        calls.append(Call(name: "anonymize", params: []))
    }

    func anonymize(exponeaProject: ExponeaProject, projectMapping: [EventType: [ExponeaProject]]?) {
        calls.append(Call(name: "anonymize", params: [exponeaProject, projectMapping]))
    }

    func trackInAppMessageClick(message: InAppMessage, buttonText: String?, buttonLink: String?) {
        calls.append(Call(name: "trackInAppMessageClick", params: [message, buttonText, buttonLink]))
    }

    func trackInAppMessageClose(message: InAppMessage) {
        calls.append(Call(name: "trackInAppMessageClose", params: [message]))
    }

    func trackPushOpenedWithoutTrackingConsent(with userInfo: [AnyHashable: Any]) {
        fatalError("Not implemented")
    }

    func handlePushNotificationOpenedWithoutTrackingConsent(userInfo: [AnyHashable: Any], actionIdentifier: String?) {
        fatalError("Not implemented")
    }

    func trackInAppMessageClickWithoutTrackingConsent(
        message: ExponeaSDK.InAppMessage,
        buttonText: String?,
        buttonLink: String?
    ) {
        fatalError("Not implemented")
    }

    func trackInAppMessageCloseClickWithoutTrackingConsent(message: ExponeaSDK.InAppMessage) {
        fatalError("Not implemented")
    }

    func trackAppInboxOpened(message: ExponeaSDK.MessageItem) {
        fatalError("Not implemented")
    }

    func trackAppInboxOpenedWithoutTrackingConsent(message: ExponeaSDK.MessageItem) {
        fatalError("Not implemented")
    }

    func trackAppInboxClick(action: ExponeaSDK.MessageItemAction, message: ExponeaSDK.MessageItem) {
        fatalError("Not implemented")
    }

    func trackAppInboxClickWithoutTrackingConsent(
        action: ExponeaSDK.MessageItemAction,
        message: ExponeaSDK.MessageItem
    ) {
        fatalError("Not implemented")
    }

    func markAppInboxAsRead(_ message: ExponeaSDK.MessageItem, completition: ((Bool) -> Void)?) {
        fatalError("Not implemented")
    }

    func getAppInboxButton() -> UIButton {
        fatalError("Not implemented")
    }

    func getAppInboxListViewController() -> UIViewController {
        fatalError("Not implemented")
    }

    func getAppInboxDetailViewController(_ messageId: String) -> UIViewController {
        fatalError("Not implemented")
    }

    func fetchAppInbox(completion: @escaping (ExponeaSDK.Result<[ExponeaSDK.MessageItem]>) -> Void) {
        fatalError("Not implemented")
    }

    func fetchAppInboxItem(
        _ messageId: String,
        completion: @escaping (ExponeaSDK.Result<ExponeaSDK.MessageItem>) -> Void
    ) {
        fatalError("Not implemented")
    }

    func handlePushNotificationToken(token: String) {
        fatalError("Not implemented")
    }

    func trackPushReceived(content: UNNotificationContent) {
        fatalError("Not implemented")
    }

    func trackPushReceived(userInfo: [AnyHashable: Any]) {
        fatalError("Not implemented")
    }

    func trackPushReceivedWithoutTrackingConsent(content: UNNotificationContent) {
        fatalError("Not implemented")
    }

    func trackPushReceivedWithoutTrackingConsent(userInfo: [AnyHashable: Any]) {
        fatalError("Not implemented")
    }

    func trackInAppMessageCloseClickWithoutTrackingConsent(message: ExponeaSDK.InAppMessage, isUserInteraction: Bool?) {
        calls.append(Call(name: "trackInAppMessageCloseWithoutTrackingConsent", params: [message, isUserInteraction]))
    }

    func trackInAppMessageClose(message: ExponeaSDK.InAppMessage, isUserInteraction: Bool?) {
        calls.append(Call(name: "trackInAppMessageClose", params: [message, isUserInteraction]))
    }
}

class TestDefaultInAppDelegate: InAppMessageActionDelegate {
    public let overrideDefaultBehavior = false
    public let trackActions = true

    public func inAppMessageAction(with message: InAppMessage, button: InAppMessageButton?, interaction: Bool) {}
}
