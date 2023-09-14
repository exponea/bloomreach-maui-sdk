using Bloomreach;
using System.Text.Json;

namespace ExampleApp;

public partial class ConfigInfoPage : ContentPage
{
    public ConfigInfoPage()
    {
        InitializeComponent();

        AutomaticSessionTracking.Text = Bloomreach.BloomreachSDK.IsAutomaticSessionTracking() ? "enabled" : "disabled";
        FlushMode.Text = Bloomreach.BloomreachSDK.GetFlushMode().ToString();
        FlushPeriod.Text = Bloomreach.BloomreachSDK.GetFlushPeriod().ToString();
        LogLevel.Text = Bloomreach.BloomreachSDK.GetLogLevel().ToString();
        DefaultProperties.Text = JsonSerializer.Serialize(Bloomreach.BloomreachSDK.GetDefaultProperties());
    }

}
