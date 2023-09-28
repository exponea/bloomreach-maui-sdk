//
//  BloomreachInvokable.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK
import Foundation

public protocol BloomreachInvokable {
    var exponeaSDK: ExponeaType { get set }
    func parse(method: String?, params: String?) -> MethodResult
    func parseAsync(method: String?, params: String?, done: @escaping (MethodResult) -> Void)

    // MARK: - Base
    func anonymize(data: [String: Any]) -> MethodResult
    func configure(data: [String: Any]) -> MethodResult
    func flushData(completion: TypeBlock<MethodResult>?)
    func getConfiguration() -> MethodResult
    func getCustomerCookie() -> MethodResult
    func getCheckPushSetup() -> MethodResult
    func getDefaultProperties() -> MethodResult
    func getFlushMode() -> MethodResult
    func getFlushPeriod() -> MethodResult
    func setFlushPeriod(millis: Int64?) -> MethodResult
    func getLogLevel() -> MethodResult
    func getSessionTimeout() -> MethodResult
    func setSessionTimeout(millis: Int64?) -> MethodResult
    func getTokenTrackFrequency() -> MethodResult
    func identifyCustomer(data: [String: Any]) -> MethodResult
    func isAutomaticSessionTracking() -> MethodResult
    func isAutoPushNotification() -> MethodResult
    func isConfigured() -> MethodResult
    func isSafeMode() -> MethodResult
    func setAutomaticSessionTracking(value: Bool) -> MethodResult
    func setCheckPushSetup(value: Bool) -> MethodResult
    func setDefaultProperties(data: [String: Any]) -> MethodResult
    func setFlushMode(value: String?) -> MethodResult
    func setLogLevel(level: String?) -> MethodResult
    func setSafeMode(value: Bool) -> MethodResult

    // MARK: - Tracking
    func trackPaymentEvent(data: [String: Any], timestamp: Double?) -> MethodResult
    func trackEvent(data: [String: Any], timestamp: Double?) -> MethodResult
    func trackSessionEnd() -> MethodResult
    func trackSessionStart() -> MethodResult

    // MARK: - Notification
    func handlePushNotificationOpened(data: [String: Any]) -> MethodResult
    func handleCampaignIntent(url: String?) -> MethodResult
    func handleHmsPushToken() -> MethodResult
    func handlePushToken(token: String) -> MethodResult
    func isBloomreachNotification(userInfo: [String: Any]) -> MethodResult
    func trackClickedPush(data: [String: Any]) -> MethodResult
    func trackClickedPushWithoutTrackingConsent(data: [String: Any]) -> MethodResult
    func trackPushToken(token: String?) -> MethodResult
    func trackDeliveredPush(data: [String: Any]) -> MethodResult
    func trackDeliveredPushWithoutTrackingConsent(data: [String: Any]) -> MethodResult
    func trackHmsPushToken() -> MethodResult
    func setReceivedPushCallback(completion: TypeBlock<MethodResult>?)
    func setOpenedPushCallback(completion: TypeBlock<MethodResult>?)
    func requestPushAuthorization(completion: TypeBlock<MethodResult>?)

    // MAKR: - InApp
    func setInAppMessageActionCallback(data: [String: Any], completion: TypeBlock<MethodResult>?)
    func trackInAppMessageClick(data: [String: Any]) -> MethodResult
    func trackInAppMessageClickWithoutTrackingConsent(data: [String: Any]) -> MethodResult
    func trackInAppMessageClose(data: [String: Any]) -> MethodResult
    func trackInAppMessageCloseWithoutTrackingConsent(data: [String: Any]) -> MethodResult
}

public extension BloomreachInvokable {
    func parseAsync(method: String?, params: String?, done: @escaping TypeBlock<MethodResult>) {
        switch BloomreachAsyncMethodType(method: method, params: params, asyncBlock: done) {
        case let .flushData(data):
            flushData(completion: data)
        case .unsupported:
            break
        case let .setReceivedPushCallback(data):
            setReceivedPushCallback(completion: data)
        case let .setOpenedPushCallback(data):
            setOpenedPushCallback(completion: data)
        case let .requestPushAuthorization(data):
            requestPushAuthorization(completion: data)
        case let .setInAppMessageActionCallback(data, completion):
            setInAppMessageActionCallback(data: data, completion: completion)
        }
    }

