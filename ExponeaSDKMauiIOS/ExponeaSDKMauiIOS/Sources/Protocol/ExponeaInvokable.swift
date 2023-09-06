//
//  ExponeaInvokable.swift
//  ExponeaSDKMauiIOS
//
//  Created by Ankmara on 28.08.2023.
//

import ExponeaSDK
import Foundation

public protocol ExponeaInvokable {
    func parse(method: String?, params: String?) -> MethodResult
    func parseAsync(method: String?, params: String?, done: @escaping (MethodResult)->())
    
    // MARK: - Base
    func anonymize() -> MethodResult
    func configure(data: [String: Any]) -> MethodResult
    func flushData(completion: TypeBlock<MethodResult>?)
    func getConfiguration() -> MethodResult
    func getCustomerCookie() -> MethodResult
    func getCheckPushSetup() -> MethodResult
    func getDefaultProperties() -> MethodResult
    func getFlushMode() -> MethodResult
    func getFlushPeriod() -> MethodResult
    func getLogLevel() -> MethodResult
    func getSessionTimeout() -> MethodResult
    func getTokenTrackFrequency() -> MethodResult
    func identifyCustomer(data: [String: Any]) -> MethodResult
    func isAutomaticSessionTracking() -> MethodResult
    func isAutoPushNotification() -> MethodResult
    func isConfigured() -> MethodResult
    func isSafeMode() -> MethodResult
    func setAutomaticSessionTracking(value: Bool, timeout: Double) -> MethodResult
    func setCheckPushSetup(value: Bool) -> MethodResult
    func setDefaultProperties(data: [String: JSONConvertible]) -> MethodResult
    func setFlushMode(value: String) -> MethodResult
    func setLogLevel(level: Int) -> MethodResult
    func setSafeMode(value: Bool) -> MethodResult
    
    // MARK: - Tracking
    func trackPaymentEvent(data: [String: JSONConvertible], timestamp: Double?) -> MethodResult
    func trackEvent(data: [String: JSONConvertible], timestamp: Double?, eventyType: String?) -> MethodResult
    func trackSessionEnd() -> MethodResult
    func trackSessionStart() -> MethodResult
}

public extension ExponeaInvokable {
    func parseAsync(method: String?, params: String?, done: @escaping TypeBlock<MethodResult>) {
        switch ExponeaAsyncMethodType(method: method, params: params, asyncBlock: done) {
        case let .flushData(data):
            flushData(completion: data)
        case .unsupported:
            break
        }
    }

