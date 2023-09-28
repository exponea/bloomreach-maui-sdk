package com.bloomreach.sdk.maui.android

import android.Manifest
import android.app.Activity
import android.app.NotificationManager
import android.content.Context
import android.content.Intent
import android.content.Intent.FLAG_ACTIVITY_NEW_TASK
import android.content.IntentFilter
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Build
import androidx.core.content.ContextCompat
import com.bloomreach.sdk.maui.android.notifications.NotificationsPermissionActivity
import com.bloomreach.sdk.maui.android.notifications.NotificationsPermissionReceiver
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.filterValueIsInstance
import com.exponea.sdk.Exponea
import com.exponea.sdk.util.Logger
import com.exponea.sdk.util.logOnException


internal fun BloomreachSdkAndroid.handleRemoteMessage(
    context: Context,
    params: Map<String, Any?>
) : Boolean {
    val notifPayload = SerializeUtils.mapToStringString(params)
    return Exponea.handleRemoteMessage(
        context,
        notifPayload,
        context.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
    )
}

internal fun BloomreachSdkAndroid.handlePushNotificationOpened(
    params: Map<String, Any?>
) {
    // TODO: Android works differently from iOS, not API exposed, just tracking
    trackClickedPush(params)
}

internal fun BloomreachSdkAndroid.handlePushNotificationOpenedWithoutTrackingConsent(
    params: Map<String, Any?>
) {
    // TODO: Android works differently from iOS, not API exposed, just tracking
    trackClickedPushWithoutTrackingConsent(params)
}

internal fun BloomreachSdkAndroid.isBloomreachNotification(
    payload: Map<String, Any?>
): Boolean {
    val normalizedPayload = SerializeUtils.mapToStringString(payload)
    return Exponea.isExponeaPushNotification(normalizedPayload)
}

internal fun BloomreachSdkAndroid.trackClickedPush(
    action: Map<String, Any?>
) {
    val normalizedActionMap = SerializeUtils.mapToStringString(action)
    val notificationData = SerializeUtils.parseNotificationData(normalizedActionMap)
    val actionData = SerializeUtils.parseNotificationAction(action)
    Exponea.trackClickedPush(
        notificationData, actionData
    )
}

internal fun BloomreachSdkAndroid.trackClickedPushWithoutTrackingConsent(
    action: Map<String, Any?>
) {
    val normalizedActionMap = SerializeUtils.mapToStringString(action)
    val notificationData = SerializeUtils.parseNotificationData(normalizedActionMap)
    val actionData = SerializeUtils.parseNotificationAction(action)
    Exponea.trackClickedPushWithoutTrackingConsent(
        notificationData, actionData
    )
}

internal fun BloomreachSdkAndroid.trackDeliveredPush(
    action: Map<String, Any?>
) {
    val normalizedActionMap = SerializeUtils.mapToStringString(action)
    val notificationData = SerializeUtils.parseNotificationData(normalizedActionMap)
    Exponea.trackDeliveredPush(notificationData)
}

internal fun BloomreachSdkAndroid.trackDeliveredPushWithoutTrackingConsent(
    action: Map<String, Any?>
) {
    val normalizedActionMap = SerializeUtils.mapToStringString(action)
    val notificationData = SerializeUtils.parseNotificationData(normalizedActionMap)
    Exponea.trackDeliveredPushWithoutTrackingConsent(notificationData)
}

internal fun BloomreachSdkAndroid.handleCampaignClick(context: Context, campaignUrl: String?) {
    campaignUrl ?: return
    val campaignIntent = Intent()
    campaignIntent.action = Intent.ACTION_VIEW
    campaignIntent.data = Uri.parse(campaignUrl)
    Exponea.handleCampaignIntent(campaignIntent, context)
}

internal fun BloomreachSdkAndroid.handleHmsPushToken(context: Context, token: String?) {
    token ?: return
    Exponea.handleNewHmsToken(context, token)
}

internal fun BloomreachSdkAndroid.handlePushToken(context: Context, token: String?) {
    token ?: return
    Exponea.handleNewToken(context, token)
}

internal fun BloomreachSdkAndroid.trackPushToken(token: String?) {
    token ?: return
    Exponea.trackPushToken(token)
}

internal fun BloomreachSdkAndroid.trackHmsPushToken(token: String?) {
    token ?: return
    Exponea.trackHmsPushToken(token)
}

internal fun BloomreachSdkAndroid.requestPushAuthorization(context: Context, listener: (Boolean) -> Unit) {
    NotificationsPermissionReceiver.requestPushAuthorization(context, listener)
}

internal fun BloomreachSdkAndroid.setReceivedPushCallback(listener: (Map<String, Any>) -> Unit) {
    Exponea.notificationDataCallback = {
        runCatching {
            listener.invoke(it)
        }.logOnException()
    }
}

internal fun BloomreachSdkAndroid.setOpenedPushCallback(listener: (Map<String, Any>) -> Unit) {
    BloomreachSdkAndroid.pushNotificationClickListener = {
        runCatching {
            listener.invoke(it)
        }.logOnException()
    }
}