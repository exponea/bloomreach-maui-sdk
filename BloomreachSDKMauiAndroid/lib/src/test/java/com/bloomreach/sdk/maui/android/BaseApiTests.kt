package com.bloomreach.sdk.maui.android

import androidx.test.core.app.ApplicationProvider
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.CustomerIds
import com.exponea.sdk.models.EventType
import com.exponea.sdk.models.ExponeaConfiguration
import com.exponea.sdk.models.ExponeaProject
import com.exponea.sdk.models.FlushMode
import com.exponea.sdk.models.FlushPeriod
import com.exponea.sdk.models.PropertiesList
import com.exponea.sdk.util.Logger
import io.mockk.Runs
import io.mockk.every
import io.mockk.just
import io.mockk.mockkObject
import io.mockk.slot
import io.mockk.unmockkAll
import io.mockk.verify
import org.junit.After
import org.junit.Assert.assertEquals
import org.junit.Assert.assertFalse
import org.junit.Assert.assertNotNull
import org.junit.Assert.assertTrue
import org.junit.Before
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner
import java.io.File
import java.nio.charset.StandardCharsets
import java.util.concurrent.TimeUnit

@RunWith(RobolectricTestRunner::class)
class BaseApiTests {

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
    fun Anonymize_NoParams() {
        val result = instance.invokeMethod("Anonymize", readTestFile("Anonymize_NoParams_Input"))
        verify { Exponea.anonymize(null, null) }
        assertTrue(result.success)
    }

    @Test
    fun Anonymize_WithMappings() {
        val result =
            instance.invokeMethod("Anonymize", readTestFile("Anonymize_WithMappings_Input"))
        val mappings = mapOf(
            EventType.BANNER to listOf(
                ExponeaProject(
                    projectToken = "projToken",
                    authorization = "authToken",
                    baseUrl = "https://url.com"
                )
            )
        )
        verify {
            Exponea.anonymize(null, mappings)
        }
        assertTrue(result.success)
    }

    @Test
    fun Anonymize_WithProject() {
        val result = instance.invokeMethod("Anonymize", readTestFile("Anonymize_WithProject_Input"))
        verify {
            Exponea.anonymize(
                ExponeaProject(
                    projectToken = "projToken",
                    authorization = "authToken",
                    baseUrl = "https://url.com"
                ), null
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun Anonymize_WithProjectAndMappings() {
        val result = instance.invokeMethod(
            "Anonymize",
            readTestFile("Anonymize_WithProjectAndMappings_Input")
        )
        verify {
            Exponea.anonymize(
                ExponeaProject(
                    projectToken = "projToken",
                    authorization = "authToken",
                    baseUrl = "https://url.com"
                ), mapOf(
                    EventType.BANNER to listOf(
                        ExponeaProject(
                            projectToken = "projToken",
                            authorization = "authToken",
                            baseUrl = "https://url.com"
                        )
                    )
                )
            )
        }
        assertTrue(result.success)
    }

    @Test
    fun Configure_EmptyConfiguration() {
        val result =
            instance.invokeMethod("Configure", readTestFile("Configure_EmptyConfiguration_Input"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com"
                )
            )
        }
    }

    @Test
    fun Configure_FullConfiguration() {
        val result = instance.invokeMethod("Configure", readTestFile("Configure_FullConfiguration_Input"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com",
                    advancedAuthEnabled = false,
                    allowDefaultCustomerProperties = false,
                    automaticPushNotification = false,
                    automaticSessionTracking = true,
                    campaignTTL = 100.0,
                    defaultProperties = hashMapOf(
                        "key" to "prop",
                        "key2" to 1.0,
                        "key3" to false
                    ),
                    httpLoggingLevel = ExponeaConfiguration.HttpLoggingLevel.BODY,
                    projectRouteMap = mapOf(
                        EventType.PAYMENT to listOf(
                            ExponeaProject(
                                projectToken = "projToken",
                                authorization = "authToken",
                                baseUrl = "https://url.com"
                            )
                        )
                    ),
                    maxTries = 3,
                    sessionTimeout = 30.4,
                    tokenTrackFrequency = ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH,
                    pushIcon = 0,
                    pushAccentColor = 10302,
                    pushChannelName = "notifs",
                    pushChannelDescription = "push desc",
                    pushChannelId = "pushChanId",
                    pushNotificationImportance = 2
                )
            )
        }
    }

    @Test
    fun Configure_Configuration_Variant_1() {
        val result =
            instance.invokeMethod("Configure", readTestFile("Configure_Configuration_Variant_1"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com",
                    httpLoggingLevel = ExponeaConfiguration.HttpLoggingLevel.NONE,
                    tokenTrackFrequency = ExponeaConfiguration.TokenFrequency.DAILY
                )
            )
        }
    }

