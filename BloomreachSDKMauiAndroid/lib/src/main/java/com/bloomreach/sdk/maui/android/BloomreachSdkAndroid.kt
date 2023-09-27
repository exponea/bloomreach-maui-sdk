package com.bloomreach.sdk.maui.android

import android.content.Context
import anonymize
import com.bloomreach.sdk.maui.android.exception.BloomreachUnsupportedException
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseAsMap
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseBoolean
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseDouble
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseLong
import com.bloomreach.sdk.maui.android.util.SerializeUtils.serializeData
import com.bloomreach.sdk.maui.android.util.returnOnException
import com.exponea.sdk.Exponea
import com.exponea.sdk.util.logOnException
import configure
import flushData
import getCheckPushSetup
import getCustomerCookie
import getDefaultProperties
import getFlushMode
import getFlushPeriod
import getLogLevel
import getSessionTimeout
import getTokenTrackFrequency
import identifyCustomer
import isAutoPushNotification
import isAutomaticSessionTracking
import isConfigured
import setAutomaticSessionTracking
import setCheckPushSetup
import setDefaultProperties
import setFlushMode
import setFlushPeriod
import setLogLevel
import setSessionTimeout
import trackEvent
import trackPaymentEvent
import trackSessionEnd
import trackSessionStart


class BloomreachSdkAndroid(
    private val context: Context
) {

    companion object

    @Suppress("IMPLICIT_CAST_TO_ANY", "unused")
    fun invokeMethod(method: String?, params: String?): MethodResult = runCatching {
        val methodResult = when (method) {
            "Anonymize" -> this.anonymize(parseAsMap(params))
            "SetCheckPushSetup" -> this.setCheckPushSetup(parseBoolean(params))
            "GetCheckPushSetup" -> this.getCheckPushSetup()
            "Configure" -> this.configure(context, parseAsMap(params))
            "GetCustomerCookie" -> this.getCustomerCookie()
            "GetDefaultProperties" -> this.getDefaultProperties()
            "SetDefaultProperties" -> this.setDefaultProperties(parseAsMap(params))
            "GetFlushMode" -> this.getFlushMode()
            "SetFlushMode" -> this.setFlushMode(params!!)
            "GetFlushPeriod" -> this.getFlushPeriod()
            "SetFlushPeriod" -> this.setFlushPeriod(parseLong(params))
            "IdentifyCustomer" -> this.identifyCustomer(parseAsMap(params))
            "IsAutomaticSessionTracking" -> this.isAutomaticSessionTracking()
            "SetAutomaticSessionTracking" -> this.setAutomaticSessionTracking(parseBoolean(params))
            "IsAutoPushNotification" -> this.isAutoPushNotification()
            "IsConfigured" -> this.isConfigured()
            "GetLogLevel" -> this.getLogLevel()
            "SetLogLevel" -> this.setLogLevel(params!!)
            "GetSessionTimeout" -> this.getSessionTimeout()
            "SetSessionTimeout" -> this.setSessionTimeout(parseDouble(params))
            "GetTokenTrackFrequency" -> this.getTokenTrackFrequency()
            "TrackPaymentEvent" -> this.trackPaymentEvent(parseAsMap(params))
            "TrackEvent" -> this.trackEvent(parseAsMap(params))
            "TrackSessionEnd" -> this.trackSessionEnd()
            "TrackSessionStart" -> this.trackSessionStart()
            "HandleRemoteMessage" -> this.handleRemoteMessage(context, parseAsMap(params))
            else -> {
                throw BloomreachUnsupportedException("Method $method is currently unsupported")
            }
        }
        return@runCatching MethodResult.success(serializeData(methodResult))
    }.returnOnException { t ->
        MethodResult.failure("Method $method failed: ${t.localizedMessage ?: t.javaClass.name}")
    }

    @Suppress("unused")
    fun invokeMethodAsync(method: String?, params: String?, done: (MethodResult) -> Unit) = runCatching {
        when (method) {
            "FlushData" -> this.flushData { done(MethodResult(it, it.toString(), "")) }
            else -> {
                throw BloomreachUnsupportedException("Method $method is currently unsupported")
            }
        }
    }.logOnException()

}
