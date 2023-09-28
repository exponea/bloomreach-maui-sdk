//
//  Recommendation.swift
//  BloomreachSDKMauiIOS
//
//  Created by Ankmara on 02.09.2023.
//

public struct Recommendation {
    public let id: String
    public let fillWithRandom: Bool
    public let size: Int
    public let items: [String: String]?
    public let noTrack: Bool
    public let catalogAttributesWhitelist: [String]?

    public init(data: [String: Any]) {
        id = .assertValueFromDict(data: data, key: "id")
        fillWithRandom = .assertValueFromDict(data: data, key: "fillWithRandom")
        size = .assertValueFromDict(data: data, key: "size")
        items = data["items"] as? [String: String]
        noTrack = .assertValueFromDict(data: data, key: "noTrack")
        catalogAttributesWhitelist = data["catalogAttributesWhitelist"] as? [String]
    }
}
