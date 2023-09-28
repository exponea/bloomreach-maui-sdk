using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.OS;
using Huawei.Agconnect.Config;
using Huawei.Hms.Aaid;
using Huawei.Hms.Push;
using Android.Provider;
using Firebase.Messaging;
using Huawei.Hmf.Tasks;
using IOnSuccessListener = Android.Gms.Tasks.IOnSuccessListener;
using Task = System.Threading.Tasks.Task;

namespace ExampleApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void AttachBaseContext(Context context)
    {
        base.AttachBaseContext(context);
        if (IsGooglePlayAvailable())
        {
            Task.Run(async () =>
            {
                var token = await FirebaseMessaging.Instance.GetToken();
                Bloomreach.BloomreachSDK.TrackPushToken(token.ToString());
            });
        }
        else
        {
            // so we have to use HMS
            var config = AGConnectServicesConfig.FromContext(context);
            config.OverlayWith(new HmsLazyInputStream(context));
            HmsMessaging.GetInstance(this).AutoInitEnabled = true;
            Task.Run(() =>
            {
                var token = HmsInstanceId.GetInstance(this).GetToken("104661225", "HCM");
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
