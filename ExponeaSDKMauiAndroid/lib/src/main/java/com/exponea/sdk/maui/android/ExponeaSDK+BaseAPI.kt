import android.content.Context
import com.exponea.sdk.Exponea
import com.exponea.sdk.maui.android.ExponeaSDK
import com.exponea.sdk.maui.android.exception.ExponeaDataException
import com.exponea.sdk.maui.android.util.filterValueIsInstance
import com.exponea.sdk.maui.android.util.getNullSafely
import com.exponea.sdk.maui.android.util.getNullSafelyArray
import com.exponea.sdk.maui.android.util.getNullSafelyMap
import com.exponea.sdk.maui.android.util.getRequired
import com.exponea.sdk.models.CustomerIds
import com.exponea.sdk.models.EventType
import com.exponea.sdk.models.ExponeaConfiguration
import com.exponea.sdk.models.ExponeaProject
import com.exponea.sdk.models.FlushMode
import com.exponea.sdk.models.FlushPeriod
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.util.Logger
import java.util.concurrent.TimeUnit

internal fun ExponeaSDK.anonymize(params: Map<String, Any?>) {
    //TODO: default baseUrl should be get from Configuration, is not visible yet
    val defaultBaseUrl = "https://api.exponea.com"
    val project = params.getNullSafelyMap<Any>("project")?.let {
        parseExponeaProject(it, defaultBaseUrl)
    }
    val projectMappings = params.getNullSafelyMap<Any>("projectMapping")?.let {
        val mapping: HashMap<EventType, List<ExponeaProject>> = hashMapOf()
        it.forEach { (eventTypeString, projectListAny) ->
            try {
                val eventType = parseEventType(eventTypeString)
                val projectList = projectListAny as List<Map<String, Any?>>
                mapping[eventType] = projectList.map {
                    parseExponeaProject(it, project?.baseUrl ?: defaultBaseUrl)
                }
            } catch (e: Exception) {
                throw ExponeaDataException(
                    "Invalid project definition for event type $eventTypeString",
                    e
                )
            }
        }
        mapping
    }
    Exponea.anonymize(
        exponeaProject = project,
        projectRouteMap = projectMappings
    )
}

internal fun ExponeaSDK.setCheckPushSetup(enabled: Boolean) {
    Exponea.checkPushSetup = enabled
}

internal fun ExponeaSDK.getCheckPushSetup(): Boolean {
    return Exponea.checkPushSetup
}

internal fun parseProjectRouteMap(
    map: Map<String, Any>,
    defaultBaseUrl: String
): Map<EventType, List<ExponeaProject>> {
    val mapping: HashMap<EventType, List<ExponeaProject>> = hashMapOf()
    map.forEach { (eventTypeString, projectListAny) ->
        try {
            val eventType = parseEventType(eventTypeString)
            val projectList = projectListAny as List<Map<String, Any?>>
            mapping[eventType] = projectList.map {
                parseExponeaProject(it, defaultBaseUrl)
            }
        } catch (e: Exception) {
            throw ExponeaDataException(
                "Invalid project definition for event type $eventTypeString",
                e
            )
        }
    }
    return mapping
}

private fun parseEventType(eventTypeString: String): EventType {
    return when (eventTypeString) {
        "install" -> EventType.INSTALL
        "session_start" -> EventType.SESSION_START
        "session_end" -> EventType.SESSION_END
        "track_event" -> EventType.TRACK_EVENT
        "track_customer" -> EventType.TRACK_CUSTOMER
        "payment" -> EventType.PAYMENT
        "push_token" -> EventType.PUSH_TOKEN
        "push_delivered" -> EventType.PUSH_DELIVERED
        "push_opened" -> EventType.PUSH_OPENED
        "campaign_click" -> EventType.CAMPAIGN_CLICK
        "banner" -> EventType.BANNER
        else -> throw ExponeaDataException.invalidValue("EventType", eventTypeString)
    }
}

