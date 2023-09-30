namespace Bloomreach;

public class CustomerRecommendation
{
    public CustomerRecommendation(string engineName, string itemId, string recommendationId,
        string? recommendationVariantId, Dictionary<string, object> data)
    {
        EngineName = engineName;
        ItemId = itemId;
        RecommendationId = recommendationId;
        RecommendationVariantId = recommendationVariantId;
        Data = data;
    }

    public string EngineName { get; set; }
    public string ItemId { get; set; }
    public string RecommendationId { get; set; }
    public string? RecommendationVariantId { get; set; }
    public Dictionary<string, object> Data { get; set; }
}