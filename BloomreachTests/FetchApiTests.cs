using Bloomreach;
using BloomreachTests.Utils;

namespace BloomreachTests;

public class FetchApiTests
{

    private readonly MethodInvokeCollector _methodCollector = new MethodInvokeCollector();
    
    [SetUp]
    public void Setup()
    {
        _methodCollector.RegisterDefaultMethodResult(new MethodMauiResult(
            success: true,
            data: "",
            error: ""
        ));
        BloomreachSDK.Instance = new BloomreachSdkMock(_methodCollector);
    }
    [TearDown]
    public void TearDown()
    {
        _methodCollector.Clear();
    }

    [Test]
    public void FetchConsents_Failure()
    {
        _methodCollector.RegisterFailureMethodResult("FetchConsents", "Not init sdk");
        var task = BloomreachSDK.FetchConsents();
        _methodCollector.VerifyMethodCalled("FetchConsents");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.Empty
        );
    }

    [Test]
    public void FetchConsents_Empty()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchConsents", TestUtils.ReadFile("FetchConsents_Empty"));
        var task = BloomreachSDK.FetchConsents();
        _methodCollector.VerifyMethodCalled("FetchConsents");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(0));
    }

    [Test]
    public void FetchConsents_Single()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchConsents", TestUtils.ReadFile("FetchConsents_Single"));
        var task = BloomreachSDK.FetchConsents();
        _methodCollector.VerifyMethodCalled("FetchConsents");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(1));
        var consent = task.Result![0];
        Assert.That(consent.Id, Is.EqualTo("12345"));
        Assert.That(consent.Translations.Count, Is.EqualTo(1));
        Assert.That(consent.Translations["en"], Is.Not.Null);
        Assert.That(consent.Translations["en"]["text"], Is.EqualTo("val"));
        Assert.That(consent.LegitimateInterest, Is.EqualTo(true));
        Assert.That(consent.Sources.Imported, Is.EqualTo(false));
        Assert.That(consent.Sources.FromConsentPage, Is.EqualTo(true));
        Assert.That(consent.Sources.PrivateApi, Is.EqualTo(false));
        Assert.That(consent.Sources.PublicApi, Is.EqualTo(true));
        Assert.That(consent.Sources.CreatedFromCrm, Is.EqualTo(true));
        Assert.That(consent.Sources.TrackedFromScenario, Is.EqualTo(false));
    }

    [Test]
    public void FetchConsents_Multiple()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchConsents", TestUtils.ReadFile("FetchConsents_Multiple"));
        var task = BloomreachSDK.FetchConsents();
        _methodCollector.VerifyMethodCalled("FetchConsents");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(2));
        Assert.That(task.Result![0].Id, Is.EqualTo("12345"));
        Assert.That(task.Result![1].Id, Is.EqualTo("67890"));
    }
    
    [Test]
    public void FetchRecommendation_Input()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchRecommendation", TestUtils.ReadFile("FetchRecommendation_Empty"));
        var options = new CustomerRecommendationOptions(
            id:"12345",
            fillWithRandom:true,
            size:2,
            items:new Dictionary<string, string>()
            {
                {"prop", "val"}
            },
            noTrack:false,
            catalogAttributesWhitelist:new List<string>()
            {
                "cat1", "cat2"
            }
        );
        BloomreachSDK.FetchRecommendation(options);
        _methodCollector.VerifyMethodCalled("FetchRecommendation");
        _methodCollector.VerifyMethodInput("FetchRecommendation", TestUtils.ReadFile("FetchRecommendation_Input"));
    }
    
    [Test]
    public void FetchRecommendation_Error()
    {
        _methodCollector.RegisterFailureUiMethodResult("FetchRecommendation", "Not init sdk");
        var options = new CustomerRecommendationOptions(id: "12345", fillWithRandom: true, size: 2,
            items: new Dictionary<string, string>(), noTrack: false, catalogAttributesWhitelist: new List<string>());
        var task = BloomreachSDK.FetchRecommendation(options);
        _methodCollector.VerifyMethodCalled("FetchRecommendation");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void FetchRecommendation_Empty()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchRecommendation", TestUtils.ReadFile("FetchRecommendation_Empty"));
        var options = new CustomerRecommendationOptions(id: "12345", fillWithRandom: true, size: 2,
            items: new Dictionary<string, string>(), noTrack: false, catalogAttributesWhitelist: new List<string>());
        var task = BloomreachSDK.FetchRecommendation(options);
        _methodCollector.VerifyMethodCalled("FetchRecommendation");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void FetchRecommendation_Single()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchRecommendation", TestUtils.ReadFile("FetchRecommendation_Single"));
        var options = new CustomerRecommendationOptions(id: "12345", fillWithRandom: true, size: 2,
            items: new Dictionary<string, string>(), noTrack: false, catalogAttributesWhitelist: new List<string>());
        var task = BloomreachSDK.FetchRecommendation(options);
        _methodCollector.VerifyMethodCalled("FetchRecommendation");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(1));
        var recommendation = task.Result![0];
        Assert.That(recommendation.EngineName, Is.EqualTo("eng"));
        Assert.That(recommendation.ItemId, Is.EqualTo("12345"));
        Assert.That(recommendation.RecommendationId, Is.EqualTo("67890"));
        Assert.That(recommendation.RecommendationVariantId, Is.EqualTo("abcde"));
        Assert.That(recommendation.Data.Count, Is.EqualTo(2));
        Assert.That(recommendation.Data["prop1"], Is.EqualTo("val"));
        Assert.That(recommendation.Data["prop2"], Is.EqualTo(2));
    }
    
    [Test]
    public void FetchRecommendation_Multiple()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchRecommendation", TestUtils.ReadFile("FetchRecommendation_Multiple"));
        var options = new CustomerRecommendationOptions(id: "12345", fillWithRandom: true, size: 2,
            items: new Dictionary<string, string>(), noTrack: false, catalogAttributesWhitelist: new List<string>());
        var task = BloomreachSDK.FetchRecommendation(options);
        _methodCollector.VerifyMethodCalled("FetchRecommendation");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(task.Result, Is.Not.Null);
        Assert.That(task.Result?.Count, Is.EqualTo(2));
        Assert.That(task.Result![0].ItemId, Is.EqualTo("12345"));
        Assert.That(task.Result![1].ItemId, Is.EqualTo("xyzab"));
    }
    
}