private fun parseExponeaProject(
    it: Map<String, Any?>,
    defaultBaseUrl: String
) = ExponeaProject(
    baseUrl = it.getNullSafely("baseUrl") ?: defaultBaseUrl,
    projectToken = it.getRequired("projectToken"),
    authorization = it.getNullSafely("authorization"),
    inAppContentBlockPlaceholdersAutoLoad = it.getNullSafelyArray("inAppContentBlockPlaceholdersAutoLoad")
        ?: emptyList()
)

internal fun ExponeaSDK.configure(context: Context, params: Map<String, Any?>) {
    val configuration = ExponeaConfiguration().apply {
        projectToken = params.getRequired("projectToken")
        params.getNullSafely<String>("baseUrl")?.let {
            baseURL = it
        }
        params.getNullSafelyMap<Any>("projectRouteMap")?.let {
            projectRouteMap = parseProjectRouteMap(it, baseURL)
        }
        params.getNullSafely<String>("authorization")?.let {
            authorization = "Token $it"
        }
        params.getNullSafely<String>("httpLoggingLevel")?.let {
            httpLoggingLevel = parseHttpLoggingLevel(it)
        }
        params.getNullSafely<Double>("maxTries")?.let {
            maxTries = it.toInt()
        }
        params.getNullSafely<Double>("sessionTimeout")?.let {
            sessionTimeout = it
        }
        params.getNullSafely<Double>("campaignTTL")?.let {
            campaignTTL = it
        }
        params.getNullSafely<Boolean>("automaticSessionTracking")?.let {
            automaticSessionTracking = it
        }
        params.getNullSafely<Boolean>("automaticPushNotification")?.let {
            automaticPushNotification = it
        }
        params.getNullSafely<String>("pushIcon")?.let {
            pushIcon = findDrawableId(context, it)
        }
        params.getNullSafely<Double>("pushAccentColor")?.let {
            pushAccentColor = it.toInt()
        }
        params.getNullSafely<String>("pushChannelName")?.let {
            pushChannelName = it
        }
        params.getNullSafely<String>("pushChannelDescription")?.let {
            pushChannelDescription = it
        }
        params.getNullSafely<String>("pushChannelId")?.let {
            pushChannelId = it
        }
        params.getNullSafely<Double>("pushNotificationImportance")?.let {
            pushNotificationImportance = it.toInt()
        }
        params.getNullSafelyMap<Any>("defaultProperties")?.let {
            defaultProperties = HashMap(it)
        }
        params.getNullSafely<String>("tokenTrackFrequency")?.let {
            tokenTrackFrequency = parseTokenTrackFrequency(it)
        }
        params.getNullSafely<Boolean>("allowDefaultCustomerProperties")?.let {
            allowDefaultCustomerProperties = it
        }
        params.getNullSafely<Boolean>("advancedAuthEnabled")?.let {
            advancedAuthEnabled = it
        }
        params.getNullSafelyArray<String>("inAppContentBlockPlaceholdersAutoLoad")?.let {
            inAppContentBlockPlaceholdersAutoLoad = it
        }
    }
    Exponea.init(context = context, configuration = configuration)
}

fun parseTokenTrackFrequency(source: String): ExponeaConfiguration.TokenFrequency {
    return when (source) {
        "token_change" -> ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE
        "daily" -> ExponeaConfiguration.TokenFrequency.DAILY
        "every_launch" -> ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH
        else -> throw ExponeaDataException.invalidValue("TokenFrequency", source)
    }
}

fun findDrawableId(context: Context, drawableName: String): Int {
    return context.resources.getIdentifier(drawableName, "drawable", context.packageName)
}

