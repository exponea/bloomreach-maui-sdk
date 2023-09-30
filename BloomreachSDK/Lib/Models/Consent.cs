namespace Bloomreach;

public class Consent
{
    public Consent(string id, bool legitimateInterest, ConsentSources sources,
        Dictionary<string, Dictionary<string, string>> translations)
    {
        Id = id;
        LegitimateInterest = legitimateInterest;
        Sources = sources;
        Translations = translations;
    }

    public string Id { get; set; }
    public bool LegitimateInterest { get; set; }
    public ConsentSources Sources { get; set; }
    public Dictionary<string, Dictionary<string, string>> Translations { get; set; }
}