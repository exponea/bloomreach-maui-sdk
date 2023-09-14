//
//  BloomreachInvokable.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK
import Foundation

public protocol BloomreachInvokable {
    func parse(method: String?, params: String?) -> MethodResult
    func parseAsync(method: String?, params: String?, done: @escaping (MethodResult)->())
    
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
}

public extension BloomreachInvokable {
    func parseAsync(method: String?, params: String?, done: @escaping TypeBlock<MethodResult>) {
        switch BloomreachAsyncMethodType(method: method, params: params, asyncBlock: done) {
        case let .flushData(data):
            flushData(completion: data)
        case .unsupported:
            break
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
           let project = parseExponeaProject(projectDic, defaultBaseUrl: Exponea.shared.configuration?.baseUrl ?? Constants.Repository.baseUrl) {
            let mappings = parseProjectMappings(mappingsDic)
            Exponea.shared.anonymize(
                exponeaProject: project,
                projectMapping: mappings
            )
        } else {
            Exponea.shared.anonymize()
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
        let defaultBaseUrl = Exponea.shared.configuration?.baseUrl ?? Constants.Repository.baseUrl
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
            try Exponea.shared.configure(
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
        }
        catch let error {
            return .failure("Configuration error \(error)")
        }
    }
    
    func flushData(completion: TypeBlock<MethodResult>?) {
        Exponea.shared.flushData { result in
            switch result {
            case let .success(value):
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
        guard let conf = Exponea.shared.configuration, let data = try? JSONEncoder().encode(conf) else {
            return .failure("Cant read configuration")
        }
        return .success(data.base64EncodedString())
    }
    
    func getCustomerCookie() -> MethodResult {
        if let customerCookie = Exponea.shared.customerCookie {
            return .success(customerCookie)
        }
        return .failure("customerCookie is nil")
    }
    
    func getCheckPushSetup() -> MethodResult {
        .success(Exponea.shared.checkPushSetup ? "true" : "false")
    }
    
    func getDefaultProperties() -> MethodResult {
        guard let properties = Exponea.shared.defaultProperties,
              let jsonProps = try? JSONSerialization.data(withJSONObject: properties, options: []),
              let stringProps = String(data: jsonProps, encoding: .utf8) else {
            return .failure("Cant get properties")
        }
        return .success(stringProps)
    }
    
    func getFlushMode() -> MethodResult {
        let flushMode: String
        switch Exponea.shared.flushingMode {
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
        switch Exponea.shared.flushingMode {
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
        Exponea.shared.flushingMode = .periodic(Int(exactly: millis / 1000) ?? 0)
        return .success("Flush period set successfuly")
    }

    func getTokenTrackFrequency() -> MethodResult {
        guard let configuration = Exponea.shared.configuration else {
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
        guard let conf = Exponea.shared.configuration else {
            return .failure("Session timeout is unknown")
        }
        return .success("\(Int(conf.sessionTimeout * 1000))")
    }

    func setSessionTimeout(millis: Int64?) -> MethodResult {
        guard let millis = millis else {
            return .failure("Flush period not set")
        }
        Exponea.shared.setAutomaticSessionTracking(automaticSessionTracking: .enabled(timeout: Double(millis / 1000)))
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
        Exponea.shared.identifyCustomer(
            customerIds: customer.customerIds,
            properties: customer.properties,
            timestamp: customer.timestamp
        )
        return .success("Identify customer done")
    }

    func isAutomaticSessionTracking() -> MethodResult {
        .success(Exponea.shared.configuration?.automaticSessionTracking == true ? "true" : "false")
    }
    
    func isAutoPushNotification() -> MethodResult {
        .success(Exponea.shared.configuration?.automaticPushNotificationTracking == true ? "true" : "false")
    }
    
    func isConfigured() -> MethodResult {
        .success(Exponea.shared.isConfigured ? "true" : "false")
    }
    
    func isSafeMode() -> MethodResult {
        .success(Exponea.shared.safeModeEnabled ? "true" : "false")
    }

    func setAutomaticSessionTracking(value: Bool) -> MethodResult {
        Exponea.shared.setAutomaticSessionTracking(automaticSessionTracking: value ? .enabled() : .disabled)
        return .success("Automatic session set")
    }

    func setCheckPushSetup(value: Bool) -> MethodResult {
        Exponea.shared.checkPushSetup = value
        return .success("CheckPush has been set to \(value)")
    }
    
    func setDefaultProperties(data: [String: Any]) -> MethodResult {
        Exponea.shared.defaultProperties = JsonDataParser.parse(dictionary: data)
        return .success("Default properties set")
    }
    
    func setFlushMode(value: String?) -> MethodResult {
        guard let value = value else {
            return .failure("Cant set flush mode")
        }
        switch true {
        case value.contains("app_close"):
            Exponea.shared.flushingMode = .automatic
        case value.contains("immediate"):
            Exponea.shared.flushingMode = .immediate
        case value.contains("manual"):
            Exponea.shared.flushingMode = .manual
        case value.contains("period"):
            // default flush period = 5 min
            Exponea.shared.flushingMode = .periodic(5 * 60)
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
        Exponea.shared.safeModeEnabled = value
        return .success("Safe mode has been set to \(value)")
    }
}

// MARK: - Tracking
public extension BloomreachInvokable {
    func trackPaymentEvent(data: [String: Any], timestamp: Double?) -> MethodResult {
        Exponea.shared.trackPayment(properties: JsonDataParser.parse(dictionary: data), timestamp: timestamp)
        return .success(nil)
    }
    
    func trackEvent(
        data: [String: Any],
        timestamp: Double?
    ) -> MethodResult {
        guard let attrsAny = data["attributes"] as? [String: Any] else {
            return .failure("Unable to parse Event properties")
        }
        Exponea.shared.trackEvent(
            properties: JsonDataParser.parse(dictionary: attrsAny),
            timestamp: timestamp,
            eventType: .assertValueFromDict(data: data, key: "name")
        )
        return .success(nil)
    }

    func trackSessionEnd() -> MethodResult {
        Exponea.shared.trackSessionEnd()
        return .success(nil)
    }
    
    func trackSessionStart() -> MethodResult {
        Exponea.shared.trackSessionStart()
        return .success(nil)
    }
}
