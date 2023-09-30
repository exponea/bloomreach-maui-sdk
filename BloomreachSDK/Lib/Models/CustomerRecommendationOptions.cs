namespace Bloomreach;

public class CustomerRecommendationOptions
{
    public CustomerRecommendationOptions(string id, bool? fillWithRandom, int size, Dictionary<string, string>? items,
        bool? noTrack, List<string>? catalogAttributesWhitelist)
    {
        Id = id;
        FillWithRandom = fillWithRandom;
        Size = size;
        Items = items;
        NoTrack = noTrack;
        CatalogAttributesWhitelist = catalogAttributesWhitelist;
    }

    public string Id { get; set; }
    public bool? FillWithRandom { get; set; }
    public int Size { get; set; }
    public Dictionary<string, string>? Items { get; set; }
    public bool? NoTrack { get; set; }
    public List<string>? CatalogAttributesWhitelist { get; set; }
}