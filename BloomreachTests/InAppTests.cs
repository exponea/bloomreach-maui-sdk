using BloomreachTests.Utils;
using Bloomreach;

namespace BloomreachTests;

public class InAppTests
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
    public void In_App_Action_Click()
    {
        InAppMessage message = new InAppMessage(
            id: "id",
            name: "name",
            rawMessageType: "rawMessageType",
            rawFrequency: "rawFrequency",
            variantId: 1,
            variantName: "name",
            eventType: "event type",
            priority: 1,
            delayMs: 0,
            timeoutMs: 0,
            payloadHtml: "payloadHtml",
            isHtml: false,
            rawHasTrackingConsent: false,
            consentCategoryTracking: "category"
        );
        BloomreachSDK.TrackInAppMessageClick(message: message, buttonText: "button text", buttonLink: "buttonLink");
        _methodCollector.VerifyMethodCalled("TrackInAppMessageClick");
        var methodInput = _methodCollector.FindMethodInput("TrackInAppMessageClick");
        _methodCollector.VerifyMethodInput("TrackInAppMessageClick", TestUtils.ReadFile("TrackInAppMessageClick"));
    }
    
    [Test]
    public void In_App_Action_Close()
    {
        InAppMessage message = new InAppMessage(
            id: "id",
            name: "name",
            rawMessageType: "rawMessageType",
            rawFrequency: "rawFrequency",
            variantId: 1,
            variantName: "name",
            eventType: "event type",
            priority: 1,
            delayMs: 0,
            timeoutMs: 0,
            payloadHtml: "payloadHtml",
            isHtml: false,
            rawHasTrackingConsent: false,
            consentCategoryTracking: "category"
        );
        BloomreachSDK.TrackInAppMessageClose(message: message, isUserInteraction: true);
        _methodCollector.VerifyMethodCalled("TrackInAppMessageClose");
        var methodInput = _methodCollector.FindMethodInput("TrackInAppMessageClose");
        _methodCollector.VerifyMethodInput("TrackInAppMessageClose", TestUtils.ReadFile("TrackInAppMessageClose"));
    }
}

