﻿using Exponea;
using Color = System.Drawing.Color;

namespace ExampleApp;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        ProjectToken.Text = Preferences.Get("projectToken", "b556af1a-bf4e-11ed-ac28-de4945357d1a");
        Authorization.Text = Preferences.Get("authorization","urncrotvrtuomaircpsettnbz2wgpey1uj0zozwlylqp1ftfvw46dnvvq7rnivd8");
        Url.Text = Preferences.Get("baseURL", "https://demoapp-api.bloomreach.com");
        FlushMode.SelectedIndex = 0;

        if (Exponea.ExponeaSDK.IsConfigured())
        {
            GoToNextPage();
        }
    }


    void Configure_Clicked(Object sender, EventArgs e)
    {
        var config = new Configuration(ProjectToken.Text, Authorization.Text, Url.Text)
        {
            AutomaticSessionTracking = AutomaticSessionTracking.IsToggled,
            DefaultProperties = new Dictionary<string, object>
            {
                { "thisIsADefaultStringProperty", "This is a default string value" },
                { "thisIsADefaultIntProperty", 1 },
                { "thisIsADefaultDoubleProperty", 12.53623 }
            },
            AppGroup = "group.com.exponea.xamarin",
            PushIcon = "push_icon",
            PushAccentColor = Color.FromArgb(0, 0, 255).ToArgb(),
            AutomaticPushNotification = true,
            AdvancedAuthEnabled = false
        };

        Exponea.ExponeaSDK.Configure(config);
        var flushModeSelected = (FlushMode)FlushMode.SelectedItem;
        Exponea.ExponeaSDK.SetFlushMode(flushModeSelected);
        Exponea.ExponeaSDK.SetLogLevel(LogLevel.Verbose);
        if (flushModeSelected == Exponea.FlushMode.Period && Period.Text.Trim() != "")
        {
            var isParsable = int.TryParse(Period.Text, out var minutes);
            if (isParsable)
            {
                Exponea.ExponeaSDK.SetFlushPeriod(new TimeSpan(0, minutes: minutes, 0));
            }
            else
            {
                Console.WriteLine("Period could not be parsed.");
            }
        }
        Exponea.ExponeaSDK.SetDefaultProperties(new Dictionary<string, object>
        {
            { "thisIsADefaultStringProperty", "This is a default string value" },
            { "thisIsADefaultIntProperty", 1 },
            { "thisIsADefaultDoubleProperty", 12.53623 }
        });
        Preferences.Set("projectToken", ProjectToken.Text);
        Preferences.Set("authorization", Authorization.Text);
        Preferences.Set("baseURL", Url.Text);
        GoToNextPage();
    }

    void flushMode_SelectedIndexChanged(Object sender, EventArgs e)
    {
        Period.IsVisible = (FlushMode)FlushMode.SelectedItem == Exponea.FlushMode.Period;
    }

    private async void GoToNextPage()
    {
        await Navigation.PushAsync(new MainPage());
    }
}