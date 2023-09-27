package com.bloomreach.sdk.maui.android

import android.content.Intent
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.NotificationAction
import com.exponea.sdk.models.NotificationData
import io.mockk.every
import io.mockk.slot
import io.mockk.verify
import org.junit.Assert
import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class NotificationsApiTests : TestsBase() {

    @Test
    fun HandleRemoteMessage() {
        val payloadSlot = slot<Map<String, String>>()
        val result = instance.invokeMethod(
            "HandleRemoteMessage",
            readTestFile("SamplePushNotification")
        )
        verify {
            Exponea.handleRemoteMessage(
                any(),
                capture(payloadSlot),
                any(),
                true
            )
        }
        assertTrue(result.success)
        assertTrue(payloadSlot.isCaptured)
        Assert.assertNotNull(payloadSlot.captured)
        val capturedMap = payloadSlot.captured
        Assert.assertNotNull(capturedMap["url_ios"])
        Assert.assertNotNull(capturedMap["legacy_ios_category"])
        Assert.assertNotNull(capturedMap["title"])
        Assert.assertNotNull(capturedMap["action"])
        Assert.assertNotNull(capturedMap["message"])
        Assert.assertNotNull(capturedMap["image"])
        Assert.assertNotNull(capturedMap["actions"])
        Assert.assertNotNull(capturedMap["sound"])
        Assert.assertNotNull(capturedMap["aps"])
        Assert.assertNotNull(capturedMap["url_params"])
        Assert.assertNotNull(capturedMap["source"])
        Assert.assertNotNull(capturedMap["silent"])
        Assert.assertNotNull(capturedMap["has_tracking_consent"])
        Assert.assertNotNull(capturedMap["consent_category_tracking"])
    }

    @Test
    fun HandlePushNotificationOpened() {
        val dataSlot = slot<NotificationData>()
        val actionSlot = slot<NotificationAction>()
        val result = instance.invokeMethod(
            "HandlePushNotificationOpened",
            readTestFile("HandlePushNotificationOpened")
        )
        verify {
            Exponea.trackClickedPush(
                capture(dataSlot),
                capture(actionSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        assertTrue(actionSlot.isCaptured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("override", dataSlot.captured.attributes["prop2"])
        assertEquals("button", actionSlot.captured.actionType)
        assertEquals("Click me", actionSlot.captured.actionName)
        assertEquals("https://google.com", actionSlot.captured.url)
    }

    @Test
    fun HandlePushNotificationOpenedWithoutTrackingConsent() {
        val dataSlot = slot<NotificationData>()
        val actionSlot = slot<NotificationAction>()
        val result = instance.invokeMethod(
            "HandlePushNotificationOpenedWithoutTrackingConsent",
            readTestFile("HandlePushNotificationOpenedWithoutTrackingConsent")
        )
        verify {
            Exponea.trackClickedPushWithoutTrackingConsent(
                capture(dataSlot),
                capture(actionSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        assertTrue(actionSlot.isCaptured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("override", dataSlot.captured.attributes["prop2"])
        assertEquals("button", actionSlot.captured.actionType)
        assertEquals("Click me", actionSlot.captured.actionName)
        assertEquals("https://google.com", actionSlot.captured.url)
    }

    @Test
    fun HandleCampaignClick() {
        val intentSlot = slot<Intent>()
        val result = instance.invokeMethod(
            "HandleCampaignClick",
            readTestFile("HandleCampaignClick")
        )
        verify {
            Exponea.handleCampaignIntent(
                capture(intentSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertEquals(Intent.ACTION_VIEW, intentSlot.captured.action)
        assertEquals("https://google.com/unit/test/with?params=1&orparam=true", intentSlot.captured.data?.toString())
    }

    @Test
    fun HandleHmsPushToken() {
        val result = instance.invokeMethod(
            "HandleHmsPushToken",
            readTestFile("HandleHmsPushToken")
        )
        verify {
            Exponea.handleNewHmsToken(
                any(),
                "hms-token-12345"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun HandlePushToken() {
        val result = instance.invokeMethod(
            "HandlePushToken",
            readTestFile("HandlePushToken")
        )
        verify {
            Exponea.handleNewToken(
                any(),
                "gms-token-12345"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun IsBloomreachNotification_Valid() {
        every {
            Exponea.isExponeaPushNotification(any())
        } returns true
        val result = instance.invokeMethod(
            "IsBloomreachNotification",
            readTestFile("IsBloomreachNotification")
        )
        verify {
            Exponea.isExponeaPushNotification(
                mapOf(
                    "prop1" to "val1",
                    "prop2" to "val2"
                )
            )
        }
        assertTrue(result.success)
        assertEquals("true", result.data)
    }

    @Test
    fun IsBloomreachNotification_Invalid() {
        every {
            Exponea.isExponeaPushNotification(any())
        } returns false
        val result = instance.invokeMethod(
            "IsBloomreachNotification",
            readTestFile("IsBloomreachNotification")
        )
        verify {
            Exponea.isExponeaPushNotification(
                mapOf(
                    "prop1" to "val1",
                    "prop2" to "val2"
                )
            )
        }
        assertTrue(result.success)
        assertEquals("false", result.data)
    }

    @Test
    fun TrackClickedPush() {
        val dataSlot = slot<NotificationData>()
        val actionSlot = slot<NotificationAction>()
        val result = instance.invokeMethod(
            "TrackClickedPush",
            readTestFile("TrackClickedPush")
        )
        verify {
            Exponea.trackClickedPush(
                capture(dataSlot),
                capture(actionSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        assertTrue(actionSlot.isCaptured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("override", dataSlot.captured.attributes["prop2"])
        assertEquals("button", actionSlot.captured.actionType)
        assertEquals("Click me", actionSlot.captured.actionName)
        assertEquals("https://google.com", actionSlot.captured.url)
    }

    @Test
    fun TrackClickedPushWithoutTrackingConsent() {
        val dataSlot = slot<NotificationData>()
        val actionSlot = slot<NotificationAction>()
        val result = instance.invokeMethod(
            "TrackClickedPushWithoutTrackingConsent",
            readTestFile("TrackClickedPushWithoutTrackingConsent")
        )
        verify {
            Exponea.trackClickedPushWithoutTrackingConsent(
                capture(dataSlot),
                capture(actionSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        assertTrue(actionSlot.isCaptured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("override", dataSlot.captured.attributes["prop2"])
        assertEquals("button", actionSlot.captured.actionType)
        assertEquals("Click me", actionSlot.captured.actionName)
        assertEquals("https://google.com", actionSlot.captured.url)
    }

    @Test
    fun TrackPushToken() {
        val result = instance.invokeMethod(
            "TrackPushToken",
            readTestFile("HandlePushToken")
        )
        verify {
            Exponea.trackPushToken(
                "gms-token-12345"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackHmsPushToken() {
        val result = instance.invokeMethod(
            "TrackHmsPushToken",
            readTestFile("HandleHmsPushToken")
        )
        verify {
            Exponea.trackHmsPushToken(
                "hms-token-12345"
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun TrackDeliveredPush() {
        val dataSlot = slot<NotificationData>()
        val result = instance.invokeMethod(
            "TrackDeliveredPush",
            readTestFile("TrackDeliveredPush")
        )
        verify {
            Exponea.trackDeliveredPush(
                capture(dataSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        Assert.assertNotNull(dataSlot.captured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("val2", dataSlot.captured.attributes["prop2"])
    }

    @Test
    fun TrackDeliveredPushWithoutTrackingConsent() {
        val dataSlot = slot<NotificationData>()
        val result = instance.invokeMethod(
            "TrackDeliveredPushWithoutTrackingConsent",
            readTestFile("TrackDeliveredPush")
        )
        verify {
            Exponea.trackDeliveredPushWithoutTrackingConsent(
                capture(dataSlot),
                any()
            )
        }
        assertTrue(result.success)
        assertTrue(dataSlot.isCaptured)
        Assert.assertNotNull(dataSlot.captured)
        assertEquals("val1", dataSlot.captured.attributes["prop1"])
        assertEquals("val2", dataSlot.captured.attributes["prop2"])
    }
}
