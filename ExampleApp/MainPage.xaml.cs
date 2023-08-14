using System;
using System.Collections.Generic;
using ExponeaSDK;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace ExampleApp;

public partial class MainPage : ContentPage
{
	private int _count = 0;

	public MainPage()
	{
		InitializeComponent();
		
        var config = new Configuration(
            "b556af1a-bf4e-11ed-ac28-de4945357d1a",
            "urncrotvrtuomaircpsettnbz2wgpey1uj0zozwlylqp1ftfvw46dnvvq7rnivd8",
            "https://demoapp-api.bloomreach.com"
            );
        config.AutomaticSessionTracking = true;

        var props = new Dictionary<string, object>()
            {
                { "thisIsADefaultStringProperty", "This is a default string value" },
                { "thisIsADefaultIntProperty", 1},
                { "thisIsADefaultDoubleProperty", 12.53623}
            };

        config.DefaultProperties = props;

        config.IOsConfiguration = new iOSConfiguration(appGroup: "group.com.exponea.xamarin");

        config.AndroidConfiguration = new AndroidConfiguration(pushIcon: "push_icon", automaticPushNotification: true);

        config.AdvancedAuthEnabled = true;

        config.AllowDefaultCustomerProperties = false;
        Greetings.Text = ExponeaSDK.ExponeaSDK.ConfigureWithResult(config);

    }

	private void OnCounterClicked(object sender, EventArgs eventArgs)
	{
		_count++;

		if (_count == 1)
			CounterBtn.Text = $"Clicked {_count} time";
		else
			CounterBtn.Text = $"Clicked {_count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}


