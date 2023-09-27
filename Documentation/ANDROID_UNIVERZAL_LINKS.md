## 🔍 Enabling Android App Links
Android App Links are HTTP URLs that bring users directly to specific content in your Android app. To enable your app to handle App Links, you need to add an intent filter to your Android manifest and host a Digital Asset Link JSON file on your domain.

### Adding an intent filter to Android manifest

Since the AndroidManifest file is auto-generated in Maui, you will be doing changes in your MainActivity instead.

It's important to set `AutoVerify = true` so that the Android system can check your Digital Asset Link JSON and automatically handle App Links.

#### 💻 Example
```csharp
    [Activity(Label = "MauiExample"]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https",
        DataHost = "www.your-domain.name",
        DataPathPattern = "/yourpath/.*",
        AutoVerify = true
    )]
    public class MainActivity : MauiAppCompatActivity
   ...
```

### Adding Digital Asset Link JSON to your domain
The official [Google documentation](https://developer.android.com/training/app-links/verify-site-associations.html#web-assoc) explains how to do it in detail.

The resulting file should be hosted at `https://your-domain.name/.well-known/assetlinks.json`.

#### 💻 Example
```json
[{
  "relation": ["delegate_permission/common.handle_all_urls"],
  "target": {
    "namespace": "android_app",
    "package_name": "your.package.name",
    "sha256_cert_fingerprints":["SHA256 fingerprint of your app’s signing certificate"]
  }
}]
```

> **NOTE:** To get certificate SHA256 fingerprint you can use `keytool -list -v -keystore my-release-key.keystore`

## 🔍 Tracking Android App Links
Bloomreach SDK can automatically decide whether the Intent that opened your application is an App Link, so you just need to call `Bloomreach.BloomreachSDK.HandleCampaignClick(new Uri(campaignUrl))`.

To track session events with App Link parameters, you should call `HandleCampaignClick` **before** your Activity onResume method is called. Ideally, make the call in your MainActivity `.OnCreate` method.

#### 💻 Example
```csharp

 public class MainActivity : MauiAppCompatActivity
    {
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
```