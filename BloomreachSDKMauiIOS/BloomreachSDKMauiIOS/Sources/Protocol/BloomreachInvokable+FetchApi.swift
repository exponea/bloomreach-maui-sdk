//
//  BloomreachInvokable+FetchApi.swift
//  BloomreachSDKMauiIOS
//
//  Created by Adam Mihalik on 30/09/2023.
//

import Foundation
import ExponeaSDK
import AnyCodable

// MARK: - FetchAPI

extension ConsentSources {
    var json: [String: Any] {
        return [
            "createdFromCrm": self.isCreatedFromCRM,
            "imported": self.isImported,
            "fromConsentPage": self.isFromConsentPage,
            "privateApi": self.privateAPI,
            "publicApi": self.publicAPI,
            "trackedFromScenario": self.isTrackedFromScenario
        ]
    }
}

extension Consent {
    var json: [String: Any] {
        return [
            "id": self.id,
            "legitimateInterest": self.legitimateInterest,
            "sources": self.sources.json,
            "translations": self.translations
        ]
    }
}

extension ExponeaSDK.Recommendation<AllRecommendationData> {
    var json: [String: Any] {
        var result: [String: Any] = [
            "engineName": self.systemData.engineName,
            "itemId": self.systemData.itemId,
            "recommendationId": self.systemData.recommendationId,
            "data": self.userData.data
        ]
        if let recommendationVariantId = self.systemData.recommendationVariantId {
            result["recommendationVariantId"] = recommendationVariantId
        }
        return result
    }
}

private struct AllRecommendationData: RecommendationUserData {
    public let data: [String: Any]

    public init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: AnyKey.self)
        var data: [String: Any] = [:]
        for key in container.allKeys {
            data[key.stringValue] = (try container.decode(AnyCodable.self, forKey: key)).value
        }
        self.data = data
    }

    public func encode(to encoder: Encoder) throws {
        var container = encoder.container(keyedBy: AnyKey.self)
        for key in data.keys {
            try container.encode(AnyCodable(data[key]), forKey: AnyKey(stringValue: key))
        }
    }

    public static func == (lhs: AllRecommendationData, rhs: AllRecommendationData) -> Bool {
        return AnyCodable(lhs.data) == AnyCodable(rhs.data)
    }

    struct AnyKey: CodingKey {
        var intValue: Int?

        init(intValue: Int) {
            self.stringValue = "\(intValue)"
            self.intValue = intValue
        }

        var stringValue: String
        init(stringValue: String) {
            self.stringValue = stringValue
        }
    }
}

public extension BloomreachInvokable {
    func fetchConsents(completion: TypeBlock<MethodResult>?) {
        exponeaSDK.fetchConsents { consentsResult in
            guard let consents = consentsResult.value?.consents else {
                completion?(.failure("Fetching of Consents ends with fail, see logs"))
                return
            }
            let mauiResult = consents.map { consent in consent.json }
            completion?(.success(mauiResult.json))
        }
    }

    func fetchRecommendation(optionsData: [String: Any], completion: TypeBlock<MethodResult>?) {
        let optionsParsing = parseRecommendationOptions(optionsData)
        guard let options = optionsParsing.options else {
            completion?(optionsParsing.error ?? .failure("Fetching of Recommendation received invalid options, see logs"))
            return
        }
        exponeaSDK.fetchRecommendation(with: options) { (recomendationResult: Result<RecommendationResponse<AllRecommendationData>>) in
            switch recomendationResult {
            case .success(let response):
                guard let data = response.value else {
                    completion?(.failure("Fetching of Recommendation ends with fail, see logs"))
                    return
                }
                let mauiResult = data.map { recommendation in recommendation.json }
                completion?(.success(mauiResult.json))
            case .failure(let error):
                completion?(.failure(error.localizedDescription))
            }
        }
    }

    private func parseRecommendationOptions(_ source: [String: Any]) -> (options: RecommendationOptions?, error: MethodResult?) {
        guard let id = source["id"] as? String else {
            return (nil, .failure("Invalid value '\(String(describing: source["id"]))' for 'id'."))
        }
        guard let fillWithRandom = source["fillWithRandom"] as? Bool else {
            return (nil, .failure("Invalid value '\(String(describing: source["fillWithRandom"]))' for 'fillWithRandom'."))
        }
        guard let sizeDouble = source["size"] as? Double else {
            return (nil, .failure("Invalid value '\(String(describing: source["size"]))' for 'size'."))
        }
        return (RecommendationOptions(
            id: id,
            fillWithRandom: fillWithRandom,
            size: Int(sizeDouble),
            items: source["items"] as? [String: String],
            noTrack: source["noTrack"] as? Bool ?? false,
            catalogAttributesWhitelist: source["catalogAttributesWhitelist"] as? [String]
        ), nil)
    }
}
