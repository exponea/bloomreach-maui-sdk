package com.bloomreach.sdk.maui.android.exception

class BloomreachException : Exception {
    companion object {
        fun common(description: String): BloomreachException {
            return BloomreachException("Error occurred: $description.")
        }
    }

    constructor(message: String) : super(message)
    constructor(message: String, cause: Throwable) : super(message, cause)
}