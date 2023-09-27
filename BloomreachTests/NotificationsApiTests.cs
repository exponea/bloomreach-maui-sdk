using BloomreachTests.Utils;
using Bloomreach;
using Bloomreach.Utils;

namespace BloomreachTests;

public class NotificationsApiTests
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
    public void HandlePushNotificationOpened()
    {
        var notificationAction = new NotificationAction(
                "button",
                "Click me",
                "https://google.com"
            )
            .WithAttribute("prop1", "val1")
            .WithAttribute("prop2", 2)
            .WithAttribute("prop3", null)
            .WithAttribute("prop2", "override");
        BloomreachSDK.HandlePushNotificationOpened(notificationAction);
        _methodCollector.VerifyMethodCalled("HandlePushNotificationOpened");
        _methodCollector.VerifyMethodInput(
            "HandlePushNotificationOpened",
            TestUtils.ReadFile("HandlePushNotificationOpened")
        );
    }
    
    [Test]
    public void HandlePushNotificationOpenedWithoutTrackingConsent()
    {
        var notificationAction = new NotificationAction(
                "button",
                "Click me",
                "https://google.com"
            )
            .WithAttribute("prop1", "val1")
            .WithAttribute("prop2", 2)
            .WithAttribute("prop3", null)
            .WithAttribute("prop2", "override");
        BloomreachSDK.HandlePushNotificationOpenedWithoutTrackingConsent(notificationAction);
        _methodCollector.VerifyMethodCalled("HandlePushNotificationOpenedWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput(
            "HandlePushNotificationOpenedWithoutTrackingConsent",
            TestUtils.ReadFile("HandlePushNotificationOpenedWithoutTrackingConsent")
        );
    }
    
    [Test]
    public void HandleCampaignClick()
    {
        var campaignUrl = new Uri("https://google.com/unit/test/with?params=1&orparam=true");
        BloomreachSDK.HandleCampaignClick(campaignUrl);
        _methodCollector.VerifyMethodCalled("HandleCampaignClick");
        _methodCollector.VerifyMethodInput(
            "HandleCampaignClick",
            TestUtils.ReadFile("HandleCampaignClick")
        );
    }
    
    [Test]
    public void HandleHmsPushToken()
    {
        BloomreachSDK.HandleHmsPushToken("hms-token-12345");
        _methodCollector.VerifyMethodCalled("HandleHmsPushToken");
        _methodCollector.VerifyMethodInput(
            "HandleHmsPushToken",
            TestUtils.ReadFile("HandleHmsPushToken")
        );
    }
    
    [Test]
    public void HandlePushToken()
    {
        BloomreachSDK.HandlePushToken("gms-token-12345");
        _methodCollector.VerifyMethodCalled("HandlePushToken");
        _methodCollector.VerifyMethodInput(
            "HandlePushToken",
            TestUtils.ReadFile("HandlePushToken")
        );
    }
    
    [Test]
    public void IsBloomreachNotification_TRUE()
    {
        _methodCollector.RegisterSuccessMethodResult("IsBloomreachNotification", "true");
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"prop1", "val1"},
            {"prop2", "val2"}
        });
        var result = BloomreachSDK.IsBloomreachNotification(notificationPayload);
        _methodCollector.VerifyMethodCalled("IsBloomreachNotification");
        _methodCollector.VerifyMethodInput(
            "IsBloomreachNotification",
            TestUtils.ReadFile("IsBloomreachNotification")
        );
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void IsBloomreachNotification_FALSE()
    {
        _methodCollector.RegisterSuccessMethodResult("IsBloomreachNotification", "false");
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"prop1", "val1"},
            {"prop2", "val2"}
        });
        var result = BloomreachSDK.IsBloomreachNotification(notificationPayload);
        _methodCollector.VerifyMethodCalled("IsBloomreachNotification");
        _methodCollector.VerifyMethodInput(
            "IsBloomreachNotification",
            TestUtils.ReadFile("IsBloomreachNotification")
        );
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void IsBloomreachNotification_InvalidIsFalse()
    {
        _methodCollector.RegisterSuccessMethodResult("IsBloomreachNotification", "non-valid-value");
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"prop1", "val1"},
            {"prop2", "val2"}
        });
        var result = BloomreachSDK.IsBloomreachNotification(notificationPayload);
        _methodCollector.VerifyMethodCalled("IsBloomreachNotification");
        _methodCollector.VerifyMethodInput(
            "IsBloomreachNotification",
            TestUtils.ReadFile("IsBloomreachNotification")
        );
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void IsBloomreachNotification_ErrorIsFalse()
    {
        _methodCollector.RegisterFailureMethodResult("IsBloomreachNotification", "some error");
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"prop1", "val1"},
            {"prop2", "val2"}
        });
        var result = BloomreachSDK.IsBloomreachNotification(notificationPayload);
        _methodCollector.VerifyMethodCalled("IsBloomreachNotification");
        _methodCollector.VerifyMethodInput(
            "IsBloomreachNotification",
            TestUtils.ReadFile("IsBloomreachNotification")
        );
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void TrackClickedPush()
    {
        var notificationAction = new NotificationAction(
                "button",
                "Click me",
                "https://google.com"
            )
            .WithAttribute("prop1", "val1")
            .WithAttribute("prop2", 2)
            .WithAttribute("prop3", null)
            .WithAttribute("prop2", "override");
        BloomreachSDK.TrackClickedPush(notificationAction);
        _methodCollector.VerifyMethodCalled("TrackClickedPush");
        _methodCollector.VerifyMethodInput(
            "TrackClickedPush",
            TestUtils.ReadFile("TrackClickedPush")
        );
    }
    
    [Test]
    public void TrackClickedPushWithoutTrackingConsent()
    {
        var notificationAction = new NotificationAction(
                "button",
                "Click me",
                "https://google.com"
            )
            .WithAttribute("prop1", "val1")
            .WithAttribute("prop2", 2)
            .WithAttribute("prop3", null)
            .WithAttribute("prop2", "override");
        BloomreachSDK.TrackClickedPushWithoutTrackingConsent(notificationAction);
        _methodCollector.VerifyMethodCalled("TrackClickedPushWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput(
            "TrackClickedPushWithoutTrackingConsent",
            TestUtils.ReadFile("TrackClickedPushWithoutTrackingConsent")
        );
    }
    
    [Test]
    public void TrackPushToken()
    {
        BloomreachSDK.TrackPushToken("gms-token-12345");
        _methodCollector.VerifyMethodCalled("TrackPushToken");
        _methodCollector.VerifyMethodInput(
            "TrackPushToken",
            TestUtils.ReadFile("HandlePushToken")
        );
    }
    
    [Test]
    public void TrackHmsPushToken()
    {
        BloomreachSDK.TrackHmsPushToken("hms-token-12345");
        _methodCollector.VerifyMethodCalled("TrackHmsPushToken");
        _methodCollector.VerifyMethodInput(
            "TrackHmsPushToken",
            TestUtils.ReadFile("HandleHmsPushToken")
        );
    }

    [Test]
    public void TrackDeliveredPush()
    {
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"attributes", ConverterUtils.SerializeInput(new Dictionary<string, string>
            {
                {"prop1", "val1"},
                {"prop2", "val2"}
            })!}
        });
        BloomreachSDK.TrackDeliveredPush(notificationPayload);
        _methodCollector.VerifyMethodCalled("TrackDeliveredPush");
        _methodCollector.VerifyMethodInput(
            "TrackDeliveredPush",
            TestUtils.ReadFile("TrackDeliveredPush")
        );
    }
    
    [Test]
    public void TrackDeliveredPushWithoutTrackingConsent()
    {
        NotificationPayload notificationPayload = NotificationPayload.Parse(new Dictionary<string, string>()
        {
            {"attributes", ConverterUtils.SerializeInput(new Dictionary<string, string>
            {
                {"prop1", "val1"},
                {"prop2", "val2"}
            })!}
        });
        BloomreachSDK.TrackDeliveredPushWithoutTrackingConsent(notificationPayload);
        _methodCollector.VerifyMethodCalled("TrackDeliveredPushWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput(
            "TrackDeliveredPushWithoutTrackingConsent",
            TestUtils.ReadFile("TrackDeliveredPush")
        );
    }
    
    [Test]
    public void RequestPushAuthorization()
    {
        BloomreachSDK.RequestPushAuthorization();
        _methodCollector.VerifyMethodCalled("RequestPushAuthorization");
    }

    [Test]
    public void SetReceivedPushCallback()
    {
        _methodCollector.RegisterSuccessMethodResult("SetReceivedPushCallback", TestUtils.ReadFile("SetReceivedPushCallback"));
        NotificationPayload? result = null;
        BloomreachSDK.SetReceivedPushCallback(payload =>
        {
            result = payload;
        });
        _methodCollector.VerifyMethodCalled("SetReceivedPushCallback");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        Assert.That(
            result,
            Is.Not.Null
        );
        Assert.That(
            result!.RawData,
            Is.EqualTo(new Dictionary<string, string>
            {
                {"prop1", "val1"},
                {"prop2", "val2"}
            })
        );
    }

    [Test]
    public void SetOpenedPushCallback()
    {
        _methodCollector.RegisterSuccessMethodResult("SetOpenedPushCallback", TestUtils.ReadFile("SetOpenedPushCallback"));
        NotificationAction? result = null;
        BloomreachSDK.SetOpenedPushCallback(action =>
        {
            result = action;
        });
        _methodCollector.VerifyMethodCalled("SetOpenedPushCallback");
        Thread.Sleep(TimeSpan.FromSeconds(1));
        Assert.That(
            result,
            Is.Not.Null
        );
        Assert.That(result!.ActionType, Is.EqualTo("button"));
        Assert.That(result!.ActionName, Is.EqualTo("Click me"));
        Assert.That(result!.Url, Is.EqualTo("https://google.com"));
        Assert.That(
            result!.Attributes,
            Is.EqualTo(new Dictionary<string, object?>
            {
                {"prop1", "val1"},
                {"prop2", "override"}
            })
        );
    }

}
