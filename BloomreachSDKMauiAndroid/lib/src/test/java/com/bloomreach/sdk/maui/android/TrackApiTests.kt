package com.bloomreach.sdk.maui.android

import androidx.test.core.app.ApplicationProvider
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.ExponeaConfiguration
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.models.PurchasedItem
import io.mockk.Runs
import io.mockk.every
import io.mockk.just
import io.mockk.mockkObject
import io.mockk.slot
import io.mockk.unmockkAll
import io.mockk.verify
import org.junit.After
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import java.io.File
import java.nio.charset.StandardCharsets

@RunWith(RobolectricTestRunner::class)
class TrackApiTests {

    private lateinit var instance: BloomreachSdkAndroid

    // Directory that contains JSON files using by iOS, Maui and Android unit tests
    // Json files contain data that are expected while invoking native methods.
    private val mauiTestJsonDir = File("../../BloomreachTests/Jsons")

    @Before
    fun beforeTest() {
        mockkObject(Exponea)
        every { Exponea.anonymize(any(), any()) } just Runs
        every { Exponea.init(any(), any()) } just Runs
        every { Exponea.customerCookie } returns null
        every { Exponea.defaultProperties } returns hashMapOf()
        every { Exponea.defaultProperties = capture(slot()) } just Runs
        every { Exponea.flushMode = capture(slot()) } just Runs
        every { Exponea.flushPeriod = capture(slot()) } just Runs
        every { Exponea.sessionTimeout } returns 0.0
        every { Exponea.sessionTimeout = capture(slot()) } just Runs
        every { Exponea.tokenTrackFrequency } returns ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE
        every { Exponea.identifyCustomer(any(), any()) } just Runs
        every { Exponea.isAutomaticSessionTracking } returns true
        every { Exponea.isAutomaticSessionTracking = capture(slot()) } just Runs
        every { Exponea.isAutoPushNotification } returns true
        every { Exponea.isAutoPushNotification = capture(slot()) } just Runs
        every { Exponea.flushData(any()) } just Runs
        every { Exponea.identifyCustomer(any(), any()) } just Runs
        instance = BloomreachSdkAndroid(ApplicationProvider.getApplicationContext())
    }

    @After
    fun afterTest() {
        unmockkAll()
    }

    @Test
    fun readJsonFile() {
        val content = readTestFile("Configure_FullConfiguration_Input")
        assertNotNull(content)
    }

    private fun readTestFile(fileName: String): String {
        return File(mauiTestJsonDir, "${fileName}.json")
            .readText(StandardCharsets.UTF_8)
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
