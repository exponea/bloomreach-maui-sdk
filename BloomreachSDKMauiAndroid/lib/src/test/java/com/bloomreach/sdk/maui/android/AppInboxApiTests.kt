package com.bloomreach.sdk.maui.android

import com.exponea.sdk.Exponea
import com.exponea.sdk.models.MessageItem
import com.exponea.sdk.models.MessageItemAction
import com.exponea.sdk.models.MessageItemAction.Type
import com.exponea.sdk.models.MessageItemAction.Type.APP
import com.exponea.sdk.models.MessageItemAction.Type.BROWSER
import com.exponea.sdk.models.MessageItemAction.Type.DEEPLINK
import com.exponea.sdk.models.MessageItemAction.Type.NO_ACTION
import io.mockk.every
import io.mockk.slot
import io.mockk.verify
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertNull
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class AppInboxApiTests : TestsBase() {

    @Test
    fun GetAppInboxButton_Valid() {
        val result = instance.invokeMethodForUI("GetAppInboxButton", null)
        verify {
            Exponea.getAppInboxButton(any())
        }
        assertTrue(result.success)
        assertNotNull(result.data)
    }

    @Test
    fun GetAppInboxButton_NotInitSdk() {
        every { Exponea.getAppInboxButton(any()) } returns null
        val result = instance.invokeMethodForUI("GetAppInboxButton", null)
        verify {
            Exponea.getAppInboxButton(any())
        }
        assertFalse(result.success)
        assertNull(result.data)
    }

    @Test
    fun TrackAppInboxClick_App_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(APP)
        val result =
            instance.invokeMethod("TrackAppInboxClick", readTestFile("TrackAppInboxClick_App_Html"))
        verify {
            Exponea.trackAppInboxClick(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClick_Browser_Push() {
        val expectedMessage = buildAppInboxMessage("push")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(BROWSER)
        val result = instance.invokeMethod("TrackAppInboxClick", readTestFile("TrackAppInboxClick_Browser_Push"))
        verify {
            Exponea.trackAppInboxClick(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClick_Deeplink_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(DEEPLINK)
        val result = instance.invokeMethod("TrackAppInboxClick", readTestFile("TrackAppInboxClick_Deeplink_Html"))
        verify {
            Exponea.trackAppInboxClick(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClick_NoAction_Unknown() {
        val expectedMessage = buildAppInboxMessage("unknown")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(NO_ACTION)
        val result = instance.invokeMethod("TrackAppInboxClick", readTestFile("TrackAppInboxClick_NoAction_Unknown"))
        verify {
            Exponea.trackAppInboxClick(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClickWithoutTrackingConsent_App_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(APP)
        val result =
            instance.invokeMethod("TrackAppInboxClickWithoutTrackingConsent", readTestFile("TrackAppInboxClickWithoutTrackingConsent_App_Html"))
        verify {
            Exponea.trackAppInboxClickWithoutTrackingConsent(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClickWithoutTrackingConsent_Browser_Push() {
        val expectedMessage = buildAppInboxMessage("push")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(BROWSER)
        val result = instance.invokeMethod("TrackAppInboxClickWithoutTrackingConsent", readTestFile("TrackAppInboxClickWithoutTrackingConsent_Browser_Push"))
        verify {
            Exponea.trackAppInboxClickWithoutTrackingConsent(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(DEEPLINK)
        val result = instance.invokeMethod("TrackAppInboxClickWithoutTrackingConsent", readTestFile("TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html"))
        verify {
            Exponea.trackAppInboxClickWithoutTrackingConsent(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown() {
        val expectedMessage = buildAppInboxMessage("unknown")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val actionSlot = slot<MessageItemAction>()
        val expectedAction = buildAppInboxAction(NO_ACTION)
        val result = instance.invokeMethod("TrackAppInboxClickWithoutTrackingConsent", readTestFile("TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown"))
        verify {
            Exponea.trackAppInboxClickWithoutTrackingConsent(
                capture(actionSlot),
                expectedMessage
            )
        }
        assertTrue(result.success)
        assertEquals(expectedAction.title, actionSlot.captured.title)
        assertEquals(expectedAction.type, actionSlot.captured.type)
        assertEquals(expectedAction.url, actionSlot.captured.url)
    }

    @Test
    fun TrackAppInboxOpened_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpened", readTestFile("TrackAppInboxOpened_Html"))
        verify {
            Exponea.trackAppInboxOpened(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackAppInboxOpened_Push() {
        val expectedMessage = buildAppInboxMessage("push")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpened", readTestFile("TrackAppInboxOpened_Push"))
        verify {
            Exponea.trackAppInboxOpened(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackAppInboxOpened_Unknown() {
        val expectedMessage = buildAppInboxMessage("unknown")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpened", readTestFile("TrackAppInboxOpened_Unknown"))
        verify {
            Exponea.trackAppInboxOpened(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackAppInboxOpenedWithoutTrackingConsent_Html() {
        val expectedMessage = buildAppInboxMessage("html")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpenedWithoutTrackingConsent", readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Html"))
        verify {
            Exponea.trackAppInboxOpenedWithoutTrackingConsent(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackAppInboxOpenedWithoutTrackingConsent_Push() {
        val expectedMessage = buildAppInboxMessage("push")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpenedWithoutTrackingConsent", readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Push"))
        verify {
            Exponea.trackAppInboxOpenedWithoutTrackingConsent(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackAppInboxOpenedWithoutTrackingConsent_Unknown() {
        val expectedMessage = buildAppInboxMessage("unknown")
        every { Exponea.fetchAppInboxItem(any(), any()) } answers {
            // message fetch is done as middle step
            secondArg<(MessageItem?) -> Unit>().invoke(expectedMessage)
        }
        val result = instance.invokeMethod("TrackAppInboxOpenedWithoutTrackingConsent", readTestFile("TrackAppInboxOpenedWithoutTrackingConsent_Unknown"))
        verify {
            Exponea.trackAppInboxOpenedWithoutTrackingConsent(
                expectedMessage
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun FetchAppInbox_Empty() {
        every { Exponea.fetchAppInbox(any()) } answers {
            firstArg<(List<MessageItem>?) -> Unit>().invoke(emptyList())
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchAppInbox", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchAppInbox_Empty"))
    }

    @Test
    fun FetchAppInbox_Multiple() {
        every { Exponea.fetchAppInbox(any()) } answers {
            firstArg<(List<MessageItem>?) -> Unit>().invoke(listOf(
                buildAppInboxMessage("html", "12345"),
                buildAppInboxMessage("html", "67890")
            ))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchAppInbox", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchAppInbox_Multiple"))
    }

    @Test
    fun FetchAppInbox_Single() {
        every { Exponea.fetchAppInbox(any()) } answers {
            firstArg<(List<MessageItem>?) -> Unit>().invoke(listOf(
                buildAppInboxMessage("html", "12345")
            ))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchAppInbox", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchAppInbox_Single"))
    }

    @Test
    fun FetchAppInboxItem_Valid() {
        every { Exponea.fetchAppInboxItem("12345", any()) } answers {
            secondArg<(MessageItem?) -> Unit>().invoke(buildAppInboxMessage("html"))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchAppInboxItem", "12345") {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchAppInboxItem_Valid"))
    }

    @Test
    fun MarkAppInboxAsRead() {
        val appInboxMessage = buildAppInboxMessage("html", "abcd")
        val messageIdSlot = slot<String>()
        val messageSlot = slot<MessageItem>()
        every { Exponea.fetchAppInboxItem(capture(messageIdSlot), any()) } answers {
            secondArg<(MessageItem?) -> Unit>().invoke(appInboxMessage)
        }
        every { Exponea.markAppInboxAsRead(capture(messageSlot), any()) } answers {
            secondArg<(Boolean) -> Unit>().invoke(true)
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("MarkAppInboxAsRead", "abcd") {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(messageIdSlot.captured, "abcd")
        assertEquals(messageSlot.captured.id, "abcd")
    }

    private fun buildAppInboxMessage(messageType: String, id: String = "12345") = MessageItem(
        id = id,
        rawType = messageType,
        read = true,
        receivedTime = 10.0,
        rawContent = mapOf(
            "prop1" to "val1",
            "prop2" to 2.0
        )
    )

    private fun buildAppInboxAction(actionType: Type): MessageItemAction {
        return MessageItemAction().apply {
            this.type = actionType
            this.title = "Action title"
            this.url = "https://example.com"
        }
    }
}
