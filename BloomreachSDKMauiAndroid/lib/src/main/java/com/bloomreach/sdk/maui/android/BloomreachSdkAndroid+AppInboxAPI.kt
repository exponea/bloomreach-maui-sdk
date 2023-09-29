import android.content.Context
import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.MethodResult
import com.bloomreach.sdk.maui.android.MethodResultForView
import com.bloomreach.sdk.maui.android.MethodResultForView.Companion
import com.bloomreach.sdk.maui.android.exception.BloomreachDataException
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.currentTimeSeconds
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.getNullSafelyMap
import com.bloomreach.sdk.maui.android.util.getRequired
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.AppInboxMessateType
import com.exponea.sdk.models.AppInboxMessateType.HTML
import com.exponea.sdk.models.AppInboxMessateType.PUSH
import com.exponea.sdk.models.AppInboxMessateType.UNKNOWN
import com.exponea.sdk.models.MessageItem
import com.exponea.sdk.models.MessageItemAction
import com.exponea.sdk.models.MessageItemAction.Type
import com.exponea.sdk.style.appinbox.StyledAppInboxProvider
import com.exponea.sdk.util.logOnException
import com.exponea.style.AppInboxStyleParser

internal fun BloomreachSdkAndroid.getAppInboxButton(context: Context): MethodResultForView {
    val button = Exponea.getAppInboxButton(context);
    return if (button == null) {
        MethodResultForView.failure("AppInbox button not available, see logs")
    } else {
        Companion.success(button)
    }
}

internal fun BloomreachSdkAndroid.setAppInboxProvider(styleMap: Map<String, Any?>) {
    val style = AppInboxStyleParser(styleMap).parse()
    Exponea.appInboxProvider = StyledAppInboxProvider(style)
}

internal fun BloomreachSdkAndroid.trackAppInboxClick(dataMap: Map<String, Any?>) {
    val (action, message) = parseAppInboxActionAndMessage(dataMap)
    Exponea.trackAppInboxClick(action, message)
}

internal fun BloomreachSdkAndroid.trackAppInboxClickWithoutTrackingConsent(
    dataMap: Map<String, Any?>
) {
    val (action, message) = parseAppInboxActionAndMessage(dataMap)
    Exponea.trackAppInboxClickWithoutTrackingConsent(action, message)
}

internal fun BloomreachSdkAndroid.trackAppInboxOpened(dataMap: Map<String, Any?>) {
    val message = parseAppInboxMessage(dataMap)
    Exponea.trackAppInboxOpened(message)
}

internal fun BloomreachSdkAndroid.trackAppInboxOpenedWithoutTrackingConsent(dataMap: Map<String, Any?>) {
    val message = parseAppInboxMessage(dataMap)
    Exponea.trackAppInboxOpenedWithoutTrackingConsent(message)
}

internal fun BloomreachSdkAndroid.fetchAppInbox(done: (MethodResult) -> Unit) {
    Exponea.fetchAppInbox { appInbox ->
        runCatching {
            if (appInbox == null) {
                done(MethodResult.failure("AppInbox messages load failed, see logs"))
            } else {
                val result = appInbox
                    .map { messageItem -> messageItem.toMap() }
                done(MethodResult.success(SerializeUtils.serializeData(result)))
            }
        }.logOnException()
    }
}

internal fun BloomreachSdkAndroid.fetchAppInboxItem(messageId: String?, done: (MethodResult) -> Unit) {
    messageId ?: throw BloomreachDataException.missingProperty("messageId")
    Exponea.fetchAppInboxItem(messageId) { appInboxMessage ->
        runCatching {
            if (appInboxMessage == null) {
                done(MethodResult.failure("AppInbox message not found, see logs"))
            } else {
                done(MethodResult.success(SerializeUtils.serializeData(appInboxMessage.toMap())))
            }
        }.logOnException()
    }
}

internal fun BloomreachSdkAndroid.markAppInboxAsRead(messageId: String?, done: (MethodResult) -> Unit) {
    messageId ?: throw BloomreachDataException.missingProperty("messageId")
    // we need load message to ensure fresh assignments
    Exponea.fetchAppInboxItem(messageId) { appInboxMessage ->
        runCatching {
            if (appInboxMessage == null) {
                done(MethodResult.failure("AppInbox message not found, see logs"))
                return@runCatching
            }
            Exponea.markAppInboxAsRead(appInboxMessage) { marked ->
                runCatching {
                    done(MethodResult.success(marked.toString()))
                }.logOnException()
            }
        }.logOnException()
    }
}

private fun parseAppInboxActionAndMessage(dataMap: Map<String, Any?>): Pair<MessageItemAction, MessageItem> {
    val actionPayload = dataMap.getNullSafelyMap<String>("action")
        ?: throw BloomreachDataException.missingProperty("action")
    val action = MessageItemAction().apply {
        this.type = parseAppInboxActionType(actionPayload.getRequired("type"))
            ?: throw BloomreachDataException.invalidValue(
                "action.type",
                actionPayload.getRequired("type")
            )
        this.url = actionPayload.getNullSafely("url")
        this.title = actionPayload.getNullSafely("title")
    }
    val messagePayload = dataMap.getNullSafelyMap<Any>("message")
        ?: throw BloomreachDataException.missingProperty("message")
    val message = parseAppInboxMessage(messagePayload)
    return Pair(action, message)
}

private fun MessageItem.toMap(): Map<String, Any> {
    return mapOf(
        "id" to this.id,
        "type" to toMauiValue(this.type),
        "isRead" to (this.read ?: false),
        "receivedTime" to (this.receivedTime ?: currentTimeSeconds()),
        "content" to (this.rawContent ?: emptyMap())
    )
}

private fun parseAppInboxMessage(source: Map<String, Any?>): MessageItem {
    return MessageItem(
        id = source.getRequired("id"),
        rawType = parseAppInboxMessageType(source.getRequired("type")),
        read = source.getNullSafely("isRead", false),
        receivedTime = source.getNullSafely("receivedTime"),
        rawContent = source.getNullSafelyMap("content")
    )
}

fun toMauiValue(source: AppInboxMessateType): String {
    return when (source) {
        PUSH -> "Push"
        HTML -> "Html"
        UNKNOWN -> "Unknown"
    }
}

private fun parseAppInboxMessageType(source: String): String {
    return when (source) {
        "Push" -> "push"
        "Html" -> "html"
        else -> "unknown"
    }
}

private fun parseAppInboxActionType(source: String): Type? {
    return when (source) {
        "App" -> Type.APP
        "Browser" -> Type.BROWSER
        "Deeplink" -> Type.DEEPLINK
        "NoAction" -> Type.NO_ACTION
        else -> null
    }
}
