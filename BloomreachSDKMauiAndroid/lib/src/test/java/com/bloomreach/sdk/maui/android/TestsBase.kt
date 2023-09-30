package com.bloomreach.sdk.maui.android

import android.widget.Button
import androidx.test.core.app.ApplicationProvider
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.ExponeaConfiguration
import io.mockk.Runs
import io.mockk.every
import io.mockk.just
import io.mockk.mockkObject
import io.mockk.slot
import io.mockk.unmockkAll
import org.junit.After
import org.junit.Before
import java.io.File
import java.nio.charset.StandardCharsets

abstract class TestsBase {

    internal lateinit var instance: BloomreachSdkAndroid

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
        every { Exponea.handleRemoteMessage(any(), any(), any(), any()) } returns false
        every { Exponea.trackClickedPush(any(), any(), any()) } just Runs
        every { Exponea.trackClickedPushWithoutTrackingConsent(any(), any()) } just Runs
        every { Exponea.handleCampaignIntent(any(), any()) } returns false
        every { Exponea.handleNewHmsToken(any(), any()) } just Runs
        every { Exponea.handleNewToken(any(), any()) } just Runs
        every { Exponea.isExponeaPushNotification(any()) } returns false
        every { Exponea.trackPushToken(any()) } just Runs
        every { Exponea.trackHmsPushToken(any()) } just Runs
        every { Exponea.trackDeliveredPush(any(), any()) } just Runs
        every { Exponea.trackDeliveredPushWithoutTrackingConsent(any(), any()) } just Runs
        every { Exponea.getAppInboxButton(any()) } returns Button(
            ApplicationProvider.getApplicationContext()
        )
        every { Exponea.trackAppInboxClick(any(), any()) } just Runs
        every { Exponea.trackAppInboxClickWithoutTrackingConsent(any(), any()) } just Runs
        every { Exponea.trackAppInboxOpened(any()) } just Runs
        every { Exponea.fetchAppInbox(any()) } just Runs
        every { Exponea.fetchAppInboxItem(any(), any()) } just Runs
        every { Exponea.markAppInboxAsRead(any(), any()) } just Runs
        every { Exponea.getConsents(any(), any()) } just Runs
        every { Exponea.fetchRecommendation(any(), any(), any()) } just Runs
        instance = BloomreachSdkAndroid(ApplicationProvider.getApplicationContext())
    }

    @After
    fun afterTest() {
        unmockkAll()
    }

    fun readTestFile(fileName: String): String {
        return File(mauiTestJsonDir, "${fileName}.json")
            .readText(StandardCharsets.UTF_8)
    }
}