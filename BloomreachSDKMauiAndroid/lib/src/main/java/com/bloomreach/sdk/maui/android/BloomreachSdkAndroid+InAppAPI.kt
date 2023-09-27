import android.content.Context
import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.MethodResult
import com.bloomreach.sdk.maui.android.exception.BloomreachDataException
import com.bloomreach.sdk.maui.android.util.InAppMessageParser
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.parseFrom
import com.bloomreach.sdk.maui.android.util.toMap
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.InAppMessage
import com.exponea.sdk.models.InAppMessageButton
import com.exponea.sdk.models.InAppMessageCallback
import com.exponea.sdk.util.logOnException

internal fun BloomreachSdkAndroid.trackInAppMessageClick(params: InAppMessageAction?) {
    val messageJson = params?.message ?: throw BloomreachDataException.missingProperty("message")
    val message = InAppMessageParser.parseFrom(SerializeUtils.parseAsMap(messageJson))
    val buttonText = params.buttonText
    val buttonLink = params.buttonLink
    Exponea.trackInAppMessageClick(
        message, buttonText, buttonLink
    )
}

internal fun BloomreachSdkAndroid.trackInAppMessageClickWithoutTrackingConsent(params: InAppMessageAction?) {
    val messageJson = params?.message ?: throw BloomreachDataException.missingProperty("message")
    val message = InAppMessageParser.parseFrom(SerializeUtils.parseAsMap(messageJson))
    val buttonText = params.buttonText
    val buttonLink = params.buttonLink
    Exponea.trackInAppMessageClickWithoutTrackingConsent(
        message, buttonText, buttonLink
    )
}

internal fun BloomreachSdkAndroid.trackInAppMessageClose(params: InAppMessageAction?) {
    val messageJson = params?.message ?: throw BloomreachDataException.missingProperty("message")
    val message = InAppMessageParser.parseFrom(SerializeUtils.parseAsMap(messageJson))
    val interaction = params.isUserInteraction
    Exponea.trackInAppMessageClose(
        message, interaction
    )
}

internal fun BloomreachSdkAndroid.trackInAppMessageCloseWithoutTrackingConsent(params: InAppMessageAction?) {
    val messageJson = params?.message ?: throw BloomreachDataException.missingProperty("message")
    val message = InAppMessageParser.parseFrom(SerializeUtils.parseAsMap(messageJson))
    val interaction = params.isUserInteraction
    Exponea.trackInAppMessageCloseWithoutTrackingConsent(
        message, interaction
    )
}

internal fun BloomreachSdkAndroid.setInAppMessageActionCallback(params: Map<String, Any?>, callback: (MethodResult) -> Unit) {
    val overrideDefaultBehaviorFlag = params.getNullSafely("overrideDefaultBehavior", false)!!
    val trackActionsFlag = params.getNullSafely("trackActions", true)!!
    Exponea.inAppMessageActionCallback = object : InAppMessageCallback {
        override val overrideDefaultBehavior: Boolean
            get() = overrideDefaultBehaviorFlag
        override var trackActions: Boolean
            get() = trackActionsFlag
            set(_) {}

        override fun inAppMessageAction(
            message: InAppMessage,
            button: InAppMessageButton?,
            interaction: Boolean,
            context: Context
        ) {
            runCatching {
                val action = InAppMessageAction().apply {
                    this.message = SerializeUtils.serializeData(message.toMap())
                    this.buttonLink = button?.url
                    this.buttonText = button?.text
                    this.isUserInteraction = interaction
                }
                callback.invoke(MethodResult.success(SerializeUtils.serializeData(action)))
            }.logOnException()
        }
    }
}

/**
 * Simple class to represents JSON structure for some InApp API methods
 */
internal class InAppMessageAction {
    var message: String? = null
    var buttonText: String? = null
    var buttonLink: String? = null
    var isUserInteraction: Boolean? = null
}