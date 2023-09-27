package com.bloomreach.sdk.maui.android

import androidx.test.core.app.ApplicationProvider
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.DateFilter
import com.exponea.sdk.models.ExponeaConfiguration
import com.exponea.sdk.models.InAppMessage
import com.exponea.sdk.models.InAppMessageButton
import com.exponea.sdk.models.InAppMessagePayload
import com.exponea.sdk.models.InAppMessagePayloadButton
import com.exponea.sdk.models.InAppMessageType
import com.exponea.sdk.models.InAppMessageType.FREEFORM
import com.exponea.sdk.models.InAppMessageType.MODAL
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.models.PurchasedItem
import com.exponea.sdk.models.eventfilter.EventFilter
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
import org.junit.Assert

@RunWith(RobolectricTestRunner::class)
class InAppApiTests {

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
        every { Exponea.trackInAppMessageClick(any(), any(), any()) } just Runs
        every { Exponea.trackInAppMessageClickWithoutTrackingConsent(any(), any(), any()) } just Runs
        every { Exponea.trackInAppMessageClose(any(), any()) } just Runs
        every { Exponea.trackInAppMessageCloseWithoutTrackingConsent(any(), any()) } just Runs
        instance = BloomreachSdkAndroid(ApplicationProvider.getApplicationContext())
    }

    @After
    fun afterTest() {
        unmockkAll()
    }

    @Test
    fun readJsonFile() {
        val content = readTestFile("TrackInAppMessageClick")
        assertNotNull(content)
    }

    private fun readTestFile(fileName: String): String {
        return File(mauiTestJsonDir, "${fileName}.json")
            .readText(StandardCharsets.UTF_8)
    }

    @Test
    fun TrackInAppMessageClick() {
        val result = instance.invokeMethod("TrackInAppMessageClick", readTestFile("TrackInAppMessageClick"))
        verify {
            Exponea.trackInAppMessageClick(
                InAppMessage(
                    id = "id",
                    name = "name",
                    rawMessageType = "rawMessageType",
                    rawFrequency = "rawFrequency",
                    variantId = 1,
                    variantName = "name",
                    priority = 1,
                    delay = 0,
                    timeout = 0,
                    payloadHtml = "payloadHtml",
                    isHtml = false,
                    rawHasTrackingConsent = false,
                    consentCategoryTracking = "category",
                    payload = null,
                    trigger = EventFilter("event type", arrayListOf()),
                    dateFilter = DateFilter(false)
                ),
                "button text",
                "buttonLink"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackInAppMessageClickWithoutTrackingConsent() {
        val result = instance.invokeMethod("TrackInAppMessageClickWithoutTrackingConsent", readTestFile("TrackInAppMessageClick"))
        verify {
            Exponea.trackInAppMessageClickWithoutTrackingConsent(
                InAppMessage(
                    id = "id",
                    name = "name",
                    rawMessageType = "rawMessageType",
                    rawFrequency = "rawFrequency",
                    variantId = 1,
                    variantName = "name",
                    priority = 1,
                    delay = 0,
                    timeout = 0,
                    payloadHtml = "payloadHtml",
                    isHtml = false,
                    rawHasTrackingConsent = false,
                    consentCategoryTracking = "category",
                    payload = null,
                    trigger = EventFilter("event type", arrayListOf()),
                    dateFilter = DateFilter(false)
                ),
                "button text",
                "buttonLink"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackInAppMessageClose() {
        val result = instance.invokeMethod("TrackInAppMessageClose", readTestFile("TrackInAppMessageClose"))
        verify {
            Exponea.trackInAppMessageClose(
                InAppMessage(
                    id = "id",
                    name = "name",
                    rawMessageType = "rawMessageType",
                    rawFrequency = "rawFrequency",
                    variantId = 1,
                    variantName = "name",
                    priority = 1,
                    delay = 0,
                    timeout = 0,
                    payloadHtml = "payloadHtml",
                    isHtml = false,
                    rawHasTrackingConsent = false,
                    consentCategoryTracking = "category",
                    payload = null,
                    trigger = EventFilter("event type", arrayListOf()),
                    dateFilter = DateFilter(false)
                ),
                true
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackInAppMessageCloseWithoutTrackingConsent() {
        val result = instance.invokeMethod("TrackInAppMessageCloseWithoutTrackingConsent", readTestFile("TrackInAppMessageClose"))
        verify {
            Exponea.trackInAppMessageCloseWithoutTrackingConsent(
                InAppMessage(
                    id = "id",
                    name = "name",
                    rawMessageType = "rawMessageType",
                    rawFrequency = "rawFrequency",
                    variantId = 1,
                    variantName = "name",
                    priority = 1,
                    delay = 0,
                    timeout = 0,
                    payloadHtml = "payloadHtml",
                    isHtml = false,
                    rawHasTrackingConsent = false,
                    consentCategoryTracking = "category",
                    payload = null,
                    trigger = EventFilter("event type", arrayListOf()),
                    dateFilter = DateFilter(false)
                ),
                true
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun SetInAppMessageActionCallback_False_False() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.failure(NullPointerException("Developer likes this")))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync(
            "SetInAppMessageActionCallback",
            readTestFile("SetInAppMessageActionCallback_False_False"),
            done = {
                result = it
            })
        Assert.assertEquals(false, Exponea.inAppMessageActionCallback.overrideDefaultBehavior)
        Assert.assertEquals(false, Exponea.inAppMessageActionCallback.trackActions)
        Exponea.inAppMessageActionCallback.inAppMessageAction(
            getInAppMessage(),
            InAppMessageButton("button text", "https://example.com/test"),
            true,
            ApplicationProvider.getApplicationContext()
        )
        Assert.assertTrue(result!!.success)
        Assert.assertEquals(result!!.data, readTestFile("SetInAppMessageActionCallback_Output"))
    }

    @Test
    fun SetInAppMessageActionCallback_False_True() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.failure(NullPointerException("Developer likes this")))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync(
            "SetInAppMessageActionCallback",
            readTestFile("SetInAppMessageActionCallback_False_True"),
            done = {
                result = it
            })
        Assert.assertEquals(false, Exponea.inAppMessageActionCallback.overrideDefaultBehavior)
        Assert.assertEquals(true, Exponea.inAppMessageActionCallback.trackActions)
        Exponea.inAppMessageActionCallback.inAppMessageAction(
            getInAppMessage(),
            InAppMessageButton("button text", "https://example.com/test"),
            true,
            ApplicationProvider.getApplicationContext()
        )
        Assert.assertTrue(result!!.success)
        Assert.assertEquals(result!!.data, readTestFile("SetInAppMessageActionCallback_Output"))
    }

    @Test
    fun SetInAppMessageActionCallback_True_False() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.failure(NullPointerException("Developer likes this")))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync(
            "SetInAppMessageActionCallback",
            readTestFile("SetInAppMessageActionCallback_True_False"),
            done = {
                result = it
            })
        Assert.assertEquals(true, Exponea.inAppMessageActionCallback.overrideDefaultBehavior)
        Assert.assertEquals(false, Exponea.inAppMessageActionCallback.trackActions)
        Exponea.inAppMessageActionCallback.inAppMessageAction(
            getInAppMessage(),
            InAppMessageButton("button text", "https://example.com/test"),
            true,
            ApplicationProvider.getApplicationContext()
        )
        Assert.assertTrue(result!!.success)
        Assert.assertEquals(result!!.data, readTestFile("SetInAppMessageActionCallback_Output"))
    }

    @Test
    fun SetInAppMessageActionCallback_True_True() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.failure(NullPointerException("Developer likes this")))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync(
            "SetInAppMessageActionCallback",
            readTestFile("SetInAppMessageActionCallback_True_True"),
            done = {
                result = it
            })
        Assert.assertEquals(true, Exponea.inAppMessageActionCallback.overrideDefaultBehavior)
        Assert.assertEquals(true, Exponea.inAppMessageActionCallback.trackActions)
        Exponea.inAppMessageActionCallback.inAppMessageAction(
            getInAppMessage(),
            InAppMessageButton("button text", "https://example.com/test"),
            true,
            ApplicationProvider.getApplicationContext()
        )
        Assert.assertTrue(result!!.success)
        Assert.assertEquals(result!!.data, readTestFile("SetInAppMessageActionCallback_Output"))
    }

    private fun getInAppMessage(
        id: String? = null,
        dateFilter: DateFilter? = null,
        trigger: EventFilter? = null,
        frequency: String? = null,
        imageUrl: String? = null,
        priority: Int? = null,
        timeout: Long? = null,
        delay: Long? = null,
        type: InAppMessageType = MODAL
    ): InAppMessage {
        var payload: InAppMessagePayload? = null
        var payloadHtml: String? = null
        if (type == FREEFORM) {
            payloadHtml = "<html>" +
                "<head>" +
                "<style>" +
                ".css-image {" +
                "   background-image: url('https://i.ytimg.com/vi/t4nM1FoUqYs/maxresdefault.jpg')" +
                "}" +
                "</style>" +
                "</head>" +
                "<body>" +
                "<img src='https://i.ytimg.com/vi/t4nM1FoUqYs/maxresdefault.jpg'/>" +
                "<div data-actiontype='close'>Close</div>" +
                "<div data-link='https://someaddress.com'>Action 1</div>" +
                "</body></html>"
        } else {
            payload = InAppMessagePayload(
                imageUrl = imageUrl ?: "https://i.ytimg.com/vi/t4nM1FoUqYs/maxresdefault.jpg",
                title = "filip.vozar@exponea.com",
                titleTextColor = "#000000",
                titleTextSize = "22px",
                bodyText = "This is an example of your in-app message body text.",
                bodyTextColor = "#000000",
                bodyTextSize = "14px",
                backgroundColor = "#ffffff",
                closeButtonColor = "#ffffff",
                buttons = arrayListOf(
                    InAppMessagePayloadButton(
                        rawButtonType = "deep-link",
                        buttonText = "Action",
                        buttonLink = "https://someaddress.com",
                        buttonTextColor = "#ffffff",
                        buttonBackgroundColor = "#f44cac"
                    ),
                    InAppMessagePayloadButton(
                        rawButtonType = "cancel",
                        buttonText = "Cancel",
                        buttonLink = null,
                        buttonTextColor = "#ffffff",
                        buttonBackgroundColor = "#f44cac"
                    )
                )
            )
        }
        return InAppMessage(
            id = id ?: "5dd86f44511946ea55132f29",
            name = "Test serving in-app message",
            rawMessageType = type.value,
            rawFrequency = frequency ?: "unknown",
            variantId = 0,
            variantName = "Variant A",
            trigger = trigger ?: EventFilter("session_start", arrayListOf()),
            dateFilter = dateFilter ?: DateFilter(false, null, null),
            priority = priority,
            delay = delay,
            timeout = timeout,
            payload = payload,
            payloadHtml = payloadHtml,
            isHtml = type == FREEFORM,
            consentCategoryTracking = null,
            rawHasTrackingConsent = null
        )
    }
}
