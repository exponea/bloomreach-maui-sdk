package com.bloomreach.sdk.maui.android

data class MethodResult(
    val success: Boolean,
    val data: String,
    val error: String
) {
    companion object {
        fun success(data: String? = null): MethodResult = MethodResult(
            success = true,
            data = data ?: "",
            error = ""
        )
        fun failure(message: String): MethodResult = MethodResult(
            success = false,
            data = "",
            error = message
        )
    }
}
