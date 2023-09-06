package com.exponea.sdk.maui.android.util

import com.google.gson.GsonBuilder
import com.google.gson.reflect.TypeToken

object SerializeUtils {

    private val GSON = GsonBuilder().create()

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
}