    @Test
    fun Configure_Configuration_Variant_2() {
        val result =
            instance.invokeMethod("Configure", readTestFile("Configure_Configuration_Variant_2"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com",
                    httpLoggingLevel = ExponeaConfiguration.HttpLoggingLevel.BASIC,
                    tokenTrackFrequency = ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH
                )
            )
        }
    }

    @Test
    fun Configure_Configuration_Variant_3() {
        val result =
            instance.invokeMethod("Configure", readTestFile("Configure_Configuration_Variant_3"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com",
                    httpLoggingLevel = ExponeaConfiguration.HttpLoggingLevel.HEADERS,
                    tokenTrackFrequency = ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE
                )
            )
        }
    }

    @Test
    fun Configure_Configuration_Variant_4() {
        val result =
            instance.invokeMethod("Configure", readTestFile("Configure_Configuration_Variant_4"))
        assertTrue(result.success)
        verify {
            Exponea.init(
                any(), ExponeaConfiguration(
                    projectToken = "projToken",
                    authorization = "Token authToken",
                    baseURL = "https://url.com",
                    httpLoggingLevel = ExponeaConfiguration.HttpLoggingLevel.BODY,
                    tokenTrackFrequency = ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE
                )
            )
        }
    }

    @Test
    fun IsConfigured_False() {
        every { Exponea.isInitialized } returns false
        val result = instance.invokeMethod("IsConfigured", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("Boolean_False"))
    }

    @Test
    fun IsConfigured_True() {
        every { Exponea.isInitialized } returns true
        val result = instance.invokeMethod("IsConfigured", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("Boolean_True"))
    }

    @Test
    fun FlushData_Async_False() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.failure(NullPointerException("Developer likes this")))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FlushData", null, done = {
            result = it
        })
        assertFalse(result!!.success)
        assertEquals(result!!.data, readTestFile("Boolean_False"))
    }

    @Test
    fun FlushData_Async_True() {
        every { Exponea.flushData(any()) } answers {
            firstArg<(Result<Unit>) -> Unit>().invoke(Result.success(Unit))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FlushData", null, done = {
            result = it
        })
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("Boolean_True"))
    }

    @Test
    fun IdentifyCustomer_Registered() {
        val result =
            instance.invokeMethod("IdentifyCustomer", readTestFile("IdentifyCustomer_Registered"))
        assertTrue(result.success)
        verify {
            Exponea.identifyCustomer(
                CustomerIds(
                    hashMapOf(
                        "registered" to "id12345"
                    )
                ),
                PropertiesList(hashMapOf())
            )
        }
    }

    @Test
    fun IdentifyCustomer_CustomHardId() {
        val result =
            instance.invokeMethod("IdentifyCustomer", readTestFile("IdentifyCustomer_CustomHardId"))
        assertTrue(result.success)
        verify {
            Exponea.identifyCustomer(
                CustomerIds(
                    hashMapOf(
                        "login" to "id12345"
                    )
                ),
                PropertiesList(hashMapOf())
            )
        }
    }

    @Test
    fun IdentifyCustomer_MultipleHardIds() {
        val result = instance.invokeMethod(
            "IdentifyCustomer",
            readTestFile("IdentifyCustomer_MultipleHardIds")
        )
        assertTrue(result.success)
        verify {
            Exponea.identifyCustomer(
                CustomerIds(
                    hashMapOf(
                        "id1" to "12345",
                        "id2" to "123456"
                    )
                ),
                PropertiesList(hashMapOf())
            )
        }
    }

    @Test
    fun IdentifyCustomer_Registered_WithProperties() {
        val result = instance.invokeMethod(
            "IdentifyCustomer",
            readTestFile("IdentifyCustomer_Registered_WithProperties")
        )
        assertTrue(result.success)
        verify {
            Exponea.identifyCustomer(
                CustomerIds(
                    hashMapOf(
                        "registered" to "id12345"
                    )
                ),
                PropertiesList(
                    hashMapOf(
                        "prop1" to "val1",
                        "prop2" to 2.0,
                        "prop3" to 3.0
                    )
                )
            )
        }
    }

    @Test
    fun GetCustomerCookie_Empty() {
        every { Exponea.customerCookie } returns ""
        val result = instance.invokeMethod("GetCustomerCookie", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetCustomerCookie_Empty"))
    }

    @Test
    fun GetCustomerCookie_SomeNonnull() {
        every { Exponea.customerCookie } returns "123456-asdfgh-123456"
        val result = instance.invokeMethod("GetCustomerCookie", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetCustomerCookie_SomeNonnull"))
    }

    @Test
    fun GetDefaultProperties_Null() {
        every { Exponea.customerCookie } returns null
        val result = instance.invokeMethod("GetCustomerCookie", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetCustomerCookie_Empty"))
    }

    @Test
    fun GetDefaultProperties_EmptyDictionary() {
        every { Exponea.defaultProperties } returns hashMapOf()
        val result = instance.invokeMethod("GetDefaultProperties", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetDefaultProperties_EmptyDictionary"))
    }

    @Test
    fun GetDefaultProperties_NonEmptyDictionary_WithNull() {
        // Null cannot be added into hashmap, but we create this test just to have it
        every { Exponea.defaultProperties } returns hashMapOf(
            "prop1" to "val1",
            "prop2" to 2
        )
        val result = instance.invokeMethod("GetDefaultProperties", null)
        assertTrue(result.success)
        assertEquals(
            SerializeUtils.parseAsMap(result.data),
            SerializeUtils.parseAsMap(readTestFile("GetDefaultProperties_NonEmptyDictionary"))
        )
    }

    @Test
    fun GetFlushMode_Immediate() {
        every { Exponea.flushMode } returns FlushMode.IMMEDIATE
        val result = instance.invokeMethod("GetFlushMode", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushMode_Immediate"))
    }

    @Test
    fun GetFlushMode_Manual() {
        every { Exponea.flushMode } returns FlushMode.MANUAL
        val result = instance.invokeMethod("GetFlushMode", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushMode_Manual"))
    }

    @Test
    fun GetFlushMode_Period() {
        every { Exponea.flushMode } returns FlushMode.PERIOD
        val result = instance.invokeMethod("GetFlushMode", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushMode_Period"))
    }

    @Test
    fun GetFlushMode_AppClose() {
        every { Exponea.flushMode } returns FlushMode.APP_CLOSE
        val result = instance.invokeMethod("GetFlushMode", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushMode_AppClose"))
    }

    @Test
    fun GetFlushPeriod_Zero() {
        every { Exponea.flushPeriod } returns FlushPeriod(0, TimeUnit.SECONDS)
        val result = instance.invokeMethod("GetFlushPeriod", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushPeriod_Zero"))
    }

    @Test
    fun GetFlushPeriod_SomeValue() {
        every { Exponea.flushPeriod } returns FlushPeriod(10000, TimeUnit.MILLISECONDS)
        val result = instance.invokeMethod("GetFlushPeriod", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetFlushPeriod_SomeValue"))
    }

    @Test
    fun GetLogLevel_Error() {
        every { Exponea.loggerLevel } returns Logger.Level.ERROR
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Error"))
    }

    @Test
    fun GetLogLevel_Debug() {
        every { Exponea.loggerLevel } returns Logger.Level.DEBUG
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Debug"))
    }

    @Test
    fun GetLogLevel_Info() {
        every { Exponea.loggerLevel } returns Logger.Level.INFO
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Info"))
    }

    @Test
    fun GetLogLevel_Verbose() {
        every { Exponea.loggerLevel } returns Logger.Level.VERBOSE
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Verbose"))
    }

    @Test
    fun GetLogLevel_Off() {
        every { Exponea.loggerLevel } returns Logger.Level.OFF
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Off"))
    }

    @Test
    fun GetLogLevel_Warn() {
        every { Exponea.loggerLevel } returns Logger.Level.WARN
        val result = instance.invokeMethod("GetLogLevel", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetLogLevel_Warn"))
    }

    @Test
    fun GetSessionTimeout_Zero() {
        every { Exponea.sessionTimeout } returns 0.0
        val result = instance.invokeMethod("GetSessionTimeout", null)
        assertTrue(result.success)
        assertEquals(
            SerializeUtils.parseDouble(result.data),
            SerializeUtils.parseDouble(readTestFile("GetSessionTimeout_Zero")),
            0.1
        )
    }

    @Test
    fun GetSessionTimeout_SomeValue() {
        every { Exponea.sessionTimeout } returns 10000.0
        val result = instance.invokeMethod("GetSessionTimeout", null)
        assertTrue(result.success)
        assertEquals(
            SerializeUtils.parseDouble(result.data),
            SerializeUtils.parseDouble(readTestFile("GetSessionTimeout_SomeValue")),
            0.1
        )
    }

    @Test
    fun SetDefaultProperties_Empty() {
        val propsSlot = slot<HashMap<String, Any>>()
        every {
            Exponea.defaultProperties = capture(propsSlot)
        } just Runs
        val result = instance.invokeMethod(
            "SetDefaultProperties",
            readTestFile("SetDefaultProperties_Empty")
        )
        assertTrue(result.success)
        assertTrue(propsSlot.isCaptured)
        assertEquals(propsSlot.captured.size, 0)
    }

    @Test
    fun SetDefaultProperties_Simple() {
        val propsSlot = slot<HashMap<String, Any>>()
        every {
            Exponea.defaultProperties = capture(propsSlot)
        } just Runs
        val result = instance.invokeMethod(
            "SetDefaultProperties",
            readTestFile("SetDefaultProperties_Simple")
        )
        assertTrue(result.success)
        assertTrue(propsSlot.isCaptured)
        assertEquals(propsSlot.captured.size, 2)
        assertTrue(propsSlot.captured.containsKey("defProp1"))
        assertTrue(propsSlot.captured.containsKey("defProp2"))
        assertEquals(propsSlot.captured["defProp1"], "defVal1")
        assertEquals(propsSlot.captured["defProp2"], 2.0)
    }

    @Test
    fun SetFlushPeriod_Negative() {
        val slot = slot<FlushPeriod>()
        every {
            Exponea.flushPeriod = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetFlushPeriod", readTestFile("SetFlushPeriod_Negative"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, FlushPeriod(-10000, TimeUnit.MILLISECONDS))
    }

    @Test
    fun SetFlushPeriod_Zero() {
        val slot = slot<FlushPeriod>()
        every {
            Exponea.flushPeriod = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetFlushPeriod", readTestFile("SetFlushPeriod_Zero"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, FlushPeriod(0, TimeUnit.MILLISECONDS))
    }

    @Test
    fun SetFlushPeriod_Positive() {
        val slot = slot<FlushPeriod>()
        every {
            Exponea.flushPeriod = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetFlushPeriod", readTestFile("SetFlushPeriod_Positive"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, FlushPeriod(10000, TimeUnit.MILLISECONDS))
    }

    @Test
    fun SetLogLevel_Unknown() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Unknown"))
        assertFalse(result.success)
        assertFalse(slot.isCaptured)
    }

    @Test
    fun SetLogLevel_Debug() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Debug"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.DEBUG)
    }

    @Test
    fun SetLogLevel_Error() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Error"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.ERROR)
    }

    @Test
    fun SetLogLevel_Info() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Info"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.INFO)
    }

    @Test
    fun SetLogLevel_Verbose() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Verbose"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.VERBOSE)
    }

    @Test
    fun SetLogLevel_Off() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Off"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.OFF)
    }

    @Test
    fun SetLogLevel_Warn() {
        val slot = slot<Logger.Level>()
        every {
            Exponea.loggerLevel = capture(slot)
        } just Runs
        val result = instance.invokeMethod("SetLogLevel", readTestFile("SetLogLevel_Warn"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, Logger.Level.WARN)
    }

    @Test
    fun SetSessionTimeout_Negative() {
        val slot = slot<Double>()
        every {
            Exponea.sessionTimeout = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetSessionTimeout", readTestFile("SetSessionTimeout_Negative"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, -10000.0, 0.1)
    }

    @Test
    fun SetSessionTimeout_Zero() {
        val slot = slot<Double>()
        every {
            Exponea.sessionTimeout = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetSessionTimeout", readTestFile("SetSessionTimeout_Zero"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, 0.0, 0.1)
    }

    @Test
    fun SetSessionTimeout_Positive() {
        val slot = slot<Double>()
        every {
            Exponea.sessionTimeout = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetSessionTimeout", readTestFile("SetSessionTimeout_Positive"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, 10000.0, 0.1)
    }

    @Test
    fun GetCheckPushSetup_False() {
        every { Exponea.checkPushSetup } returns false
        val result = instance.invokeMethod("GetCheckPushSetup", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetCheckPushSetup_False"))
    }

    @Test
    fun GetCheckPushSetup_True() {
        every { Exponea.checkPushSetup } returns true
        val result = instance.invokeMethod("GetCheckPushSetup", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetCheckPushSetup_True"))
    }

    @Test
    fun GetTokenTrackFrequency_OnTokenChange() {
        every { Exponea.tokenTrackFrequency } returns ExponeaConfiguration.TokenFrequency.ON_TOKEN_CHANGE
        val result = instance.invokeMethod("GetTokenTrackFrequency", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetTokenTrackFrequency_OnTokenChange"))
    }

    @Test
    fun GetTokenTrackFrequency_EveryLaunch() {
        every { Exponea.tokenTrackFrequency } returns ExponeaConfiguration.TokenFrequency.EVERY_LAUNCH
        val result = instance.invokeMethod("GetTokenTrackFrequency", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetTokenTrackFrequency_EveryLaunch"))
    }

    @Test
    fun GetTokenTrackFrequency_Daily() {
        every { Exponea.tokenTrackFrequency } returns ExponeaConfiguration.TokenFrequency.DAILY
        val result = instance.invokeMethod("GetTokenTrackFrequency", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("GetTokenTrackFrequency_Daily"))
    }

    @Test
    fun IsAutomaticSessionTracking_False() {
        every { Exponea.isAutomaticSessionTracking } returns false
        val result = instance.invokeMethod("IsAutomaticSessionTracking", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("IsAutomaticSessionTracking_False"))
    }

    @Test
    fun IsAutomaticSessionTracking_True() {
        every { Exponea.isAutomaticSessionTracking } returns true
        val result = instance.invokeMethod("IsAutomaticSessionTracking", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("IsAutomaticSessionTracking_True"))
    }

    @Test
    fun IsAutoPushNotification_False() {
        every { Exponea.isAutoPushNotification } returns false
        val result = instance.invokeMethod("IsAutoPushNotification", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("IsAutoPushNotification_False"))
    }

    @Test
    fun IsAutoPushNotification_True() {
        every { Exponea.isAutoPushNotification } returns true
        val result = instance.invokeMethod("IsAutoPushNotification", null)
        assertTrue(result.success)
        assertEquals(result.data, readTestFile("IsAutoPushNotification_True"))
    }

    @Test
    fun SetCheckPushSetup_Disable() {
        val slot = slot<Boolean>()
        every {
            Exponea.checkPushSetup = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetCheckPushSetup", readTestFile("SetCheckPushSetup_Disable"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, false)
    }

    @Test
    fun SetCheckPushSetup_Enable() {
        val slot = slot<Boolean>()
        every {
            Exponea.checkPushSetup = capture(slot)
        } just Runs
        val result =
            instance.invokeMethod("SetCheckPushSetup", readTestFile("SetCheckPushSetup_Enable"))
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, true)
    }

    @Test
    fun SetAutomaticSessionTracking_Disabled() {
        val slot = slot<Boolean>()
        every {
            Exponea.isAutomaticSessionTracking = capture(slot)
        } just Runs
        val result = instance.invokeMethod(
            "SetAutomaticSessionTracking",
            readTestFile("SetAutomaticSessionTracking_Disabled")
        )
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, false)
    }

    @Test
    fun SetAutomaticSessionTracking_Enabled() {
        val slot = slot<Boolean>()
        every {
            Exponea.isAutomaticSessionTracking = capture(slot)
        } just Runs
        val result = instance.invokeMethod(
            "SetAutomaticSessionTracking",
            readTestFile("SetAutomaticSessionTracking_Enabled")
        )
        assertTrue(result.success)
        assertTrue(slot.isCaptured)
        assertEquals(slot.captured, true)
    }
}
