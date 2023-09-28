package com.bloomreach.sdk.maui.android.util

import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.exception.BloomreachException
import com.exponea.sdk.models.DateFilter
import com.exponea.sdk.models.InAppMessage
import com.exponea.sdk.models.InAppMessagePayload
import com.exponea.sdk.models.eventfilter.EventFilter
import com.exponea.sdk.util.Logger
import java.util.Date
import kotlin.reflect.KClass
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.Job
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch

fun <T> Result<T>.returnOnException(mapThrowable: (e: Throwable) -> T): T {
    return this.getOrElse {
        try {
            Logger.e(BloomreachSdkAndroid, "Safe Mode wrapper caught unhandled error", it)
        } catch (e: Throwable) {
            // cannot log problem, swallowing
        }
        // TODO: Arch for Exponea.safeModeEnabled
        return mapThrowable(it)
    }
}

internal inline fun <reified T : Any> Map<String, Any?>.getRequired(key: String): T {
    return getSafely(key, T::class)
}

internal fun <T : Any> Map<String, Any?>.getSafely(key: String, type: KClass<T>): T {
    val value = this[key] ?: throw BloomreachException.common("Property '$key' cannot be null.")
    if (value::class == type) {
        @Suppress("UNCHECKED_CAST")
        return value as T
    } else {
        throw BloomreachException.common(
            "Incorrect type for key '$key'. Expected ${type.simpleName} got ${value::class.simpleName}"
        )
    }
}

internal inline fun <reified T : Any> Map<String, Any?>.getNullSafelyMap(key: String, defaultValue: Map<String, T>? = null): Map<String, T>? {
    return getNullSafelyMap(key, T::class, defaultValue)
}

internal inline fun <reified T : Any> Map<String, Any?>.getNullSafelyMap(key: String, type: KClass<T>, defaultValue: Map<String, T>? = null): Map<String, T>? {
    val value = this[key] ?: return defaultValue
    @Suppress("UNCHECKED_CAST")
    val mapOfAny = value as? Map<String, Any?> ?: throw BloomreachException.common(
        "Non-map type for key '$key'. Got ${value::class.simpleName}"
    )
    return mapOfAny.filterValueIsInstance(type.java)
}

/**
 * Returns a map containing all key-value pairs with values are instances of specified class.
 *
 * The returned map preserves the entry iteration order of the original map.
 */
internal fun <K, V, R> Map<out K, V>.filterValueIsInstance(klass: Class<R>): Map<K, R> {
    val result = LinkedHashMap<K, R>()
    for (entry in this) {
        if (klass.isInstance(entry.value)) {
            @Suppress("UNCHECKED_CAST")
            ((entry.value as R).also { result[entry.key] = it })
        }
    }
    return result
}

internal inline fun <reified T : Any> Map<String, Any?>.getNullSafelyArray(key: String, defaultValue: List<T>? = null): List<T>? {
    return getNullSafelyArray(key, T::class, defaultValue)
}

internal inline fun <reified T : Any> Map<String, Any?>.getNullSafelyArray(key: String, type: KClass<T>, defaultValue: List<T>? = null): List<T>? {
    val value = this[key] ?: return defaultValue
    val arrayOfAny = value as? List<Any?> ?: throw BloomreachException.common(
        "Non-array type for key '$key'. Got ${value::class.simpleName}"
    )
    return arrayOfAny
        .filterIsInstance(type.java)
}

internal inline fun <reified T : Any> Map<String, Any?>.getNullSafely(key: String, defaultValue: T? = null): T? {
    return getNullSafely(key, T::class, defaultValue)
}

internal fun <T : Any> Map<String, Any?>.getNullSafely(key: String, type: KClass<T>, defaultValue: T? = null): T? {
    val value = this[key] ?: return defaultValue
    @Suppress("UNCHECKED_CAST")
    return (value as? T) ?: throw BloomreachException.common(
        "Incorrect type for key '$key'. Expected ${type.simpleName} got ${value::class.simpleName}"
    )
}

internal fun currentTimeSeconds(): Double {
    return Date().time / 1000.0
}

val InAppMessageParser: InAppMessage? = null
internal fun InAppMessage?.parseFrom(source: Map<String, Any?>): InAppMessage = InAppMessage(
    source.getRequired("id"),
    source.getRequired("name"),
    source.getNullSafely("rawMessageType", "modal"),
    source.getRequired("rawFrequency"),
    null,
    source.getRequired<Double>("variantId").toInt(),
    source.getRequired("variantName"),
    EventFilter(source.getRequired("eventType"), listOf()),
    DateFilter(false),
    source.getNullSafely<Double>("priority")?.toInt(),
    source.getNullSafely<Double>("delayMS")?.toLong(),
    source.getNullSafely<Double>("timeoutMS")?.toLong(),
    source.getNullSafely("payloadHtml"),
    source.getNullSafely("isHtml"),
    source.getNullSafely("rawHasTrackingConsent"),
    source.getNullSafely("consentCategoryTracking")
)
internal fun InAppMessage.toMap(): Map<String, Any> {
    val target = mutableMapOf<String, Any>()
    target["id"] = this.id
    target["name"] = this.name
    target["rawMessageType"] = this.messageType.value
    this.frequency?.value?.let {
        target["rawFrequency"] = it
    }
    target["variantId"] = this.variantId
    target["variantName"] = this.variantName
    this.trigger?.eventType?.let {
        target["eventType"] = it
    }
    this.priority?.let {
        target["priority"] = it
    }
    this.delay?.let {
        target["delayMS"] = it
    }
    this.timeout?.let {
        target["timeoutMS"] = it
    }
    this.payloadHtml?.let {
        target["payloadHtml"] = it
    }
    this.isHtml?.let {
        target["isHtml"] = it
    }
    target["rawHasTrackingConsent"] = this.hasTrackingConsent
    this.consentCategoryTracking?.let {
        target["consentCategoryTracking"] = it
    }
    return target
}

internal var mainThreadDispatcher = CoroutineScope(Dispatchers.Main)
internal var backgroundThreadDispatcher = CoroutineScope(Dispatchers.Default)

internal inline fun runOnMainThread(crossinline block: () -> Unit): Job {
    return mainThreadDispatcher.launch {
        block.invoke()
    }
}

internal inline fun runOnMainThread(delayMillis: Long, crossinline block: () -> Unit): Job {
    return mainThreadDispatcher.launch {
        try {
            delay(delayMillis)
        } catch (e: Exception) {
            Logger.w(this, "Delayed task has been cancelled: ${e.localizedMessage}")
            return@launch
        }
        block.invoke()
    }
}

internal inline fun runOnBackgroundThread(crossinline block: () -> Unit): Job {
    return backgroundThreadDispatcher.launch {
        block.invoke()
    }
}

internal inline fun runOnBackgroundThread(delayMillis: Long, crossinline block: () -> Unit): Job {
    return backgroundThreadDispatcher.launch {
        try {
            delay(delayMillis)
        } catch (e: Exception) {
            Logger.w(this, "Delayed task has been cancelled: ${e.localizedMessage}")
            return@launch
        }
        block.invoke()
    }
}