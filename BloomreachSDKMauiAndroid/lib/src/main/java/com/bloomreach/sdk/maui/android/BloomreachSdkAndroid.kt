package com.bloomreach.sdk.maui.android

import InAppMessageAction
import android.content.Context
import anonymize
import com.bloomreach.sdk.maui.android.MethodResult.Companion
import com.bloomreach.sdk.maui.android.exception.BloomreachUnsupportedException
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.SerializeUtils.deserializeData
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseAsMap
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseBoolean
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseDouble
import com.bloomreach.sdk.maui.android.util.SerializeUtils.parseLong
import com.bloomreach.sdk.maui.android.util.SerializeUtils.serializeData
import com.bloomreach.sdk.maui.android.util.returnOnException
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
import setInAppMessageActionCallback
import setLogLevel
import setSessionTimeout
import trackEvent
import trackInAppMessageClick
import trackInAppMessageClickWithoutTrackingConsent
import trackInAppMessageClose
import trackInAppMessageCloseWithoutTrackingConsent
import trackPaymentEvent
import trackSessionEnd
import trackSessionStart


class BloomreachSdkAndroid(
    private val context: Context
) {

    companion object {

        private var pendingOpenedPush: Map<String, Any>? = null
            set(value) {
                pushNotificationClickListener?.let { listener ->
                    value?.let { listener.invoke(it) }
                    field = null
                } ?: run {
                    field = value
                }
            }
        internal var pushNotificationClickListener: ((Map<String, Any>) -> Unit)? = null
            set(value) {
                field = value
                value?.let { newListener ->
                    pendingOpenedPush?.let {pendingData ->
                        newListener.invoke(pendingData)
                        pendingOpenedPush = null
                    }
                }
            }
        fun onPushNotificationClicked(
            action: String,
            url: String?,
            additionalData: Map<String, Any?>
        ) {

        }
    }

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
            "HandlePushNotificationOpened" -> this.handlePushNotificationOpened(parseAsMap(params))
            "HandlePushNotificationOpenedWithoutTrackingConsent" -> this.handlePushNotificationOpenedWithoutTrackingConsent(parseAsMap(params))
            "HandleCampaignClick" -> this.handleCampaignClick(context, params)
            "HandleHmsPushToken" -> this.handleHmsPushToken(context, params)
            "HandlePushToken" -> this.handlePushToken(context, params)
            "IsBloomreachNotification" -> this.isBloomreachNotification(parseAsMap(params))
            "TrackClickedPush" -> this.trackClickedPush(parseAsMap(params))
            "TrackClickedPushWithoutTrackingConsent" -> this.trackClickedPushWithoutTrackingConsent(parseAsMap(params))
            "TrackPushToken" -> this.trackPushToken(params)
            "TrackDeliveredPush" -> this.trackDeliveredPush(parseAsMap(params))
            "TrackDeliveredPushWithoutTrackingConsent" -> this.trackDeliveredPushWithoutTrackingConsent(parseAsMap(params))
            "TrackHmsPushToken" -> this.trackHmsPushToken(params)
            "TrackInAppMessageClick" ->
                this.trackInAppMessageClick(deserializeData(params))
            "TrackInAppMessageClickWithoutTrackingConsent" ->
                this.trackInAppMessageClickWithoutTrackingConsent(deserializeData(params))
            "TrackInAppMessageClose" -> this.trackInAppMessageClose(deserializeData(params))
            "TrackInAppMessageCloseWithoutTrackingConsent" ->
                this.trackInAppMessageCloseWithoutTrackingConsent(deserializeData(params))
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
            "FlushData" -> this.flushData {
                runCatching {
                    done(MethodResult(it, it.toString(), ""))
                }.logOnException()
            }
            "SetReceivedPushCallback" -> this.setReceivedPushCallback {
                runCatching {
                    val response = serializeData(it)
                    done(MethodResult.success(response))
                }.logOnException()
            }
            "SetOpenedPushCallback" -> this.setOpenedPushCallback {
                runCatching {
                    val response = serializeData(it)
                    done(MethodResult.success(response))
                }.logOnException()
            }
            "RequestPushAuthorization" -> {
                runCatching {
                    this.requestPushAuthorization(context) {
                        done(MethodResult(it, it.toString(), ""))
                    }
                }.logOnException()
            }
            "SetInAppMessageActionCallback" ->
                this.setInAppMessageActionCallback(parseAsMap(params)) { done(it) }
            else -> {
                throw BloomreachUnsupportedException("Method $method is currently unsupported")
            }
        }
    }.logOnException()

}