    func parse(method: String?, params: String?) -> MethodResult {
        switch BloomreachMethodType(method: method, params: params) {
        case let .anonymize(data):
            return anonymize(data: data)
        case .getCustomerCookie:
            return getCustomerCookie()
        case .unsupported:
            return MethodResult.unsupportedMethod(method ?? "nil")
        case let .configure(data):
            return configure(data: data)
        case .getConfiguration:
            return getConfiguration()
        case .getCheckPushSetup:
            return getCheckPushSetup()
        case .getDefaultProperties:
            return getDefaultProperties()
        case .getFlushMode:
            return getFlushMode()
        case .getFlushPeriod:
            return getFlushPeriod()
        case let .setFlushPeriod(data):
            return setFlushPeriod(millis: data)
        case .getLogLevel:
            return getLogLevel()
        case .getSessionTimeout:
            return getSessionTimeout()
        case let .setSessionTimeout(data):
            return setSessionTimeout(millis: data)
        case .getTokenTrackFrequency:
            return getTokenTrackFrequency()
        case let .identifyCustomer(data):
            return identifyCustomer(data: data)
        case .isAutomaticSessionTracking:
            return isAutomaticSessionTracking()
        case .isAutoPushNotification:
            return isAutoPushNotification()
        case .isConfigured:
            return isConfigured()
        case .isSafeMode:
            return isSafeMode()
        case let .setAutomaticSessionTracking(data):
            return setAutomaticSessionTracking(value: data)
        case let .setCheckPushSetup(data):
            return setCheckPushSetup(value: data)
        case let .setDefaultProperties(data):
            return setDefaultProperties(data: data)
        case let .setFlushMode(data):
            return setFlushMode(value: data)
        case let .setLogLevel(data):
            return setLogLevel(level: data)
        case let .setSafeMode(data):
            return setSafeMode(value: data)
        case let .trackPaymentEvent(data):
            return trackPaymentEvent(data: data["payment"] as! [String: Any], timestamp: .assertValueFromDict(data: data, key: "timestamp"))
        case let .trackEvent(data):
            return trackEvent(data: data["event"] as! [String: Any], timestamp: .assertValueFromDict(data: data, key: "timestamp"))
        case .trackSessionEnd:
            return trackSessionEnd()
        case .trackSessionStart:
            return trackSessionStart()
        case let .handlePushNotificationOpened(data):
            return handlePushNotificationOpened(data: data)
        case let .handleCampaignIntent(data):
            return handleCampaignIntent(url: data)
        case .handleHmsPushToken:
            return handleHmsPushToken()
        case let .handlePushToken(data):
            return handlePushToken(token: data ?? "")
        case let .isBloomreachNotification(data):
            return isBloomreachNotification(userInfo: data)
        case let .trackClickedPush(data):
            return trackClickedPush(data: data)
        case let .trackClickedPushWithoutTrackingConsent(data):
            return trackClickedPushWithoutTrackingConsent(data: data)
        case let .trackPushToken(data):
            return trackPushToken(token: data)
        case let .trackDeliveredPush(data):
            return trackDeliveredPush(data: data)
        case let .trackDeliveredPushWithoutTrackingConsent(data):
            return trackDeliveredPushWithoutTrackingConsent(data: data)
        case .trackHmsPushToken:
            return trackHmsPushToken()
        case .handleRemoteMessageTimeWillExpire:
            return handleRemoteMessageTimeWillExpire()
        case let .trackInAppMessageClick(data):
            return trackInAppMessageClick(data: data)
        case let .trackInAppMessageClickWithoutTrackingConsent(data):
            return trackInAppMessageClickWithoutTrackingConsent(data: data)
        case let .trackInAppMessageClose(data):
            return trackInAppMessageClose(data: data)
        case let .trackInAppMessageCloseWithoutTrackingConsent(data):
            return trackInAppMessageCloseWithoutTrackingConsent(data: data)
        default:
            return .unsupportedMethod(method ?? "Uknown method")
        }
    }
}

// MARK: - Base
public extension BloomreachInvokable {
    func anonymize(data: [String: Any]) -> MethodResult {
        if let projectDic = data["project"] as? [String: Any],
           let mappingsDic = data["projectMapping"] as? [String: Any],
           let project = parseExponeaProject(projectDic, defaultBaseUrl: exponeaSDK.configuration?.baseUrl ?? Constants.Repository.baseUrl) {
            let mappings = parseProjectMappings(mappingsDic)
            exponeaSDK.anonymize(
                exponeaProject: project,
                projectMapping: mappings
            )
        } else {
            exponeaSDK.anonymize()
        }
        return MethodResult.success("Anonymized")
    }

