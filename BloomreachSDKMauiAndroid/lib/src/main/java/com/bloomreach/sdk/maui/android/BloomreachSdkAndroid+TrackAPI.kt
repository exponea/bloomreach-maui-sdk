import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.exception.BloomreachDataException
import com.bloomreach.sdk.maui.android.util.currentTimeSeconds
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.getNullSafelyMap
import com.bloomreach.sdk.maui.android.util.getRequired
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.models.PurchasedItem

internal fun BloomreachSdkAndroid.trackPaymentEvent(params: Map<String, Any?>) {
    val timestamp = params.getNullSafely("timestamp") ?: currentTimeSeconds()
    val paymentData = params.getNullSafelyMap<Any>("payment")
    if (paymentData.isNullOrEmpty()) {
        throw BloomreachDataException.missingProperty("payment")
    }
    val purchasedItem = PurchasedItem(
        value = paymentData.getRequired("value"),
        currency = paymentData.getRequired("currency"),
        paymentSystem = paymentData.getNullSafely("system") ?: "",
        productId = paymentData.getNullSafely("productId") ?: "",
        productTitle = paymentData.getNullSafely("productTitle") ?: "",
        receipt = paymentData.getNullSafely("receipt")
    )
    Exponea.trackPaymentEvent(
        timestamp = timestamp, purchasedItem = purchasedItem
    )
}

internal fun BloomreachSdkAndroid.trackEvent(params: Map<String, Any?>) {
    val timestamp = params.getNullSafely("timestamp") ?: currentTimeSeconds()
    val eventData = params.getNullSafelyMap<Any>("event")
    if (eventData.isNullOrEmpty()) {
        throw BloomreachDataException.missingProperty("event")
    }
    val eventProps = eventData.getNullSafelyMap<Any>("attributes")
        ?.let { HashMap(it) }
        ?: hashMapOf()
    val eventType = eventData.getRequired<String>("name")
    Exponea.trackEvent(
        properties = PropertiesList(eventProps),
        timestamp = timestamp,
        eventType = eventType
    )
}

internal fun BloomreachSdkAndroid.trackSessionEnd() {
    Exponea.trackSessionEnd()
}

internal fun BloomreachSdkAndroid.trackSessionStart() {
    Exponea.trackSessionStart()
}