fun parseHttpLoggingLevel(source: String): ExponeaConfiguration.HttpLoggingLevel {
    return when (source) {
        "body" -> ExponeaConfiguration.HttpLoggingLevel.BODY
        "headers" -> ExponeaConfiguration.HttpLoggingLevel.HEADERS
        "basic" -> ExponeaConfiguration.HttpLoggingLevel.BASIC
        "none" -> ExponeaConfiguration.HttpLoggingLevel.NONE
        else -> throw ExponeaDataException.invalidValue("HttpLoggingLevel", source)
    }
}

internal fun ExponeaSDK.getCustomerCookie(): String? {
    return Exponea.customerCookie
}

internal fun ExponeaSDK.getDefaultProperties(): HashMap<String, Any> {
    return Exponea.defaultProperties
}

internal fun ExponeaSDK.setDefaultProperties(params: Map<String, Any?>) {
    Exponea.defaultProperties = HashMap(params.filterValueIsInstance(Any::class.java))
}

internal fun ExponeaSDK.getFlushMode(): String {
    return when (Exponea.flushMode) {
        FlushMode.APP_CLOSE -> "app_close"
        FlushMode.PERIOD -> "period"
        FlushMode.MANUAL -> "manual"
        FlushMode.IMMEDIATE -> "immediate"
    }
}

internal fun ExponeaSDK.getFlushPeriod(): Long {
    return Exponea.flushPeriod.timeUnit.toMillis(Exponea.flushPeriod.amount)
}

internal fun ExponeaSDK.setFlushPeriod(millis: Long) {
    Exponea.flushPeriod = FlushPeriod(millis, TimeUnit.MILLISECONDS)
}

internal fun ExponeaSDK.identifyCustomer(params: Map<String, Any?>) {
    val customerIds = params.getNullSafelyMap<String>("customerIds") ?: emptyMap()
    val customerProperties = params.getNullSafelyMap<Any>("properties") ?: emptyMap()
    Exponea.identifyCustomer(
        customerIds = CustomerIds(HashMap(customerIds)),
        properties = PropertiesList(HashMap(customerProperties))
    )
}

internal fun ExponeaSDK.isAutomaticSessionTracking(): Boolean {
    return Exponea.isAutomaticSessionTracking
}

internal fun ExponeaSDK.setAutomaticSessionTracking(enabled: Boolean) {
    Exponea.isAutomaticSessionTracking = enabled
}

internal fun ExponeaSDK.isAutoPushNotification(): Boolean {
    return Exponea.isAutoPushNotification
}

internal fun ExponeaSDK.isConfigured(): Boolean {
    return Exponea.isInitialized
}

internal fun ExponeaSDK.getLogLevel(): String {
    return when (Exponea.loggerLevel) {
        Logger.Level.OFF -> "off"
        Logger.Level.ERROR -> "error"
        Logger.Level.WARN -> "warning"
        Logger.Level.INFO -> "info"
        Logger.Level.DEBUG -> "debug"
        Logger.Level.VERBOSE -> "verbose"
    }
}

internal fun ExponeaSDK.setLogLevel(params: String) {
    Exponea.loggerLevel = when (params) {
        "off" -> Logger.Level.OFF
        "error" -> Logger.Level.ERROR
        "warning" -> Logger.Level.WARN
        "info" -> Logger.Level.INFO
        "debug" -> Logger.Level.DEBUG
        "verbose" -> Logger.Level.VERBOSE
        else -> throw ExponeaDataException.invalidValue("logLevel", params)
    }
}

internal fun ExponeaSDK.getSessionTimeout(): Double {
    return Exponea.sessionTimeout
}

internal fun ExponeaSDK.setSessionTimeout(params: Double) {
    Exponea.sessionTimeout = params
}

internal fun ExponeaSDK.getTokenTrackFrequency(): String {
    return when (Exponea.tokenTrackFrequency) {
        ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE -> "token_change"
        ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH -> "every_launch"
        ExponeaConfiguration.TokenFrequency.DAILY -> "daily"
    }
}

internal fun ExponeaSDK.flushData(done: (Boolean) -> Unit) {
    Exponea.flushData {
        done(it.isSuccess)
    }
}