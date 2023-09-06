using Exponea;
using ExponeaTests.Utils;

namespace ExponeaTests;

public class TrackApiTests
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
        ExponeaSDK.Instance = new ExponeaSdkMock(_methodCollector);
    }
    [TearDown]
    public void TearDown()
    {
        _methodCollector.Clear();
    }

    [Test]
    public void LoadJsonFile()
    {
        Assert.That(
            TestUtils.ReadFile("Anonymize_NoParams_Input"),
            Is.EqualTo("{}")
        );
    }

    [Test]
    public void TrackPaymentEvent_Empty_WithoutTime()
    {
        ExponeaSDK.TrackPaymentEvent(new Payment(
            value: 10.4,
            currency: "Eur"
        ));
        _methodCollector.VerifyMethodCalled("TrackPaymentEvent");
        var methodInput = _methodCollector.FindMethodInput("TrackPaymentEvent");
        // just verify that `timestamp` is filled (with `Now` time)
        Assert.That(
            methodInput,
            Contains.Substring("timestamp")
        );
        // other fields are verified in other tests
    }

    [Test]
    public void TrackPaymentEvent_Empty_WithTime()
    {
        ExponeaSDK.TrackPaymentEvent(new Payment(
            value: 10.4,
            currency: "Eur"
        ), timestamp: 10000.0);
        _methodCollector.VerifyMethodCalled("TrackPaymentEvent");
        _methodCollector.VerifyMethodInput("TrackPaymentEvent", TestUtils.ReadFile("TrackPaymentEvent_Empty_WithTime"));
    }

    [Test]
    public void TrackPaymentEvent_Full_WithoutTime()
    {
        ExponeaSDK.TrackPaymentEvent(new Payment(
            value: 10.4,
            currency: "Eur",
            system: "credit",
            productId: "12345",
            productTitle: "Fine stuff",
            receipt: "abc-1234567890"
        ));
        _methodCollector.VerifyMethodCalled("TrackPaymentEvent");
        var methodInput = _methodCollector.FindMethodInput("TrackPaymentEvent");
        // just verify that `timestamp` is filled (with `Now` time)
        Assert.That(
            methodInput,
            Contains.Substring("timestamp")
        );
        // other fields are verified in other tests
    }

    [Test]
    public void TrackPaymentEvent_Full_WithTime()
    {
        ExponeaSDK.TrackPaymentEvent(new Payment(
            value: 10.4,
            currency: "Eur",
            system: "credit",
            productId: "12345",
            productTitle: "Fine stuff",
            receipt: "abc-1234567890"
        ), timestamp: 10000.0);
        _methodCollector.VerifyMethodCalled("TrackPaymentEvent");
        _methodCollector.VerifyMethodInput("TrackPaymentEvent", TestUtils.ReadFile("TrackPaymentEvent_Full_WithTime"));
    }

    [Test]
    public void TrackEvent_WithTime()
    {
        var evt = new Event("custom_event")
        {
            ["prop1"] = "val1",
#pragma warning disable CA2244
            ["prop2"] = "val2",
#pragma warning restore CA2244
            ["prop2"] = 2,
            ["prop3"] = null
        };
        ExponeaSDK.TrackEvent(evt, timestamp: 10000.0);
        _methodCollector.VerifyMethodCalled("TrackEvent");
        _methodCollector.VerifyMethodInput("TrackEvent", TestUtils.ReadFile("TrackEvent_WithTime"));
    }

    [Test]
    public void TrackEvent_WithoutTime()
    {
        var evt = new Event("custom_event")
        {
            ["prop1"] = "val1",
#pragma warning disable CA2244
            ["prop2"] = "val2",
#pragma warning restore CA2244
            ["prop2"] = 2,
            ["prop3"] = null
        };
        ExponeaSDK.TrackEvent(evt, timestamp: 10000.0);
        _methodCollector.VerifyMethodCalled("TrackEvent");
        var methodInput = _methodCollector.FindMethodInput("TrackEvent");
        // just verify that `timestamp` is filled (with `Now` time)
        Assert.That(
            methodInput,
            Contains.Substring("timestamp")
        );
        // other fields are verified in other tests
    }

    [Test]
    public void TrackSessionStart()
    {
        ExponeaSDK.TrackSessionStart();
        _methodCollector.VerifyMethodCalled("TrackSessionStart");
        _methodCollector.VerifyMethodInput("TrackSessionStart", null);
    }

    [Test]
    public void TrackSessionEnd()
    {
        ExponeaSDK.TrackSessionEnd();
        _methodCollector.VerifyMethodCalled("TrackSessionEnd");
        _methodCollector.VerifyMethodInput("TrackSessionEnd", null);
    }
    
}
