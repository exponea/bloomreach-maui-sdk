import android.content.Context
import com.exponea.sdk.Exponea
import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.exception.BloomreachDataException
import com.bloomreach.sdk.maui.android.util.filterValueIsInstance
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.getNullSafelyArray
import com.bloomreach.sdk.maui.android.util.getNullSafelyMap
import com.bloomreach.sdk.maui.android.util.getRequired
import com.exponea.sdk.models.CustomerIds
import com.exponea.sdk.models.EventType
import com.exponea.sdk.models.ExponeaConfiguration
import com.exponea.sdk.models.ExponeaProject
import com.exponea.sdk.models.FlushMode
import com.exponea.sdk.models.FlushPeriod
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.util.Logger
import com.exponea.sdk.util.logOnException
import java.util.concurrent.TimeUnit

internal fun BloomreachSdkAndroid.anonymize(params: Map<String, Any?>) {
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
                throw BloomreachDataException(
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

internal fun BloomreachSdkAndroid.setCheckPushSetup(enabled: Boolean) {
    Exponea.checkPushSetup = enabled
}

internal fun BloomreachSdkAndroid.getCheckPushSetup(): Boolean {
    return Exponea.checkPushSetup
}

internal fun parseProjectRouteMap(
    map: Map<String, Any>,
    defaultBaseUrl: String
): Map<EventType, List<ExponeaProject>> {
    val mapping: HashMap<EventType, List<ExponeaProject>> = hashMapOf()
    for ((eventTypeString, projectListAny) in map) {
        try {
            val eventType = parseEventType(eventTypeString)
            val projectList = projectListAny as List<Map<String, Any?>>
            mapping[eventType] = projectList.map {
                parseExponeaProject(it, defaultBaseUrl)
            }
        } catch (e: Exception) {
            throw BloomreachDataException(
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
        else -> throw BloomreachDataException.invalidValue("EventType", eventTypeString)
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

internal fun BloomreachSdkAndroid.configure(context: Context, params: Map<String, Any?>) {
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
        else -> throw BloomreachDataException.invalidValue("TokenFrequency", source)
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
        else -> throw BloomreachDataException.invalidValue("HttpLoggingLevel", source)
    }
}

internal fun BloomreachSdkAndroid.getCustomerCookie(): String? {
    return Exponea.customerCookie
}

internal fun BloomreachSdkAndroid.getDefaultProperties(): HashMap<String, Any> {
    return Exponea.defaultProperties
}

internal fun BloomreachSdkAndroid.setDefaultProperties(params: Map<String, Any?>) {
    Exponea.defaultProperties = HashMap(params.filterValueIsInstance(Any::class.java))
}

internal fun BloomreachSdkAndroid.getFlushMode(): String {
    return when (Exponea.flushMode) {
        FlushMode.APP_CLOSE -> "app_close"
        FlushMode.PERIOD -> "period"
        FlushMode.MANUAL -> "manual"
        FlushMode.IMMEDIATE -> "immediate"
    }
}

internal fun BloomreachSdkAndroid.setFlushMode(source: String) {
    Exponea.flushMode = when (source) {
        "app_close" -> FlushMode.APP_CLOSE
        "period" -> FlushMode.PERIOD
        "manual" -> FlushMode.MANUAL
        "immediate" -> FlushMode.IMMEDIATE
        else -> throw BloomreachDataException.invalidValue("flushMode", source)
    }
}

internal fun BloomreachSdkAndroid.getFlushPeriod(): Long {
    return Exponea.flushPeriod.timeUnit.toMillis(Exponea.flushPeriod.amount)
}

internal fun BloomreachSdkAndroid.setFlushPeriod(millis: Long) {
    Exponea.flushPeriod = FlushPeriod(millis, TimeUnit.MILLISECONDS)
}

internal fun BloomreachSdkAndroid.identifyCustomer(params: Map<String, Any?>) {
    val customerIds = params.getNullSafelyMap<String>("customerIds") ?: emptyMap()
    val customerProperties = params.getNullSafelyMap<Any>("properties") ?: emptyMap()
    Exponea.identifyCustomer(
        customerIds = CustomerIds(HashMap(customerIds)),
        properties = PropertiesList(HashMap(customerProperties))
    )
}

internal fun BloomreachSdkAndroid.isAutomaticSessionTracking(): Boolean {
    return Exponea.isAutomaticSessionTracking
}

internal fun BloomreachSdkAndroid.setAutomaticSessionTracking(enabled: Boolean) {
    Exponea.isAutomaticSessionTracking = enabled
}

internal fun BloomreachSdkAndroid.isAutoPushNotification(): Boolean {
    return Exponea.isAutoPushNotification
}

internal fun BloomreachSdkAndroid.isConfigured(): Boolean {
    return Exponea.isInitialized
}

internal fun BloomreachSdkAndroid.getLogLevel(): String {
    return when (Exponea.loggerLevel) {
        Logger.Level.OFF -> "off"
        Logger.Level.ERROR -> "error"
        Logger.Level.WARN -> "warning"
        Logger.Level.INFO -> "info"
        Logger.Level.DEBUG -> "debug"
        Logger.Level.VERBOSE -> "verbose"
    }
}

internal fun BloomreachSdkAndroid.setLogLevel(params: String) {
    Exponea.loggerLevel = when (params) {
        "off" -> Logger.Level.OFF
        "error" -> Logger.Level.ERROR
        "warning" -> Logger.Level.WARN
        "info" -> Logger.Level.INFO
        "debug" -> Logger.Level.DEBUG
        "verbose" -> Logger.Level.VERBOSE
        else -> throw BloomreachDataException.invalidValue("logLevel", params)
    }
}

internal fun BloomreachSdkAndroid.getSessionTimeout(): Double {
    return Exponea.sessionTimeout
}

internal fun BloomreachSdkAndroid.setSessionTimeout(params: Double) {
    Exponea.sessionTimeout = params
}

internal fun BloomreachSdkAndroid.getTokenTrackFrequency(): String {
    return when (Exponea.tokenTrackFrequency) {
        ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE -> "token_change"
        ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH -> "every_launch"
        ExponeaConfiguration.TokenFrequency.DAILY -> "daily"
    }
}

internal fun BloomreachSdkAndroid.flushData(done: (Boolean) -> Unit) {
    Exponea.flushData {
        runCatching {
            done(it.isSuccess)
        }.logOnException()
    }
}