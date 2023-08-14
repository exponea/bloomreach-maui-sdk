package com.exponea.sdk.maui.android

import android.content.Context
import com.exponea.sdk.Exponea
import com.exponea.sdk.maui.android.exception.ExponeaUnsupportedException
import com.exponea.sdk.maui.android.exception.InvalidConfigurationException
import com.exponea.sdk.maui.android.util.returnOnException
import com.exponea.sdk.models.ExponeaConfiguration
import com.google.gson.GsonBuilder

class ExponeaSDK(
    private val context: Context
) {

    private val GSON = GsonBuilder().create()

    fun invokeMethod(method: String?, params: String?): MethodResult = runCatching {
        return@runCatching when (method) {
            "greetings" -> sayHello()
            "configure" -> invokeInit(params)
            "configureWithResult" -> invokeInit2(params)
            else -> {
                throw ExponeaUnsupportedException("Method $method is currently unsupported")
            }
        }
    }.returnOnException { t ->
        MethodResult.failure("Method $method failed: ${t.localizedMessage ?: t.javaClass.name}")
    }

    private fun sayHello(): MethodResult {
        return MethodResult.success("Hello from native Android 12345")
    }

    private fun invokeInit(params: String?): MethodResult {
        if (params.isNullOrBlank()) {
            throw InvalidConfigurationException("Unable to init SDK with empty configuration input")
        }
//        Logger.i(this, "Got conf: $params")
        val sdkConf = GSON.fromJson(params, ExponeaConfiguration::class.java)
//        Logger.i(this, "Conf parsed")
        Exponea.init(
            context = context.applicationContext,
            configuration = sdkConf
        )
        return MethodResult.success()
    }

    private fun invokeInit2(params: String?): MethodResult {
        try {
            if (params.isNullOrBlank()) {
                return MethodResult.success("Unable to init SDK with empty configuration input")
            }
//            Logger.i(this, "Got conf: $params")
            val sdkConf = GSON.fromJson(params, ExponeaConfiguration::class.java)
//            Logger.i(this, "Conf parsed")
            Exponea.init(
                context = context.applicationContext,
                configuration = sdkConf
            )
            return MethodResult.success("DONE")
        } catch (e: Exception) {
            return MethodResult.success("Error: ${e.localizedMessage}")
        }
    }
}
