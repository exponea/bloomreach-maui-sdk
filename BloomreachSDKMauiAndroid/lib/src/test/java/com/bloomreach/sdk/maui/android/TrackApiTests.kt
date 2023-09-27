package com.bloomreach.sdk.maui.android

import com.exponea.sdk.Exponea
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.models.PurchasedItem
import io.mockk.verify
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class TrackApiTests : TestsBase() {

    @Test
    fun readJsonFile() {
        val content = readTestFile("Configure_FullConfiguration_Input")
        assertNotNull(content)
    }

    @Test
    fun TrackPaymentEvent_Empty_WithTime() {
        val result = instance.invokeMethod("TrackPaymentEvent", readTestFile("TrackPaymentEvent_Empty_WithTime"))
        verify {
            Exponea.trackPaymentEvent(10000.0, PurchasedItem(
                value = 10.4,
                currency = "Eur",
                paymentSystem = "",
                productId = "",
                productTitle = "",
                receipt = null
            ))
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackPaymentEvent_Full_WithTime() {
        val result = instance.invokeMethod("TrackPaymentEvent", readTestFile("TrackPaymentEvent_Full_WithTime"))
        verify {
            Exponea.trackPaymentEvent(10000.0, PurchasedItem(
                value = 10.4,
                currency = "Eur",
                paymentSystem = "credit",
                productId = "12345",
                productTitle = "Fine stuff",
                receipt = "abc-1234567890"
            ))
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackEvent_WithTime() {
        val result = instance.invokeMethod("TrackEvent", readTestFile("TrackEvent_WithTime"))
        verify {
            Exponea.trackEvent(
                properties = PropertiesList(hashMapOf(
                    "prop1" to "val1",
                    "prop2" to 2.0
                )),
                timestamp = 10000.0,
                eventType = "custom_event"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackSessionEnd() {
        val result = instance.invokeMethod("TrackSessionEnd", null)
        verify {
            Exponea.trackSessionEnd(any())
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackSessionStart() {
        val result = instance.invokeMethod("TrackSessionStart", null)
        verify {
            Exponea.trackSessionStart(any())
        }
        assertTrue(result.success)
    }
}