    private func parseExponeaProject(_ source: [String: Any], defaultBaseUrl: String) -> ExponeaProject? {
        guard let projectToken = source["projectToken"] as? String,
              let authorizationString = source["authorization"] as? String else {
            return nil
        }
        let baseUrl = source["baseUrl"] as? String ?? defaultBaseUrl
        return ExponeaProject(baseUrl: baseUrl, projectToken: projectToken, authorization: .token(authorizationString))
    }

    private func parseProjectMappings(_ source: [String: Any]) -> [EventType: [ExponeaProject]] {
        let defaultBaseUrl = exponeaSDK.configuration?.baseUrl ?? Constants.Repository.baseUrl
        var mapping: [EventType: [ExponeaProject]]  = [:]
        for (key, value) in source {
            let eventType: EventType
            switch key {
            case "install":
                eventType = EventType.install
            case "session_start":
                eventType = EventType.sessionStart
            case "session_end":
                eventType = EventType.sessionEnd
            case "track_event":
                eventType = EventType.customEvent
            case "track_customer":
                eventType = EventType.identifyCustomer
            case "payment":
                eventType = EventType.payment
            case "push_token":
                eventType = EventType.registerPushToken
            case "push_delivered":
                eventType = EventType.pushDelivered
            case "push_opened":
                eventType = EventType.pushOpened
            case "campaign_click":
                eventType = EventType.campaignClick
            case "banner":
                eventType = EventType.banner
            default:
                eventType = EventType.customEvent
            }
            guard let projectArray: [Any] = value as? [Any] else {
                continue
            }
            var exponeaProjects: [ExponeaProject] = []
            for item in projectArray {
                guard let projectDic = item as? [String: Any],
                      let project = parseExponeaProject(projectDic, defaultBaseUrl: defaultBaseUrl) else {
                    continue
                }
                exponeaProjects.append(project)
            }
            mapping[eventType] = exponeaProjects
        }
        return mapping
    }

    func configure(data: [String: Any]) -> MethodResult {
        let configuration: Configuration = .init(data: data)
        do {
            try (exponeaSDK as! ExponeaInternal).configure(
                with: .init(
                    projectToken: configuration.projectToken,
                    projectMapping: nil,
                    authorization: .token(configuration.token),
                    baseUrl: configuration.baseUrl,
                    defaultProperties: nil,
                    inAppContentBlocksPlaceholders: nil,
                    sessionTimeout: configuration.sessionTimeout,
                    automaticSessionTracking: configuration.automaticSessionTracking,
                    automaticPushNotificationTracking: configuration.automaticPushNotificationTracking,
                    requirePushAuthorization: configuration.requirePushAuthorization,
                    tokenTrackFrequency: configuration.tokenTrackFrequency,
                    appGroup: configuration.appGroup,
                    flushEventMaxRetries: configuration.flushEventMaxRetries,
                    allowDefaultCustomerProperties: configuration.allowDefaultCustomerProperties,
                    advancedAuthEnabled: configuration.advancedAuthEnabled
                )
            )
            return .success("Configuration done")
        } catch let error {
            return .failure("Configuration error \(error)")
        }
    }

    func flushData(completion: TypeBlock<MethodResult>?) {
        exponeaSDK.flushData { result in
            switch result {
            case .success:
                completion?(.success("true"))
            case .flushAlreadyInProgress:
                completion?(.failure("¨Flush already in progress"))
            case .noInternetConnection:
                completion?(.failure("No internet connection"))
            case let .error(error):
                completion?(.failure("Flush error \(error.localizedDescription)"))
            }
        }
    }

    func getConfiguration() -> MethodResult {
        guard let conf = exponeaSDK.configuration, let data = try? JSONEncoder().encode(conf) else {
            return .failure("Cant read configuration")
        }
        return .success(data.base64EncodedString())
    }

    func getCustomerCookie() -> MethodResult {
        if let customerCookie = exponeaSDK.customerCookie {
            return .success(customerCookie)
        }
        return .failure("customerCookie is nil")
    }

