using Exponea;
using System.Text.Json;

namespace ExampleApp;

public partial class ConfigInfoPage : ContentPage
{
    public ConfigInfoPage()
    {
        InitializeComponent();

        AutomaticSessionTracking.Text = Exponea.ExponeaSDK.IsAutomaticSessionTracking() ? "enabled" : "disabled";
        FlushMode.Text = Exponea.ExponeaSDK.GetFlushMode().ToString();
        FlushPeriod.Text = Exponea.ExponeaSDK.GetFlushPeriod().ToString();
        LogLevel.Text = Exponea.ExponeaSDK.GetLogLevel().ToString();
        DefaultProperties.Text = JsonSerializer.Serialize(Exponea.ExponeaSDK.GetDefaultProperties());
    }

}