    func parse(method: String?, params: String?) -> MethodResult {
        switch ExponeaMethodType(method: method, params: params) {
        case .anonymize:
            return anonymize()
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
        case .getLogLevel:
            return getLogLevel()
        case .getSessionTimeout:
            return getSessionTimeout()
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
            return setAutomaticSessionTracking(value: .assertValueFromDict(data: data, key: "value"), timeout: .assertValueFromDict(data: data, key: "timeout"))
        case let .setCheckPushSetup(data):
            return setCheckPushSetup(value: .assertValueFromDict(data: data, key: "value"))
        case let .setDefaultProperties(data):
            return setDefaultProperties(data: data as! [String: JSONConvertible])
        case let .setFlushMode(data):
            return setFlushMode(value: .assertValueFromDict(data: data, key: "value"))
        case let .setLogLevel(data):
            return setLogLevel(level: .assertValueFromDict(data: data, key: "level"))
        case let .setSafeMode(data):
            return setSafeMode(value: .assertValueFromDict(data: data, key: "value"))
        case let .trackPaymentEvent(data):
            return trackPaymentEvent(data: data["data"] as! [String: JSONConvertible], timestamp: .assertValueFromDict(data: data, key: "timestamp"))
        case let .trackEvent(data):
            return trackEvent(data: data["data"] as! [String: JSONConvertible], timestamp: .assertValueFromDict(data: data, key: "timestamp"), eventyType: .assertValueFromDict(data: data, key: "eventType"))
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
public extension ExponeaInvokable {
    func anonymize() -> MethodResult {
        Exponea.shared.anonymize()
        return MethodResult.success("Anonymized")
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
                completion?(.success("\(value)"))
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
        guard let properties = Exponea.shared.defaultProperties else {
            return .failure("Cant get properties")
        }
        let data = try? NSKeyedArchiver.archivedData(withRootObject: properties, requiringSecureCoding: false)
        return .success(data?.base64EncodedString())
    }
    
    func getFlushMode() -> MethodResult {
        let flushMode: String
        switch Exponea.shared.flushingMode {
        case .automatic:
            flushMode = "automatic"
        case .immediate:
            flushMode = "immediate"
        case .manual:
            flushMode = "manual"
        default:
            flushMode = ""
            assertionFailure()
        }
        return .success(flushMode)
    }
    
    func getFlushPeriod() -> MethodResult {
        let period: String
        switch Exponea.shared.flushingMode {
        case let .periodic(value):
            period = "\(value)"
        default:
            return .failure("Unknown flush period")
        }
        return .success(period)
    }
    
    func getTokenTrackFrequency() -> MethodResult {
        .success(Exponea.shared.configuration?.tokenTrackFrequency.rawValue)
    }
    
    func getSessionTimeout() -> MethodResult {
        .success("\(Exponea.shared.configuration?.sessionTimeout ?? 0)")
    }
    
    func getLogLevel() -> MethodResult {
        .success(Exponea.logger.logLevel.name)
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

    func setAutomaticSessionTracking(value: Bool, timeout: Double) -> MethodResult {
        Exponea.shared.setAutomaticSessionTracking(automaticSessionTracking: value ? .enabled(timeout: timeout) : .disabled)
        return .success("Automatic session set")
    }

    func setCheckPushSetup(value: Bool) -> MethodResult {
        Exponea.shared.checkPushSetup = value
        return .success("CheckPush has been set to \(value)")
    }
    
    func setDefaultProperties(data: [String: JSONConvertible]) -> MethodResult {
        Exponea.shared.defaultProperties = data
        return .success("Default properties set")
    }
    
    func setFlushMode(value: String) -> MethodResult {
        switch true {
        case value.contains("automatic"):
            Exponea.shared.flushingMode = .automatic
        case value.contains("immediate"):
            Exponea.shared.flushingMode = .immediate
        case value.contains("manual"):
            Exponea.shared.flushingMode = .manual
        case value.contains("periodic"):
            Exponea.shared.flushingMode = .periodic(Int(value.components(separatedBy: ":").last ?? "0") ?? 0)
        default:
            assertionFailure()
            return .failure("Cant set flush mode")
        }
        return .success("Flush mode has been set")
    }

    /// Produce a greeting string for the given `subject`.
    ///
    /// ```
    /// setLogLevel(level: 1) // .error
    /// ```
    ///
    /// - Parameters:
    ///     - 0: none
    ///     - 1: error
    ///     - 2: warning
    ///     - 3: verbose
    func setLogLevel(level: Int) -> MethodResult {
        Exponea.logger.logLevel = .init(rawValue: level) ?? .none
        return .success("Log level has been set")
    }
    
    func setSafeMode(value: Bool) -> MethodResult {
        Exponea.shared.safeModeEnabled = value
        return .success("Safe mode has been set to \(value)")
    }
}

// MARK: - Tracking
public extension ExponeaInvokable {
    func trackPaymentEvent(data: [String: JSONConvertible], timestamp: Double?) -> MethodResult {
        Exponea.shared.trackPayment(properties: data, timestamp: timestamp)
        return .success(nil)
    }
    
    func trackEvent(
        data: [String: JSONConvertible],
        timestamp: Double?,
        eventyType: String?
    ) -> MethodResult {
        Exponea.shared.trackEvent(properties: data, timestamp: timestamp, eventType: eventyType)
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
