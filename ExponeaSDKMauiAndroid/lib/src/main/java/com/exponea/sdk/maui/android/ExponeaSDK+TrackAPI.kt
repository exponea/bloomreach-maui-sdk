import android.content.Context
import com.exponea.sdk.Exponea
import com.exponea.sdk.maui.android.ExponeaSDK
import com.exponea.sdk.maui.android.exception.ExponeaDataException
import com.exponea.sdk.maui.android.util.currentTimeSeconds
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
import com.exponea.sdk.models.PurchasedItem
import com.exponea.sdk.util.Logger
import java.util.concurrent.TimeUnit

internal fun ExponeaSDK.trackPaymentEvent(params: Map<String, Any?>) {
    val timestamp = params.getNullSafely("timestamp") ?: currentTimeSeconds()
    val paymentData = params.getNullSafelyMap<Any>("payment")
    if (paymentData.isNullOrEmpty()) {
        throw ExponeaDataException.missingProperty("payment")
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

internal fun ExponeaSDK.trackEvent(params: Map<String, Any?>) {
    val timestamp = params.getNullSafely("timestamp") ?: currentTimeSeconds()
    val eventData = params.getNullSafelyMap<Any>("event")
    if (eventData.isNullOrEmpty()) {
        throw ExponeaDataException.missingProperty("event")
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

internal fun ExponeaSDK.trackSessionEnd() {
    Exponea.trackSessionEnd()
}

internal fun ExponeaSDK.trackSessionStart() {
    Exponea.trackSessionStart()
}