    func getCheckPushSetup() -> MethodResult {
        .success(exponeaSDK.checkPushSetup ? "true" : "false")
    }

    func getDefaultProperties() -> MethodResult {
        guard let properties = exponeaSDK.defaultProperties,
              let jsonProps = try? JSONSerialization.data(withJSONObject: properties, options: []),
              let stringProps = String(data: jsonProps, encoding: .utf8) else {
            return .failure("Cant get properties")
        }
        return .success(stringProps)
    }

    func getFlushMode() -> MethodResult {
        let flushMode: String
        switch exponeaSDK.flushingMode {
        case .automatic:
            flushMode = "app_close"
        case .immediate:
            flushMode = "immediate"
        case .manual:
            flushMode = "manual"
        case .periodic:
            flushMode = "period"
        }
        return .success(flushMode)
    }

    func getFlushPeriod() -> MethodResult {
        let period: String
        switch exponeaSDK.flushingMode {
        case let .periodic(value):
            period = "\(value * 1000)"
        default:
            return .failure("Unknown flush period")
        }
        return .success(period)
    }

    func setFlushPeriod(millis: Int64?) -> MethodResult {
        guard let millis = millis else {
            return .failure("Flush period not set")
        }
        exponeaSDK.flushingMode = .periodic(Int(exactly: millis / 1000) ?? 0)
        return .success("Flush period set successfuly")
    }

    func getTokenTrackFrequency() -> MethodResult {
        guard let configuration = exponeaSDK.configuration else {
            return .failure("Token track frequency is unknown")
        }
        let tokenFrequencyString: String
        switch configuration.tokenTrackFrequency {
        case .daily:
            tokenFrequencyString = "daily"
        case .everyLaunch:
            tokenFrequencyString = "every_launch"
        case .onTokenChange:
            tokenFrequencyString = "on_token_change"
        }
        return .success(tokenFrequencyString)
    }

    func getSessionTimeout() -> MethodResult {
        guard let conf = exponeaSDK.configuration else {
            return .failure("Session timeout is unknown")
        }
        return .success("\(Int(conf.sessionTimeout * 1000))")
    }

    func setSessionTimeout(millis: Int64?) -> MethodResult {
        guard let millis = millis else {
            return .failure("Flush period not set")
        }
        (exponeaSDK as! ExponeaInternal).setAutomaticSessionTracking(automaticSessionTracking: .enabled(timeout: Double(millis / 1000)))
        return .success("Flush period set successfuly")
    }

    func getLogLevel() -> MethodResult {
        let logLevelString: String
        switch Exponea.logger.logLevel {
        case .error:
            logLevelString = "error"
        case .verbose:
            logLevelString = "verbose"
        case .none:
            logLevelString = "off"
        case .warning:
            logLevelString = "warn"
        }
        return .success(logLevelString)
    }

    @discardableResult
    func identifyCustomer(data: [String: Any]) -> MethodResult {
        let customer: Customer = .init(data: data)
        exponeaSDK.identifyCustomer(
            customerIds: customer.customerIds,
            properties: customer.properties,
            timestamp: customer.timestamp
        )
        return .success("Identify customer done")
    }

    func isAutomaticSessionTracking() -> MethodResult {
        .success(exponeaSDK.configuration?.automaticSessionTracking == true ? "true" : "false")
    }

    func isAutoPushNotification() -> MethodResult {
        .success(exponeaSDK.configuration?.automaticPushNotificationTracking == true ? "true" : "false")
    }

    func isConfigured() -> MethodResult {
        .success(exponeaSDK.isConfigured ? "true" : "false")
    }

    func isSafeMode() -> MethodResult {
        .success(exponeaSDK.safeModeEnabled ? "true" : "false")
    }

    func setAutomaticSessionTracking(value: Bool) -> MethodResult {
        (exponeaSDK as! ExponeaInternal).setAutomaticSessionTracking(automaticSessionTracking: value ? .enabled() : .disabled)
        return .success("Automatic session set")
    }

    func setCheckPushSetup(value: Bool) -> MethodResult {
        exponeaSDK.checkPushSetup = value
        return .success("CheckPush has been set to \(value)")
    }

    func setDefaultProperties(data: [String: Any]) -> MethodResult {
        exponeaSDK.defaultProperties = JsonDataParser.parse(dictionary: data)
        return .success("Default properties set")
    }

