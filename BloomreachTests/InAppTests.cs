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
    
    [Test]
    public void ParseInAppMessage()
    {
        var data = new Dictionary<string, object>()
        {
            {"id", "id"},
            {"name", "name"},
            {"rawMessageType", "rawMessageType"},
            {"rawFrequency", "rawFrequency"},
            {"variantId", 1},
            {"variantName", "name"},
            {"eventType", "event type"},
            {"priority", 1},
            {"delayMs", 10},
            {"timeoutMs", 20},
            {"payloadHtml", "payloadHtml"},
            {"isHtml", false},
            {"rawHasTrackingConsent", false},
            {"consentCategoryTracking", "category"}
        };
        var parsedMessage = new InAppMessage(data);
        var messageExpected = new InAppMessage(
            id: "id",
            name: "name",
            rawMessageType: "rawMessageType",
            rawFrequency: "rawFrequency",
            variantId: 1,
            variantName: "name",
            eventType: "event type",
            priority: 1,
            delayMs: 10,
            timeoutMs: 20,
            payloadHtml: "payloadHtml",
            isHtml: false,
            rawHasTrackingConsent: false,
            consentCategoryTracking: "category"
        );
        Assert.That(parsedMessage.Id, Is.EqualTo(messageExpected.Id));
        Assert.That(parsedMessage.Name, Is.EqualTo(messageExpected.Name));
        Assert.That(parsedMessage.RawMessageType, Is.EqualTo(messageExpected.RawMessageType));
        Assert.That(parsedMessage.RawFrequency, Is.EqualTo(messageExpected.RawFrequency));
        Assert.That(parsedMessage.VariantId, Is.EqualTo(messageExpected.VariantId));
        Assert.That(parsedMessage.VariantName, Is.EqualTo(messageExpected.VariantName));
        Assert.That(parsedMessage.EventType, Is.EqualTo(messageExpected.EventType));
        Assert.That(parsedMessage.Priority, Is.EqualTo(messageExpected.Priority));
        Assert.That(parsedMessage.DelayMS, Is.EqualTo(messageExpected.DelayMS));
        Assert.That(parsedMessage.TimeoutMS, Is.EqualTo(messageExpected.TimeoutMS));
        Assert.That(parsedMessage.PayloadHtml, Is.EqualTo(messageExpected.PayloadHtml));
        Assert.That(parsedMessage.IsHtml, Is.EqualTo(messageExpected.IsHtml));
        Assert.That(parsedMessage.RawHasTrackingConsent, Is.EqualTo(messageExpected.RawHasTrackingConsent));
        Assert.That(parsedMessage.ConsentCategoryTracking, Is.EqualTo(messageExpected.ConsentCategoryTracking));
    }
}

