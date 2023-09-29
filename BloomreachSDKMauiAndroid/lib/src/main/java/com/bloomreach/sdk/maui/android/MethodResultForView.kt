package com.bloomreach.sdk.maui.android

import android.view.View

data class MethodResultForView(
    val success: Boolean,
    val data: View?,
    val error: String
) {
    companion object {
        fun success(view: View? = null): MethodResultForView = MethodResultForView(
            success = true,
            data = view,
            error = ""
        )
        fun failure(message: String): MethodResultForView = MethodResultForView(
            success = false,
            data = null,
            error = message
        )
    }
}
