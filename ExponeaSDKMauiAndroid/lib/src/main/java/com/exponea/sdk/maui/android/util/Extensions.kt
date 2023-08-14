package com.exponea.sdk.maui.android.util

import com.exponea.sdk.Exponea
import com.exponea.sdk.util.Logger

fun <T> Result<T>.returnOnException(mapThrowable: (e: Throwable) -> T): T {
    return this.getOrElse {
        try {
            Logger.e(Exponea, "Exponea Safe Mode wrapper caught unhandled error", it)
        } catch (e: Throwable) {
            // cannot log problem, swallowing
        }
        // TODO: Arch for Exponea.safeModeEnabled
        return mapThrowable(it)
    }
}
