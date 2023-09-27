package com.bloomreach.sdk.maui.android.util

import com.exponea.sdk.models.CampaignData
import com.exponea.sdk.models.NotificationAction
import com.exponea.sdk.models.NotificationData
import com.exponea.sdk.util.ExponeaGson
import com.exponea.sdk.util.GdprTracking
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken

object SerializeUtils {

    private val GSON = ExponeaGson.instance

    fun parseAsMap(params: String?): Map<String, Any?> {
        if (params == null) {
            return emptyMap();
        }
        val mapType = object : TypeToken<Map<String, Any?>>() {}.type
        return GSON.fromJson(params, mapType)
    }

    fun parseBoolean(params: String?): Boolean {
        return params?.equals("true") ?: false
    }

    fun parseDouble(params: String?): Double {
        return params?.toDoubleOrNull() ?: 0.0
    }

    fun parseLong(params: String?): Long {
        return params?.toLongOrNull() ?: 0
    }

    fun serializeData(source: Any?): String? {
        if (source == null) {
            return null
        }
        when (source) {
            is String -> return source.trim().trim('"')
        }
        return GSON.toJson(source)
    }

    fun mapToStringString(source: Map<String, Any?>?): Map<String, String>? {
        source ?: return null
        return source
            .mapValues { serializeData(it.value) }
            .filterValueIsInstance(String::class.java)
    }

    fun parseNotificationData(source: Map<String, String>?): NotificationData? {
        source ?: return null
        val dataMap: HashMap<String, Any> = GSON.fromJson(source["data"] ?: source["attributes"] ?: "{}")
        val campaignMap: Map<String, String> = GSON.fromJson(source["url_params"] ?: "{}")
        val consentCategoryTracking: String? = source["consent_category_tracking"]
        val hasTrackingConsent: Boolean = GdprTracking.hasTrackingConsent(source["has_tracking_consent"])
        return NotificationData(
            dataMap,
            parseCampaignData(campaignMap),
            consentCategoryTracking,
            hasTrackingConsent
        )
    }

    private fun parseCampaignData(source: Map<String, String>): CampaignData {
        return CampaignData(
            source = source["utm_source"],
            campaign = source["utm_campaign"],
            content = source["utm_content"],
            medium = source["utm_medium"],
            term = source["utm_term"],
            payload = source["xnpe_cmp"],
            createdAt = currentTimeSeconds(),
            completeUrl = null
        )
    }

    fun parseNotificationAction(source: Map<String, Any?>?): NotificationAction? {
        source ?: return null
        val actionType: String = source.getNullSafely("actionType") ?: return null
        return NotificationAction(
            actionType = actionType,
            actionName = source.getNullSafely("actionName"),
            url = source.getNullSafely("url")
        )
    }
}

internal inline fun <reified T> Gson.fromJson(json: String) = this.fromJson<T>(json, object : TypeToken<T>() {}.type)