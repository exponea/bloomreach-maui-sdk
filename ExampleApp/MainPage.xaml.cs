using System;
using System.Collections.Generic;
using Bloomreach;
using Bloomreach.View;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Embedding;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Storage;
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
        CustomizeAppInboxStyle();
        var appInbox = Bloomreach.BloomreachSDK.GetAppInboxButton();
        if (appInbox != null)
        {
            AppInboxTargetLayout.Add(appInbox);
        }
        RegisterCustomizedInAppHandler();
    }

    private static void CustomizeAppInboxStyle()
    {
        Bloomreach.BloomreachSDK.SetAppInboxProvider(new AppInboxStyle()
        {
            AppInboxButton = new ButtonStyle()
            {
                BackgroundColor = "#191970",
                BorderRadius = "10dp",
            },
            DetailView = new DetailViewStyle()
            {
                Title = new TextViewStyle()
                {
                    TextColor = "rbga(11, 156, 49, 0.6)"
                },
                Content = new TextViewStyle()
                {
                    TextColor = "darkmagenta"
                }
            }
        });
    }

    private void RegisterCustomizedInAppHandler()
    {
        Bloomreach.BloomreachSDK.SetInAppMessageActionCallback(false, false,
            (message, text, url, interaction) =>
            {
                var infoMessage = $"Msg name: {message.Name}\n" +
                                  $"Button name: {text}\n" +
                                  $"URL: {url}\n" +
                                  $"User interacts: {interaction}";
                if (url == null)
                {
                    Bloomreach.BloomreachSDK.TrackInAppMessageClose(message, interaction);
                }
                else
                {
                    Bloomreach.BloomreachSDK.TrackInAppMessageClick(message, text, url);
                }
            }
        );
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

    void Register_For_Push_Clicked(object sender, EventArgs e)
    {
        Bloomreach.BloomreachSDK.RequestPushAuthorization();
    }

    void Track_Delivered_Clicked(object sender, EventArgs e)
    {
        var payload = NotificationPayload.Parse(new Dictionary<string, string>
        {
            {"campaign_id", "id"}
        });
        Bloomreach.BloomreachSDK.TrackDeliveredPush(payload);
    }

    void Track_Clicked_Clicked(object sender, EventArgs e)
    {
        var action = new NotificationAction(
            "click",
            "action",
            "https://bloomreach.com"
        ).WithAttribute("campaign_id", "id");
        Bloomreach.BloomreachSDK.TrackClickedPush(action);
    }
    
    async void Fetch_AppInbox_ClickedAsync(System.Object sender, System.EventArgs e)
    {
        try
        {
            var res = await Bloomreach.BloomreachSDK.FetchAppInbox();
            await DisplayAlert("AppInbox fetched", "Got messages: " + res.Count, "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("AppInbox fetch failed", exception.Message, "OK");
        }
    }

    async void Fetch_AppInboxItem_ClickedAsync(System.Object sender, System.EventArgs e)
    {
        try
        {
            var all = await Bloomreach.BloomreachSDK.FetchAppInbox();
            var res = await Bloomreach.BloomreachSDK.FetchAppInboxItem(all[0].Id);
            await DisplayAlert("AppInbox fetched", "Got message", "OK");
        }
        catch (Exception exception)
        {
            await DisplayAlert("Recommendations fetch failed", exception.Message, "OK");
        }
    }
}