    func setFlushMode(value: String?) -> MethodResult {
        guard let value = value else {
            return .failure("Cant set flush mode")
        }
        switch true {
        case value.contains("app_close"):
            exponeaSDK.flushingMode = .automatic
        case value.contains("immediate"):
            exponeaSDK.flushingMode = .immediate
        case value.contains("manual"):
            exponeaSDK.flushingMode = .manual
        case value.contains("period"):
            // default flush period = 5 min
            exponeaSDK.flushingMode = .periodic(5 * 60)
        default:
            assertionFailure()
            return .failure("Cant set flush mode")
        }
        return .success("Flush mode has been set")
    }

    func setLogLevel(level: String?) -> MethodResult {
        guard let level = level else {
            return .failure("Log level has not been set")
        }
        switch level {
        case "unknown", "off":
            Exponea.logger.logLevel = .none
        case "error":
            Exponea.logger.logLevel = .error
        case "warning":
            Exponea.logger.logLevel = .warning
        case "info", "debug", "verbose":
            Exponea.logger.logLevel = .verbose
        default:
            return .failure("Log level has not been set")
        }
        return .success("Log level has been set")
    }

    func setSafeMode(value: Bool) -> MethodResult {
        exponeaSDK.safeModeEnabled = value
        return .success("Safe mode has been set to \(value)")
    }
}

// MARK: - Tracking
public extension BloomreachInvokable {
    func trackPaymentEvent(data: [String: Any], timestamp: Double?) -> MethodResult {
        exponeaSDK.trackPayment(properties: JsonDataParser.parse(dictionary: data), timestamp: timestamp)
        return .success(nil)
    }

    func trackEvent(
        data: [String: Any],
        timestamp: Double?
    ) -> MethodResult {
        guard let attrsAny = data["attributes"] as? [String: Any] else {
            return .failure("Unable to parse Event properties")
        }
        exponeaSDK.trackEvent(
            properties: JsonDataParser.parse(dictionary: attrsAny),
            timestamp: timestamp,
            eventType: .assertValueFromDict(data: data, key: "name")
        )
        return .success(nil)
    }

    func trackSessionEnd() -> MethodResult {
        exponeaSDK.trackSessionEnd()
        return .success(nil)
    }

    func trackSessionStart() -> MethodResult {
        exponeaSDK.trackSessionStart()
        return .success(nil)
    }
}

// MARK: - Notification
public extension BloomreachInvokable {
    func handlePushNotificationOpened(data: [String: Any]) -> MethodResult {
        guard let userInfo = data["attributes"] as? [String: Any] else {
            return .failure("Push notification payload is missing")
        }
        let identifier = data["url"] as? String
        exponeaSDK.handlePushNotificationOpened(userInfo: userInfo, actionIdentifier: identifier)
        return .success(nil)
    }

    func handleCampaignIntent(url: String?) -> MethodResult {
        guard let url = URL(string: url ?? "") else {
            return .failure("Incorrect URL")
        }
        exponeaSDK.trackCampaignClick(url: url, timestamp: Date().timeIntervalSince1970)
        return .success(nil)
    }

    func handleHmsPushToken() -> MethodResult {
        .unsupportedMethod("unsupported method")
    }

    func handlePushToken(token: String) -> MethodResult {
        exponeaSDK.handlePushNotificationToken(token: token)
        return .success(nil)
    }

    func isBloomreachNotification(userInfo: [String: Any]) -> MethodResult {
        let isExponea = Exponea.isExponeaNotification(userInfo: userInfo)
        return .success(isExponea ? "true" : "false")
    }

    func handleRemoteMessageTimeWillExpire() -> MethodResult {
        guard let notifService = BloomreachSdkIOS.instance.notificationService else {
            return .failure("No previous Push notification handling")
        }
        notifService.serviceExtensionTimeWillExpire()
        BloomreachSdkIOS.instance.notificationService = nil
        return .success("true")
    }

    func trackClickedPush(data: [String: Any]) -> MethodResult {
        exponeaSDK.trackPushOpened(with: data)
        return .success(nil)
    }

