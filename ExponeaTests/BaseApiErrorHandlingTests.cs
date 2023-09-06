using Exponea;
using ExponeaTests.Utils;

namespace ExponeaTests;

public class BaseApiErrorHandlingTests
{

    private readonly MethodInvokeCollector _methodCollector = new MethodInvokeCollector();
    
    [SetUp]
    public void Setup()
    {
        ExponeaSDK.Instance = new ExponeaSdkMock(_methodCollector);
    }
    [TearDown]
    public void TearDown()
    {
        _methodCollector.Clear();
        ExponeaSDK.SetSafeMode(true);
    }

    [Test]
    public void Anonymize_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("Anonymize", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.Anonymize();
        _methodCollector.VerifyMethodCalled("Anonymize");
    }

    [Test]
    public void Anonymize_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("Anonymize", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.Anonymize();
        });
    }

    [Test]
    public void Anonymize_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("Anonymize", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.Anonymize();
        _methodCollector.VerifyMethodCalled("Anonymize");
    }

    [Test]
    public void Anonymize_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("Anonymize", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.Anonymize();
        });
    }

    [Test]
    public void Configure_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("Configure", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.Configure(new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        ));
        _methodCollector.VerifyMethodCalled("Configure");
    }

    [Test]
    public void Configure_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("Configure", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.Configure(new Configuration(
                "projToken",
                "authToken",
                "https://url.com"
            ));
        });
    }

    [Test]
    public void Configure_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("Configure", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.Configure(new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        ));
        _methodCollector.VerifyMethodCalled("Configure");
    }

    [Test]
    public void Configure_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("Configure", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.Configure(new Configuration(
                "projToken",
                "authToken",
                "https://url.com"
            ));
        });
    }

    [Test]
    public void IsConfigured_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsConfigured", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var isConfigured = ExponeaSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.False);
    }

    [Test]
    public void IsConfigured_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsConfigured", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.IsConfigured();
        });
    }

    [Test]
    public void IsConfigured_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsConfigured", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var isConfigured = ExponeaSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.False);
    }

    [Test]
    public void IsConfigured_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsConfigured", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.IsConfigured();
        });
    }

    [Test]
    public void FlushData_Error_Safe_ShouldResultFalse()
    {
        _methodCollector.RegisterFailureMethodResult("FlushData", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var flushTask = ExponeaSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
        flushTask.Wait(1000);
        Assert.That(
            flushTask.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            flushTask.Result,
            Is.False
        );
    }

    [Test]
    public void FlushData_Error_UnSafe_ShouldResultFalse()
    {
        _methodCollector.RegisterFailureMethodResult("FlushData", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        var flushTask = ExponeaSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
        flushTask.Wait(1000);
        Assert.That(
            flushTask.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            flushTask.Result,
            Is.False
        );
    }

    [Test]
    public void FlushData_Exception_Safe_ShouldResultFalse()
    {
        _methodCollector.RegisterFailureMethodResult("FlushData", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var flushTask = ExponeaSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
        flushTask.Wait(1000);
        Assert.That(
            flushTask.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            flushTask.Result,
            Is.False
        );
    }

    [Test]
    public void FlushData_Exception_UnSafe_ShouldResultException()
    {
        _methodCollector.RegisterFailureMethodResult("FlushData", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        var flushTask = ExponeaSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
        // task with exception will be Faulted, Wait() will throw that exception
        Assert.Throws<AggregateException>(() =>
        {
            flushTask.Wait(1000);
        });
        Assert.That(
            flushTask.Status,
            Is.EqualTo(TaskStatus.Faulted)
        );
    }

    [Test]
    public void IdentifyCustomer_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IdentifyCustomer", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.IdentifyCustomer(new Customer("id12345"));
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
    }

    [Test]
    public void IdentifyCustomer_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IdentifyCustomer", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.IdentifyCustomer(new Customer("id12345"));
        });
    }

    [Test]
    public void IdentifyCustomer_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IdentifyCustomer", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.IdentifyCustomer(new Customer("id12345"));
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
    }

    [Test]
    public void IdentifyCustomer_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IdentifyCustomer", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.IdentifyCustomer(new Customer("id12345"));
        });
    }

    [Test]
    public void GetCustomerCookie_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCustomerCookie", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var cookies = ExponeaSDK.GetCustomerCookie();
        _methodCollector.VerifyMethodCalled("GetCustomerCookie");
        Assert.That(cookies, Is.Null);
    }

    [Test]
    public void GetCustomerCookie_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCustomerCookie", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetCustomerCookie();
        });
    }

    [Test]
    public void GetCustomerCookie_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCustomerCookie", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var cookies = ExponeaSDK.GetCustomerCookie();
        _methodCollector.VerifyMethodCalled("GetCustomerCookie");
        Assert.That(cookies, Is.Null);
    }

    [Test]
    public void GetCustomerCookie_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCustomerCookie", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetCustomerCookie();
        });
    }

    [Test]
    public void GetFlushMode_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushMode", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var flushMode = ExponeaSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(flushMode, Is.EqualTo(FlushMode.Unknown));
    }

    [Test]
    public void GetFlushMode_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushMode", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetFlushMode();
        });
    }

    [Test]
    public void GetFlushMode_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushMode", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var flushMode = ExponeaSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(flushMode, Is.EqualTo(FlushMode.Unknown));
    }

    [Test]
    public void GetFlushMode_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushMode", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetFlushMode();
        });
    }

    [Test]
    public void GetFlushPeriod_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushPeriod", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var flushPeriod = ExponeaSDK.GetFlushPeriod();
        _methodCollector.VerifyMethodCalled("GetFlushPeriod");
        Assert.That(flushPeriod.Ticks, Is.EqualTo(0));
    }

    [Test]
    public void GetFlushPeriod_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushPeriod", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetFlushPeriod();
        });
    }

    [Test]
    public void GetFlushPeriod_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushPeriod", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var flushPeriod = ExponeaSDK.GetFlushPeriod();
        _methodCollector.VerifyMethodCalled("GetFlushPeriod");
        Assert.That(flushPeriod.Ticks, Is.EqualTo(0));
    }

    [Test]
    public void GetFlushPeriod_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetFlushPeriod", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetFlushPeriod();
        });
    }

    [Test]
    public void GetLogLevel_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetLogLevel", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var logLevel = ExponeaSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(logLevel, Is.EqualTo(LogLevel.Unknown));
    }

    [Test]
    public void GetLogLevel_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetLogLevel", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetLogLevel();
        });
    }

    [Test]
    public void GetLogLevel_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetLogLevel", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var logLevel = ExponeaSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(logLevel, Is.EqualTo(LogLevel.Unknown));
    }

    [Test]
    public void GetLogLevel_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetLogLevel", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetLogLevel();
        });
    }

    [Test]
    public void GetSessionTimeout_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetSessionTimeout", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var sessionTimeout = ExponeaSDK.GetSessionTimeout();
        _methodCollector.VerifyMethodCalled("GetSessionTimeout");
        Assert.That(sessionTimeout.Ticks, Is.EqualTo(0));
    }

    [Test]
    public void GetSessionTimeout_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetSessionTimeout", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetSessionTimeout();
        });
    }

    [Test]
    public void GetSessionTimeout_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetSessionTimeout", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var sessionTimeout = ExponeaSDK.GetSessionTimeout();
        _methodCollector.VerifyMethodCalled("GetSessionTimeout");
        Assert.That(sessionTimeout.Ticks, Is.EqualTo(0));
    }

    [Test]
    public void GetSessionTimeout_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetSessionTimeout", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetSessionTimeout();
        });
    }

    [Test]
    public void SetDefaultProperties_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetDefaultProperties", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>());
        _methodCollector.VerifyMethodCalled("SetDefaultProperties");
    }

    [Test]
    public void SetDefaultProperties_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetDefaultProperties", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>());
        });
    }

    [Test]
    public void SetDefaultProperties_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetDefaultProperties", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>());
        _methodCollector.VerifyMethodCalled("SetDefaultProperties");
    }

    [Test]
    public void SetDefaultProperties_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetDefaultProperties", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>());
        });
    }

    [Test]
    public void SetFlushPeriod_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetFlushPeriod", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(-10000));
        _methodCollector.VerifyMethodCalled("SetFlushPeriod");
    }

    [Test]
    public void SetFlushPeriod_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetFlushPeriod", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(-10000));
        });
    }

    [Test]
    public void SetFlushPeriod_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetFlushPeriod", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(-10000));
        _methodCollector.VerifyMethodCalled("SetFlushPeriod");
    }

    [Test]
    public void SetFlushPeriod_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetFlushPeriod", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(-10000));
        });
    }

    [Test]
    public void SetLogLevel_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetLogLevel", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetLogLevel(LogLevel.Unknown);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
    }

    [Test]
    public void SetLogLevel_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetLogLevel", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetLogLevel(LogLevel.Unknown);
        });
    }

    [Test]
    public void SetLogLevel_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetLogLevel", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetLogLevel(LogLevel.Unknown);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
    }

    [Test]
    public void SetLogLevel_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetLogLevel", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetLogLevel(LogLevel.Unknown);
        });
    }

    [Test]
    public void SetSessionTimeout_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetSessionTimeout", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(10000));
        _methodCollector.VerifyMethodCalled("SetSessionTimeout");
    }

    [Test]
    public void SetSessionTimeout_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetSessionTimeout", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(10000));
        });
    }

    [Test]
    public void SetSessionTimeout_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetSessionTimeout", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(10000));
        _methodCollector.VerifyMethodCalled("SetSessionTimeout");
    }

    [Test]
    public void SetSessionTimeout_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetSessionTimeout", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(10000));
        });
    }

    [Test]
    public void GetCheckPushSetup_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCheckPushSetup", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.GetCheckPushSetup();
        _methodCollector.VerifyMethodCalled("GetCheckPushSetup");
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetCheckPushSetup_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCheckPushSetup", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetCheckPushSetup();
        });
    }

    [Test]
    public void GetCheckPushSetup_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCheckPushSetup", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.GetCheckPushSetup();
        _methodCollector.VerifyMethodCalled("GetCheckPushSetup");
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetCheckPushSetup_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetCheckPushSetup", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetCheckPushSetup();
        });
    }

    [Test]
    public void GetTokenTrackFrequency_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetTokenTrackFrequency", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var tokenFrequency = ExponeaSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.OnTokenChange));
    }

    [Test]
    public void GetTokenTrackFrequency_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetTokenTrackFrequency", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.GetTokenTrackFrequency();
        });
    }

    [Test]
    public void GetTokenTrackFrequency_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("GetTokenTrackFrequency", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var tokenFrequency = ExponeaSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.OnTokenChange));
    }

    [Test]
    public void GetTokenTrackFrequency_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("GetTokenTrackFrequency", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.GetTokenTrackFrequency();
        });
    }

    [Test]
    public void IsAutomaticSessionTracking_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutomaticSessionTracking", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.IsAutomaticSessionTracking();
        _methodCollector.VerifyMethodCalled("IsAutomaticSessionTracking");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutomaticSessionTracking_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutomaticSessionTracking", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.IsAutomaticSessionTracking();
        });
    }

    [Test]
    public void IsAutomaticSessionTracking_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutomaticSessionTracking", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.IsAutomaticSessionTracking();
        _methodCollector.VerifyMethodCalled("IsAutomaticSessionTracking");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutomaticSessionTracking_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutomaticSessionTracking", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.IsAutomaticSessionTracking();
        });
    }

    [Test]
    public void IsAutoPushNotification_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutoPushNotification", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.IsAutoPushNotification();
        _methodCollector.VerifyMethodCalled("IsAutoPushNotification");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutoPushNotification_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutoPushNotification", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.IsAutoPushNotification();
        });
    }

    [Test]
    public void IsAutoPushNotification_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutoPushNotification", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        var result = ExponeaSDK.IsAutoPushNotification();
        _methodCollector.VerifyMethodCalled("IsAutoPushNotification");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutoPushNotification_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("IsAutoPushNotification", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.IsAutoPushNotification();
        });
    }

    [Test]
    public void SetCheckPushSetup_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetCheckPushSetup", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetCheckPushSetup(false);
        _methodCollector.VerifyMethodCalled("SetCheckPushSetup");
    }

    [Test]
    public void SetCheckPushSetup_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetCheckPushSetup", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetCheckPushSetup(false);
        });
    }

    [Test]
    public void SetCheckPushSetup_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetCheckPushSetup", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetCheckPushSetup(false);
        _methodCollector.VerifyMethodCalled("SetCheckPushSetup");
    }

    [Test]
    public void SetCheckPushSetup_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetCheckPushSetup", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetCheckPushSetup(false);
        });
    }

    [Test]
    public void SetAutomaticSessionTracking_Error_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetAutomaticSessionTracking", "Some error occurs");
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetAutomaticSessionTracking(false);
        _methodCollector.VerifyMethodCalled("SetAutomaticSessionTracking");
    }

    [Test]
    public void SetAutomaticSessionTracking_Error_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetAutomaticSessionTracking", "Some error occurs");
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<Exception>(() =>
        {
            ExponeaSDK.SetAutomaticSessionTracking(false);
        });
    }

    [Test]
    public void SetAutomaticSessionTracking_Exception_Safe()
    {
        _methodCollector.RegisterFailureMethodResult("SetAutomaticSessionTracking", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(true);
        ExponeaSDK.SetAutomaticSessionTracking(false);
        _methodCollector.VerifyMethodCalled("SetAutomaticSessionTracking");
    }

    [Test]
    public void SetAutomaticSessionTracking_Exception_UnSafe()
    {
        _methodCollector.RegisterFailureMethodResult("SetAutomaticSessionTracking", new NullReferenceException("Developers well known exception"));
        ExponeaSDK.SetSafeMode(false);
        Assert.Throws<NullReferenceException>(() =>
        {
            ExponeaSDK.SetAutomaticSessionTracking(false);
        });
    }
}
