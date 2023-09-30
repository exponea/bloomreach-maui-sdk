namespace Bloomreach;

public class ConsentSources
{
    public ConsentSources(bool createdFromCrm, bool imported, bool fromConsentPage, bool privateApi, bool publicApi,
        bool trackedFromScenario)
    {
        CreatedFromCrm = createdFromCrm;
        Imported = imported;
        FromConsentPage = fromConsentPage;
        PrivateApi = privateApi;
        PublicApi = publicApi;
        TrackedFromScenario = trackedFromScenario;
    }

    public bool CreatedFromCrm { get; set; }
    public bool Imported { get; set; }
    public bool FromConsentPage { get; set; }
    public bool PrivateApi { get; set; }
    public bool PublicApi { get; set; }
    public bool TrackedFromScenario { get; set; }
}