package com.bloomreach.sdk.maui.android.exception

class BloomreachDataException : Exception {
    companion object {
        fun missingProperty(property: String): BloomreachDataException {
            return BloomreachDataException("Property $property is required.")
        }

        fun invalidType(property: String): BloomreachDataException {
            return BloomreachDataException("Invalid type for $property.")
        }

        fun invalidValue(property: String, value: String): BloomreachDataException {
            return BloomreachDataException("Invalid value $value for $property.")
        }
    }

    constructor(message: String) : super(message)
    constructor(message: String, cause: Throwable) : super(message, cause)
}