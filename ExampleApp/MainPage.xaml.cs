using Bloomreach;

#if IOS
using Foundation;
using UIKit;
#endif

namespace ExampleApp;

public partial class MainPage : ContentPage
{
    
    public MainPage()
    {
        InitializeComponent();

        CustomerCookie.Text = "Customer cookie: \n" + Bloomreach.BloomreachSDK.GetCustomerCookie();
        SessionStartButton.IsVisible = !Bloomreach.BloomreachSDK.IsAutomaticSessionTracking();
        SessionEndButton.IsVisible = !Bloomreach.BloomreachSDK.IsAutomaticSessionTracking();
        RegisterForRemoteNotifications();
    }

    private static void RegisterForRemoteNotifications()
    {
#if IOS
        if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
        {
            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }
        else
        {
            UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
            UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
        }
#endif
    }

    async void ShowConfiguration(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfigInfoPage());
    }

    void TrackCustomEvent(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.TrackEvent(new Event("custom_event") { ["thisIsAStringProperty"] = "thisIsAStringValue" });
    }

    void TrackPayment(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.TrackPaymentEvent(new Payment(12.34, "EUR", "Virtual", "handbag", "Awesome leather handbag"));
    }

    void SessionStart(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.TrackSessionStart();
    }

    void SessionEnd(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.TrackSessionEnd();
    }

    void Anonymize(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.Anonymize();
        CustomerCookie.Text = "Customer cookie: \n" + Bloomreach.BloomreachSDK.GetCustomerCookie();
    }

    async void Flush(object sender, EventArgs e)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        try
        {
            await Bloomreach.BloomreachSDK.FlushData();
            await DisplayAlert("Flush finished", "Flush finished", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Flush failed", ex.Message, "OK");
        }
    }

    async void IdentifyCustomer(object sender, EventArgs e)
    {
        var registered = await DisplayPromptAsync("Identify customer", "Registered");
        var propertyName = await DisplayPromptAsync("Identify customer", "Property name");
        var propertyValue = await DisplayPromptAsync("Identify customer", "Property value");
        // Preparing the data.
        if (string.IsNullOrEmpty(registered) || string.IsNullOrEmpty(propertyName) ||
            string.IsNullOrEmpty(propertyValue))
        {
            await DisplayAlert("Error", "One or more fields were left empty, skipping identifying the customer.", "OK");
            return;
        }

        var customer = new Customer(registered)
            .WithProperty(propertyName, propertyValue);
        Bloomreach.BloomreachSDK.IdentifyCustomer(customer);
    }

    async void Switch_Project_ClickedAsync(object sender, EventArgs e)
    {
        var projectToken = await DisplayPromptAsync("Switch project", "Project token");
        var authorization = await DisplayPromptAsync("Switch project", "Authorization");
        var baseUrl = await DisplayPromptAsync("Switch project", "Base URL");

        if (string.IsNullOrEmpty(projectToken) || string.IsNullOrEmpty(authorization) || string.IsNullOrEmpty(baseUrl))
        {
            await DisplayAlert("Error", "One or more fields were left empty, skipping switching the project.", "OK");
            return;
        }

        Bloomreach.BloomreachSDK.Anonymize(new Project(projectToken, authorization, baseUrl));
        CustomerCookie.Text = "Customer cookie: \n" + Bloomreach.BloomreachSDK.GetCustomerCookie();
        Preferences.Set("projectToken", projectToken);
        Preferences.Set("authorization", authorization);
        Preferences.Set("baseURL", baseUrl);
    }
}