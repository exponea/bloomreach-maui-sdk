package com.bloomreach.sdk.maui.android

import android.app.NotificationManager
import android.content.Context
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.filterValueIsInstance
import com.exponea.sdk.Exponea

internal fun BloomreachSdkAndroid.handleRemoteMessage(
    context: Context,
    params: Map<String, Any?>
) : Boolean {
    val notifPayload = params
        .mapValues { value -> SerializeUtils.serializeData(value) }
        .filterValueIsInstance(String::class.java)
    return Exponea.handleRemoteMessage(
        context,
        notifPayload,
        context.getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager
    )
}