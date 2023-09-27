using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Huawei.Agconnect.Config;
using Huawei.Hms.Aaid;
using Huawei.Hms.Push;
using Android.Provider;
using Huawei.Hmf.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ExampleApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void AttachBaseContext(Context context)
    {
        base.AttachBaseContext(context);
        Console.WriteLine("HMS-BR attaching base context");
        if (!IsGooglePlayAvailable())
        {
            // so we have to use HMS
            Console.WriteLine("HMS-BR using HMS");
            var config = AGConnectServicesConfig.FromContext(context);
            config.OverlayWith(new HmsLazyInputStream(context));
            HmsMessaging.GetInstance(this).AutoInitEnabled = true;
            Console.WriteLine("HMS-BR Push turned on");
            Task.Run(() =>
            {
                var token = HmsInstanceId.GetInstance(this).GetToken("104661225", "HCM");
                Console.WriteLine("HMS-BR Push token: " + token);
                Bloomreach.BloomreachSDK.TrackHmsPushToken(token);
            });
        }
    }

    private bool IsGooglePlayAvailable()
    {
        return GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this) == ConnectionResult.Success;
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var campaignUrl = Intent?.Data?.ToString();
        if (campaignUrl != null)
        {
            Bloomreach.BloomreachSDK.HandleCampaignClick(new Uri(campaignUrl));
        }
    }
}