    func trackClickedPushWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        exponeaSDK.trackPushOpenedWithoutTrackingConsent(with: data)
        return .success(nil)
    }

    func trackPushToken(token: String?) -> MethodResult {
        exponeaSDK.trackPushToken(token)
        return .success(nil)
    }

    func trackDeliveredPush(data: [String: Any]) -> MethodResult {
        exponeaSDK.trackPushReceived(userInfo: data)
        return .success(nil)
    }

    func trackDeliveredPushWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        exponeaSDK.trackPushReceivedWithoutTrackingConsent(userInfo: data)
        return .success(nil)
    }

    func trackHmsPushToken() -> MethodResult {
        .unsupportedMethod("unsupported method")
    }

    func setOpenedPushCallback(completion: TypeBlock<MethodResult>?) {
        exponeaSDK.pushNotificationsDelegate = PushNotificationManagerDelegateObject(completion: completion)
    }

    func setReceivedPushCallback(completion: TypeBlock<MethodResult>?) {
        exponeaSDK.pushNotificationsDelegate = PushNotificationManagerDelegateObject(completion: completion)
    }

    func requestPushAuthorization(completion: TypeBlock<MethodResult>?) {
        UNUserNotificationCenter.current().requestAuthorization(options: [.badge, .alert, .sound]) { (granted, error) in
            if let error {
                completion?(.failure(error.localizedDescription))
            } else if granted {
                DispatchQueue.main.async {
                    UIApplication.shared.registerForRemoteNotifications()
                }
                completion?(.success("true"))
            } else {
                completion?(.success("false"))
            }
        }
    }
}

// MARK: - InApp
public extension BloomreachInvokable {
    func setInAppMessageActionCallback(data: [String: Any], completion: TypeBlock<MethodResult>?) {
        exponeaSDK.inAppMessagesDelegate = InAppMessageActionDelegateObject(
            overrideDefaultBehavior: data["overrideDefaultBehavior"] as? Bool ?? false,
            trackActions: data["trackActions"] as? Bool ?? true,
            completion: completion
        )
    }

    func trackInAppMessageClick(data: [String: Any]) -> MethodResult {
        guard let messageData = data["message"] as? String else {
            return .failure("InApp message data are invalid")
        }
        exponeaSDK.trackInAppMessageClick(
            message: .init(
                data: messageData.json
            ),
            buttonText: data["buttonText"] as? String,
            buttonLink: data["buttonLink"] as? String
        )
        return .success(nil)
    }

    func trackInAppMessageClickWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        guard let messageData = data["message"] as? String else {
            return .failure("InApp message data are invalid")
        }
        exponeaSDK.trackInAppMessageClickWithoutTrackingConsent(
            message: .init(
                data: messageData.json
            ),
            buttonText: data["buttonText"] as? String,
            buttonLink: data["buttonLink"] as? String
        )
        return .success(nil)
    }

    func trackInAppMessageClose(data: [String: Any]) -> MethodResult {
        guard let messageData = data["message"] as? String else {
            return .failure("InApp message data are invalid")
        }
        exponeaSDK.trackInAppMessageClose(
            message: .init(
                data: messageData.json
            ),
            isUserInteraction: data["isUserInteraction"] as? Bool ?? false
        )
        return .success(nil)
    }

    func trackInAppMessageCloseWithoutTrackingConsent(data: [String: Any]) -> MethodResult {
        guard let messageData = data["message"] as? String else {
            return .failure("InApp message data are invalid")
        }
        exponeaSDK.trackInAppMessageCloseClickWithoutTrackingConsent(
            message: .init(
                data: messageData.json
            ),
            isUserInteraction: data["isUserInteraction"] as? Bool ?? false
        )
        return .success(nil)
    }
}

extension InAppMessage {
    init(data: [String: Any]) {
        self.init(
            id: data["id"] as? String ?? "",
            name: data["name"] as? String ?? "",
            rawMessageType: data["rawMessageType"] as? String,
            rawFrequency: data["rawFrequency"] as? String ?? "",
            variantId: data["variantId"] as? Int ?? 0,
            variantName: data["variantName"] as? String ?? "",
            trigger: .init(eventType: data["eventType"] as? String ?? "", filter: []),
            dateFilter: .init(enabled: false, startDate: nil, endDate: nil),
            payloadHtml: data["payloadHtml"] as? String,
            isHtml: data["isHtml"] as? Bool,
            hasTrackingConsent: data["hasTrackingConsent"] as? Bool,
            consentCategoryTracking: data["consentCategoryTracking"] as? String
        )
    }
}
