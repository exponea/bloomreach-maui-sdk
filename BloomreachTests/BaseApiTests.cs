﻿using BloomreachTests.Utils;
using Bloomreach;

namespace BloomreachTests;

public class BaseApiTests
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
    public void LoadJsonFile()
    {
        Assert.That(
            TestUtils.ReadFile("Anonymize_NoParams_Input"),
            Is.EqualTo("{}")
        );
    }

    [Test]
    public void Anonymize_NoParams()
    {
        BloomreachSDK.Anonymize();
        _methodCollector.VerifyMethodCalled("Anonymize");
        _methodCollector.VerifyMethodInput("Anonymize", TestUtils.ReadFile("Anonymize_NoParams_Input"));
    }
    
    [Test]
    public void Anonymize_WithProject()
    {
        BloomreachSDK.Anonymize(new Project(
            "projToken",
            "authToken",
            "https://url.com"
        ));
        _methodCollector.VerifyMethodCalled("Anonymize");
        _methodCollector.VerifyMethodInput("Anonymize", TestUtils.ReadFile("Anonymize_WithProject_Input"));
    }
    
    [Test]
    public void Anonymize_WithMappings()
    {
        BloomreachSDK.Anonymize(null, new Dictionary<EventType, IList<Project>>()
        {
            { EventType.Banner, new[]
            {
                new Project(
                    "projToken",
                    "authToken",
                    "https://url.com"
                )
            } }
        });
        _methodCollector.VerifyMethodCalled("Anonymize");
        _methodCollector.VerifyMethodInput("Anonymize", TestUtils.ReadFile("Anonymize_WithMappings_Input"));
    }
    
    [Test]
    public void Anonymize_WithProjectAndMappings()
    {
        BloomreachSDK.Anonymize(new Project(
            "projToken",
            "authToken",
            "https://url.com"
        ), new Dictionary<EventType, IList<Project>>()
        {
            { EventType.Banner, new[]
            {
                new Project(
                    "projToken",
                    "authToken",
                    "https://url.com"
                )
            } }
        });
        _methodCollector.VerifyMethodCalled("Anonymize");
        _methodCollector.VerifyMethodInput("Anonymize", TestUtils.ReadFile("Anonymize_WithProjectAndMappings_Input"));
    }

    [Test]
    public void Configure_EmptyConfiguration()
    {
        BloomreachSDK.Configure(new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        ));
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_EmptyConfiguration_Input"));
    }

    [Test]
    public void Configure_FullConfiguration()
    {
        var config = new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        )
        {
            AdvancedAuthEnabled = false,
            AllowDefaultCustomerProperties = false,
            AppGroup = "group",
            AutomaticPushNotification = false,
            AutomaticSessionTracking = true,
            CampaignTTL = 100,
            DefaultProperties = new Dictionary<string, object>(),
            HttpLoggingLevel = HttpLoggingLevel.BODY,
            ProjectRouteMap = new Dictionary<EventType, IList<Project>>(),
            MaxTries = 3,
            SessionTimeout = 30.4,
            TokenTrackFrequency = TokenTrackFrequency.EveryLaunch,
            PushIcon = "icon.png",
            PushAccentColor = 10302,
            PushChannelName = "notifs",
            PushChannelDescription = "push desc",
            PushChannelId = "pushChanId",
            PushNotificationImportance = 2,
            RequirePushAuthorization = true
        };
        config.DefaultProperties!.Add("key", "prop");
        config.DefaultProperties.Add("key2", 1);
        config.DefaultProperties.Add("key3", false);
        config.ProjectRouteMap!.Add(EventType.Payment, new []{
            new Project(
                "projToken",
                "authToken",
                "https://url.com"
            )            
        });
        BloomreachSDK.Configure(config);
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_FullConfiguration_Input"));
    }

    [Test]
    public void Configure_Configuration_Variant_1()
    {
        var config = new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        )
        {
            HttpLoggingLevel = HttpLoggingLevel.NONE,
            TokenTrackFrequency = TokenTrackFrequency.Daily,
        };
        BloomreachSDK.Configure(config);
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_Configuration_Variant_1"));
    }

    [Test]
    public void Configure_Configuration_Variant_2()
    {
        var config = new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        )
        {
            HttpLoggingLevel = HttpLoggingLevel.BASIC,
            TokenTrackFrequency = TokenTrackFrequency.EveryLaunch,
        };
        BloomreachSDK.Configure(config);
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_Configuration_Variant_2"));
    }

    [Test]
    public void Configure_Configuration_Variant_3()
    {
        var config = new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        )
        {
            HttpLoggingLevel = HttpLoggingLevel.HEADERS,
            TokenTrackFrequency = TokenTrackFrequency.OnTokenChange,
        };
        BloomreachSDK.Configure(config);
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_Configuration_Variant_3"));
    }

    [Test]
    public void Configure_Configuration_Variant_4()
    {
        var config = new Configuration(
            "projToken",
            "authToken",
            "https://url.com"
        )
        {
            HttpLoggingLevel = HttpLoggingLevel.BODY,
            TokenTrackFrequency = TokenTrackFrequency.OnTokenChange,
        };
        BloomreachSDK.Configure(config);
        _methodCollector.VerifyMethodCalled("Configure");
        _methodCollector.VerifyMethodInput("Configure", TestUtils.ReadFile("Configure_Configuration_Variant_4"));
    }

    [Test]
    public void IsConfigured_NullIsFalse()
    {
        _methodCollector.RegisterSuccessMethodResult("IsConfigured", null);
        var isConfigured = BloomreachSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.False);
    }

    [Test]
    public void IsConfigured_False()
    {
        _methodCollector.RegisterSuccessMethodResult("IsConfigured", TestUtils.ReadFile("Boolean_False"));
        var isConfigured = BloomreachSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.False);
    }

    [Test]
    public void IsConfigured_True()
    {
        _methodCollector.RegisterSuccessMethodResult("IsConfigured", TestUtils.ReadFile("Boolean_True"));
        var isConfigured = BloomreachSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.True);
    }

    [Test]
    public void IsConfigured_InvalidIsFalse()
    {
        _methodCollector.RegisterSuccessMethodResult("IsConfigured", TestUtils.ReadFile("Boolean_Invalid"));
        var isConfigured = BloomreachSDK.IsConfigured();
        _methodCollector.VerifyMethodCalled("IsConfigured");
        Assert.That(isConfigured, Is.False);
    }

    [Test]
    public void FlushData_Sync()
    {
        _methodCollector.RegisterSuccessMethodResult("FlushData", TestUtils.ReadFile("Boolean_True"));
        BloomreachSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
    }

    [Test]
    public void FlushData_Async_False()
    {
        _methodCollector.RegisterSuccessMethodResult("FlushData", TestUtils.ReadFile("Boolean_False"));
        var flushTask = BloomreachSDK.FlushData();
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
    public void FlushData_Async_True()
    {
        _methodCollector.RegisterSuccessMethodResult("FlushData", TestUtils.ReadFile("Boolean_True"));
        var flushTask = BloomreachSDK.FlushData();
        _methodCollector.VerifyMethodCalled("FlushData");
        flushTask.Wait(1000);
        Assert.That(
            flushTask.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            flushTask.Result,
            Is.True
        );
    }

    [Test]
    public void FlushData_Async_InvalidIsFalse()
    {
        _methodCollector.RegisterSuccessMethodResult("FlushData", TestUtils.ReadFile("Boolean_Invalid"));
        var flushTask = BloomreachSDK.FlushData();
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
    public void IdentifyCustomer_Registered()
    {
        BloomreachSDK.IdentifyCustomer(new Customer("id12345"));
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
        _methodCollector.VerifyMethodInput("IdentifyCustomer", TestUtils.ReadFile("IdentifyCustomer_Registered"));
    }

    [Test]
    public void IdentifyCustomer_CustomHardId()
    {
        BloomreachSDK.IdentifyCustomer(new Customer("login", "id12345"));
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
        _methodCollector.VerifyMethodInput("IdentifyCustomer", TestUtils.ReadFile("IdentifyCustomer_CustomHardId"));
    }

    [Test]
    public void IdentifyCustomer_MultipleHardIds()
    {
        BloomreachSDK.IdentifyCustomer(new Customer(new Dictionary<string, string>
        {
            { "id1", "12345" },
            { "id2", "123456" }
        }));
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
        _methodCollector.VerifyMethodInput("IdentifyCustomer", TestUtils.ReadFile("IdentifyCustomer_MultipleHardIds"));
    }

    [Test]
    public void IdentifyCustomer_Registered_WithProperties()
    {
        var customer = new Customer("id12345");
        customer.WithProperty("prop1", "val1");
        customer.WithProperty("prop2", "val2");
        customer.WithProperty("prop2", 2);
        customer.WithProperty("prop3", 3);
        customer.WithProperty("prop4", null);
        BloomreachSDK.IdentifyCustomer(customer);
        _methodCollector.VerifyMethodCalled("IdentifyCustomer");
        _methodCollector.VerifyMethodInput("IdentifyCustomer", TestUtils.ReadFile("IdentifyCustomer_Registered_WithProperties"));
    }

    [Test]
    public void GetCustomerCookie_Empty()
    {
        _methodCollector.RegisterSuccessMethodResult("GetCustomerCookie", TestUtils.ReadFile("GetCustomerCookie_Empty"));
        var cookies = BloomreachSDK.GetCustomerCookie();
        _methodCollector.VerifyMethodCalled("GetCustomerCookie");
        Assert.That(cookies, Is.EqualTo(TestUtils.ReadFile("GetCustomerCookie_Empty")));
    }

    [Test]
    public void GetCustomerCookie_SomeNonnull()
    {
        _methodCollector.RegisterSuccessMethodResult("GetCustomerCookie", TestUtils.ReadFile("GetCustomerCookie_SomeNonnull"));
        var cookies = BloomreachSDK.GetCustomerCookie();
        _methodCollector.VerifyMethodCalled("GetCustomerCookie");
        Assert.That(cookies, Is.EqualTo(TestUtils.ReadFile("GetCustomerCookie_SomeNonnull")));
    }

    [Test]
    public void GetDefaultProperties_Null()
    {
        _methodCollector.RegisterSuccessMethodResult("GetDefaultProperties", null);
        var props = BloomreachSDK.GetDefaultProperties();
        _methodCollector.VerifyMethodCalled("GetDefaultProperties");
        Assert.That(
            props.Count,
            Is.Zero
        );
    }

    [Test]
    public void GetDefaultProperties_EmptyString()
    {
        _methodCollector.RegisterSuccessMethodResult("GetDefaultProperties", "");
        var props = BloomreachSDK.GetDefaultProperties();
        _methodCollector.VerifyMethodCalled("GetDefaultProperties");
        Assert.That(
            props.Count,
            Is.Zero
        );
    }

    [Test]
    public void GetDefaultProperties_EmptyDictionary()
    {
        _methodCollector.RegisterSuccessMethodResult("GetDefaultProperties", TestUtils.ReadFile("GetDefaultProperties_EmptyDictionary"));
        var props = BloomreachSDK.GetDefaultProperties();
        _methodCollector.VerifyMethodCalled("GetDefaultProperties");
        Assert.That(
            props.Count,
            Is.Zero
        );
    }

    [Test]
    public void GetDefaultProperties_NonEmptyDictionary_WithNull()
    {
        _methodCollector.RegisterSuccessMethodResult("GetDefaultProperties", TestUtils.ReadFile("GetDefaultProperties_NonEmptyDictionary_WithNull"));
        var props = BloomreachSDK.GetDefaultProperties();
        _methodCollector.VerifyMethodCalled("GetDefaultProperties");
        Assert.That(
            props.Count,
            Is.EqualTo(2)
        );
        Assert.That(props, Contains.Key("prop1"));
        Assert.That(props, Contains.Key("prop2"));
        Assert.That(props["prop1"], Is.EqualTo("val1"));
        Assert.That(props["prop2"], Is.EqualTo(2));
        Assert.That(props, Does.Not.ContainKey("prop3"));
    }

    [Test]
    public void GetFlushMode_Unknown()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_Unknown"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.Unknown)
        );
    }

    [Test]
    public void GetFlushMode_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_Invalid"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.Unknown)
        );
    }

    [Test]
    public void GetFlushMode_Immediate()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_Immediate"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.Immediate)
        );
    }

    [Test]
    public void GetFlushMode_Manual()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_Manual"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.Manual)
        );
    }

    [Test]
    public void GetFlushMode_Period()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_Period"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.Period)
        );
    }

    [Test]
    public void GetFlushMode_AppClose()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushMode", TestUtils.ReadFile("GetFlushMode_AppClose"));
        var flushMode = BloomreachSDK.GetFlushMode();
        _methodCollector.VerifyMethodCalled("GetFlushMode");
        Assert.That(
            flushMode,
            Is.EqualTo(FlushMode.AppClose)
        );
    }

    [Test]
    public void SetFlushMode_Unknown()
    {
        BloomreachSDK.SetFlushMode(FlushMode.Unknown);
        _methodCollector.VerifyMethodCalled("SetFlushMode", calledExactly: 0, calledAtLeast: 0);
    }

    [Test]
    public void SetFlushMode_Immediate()
    {
        BloomreachSDK.SetFlushMode(FlushMode.Immediate);
        _methodCollector.VerifyMethodCalled("SetFlushMode");
        _methodCollector.VerifyMethodInput("SetFlushMode", TestUtils.ReadFile("SetFlushMode_Immediate"));
    }

    [Test]
    public void SetFlushMode_AppClose()
    {
        BloomreachSDK.SetFlushMode(FlushMode.AppClose);
        _methodCollector.VerifyMethodCalled("SetFlushMode");
        _methodCollector.VerifyMethodInput("SetFlushMode", TestUtils.ReadFile("SetFlushMode_AppClose"));
    }

    [Test]
    public void SetFlushMode_Manual()
    {
        BloomreachSDK.SetFlushMode(FlushMode.Manual);
        _methodCollector.VerifyMethodCalled("SetFlushMode");
        _methodCollector.VerifyMethodInput("SetFlushMode", TestUtils.ReadFile("SetFlushMode_Manual"));
    }

    [Test]
    public void SetFlushMode_Period()
    {
        BloomreachSDK.SetFlushMode(FlushMode.Period);
        _methodCollector.VerifyMethodCalled("SetFlushMode");
        _methodCollector.VerifyMethodInput("SetFlushMode", TestUtils.ReadFile("SetFlushMode_Period"));
    }

    [Test]
    public void GetFlushPeriod_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushPeriod", TestUtils.ReadFile("GetFlushPeriod_Invalid"));
        var flushPeriod = BloomreachSDK.GetFlushPeriod();
        _methodCollector.VerifyMethodCalled("GetFlushPeriod");
        Assert.That(
            flushPeriod.Ticks,
            Is.EqualTo(0)
        );
    }

    [Test]
    public void GetFlushPeriod_Zero()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushPeriod", TestUtils.ReadFile("GetFlushPeriod_Zero"));
        var flushPeriod = BloomreachSDK.GetFlushPeriod();
        _methodCollector.VerifyMethodCalled("GetFlushPeriod");
        Assert.That(
            flushPeriod.Ticks,
            Is.EqualTo(0)
        );
    }

    [Test]
    public void GetFlushPeriod_SomeValue()
    {
        _methodCollector.RegisterSuccessMethodResult("GetFlushPeriod", TestUtils.ReadFile("GetFlushPeriod_SomeValue"));
        var flushPeriod = BloomreachSDK.GetFlushPeriod();
        _methodCollector.VerifyMethodCalled("GetFlushPeriod");
        Assert.That(
            flushPeriod.TotalMilliseconds,
            Is.EqualTo(10000)
        );
    }

    [Test]
    public void GetLogLevel_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Invalid"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Unknown)
        );
    }

    [Test]
    public void GetLogLevel_Unknown()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Unknown"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Unknown)
        );
    }

    [Test]
    public void GetLogLevel_Error()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Error"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Error)
        );
    }

    [Test]
    public void GetLogLevel_Debug()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Debug"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Debug)
        );
    }

    [Test]
    public void GetLogLevel_Info()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Info"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Info)
        );
    }

    [Test]
    public void GetLogLevel_Verbose()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Verbose"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Verbose)
        );
    }

    [Test]
    public void GetLogLevel_Off()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Off"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Off)
        );
    }

    [Test]
    public void GetLogLevel_Warn()
    {
        _methodCollector.RegisterSuccessMethodResult("GetLogLevel", TestUtils.ReadFile("GetLogLevel_Warn"));
        var logLevel = BloomreachSDK.GetLogLevel();
        _methodCollector.VerifyMethodCalled("GetLogLevel");
        Assert.That(
            logLevel,
            Is.EqualTo(LogLevel.Warn)
        );
    }

    [Test]
    public void GetSessionTimeout_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetSessionTimeout", TestUtils.ReadFile("GetSessionTimeout_Invalid"));
        var sessionTimeout = BloomreachSDK.GetSessionTimeout();
        _methodCollector.VerifyMethodCalled("GetSessionTimeout");
        Assert.That(
            sessionTimeout.Ticks,
            Is.EqualTo(0)
        );
    }

    [Test]
    public void GetSessionTimeout_Zero()
    {
        _methodCollector.RegisterSuccessMethodResult("GetSessionTimeout", TestUtils.ReadFile("GetSessionTimeout_Zero"));
        var sessionTimeout = BloomreachSDK.GetSessionTimeout();
        _methodCollector.VerifyMethodCalled("GetSessionTimeout");
        Assert.That(
            sessionTimeout.Ticks,
            Is.EqualTo(0)
        );
    }

    [Test]
    public void GetSessionTimeout_SomeValue()
    {
        _methodCollector.RegisterSuccessMethodResult("GetSessionTimeout", TestUtils.ReadFile("GetSessionTimeout_SomeValue"));
        var sessionTimeout = BloomreachSDK.GetSessionTimeout();
        _methodCollector.VerifyMethodCalled("GetSessionTimeout");
        Assert.That(
            sessionTimeout.TotalMilliseconds,
            Is.EqualTo(10000)
        );
    }

    [Test]
    public void SetDefaultProperties_Empty()
    {
        BloomreachSDK.SetDefaultProperties(new Dictionary<string, object>());
        _methodCollector.VerifyMethodCalled("SetDefaultProperties");
        _methodCollector.VerifyMethodInput("SetDefaultProperties", TestUtils.ReadFile("SetDefaultProperties_Empty"));
    }

    [Test]
    public void SetDefaultProperties_Simple()
    {
        BloomreachSDK.SetDefaultProperties(new Dictionary<string, object>()
        {
            {"defProp1", "defVal1"},
            {"defProp2", 2}
        });
        _methodCollector.VerifyMethodCalled("SetDefaultProperties");
        _methodCollector.VerifyMethodInput("SetDefaultProperties", TestUtils.ReadFile("SetDefaultProperties_Simple"));
    }

    [Test]
    public void SetFlushPeriod_Negative()
    {
        BloomreachSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(-10000));
        _methodCollector.VerifyMethodCalled("SetFlushPeriod");
        _methodCollector.VerifyMethodInput("SetFlushPeriod", TestUtils.ReadFile("SetFlushPeriod_Negative"));
    }

    [Test]
    public void SetFlushPeriod_Zero()
    {
        BloomreachSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(0));
        _methodCollector.VerifyMethodCalled("SetFlushPeriod");
        _methodCollector.VerifyMethodInput("SetFlushPeriod", TestUtils.ReadFile("SetFlushPeriod_Zero"));
    }

    [Test]
    public void SetFlushPeriod_Positive()
    {
        BloomreachSDK.SetFlushPeriod(TimeSpan.FromMilliseconds(10000));
        _methodCollector.VerifyMethodCalled("SetFlushPeriod");
        _methodCollector.VerifyMethodInput("SetFlushPeriod", TestUtils.ReadFile("SetFlushPeriod_Positive"));
    }

    [Test]
    public void SetLogLevel_Unknown()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Unknown);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Unknown"));
    }

    [Test]
    public void SetLogLevel_Debug()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Debug);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Debug"));
    }

    [Test]
    public void SetLogLevel_Error()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Error);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Error"));
    }

    [Test]
    public void SetLogLevel_Info()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Info);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Info"));
    }

    [Test]
    public void SetLogLevel_Verbose()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Verbose);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Verbose"));
    }

    [Test]
    public void SetLogLevel_Off()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Off);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Off"));
    }

    [Test]
    public void SetLogLevel_Warn()
    {
        BloomreachSDK.SetLogLevel(LogLevel.Warn);
        _methodCollector.VerifyMethodCalled("SetLogLevel");
        _methodCollector.VerifyMethodInput("SetLogLevel", TestUtils.ReadFile("SetLogLevel_Warn"));
    }

    [Test]
    public void SetSessionTimeout_Negative()
    {
        BloomreachSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(-10000));
        _methodCollector.VerifyMethodCalled("SetSessionTimeout");
        _methodCollector.VerifyMethodInput("SetSessionTimeout", TestUtils.ReadFile("SetSessionTimeout_Negative"));
    }

    [Test]
    public void SetSessionTimeout_Zero()
    {
        BloomreachSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(0));
        _methodCollector.VerifyMethodCalled("SetSessionTimeout");
        _methodCollector.VerifyMethodInput("SetSessionTimeout", TestUtils.ReadFile("SetSessionTimeout_Zero"));
    }

    [Test]
    public void SetSessionTimeout_Positive()
    {
        BloomreachSDK.SetSessionTimeout(TimeSpan.FromMilliseconds(10000));
        _methodCollector.VerifyMethodCalled("SetSessionTimeout");
        _methodCollector.VerifyMethodInput("SetSessionTimeout", TestUtils.ReadFile("SetSessionTimeout_Positive"));
    }

    [Test]
    public void GetCheckPushSetup_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetCheckPushSetup", TestUtils.ReadFile("GetCheckPushSetup_Invalid"));
        var result = BloomreachSDK.GetCheckPushSetup();
        _methodCollector.VerifyMethodCalled("GetCheckPushSetup");
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetCheckPushSetup_False()
    {
        _methodCollector.RegisterSuccessMethodResult("GetCheckPushSetup", TestUtils.ReadFile("GetCheckPushSetup_False"));
        var result = BloomreachSDK.GetCheckPushSetup();
        _methodCollector.VerifyMethodCalled("GetCheckPushSetup");
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetCheckPushSetup_True()
    {
        _methodCollector.RegisterSuccessMethodResult("GetCheckPushSetup", TestUtils.ReadFile("GetCheckPushSetup_True"));
        var result = BloomreachSDK.GetCheckPushSetup();
        _methodCollector.VerifyMethodCalled("GetCheckPushSetup");
        Assert.That(result, Is.True);
    }

    [Test]
    public void GetTokenTrackFrequency_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("GetTokenTrackFrequency", TestUtils.ReadFile("GetTokenTrackFrequency_Invalid"));
        var tokenFrequency = BloomreachSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.OnTokenChange));
    }

    [Test]
    public void GetTokenTrackFrequency_OnTokenChange()
    {
        _methodCollector.RegisterSuccessMethodResult("GetTokenTrackFrequency", TestUtils.ReadFile("GetTokenTrackFrequency_OnTokenChange"));
        var tokenFrequency = BloomreachSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.OnTokenChange));
    }

    [Test]
    public void GetTokenTrackFrequency_EveryLaunch()
    {
        _methodCollector.RegisterSuccessMethodResult("GetTokenTrackFrequency", TestUtils.ReadFile("GetTokenTrackFrequency_EveryLaunch"));
        var tokenFrequency = BloomreachSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.EveryLaunch));
    }

    [Test]
    public void GetTokenTrackFrequency_Daily()
    {
        _methodCollector.RegisterSuccessMethodResult("GetTokenTrackFrequency", TestUtils.ReadFile("GetTokenTrackFrequency_Daily"));
        var tokenFrequency = BloomreachSDK.GetTokenTrackFrequency();
        _methodCollector.VerifyMethodCalled("GetTokenTrackFrequency");
        Assert.That(tokenFrequency, Is.EqualTo(TokenTrackFrequency.Daily));
    }

    [Test]
    public void IsAutomaticSessionTracking_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutomaticSessionTracking", TestUtils.ReadFile("IsAutomaticSessionTracking_Invalid"));
        var result = BloomreachSDK.IsAutomaticSessionTracking();
        _methodCollector.VerifyMethodCalled("IsAutomaticSessionTracking");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutomaticSessionTracking_False()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutomaticSessionTracking", TestUtils.ReadFile("IsAutomaticSessionTracking_False"));
        var result = BloomreachSDK.IsAutomaticSessionTracking();
        _methodCollector.VerifyMethodCalled("IsAutomaticSessionTracking");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutomaticSessionTracking_True()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutomaticSessionTracking", TestUtils.ReadFile("IsAutomaticSessionTracking_True"));
        var result = BloomreachSDK.IsAutomaticSessionTracking();
        _methodCollector.VerifyMethodCalled("IsAutomaticSessionTracking");
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsAutoPushNotification_Invalid()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutoPushNotification", TestUtils.ReadFile("IsAutoPushNotification_Invalid"));
        var result = BloomreachSDK.IsAutoPushNotification();
        _methodCollector.VerifyMethodCalled("IsAutoPushNotification");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutoPushNotification_False()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutoPushNotification", TestUtils.ReadFile("IsAutoPushNotification_False"));
        var result = BloomreachSDK.IsAutoPushNotification();
        _methodCollector.VerifyMethodCalled("IsAutoPushNotification");
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsAutoPushNotification_True()
    {
        _methodCollector.RegisterSuccessMethodResult("IsAutoPushNotification", TestUtils.ReadFile("IsAutoPushNotification_True"));
        var result = BloomreachSDK.IsAutoPushNotification();
        _methodCollector.VerifyMethodCalled("IsAutoPushNotification");
        Assert.That(result, Is.True);
    }

    [Test]
    public void SetCheckPushSetup_Disable()
    {
        BloomreachSDK.SetCheckPushSetup(false);
        _methodCollector.VerifyMethodCalled("SetCheckPushSetup");
        _methodCollector.VerifyMethodInput("SetCheckPushSetup", TestUtils.ReadFile("SetCheckPushSetup_Disable"));
    }

    [Test]
    public void SetCheckPushSetup_Enable()
    {
        BloomreachSDK.SetCheckPushSetup(true);
        _methodCollector.VerifyMethodCalled("SetCheckPushSetup");
        _methodCollector.VerifyMethodInput("SetCheckPushSetup", TestUtils.ReadFile("SetCheckPushSetup_Enable"));
    }

    [Test]
    public void SetAutomaticSessionTracking_Disabled()
    {
        BloomreachSDK.SetAutomaticSessionTracking(false);
        _methodCollector.VerifyMethodCalled("SetAutomaticSessionTracking");
        _methodCollector.VerifyMethodInput("SetAutomaticSessionTracking", TestUtils.ReadFile("SetAutomaticSessionTracking_Disabled"));
    }

    [Test]
    public void SetAutomaticSessionTracking_Enabled()
    {
        BloomreachSDK.SetAutomaticSessionTracking(true);
        _methodCollector.VerifyMethodCalled("SetAutomaticSessionTracking");
        _methodCollector.VerifyMethodInput("SetAutomaticSessionTracking", TestUtils.ReadFile("SetAutomaticSessionTracking_Enabled"));
    }
}
