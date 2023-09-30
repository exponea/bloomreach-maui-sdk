package com.bloomreach.sdk.maui.android

import com.exponea.sdk.Exponea
import com.exponea.sdk.models.Consent
import com.exponea.sdk.models.ConsentSources
import com.exponea.sdk.models.CustomerRecommendation
import com.exponea.sdk.models.Result
import com.google.gson.JsonPrimitive
import io.mockk.every
import org.junit.Assert.assertEquals
import org.junit.Assert.assertTrue
import org.junit.Test
import org.junit.runner.RunWith
import org.robolectric.RobolectricTestRunner

@RunWith(RobolectricTestRunner::class)
class FetchApiTests : TestsBase() {

    @Test
    fun FetchConsents_Empty() {
        every { Exponea.getConsents(any(), any()) } answers {
            firstArg<(Result<ArrayList<Consent>>) -> Unit>().invoke(
                Result(
                    success = true,
                    results = arrayListOf()
                )
            )
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchConsents", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchConsents_Empty"))
    }

    @Test
    fun FetchConsents_Single() {
        every { Exponea.getConsents(any(), any()) } answers {
            firstArg<(Result<ArrayList<Consent>>) -> Unit>().invoke(Result(
                success = true,
                results = arrayListOf(
                    buildConsent("12345")
                )
            ))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchConsents", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchConsents_Single"))
    }

    @Test
    fun FetchConsents_Multiple() {
        every { Exponea.getConsents(any(), any()) } answers {
            firstArg<(Result<ArrayList<Consent>>) -> Unit>().invoke(Result(
                success = true,
                results = arrayListOf(
                    buildConsent("12345"),
                    buildConsent("67890")
                )
            ))
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchConsents", null) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchConsents_Multiple"))
    }

    @Test
    fun FetchRecommendation_Empty() {
        every { Exponea.fetchRecommendation(any(), any(), any()) } answers {
            secondArg<(Result<ArrayList<CustomerRecommendation>>) -> Unit>().invoke(
                Result(
                    success = true,
                    results = arrayListOf()
                )
            )
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchRecommendation", readTestFile("FetchRecommendation_Input")) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchRecommendation_Empty"))
    }

    @Test
    fun FetchRecommendation_Single() {
        every { Exponea.fetchRecommendation(any(), any(), any()) } answers {
            secondArg<(Result<ArrayList<CustomerRecommendation>>) -> Unit>().invoke(
                Result(
                    success = true,
                    results = arrayListOf(
                        buildRecommendation("12345")
                    )
                )
            )
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchRecommendation", readTestFile("FetchRecommendation_Input")) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchRecommendation_Single"))
    }

    @Test
    fun FetchRecommendation_Multiple() {
        every { Exponea.fetchRecommendation(any(), any(), any()) } answers {
            secondArg<(Result<ArrayList<CustomerRecommendation>>) -> Unit>().invoke(
                Result(
                    success = true,
                    results = arrayListOf(
                        buildRecommendation("12345"),
                        buildRecommendation("xyzab")
                    )
                )
            )
        }
        var result: MethodResult? = null
        instance.invokeMethodAsync("FetchRecommendation", readTestFile("FetchRecommendation_Input")) {
            result = it
        }
        assertTrue(result!!.success)
        assertEquals(result!!.data, readTestFile("FetchRecommendation_Multiple"))
    }

    private fun buildRecommendation(itemId: String): CustomerRecommendation {
        return CustomerRecommendation(
            engineName = "eng",
            itemId = itemId,
            recommendationId = "67890",
            recommendationVariantId = "abcde",
            data = mapOf(
                "prop1" to JsonPrimitive("val"),
                "prop2" to JsonPrimitive(2)
            )
        )
    }

    private fun buildConsent(id: String): Consent {
        return Consent(
            id = id,
            legitimateInterest = true,
            sources = ConsentSources(
                createdFromCRM = true,
                imported = false,
                fromConsentPage = true,
                privateAPI = false,
                publicAPI = true,
                trackedFromScenario = false
            ),
            translations = hashMapOf(
                "en" to hashMapOf(
                    "text" to "val"
                )
            )
        )
    }
}
