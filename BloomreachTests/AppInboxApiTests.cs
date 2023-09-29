using Bloomreach;
using Bloomreach.View;
using BloomreachTests.Utils;

namespace BloomreachTests;

public class AppInboxApiTests
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
    public void TrackAppInboxOpenedWithoutTrackingConsent_Html()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxOpenedWithoutTrackingConsent(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpenedWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpenedWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxOpenedWithoutTrackingConsent_Html"));
    }

    [Test]
    public void TrackAppInboxOpenedWithoutTrackingConsent_Push()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Push);
        BloomreachSDK.TrackAppInboxOpenedWithoutTrackingConsent(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpenedWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpenedWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxOpenedWithoutTrackingConsent_Push"));
    }

    [Test]
    public void TrackAppInboxOpenedWithoutTrackingConsent_Unknown()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Unknown);
        BloomreachSDK.TrackAppInboxOpenedWithoutTrackingConsent(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpenedWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpenedWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxOpenedWithoutTrackingConsent_Unknown"));
    }

    [Test]
    public void TrackAppInboxOpened_Html()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxOpened(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpened");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpened", TestUtils.ReadFile("TrackAppInboxOpened_Html"));
    }

    [Test]
    public void TrackAppInboxOpened_Push()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Push);
        BloomreachSDK.TrackAppInboxOpened(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpened");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpened", TestUtils.ReadFile("TrackAppInboxOpened_Push"));
    }

    [Test]
    public void TrackAppInboxOpened_Unknown()
    {
        var message = BuildAppInboxMessage(AppInboxMessageType.Unknown);
        BloomreachSDK.TrackAppInboxOpened(message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxOpened");
        _methodCollector.VerifyMethodInput("TrackAppInboxOpened", TestUtils.ReadFile("TrackAppInboxOpened_Unknown"));
    }

    [Test]
    public void TrackAppInboxClickWithoutTrackingConsent_App_Html()
    {
        var action = BuildAppInboxAction(AppInboxActionType.App);
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxClickWithoutTrackingConsent(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClickWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxClickWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxClickWithoutTrackingConsent_App_Html"));
    }

    [Test]
    public void TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown()
    {
        var action = BuildAppInboxAction(AppInboxActionType.NoAction);
        var message = BuildAppInboxMessage(AppInboxMessageType.Unknown);
        BloomreachSDK.TrackAppInboxClickWithoutTrackingConsent(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClickWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxClickWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxClickWithoutTrackingConsent_NoAction_Unknown"));
    }

    [Test]
    public void TrackAppInboxClickWithoutTrackingConsent_Browser_Push()
    {
        var action = BuildAppInboxAction(AppInboxActionType.Browser);
        var message = BuildAppInboxMessage(AppInboxMessageType.Push);
        BloomreachSDK.TrackAppInboxClickWithoutTrackingConsent(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClickWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxClickWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxClickWithoutTrackingConsent_Browser_Push"));
    }

    [Test]
    public void TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html()
    {
        var action = BuildAppInboxAction(AppInboxActionType.Deeplink);
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxClickWithoutTrackingConsent(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClickWithoutTrackingConsent");
        _methodCollector.VerifyMethodInput("TrackAppInboxClickWithoutTrackingConsent", TestUtils.ReadFile("TrackAppInboxClickWithoutTrackingConsent_Deeplink_Html"));
    }

    [Test]
    public void TrackAppInboxClick_App_Html()
    {
        var action = BuildAppInboxAction(AppInboxActionType.App);
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxClick(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClick");
        _methodCollector.VerifyMethodInput("TrackAppInboxClick", TestUtils.ReadFile("TrackAppInboxClick_App_Html"));
    }

    [Test]
    public void TrackAppInboxClick_NoAction_Unknown()
    {
        var action = BuildAppInboxAction(AppInboxActionType.NoAction);
        var message = BuildAppInboxMessage(AppInboxMessageType.Unknown);
        BloomreachSDK.TrackAppInboxClick(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClick");
        _methodCollector.VerifyMethodInput("TrackAppInboxClick", TestUtils.ReadFile("TrackAppInboxClick_NoAction_Unknown"));
    }

    [Test]
    public void TrackAppInboxClick_Browser_Push()
    {
        var action = BuildAppInboxAction(AppInboxActionType.Browser);
        var message = BuildAppInboxMessage(AppInboxMessageType.Push);
        BloomreachSDK.TrackAppInboxClick(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClick");
        _methodCollector.VerifyMethodInput("TrackAppInboxClick", TestUtils.ReadFile("TrackAppInboxClick_Browser_Push"));
    }

    [Test]
    public void TrackAppInboxClick_Deeplink_Html()
    {
        var action = BuildAppInboxAction(AppInboxActionType.Deeplink);
        var message = BuildAppInboxMessage(AppInboxMessageType.Html);
        BloomreachSDK.TrackAppInboxClick(action, message);
        _methodCollector.VerifyMethodCalled("TrackAppInboxClick");
        _methodCollector.VerifyMethodInput("TrackAppInboxClick", TestUtils.ReadFile("TrackAppInboxClick_Deeplink_Html"));
    }

    [Test]
    public void MarkAppInboxAsRead_InputCheck()
    {
        BloomreachSDK.MarkAppInboxAsRead("12345");
        _methodCollector.VerifyMethodCalled("MarkAppInboxAsRead");
        _methodCollector.VerifyMethodInput("MarkAppInboxAsRead", "12345");
    }

    [Test]
    public void MarkAppInboxAsRead_Invalid()
    {
        _methodCollector.RegisterFailureMethodResult("MarkAppInboxAsRead", "Not init sdk");
        var task = BloomreachSDK.MarkAppInboxAsRead("12345");
        _methodCollector.VerifyMethodCalled("MarkAppInboxAsRead");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.False
        );
    }

    [Test]
    public void MarkAppInboxAsRead_ValidFalse()
    {
        _methodCollector.RegisterSuccessMethodResult("MarkAppInboxAsRead", "false");
        var task = BloomreachSDK.MarkAppInboxAsRead("12345");
        _methodCollector.VerifyMethodCalled("MarkAppInboxAsRead");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.False
        );
    }

    [Test]
    public void MarkAppInboxAsRead_ValidTrue()
    {
        _methodCollector.RegisterSuccessMethodResult("MarkAppInboxAsRead", "true");
        var task = BloomreachSDK.MarkAppInboxAsRead("12345");
        _methodCollector.VerifyMethodCalled("MarkAppInboxAsRead");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.True
        );
    }

    [Test]
    public void FetchAppInboxItem_InputCheck()
    {
        BloomreachSDK.FetchAppInboxItem("12345");
        _methodCollector.VerifyMethodCalled("FetchAppInboxItem");
        _methodCollector.VerifyMethodInput("FetchAppInboxItem", "12345");
    }

    [Test]
    public void FetchAppInboxItem_Error()
    {
        _methodCollector.RegisterFailureMethodResult("FetchAppInboxItem", "Not init sdk");
        var task = BloomreachSDK.FetchAppInboxItem("12345");
        _methodCollector.VerifyMethodCalled("FetchAppInboxItem");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.Null
        );
    }

    [Test]
    public void FetchAppInboxItem_Valid()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchAppInboxItem", TestUtils.ReadFile("FetchAppInboxItem_Valid"));
        var task = BloomreachSDK.FetchAppInboxItem("12345");
        _methodCollector.VerifyMethodCalled("FetchAppInboxItem");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        Assert.That(
            task.Result,
            Is.Not.Null
        );
        Assert.That(
            task.Result?.Id,
            Is.EqualTo("12345")
        );
    }

    [Test]
    public void FetchAppInbox_Error()
    {
        _methodCollector.RegisterFailureMethodResult("FetchAppInbox", "Not init sdk");
        var task = BloomreachSDK.FetchAppInbox();
        _methodCollector.VerifyMethodCalled("FetchAppInbox");
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
    public void FetchAppInbox_Empty()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchAppInbox", TestUtils.ReadFile("FetchAppInbox_Empty"));
        var task = BloomreachSDK.FetchAppInbox();
        _methodCollector.VerifyMethodCalled("FetchAppInbox");
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
    public void FetchAppInbox_Single()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchAppInbox", TestUtils.ReadFile("FetchAppInbox_Single"));
        var task = BloomreachSDK.FetchAppInbox();
        _methodCollector.VerifyMethodCalled("FetchAppInbox");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        var appInboxMessages = task.Result;
        Assert.That(
            appInboxMessages,
            Has.Count.EqualTo(1)
        );
        Assert.That(
            appInboxMessages[0].Id,
            Is.EqualTo("12345")
        );
    }

    [Test]
    public void FetchAppInbox_Multiple()
    {
        _methodCollector.RegisterSuccessMethodResult("FetchAppInbox", TestUtils.ReadFile("FetchAppInbox_Multiple"));
        var task = BloomreachSDK.FetchAppInbox();
        _methodCollector.VerifyMethodCalled("FetchAppInbox");
        task.Wait(1000);
        Assert.That(
            task.Status,
            Is.EqualTo(TaskStatus.RanToCompletion)
        );
        var appInboxMessages = task.Result;
        Assert.That(
            appInboxMessages,
            Has.Count.EqualTo(2)
        );
        Assert.That(
            appInboxMessages[0].Id,
            Is.EqualTo("12345")
        );
        Assert.That(
            appInboxMessages[1].Id,
            Is.EqualTo("67890")
        );
    }

    [Test]
    public void SetAppInboxProvider_Empty()
    {
        BloomreachSDK.SetAppInboxProvider(new AppInboxStyle());
        _methodCollector.VerifyMethodCalled("SetAppInboxProvider");
        _methodCollector.VerifyMethodInput("SetAppInboxProvider", TestUtils.ReadFile("SetAppInboxProvider_Empty"));
    }

    [Test]
    public void SetAppInboxProvider_Full()
    {
        var buttonStyle = new ButtonStyle()
        {
            BackgroundColor = "#00FF00",
            BorderRadius = "20px",
            Enabled = true,
            ShowIcon = "random.png",
            TextColor = "#FF0000",
            TextOverride = "Test unit",
            TextSize = "20sp",
            TextWeight = "bold"
        };
        var textViewStyle = new TextViewStyle()
        {
            TextColor = "red",
            TextOverride = "Test style unit",
            TextSize = "14dp",
            TextWeight = "normal",
            Visible = true
        };
        var imageViewStyle = new ImageViewStyle()
        {
            BackgroundColor = "transparent",
            Visible = false
        };
        var appInboxStyle = new AppInboxStyle()
        {
            AppInboxButton = buttonStyle,
            DetailView = new DetailViewStyle()
            {
                Button = buttonStyle,
                Content = textViewStyle,
                Image = imageViewStyle,
                ReceivedTime = textViewStyle,
                Title = textViewStyle
            },
            ListView = new ListScreenStyle()
            {
                EmptyMessage = textViewStyle,
                EmptyTitle = textViewStyle,
                ErrorMessage = textViewStyle,
                ErrorTitle = textViewStyle,
                List = new AppInboxListViewStyle()
                {
                    BackgroundColor = "#00FFFF",
                    Item = new AppInboxListItemStyle()
                    {
                        BackgroundColor = "#123456",
                        Content = textViewStyle,
                        Image = imageViewStyle,
                        ReadFlag = imageViewStyle,
                        ReceivedTime = textViewStyle,
                        Title = textViewStyle
                    }
                },
                Progress = new ProgressBarStyle()
                {
                    BackgroundColor = "#111111",
                    ProgressColor = "#ABCDEF",
                    Visible = true
                }
            }
        };
        BloomreachSDK.SetAppInboxProvider(appInboxStyle);
        _methodCollector.VerifyMethodCalled("SetAppInboxProvider");
        _methodCollector.VerifyMethodInput("SetAppInboxProvider", TestUtils.ReadFile("SetAppInboxProvider_Full"));
    }

    [Test]
    public void GetAppInboxButton_Error()
    {
        _methodCollector.RegisterFailureUiMethodResult("GetAppInboxButton", "Not init SDK");
        var button = BloomreachSDK.GetAppInboxButton();
        _methodCollector.VerifyMethodCalled("GetAppInboxButton");
        Assert.That(button, Is.Null);
    }

    [Test]
    public void GetAppInboxButton_Valid()
    {
        var response = new Button();
        _methodCollector.RegisterSuccessUiMethodResult("GetAppInboxButton", response);
        var button = BloomreachSDK.GetAppInboxButton();
        _methodCollector.VerifyMethodCalled("GetAppInboxButton");
        Assert.That(button, Is.Not.Null);
        Assert.That(button, Is.TypeOf(typeof(AppInboxButton)));
    }

    private static AppInboxAction BuildAppInboxAction(AppInboxActionType appInboxActionType)
    {
        return new AppInboxAction(
            appInboxActionType,
            "Action title",
            "https://example.com");
    }

    private static AppInboxMessage BuildAppInboxMessage(AppInboxMessageType appInboxMessageType)
    {
        return new AppInboxMessage(
            "12345",
            appInboxMessageType,
            true,
            10.0,
            new Dictionary<string, object>
            {
                {"prop1", "val1"},
                {"prop2", 2}
            });
    }

}
