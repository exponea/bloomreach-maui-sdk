import com.bloomreach.sdk.maui.android.BloomreachSdkAndroid
import com.bloomreach.sdk.maui.android.MethodResult
import com.bloomreach.sdk.maui.android.util.SerializeUtils
import com.bloomreach.sdk.maui.android.util.getNullSafely
import com.bloomreach.sdk.maui.android.util.getNullSafelyArray
import com.bloomreach.sdk.maui.android.util.getNullSafelyMap
import com.bloomreach.sdk.maui.android.util.getRequired
import com.exponea.sdk.Exponea
import com.exponea.sdk.models.Consent
import com.exponea.sdk.models.ConsentSources
import com.exponea.sdk.models.CustomerRecommendation
import com.exponea.sdk.models.CustomerRecommendationOptions
import com.exponea.sdk.util.logOnException

internal fun BloomreachSdkAndroid.fetchConsents(done: (MethodResult) -> Unit) {
    Exponea.getConsents(
        onSuccess = { consentsResult ->
            runCatching {
                if (consentsResult.success != true) {
                    done(MethodResult.failure("Fetching of Consents ends with fail, see logs"))
                    return@runCatching
                }
                val result = consentsResult.results
                    .map { item -> item.toMap() }
                done(MethodResult.success(SerializeUtils.serializeData(result)))
            }.logOnException()
        },
        onFailure = {
            runCatching {
                done(MethodResult.failure("Fetch Consents request failed, see logs"))
            }.logOnException()
        }
    )
}

internal fun BloomreachSdkAndroid.fetchRecommendation(
    optionsData: Map<String, Any?>,
    done: (MethodResult) -> Unit
) {
    val options = CustomerRecommendationOptions(
        id = optionsData.getRequired("id"),
        fillWithRandom = optionsData.getRequired("fillWithRandom"),
        size = optionsData.getRequired<Double>("size").toInt(),
        items = optionsData.getNullSafelyMap("items"),
        noTrack = optionsData.getNullSafely("noTrack"),
        catalogAttributesWhitelist = optionsData.getNullSafelyArray("catalogAttributesWhitelist")
    )
    Exponea.fetchRecommendation(
        options,
        onSuccess = { recommendationResult ->
            runCatching {
                if (recommendationResult.success != true) {
                    done(MethodResult.failure("Fetching of Recommendation ends with fail, see logs"))
                    return@runCatching
                }
                val result = recommendationResult.results
                    .map { item -> item.toMap() }
                done(MethodResult.success(SerializeUtils.serializeData(result)))
            }.logOnException()
        },
        onFailure = {
            runCatching {
                done(MethodResult.failure("Fetch Recommendation request failed, see logs"))
            }.logOnException()
        }
    )
}

private fun CustomerRecommendation.toMap(): Map<String, Any> {
    val target = mutableMapOf(
        "engineName" to this.engineName,
        "itemId" to this.itemId,
        "recommendationId" to this.recommendationId,
        "data" to this.data.filterValues { !it.isJsonNull }
    )
    this.recommendationVariantId?.let {
        target.put("recommendationVariantId", it)
    }
    return target
}

private fun Consent.toMap(): Map<String, Any> {
    return mapOf(
        "id" to this.id,
        "legitimateInterest" to this.legitimateInterest,
        "sources" to this.sources.toMap(),
        "translations" to this.translations
    )
}

private fun ConsentSources.toMap(): Map<String, Any> {
    return mapOf(
        "createdFromCrm" to this.createdFromCRM,
        "imported" to this.imported,
        "fromConsentPage" to this.fromConsentPage,
        "privateApi" to this.privateAPI,
        "publicApi" to this.publicAPI,
        "trackedFromScenario" to this.trackedFromScenario

    )
}