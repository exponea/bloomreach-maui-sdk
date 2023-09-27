
## 🔍 Enabling universal links

- Ensure you have added the Associated Domains Entitlement to your app in the Capabilities/Associated Domains, e.g.: `applinks:yourdomain.com` and `webcredentials:yourdomain.com`.
- Ensure you have set up the Apple App Site Association file on your website appropriately configured according to the [Apple's documentation](https://developer.apple.com/documentation/security/password_autofill/setting_up_an_app_s_associated_domains#3001215).

Once the setup is completed, opening the universal link should open your app.

> **NOTE:** Easiest way to test the integration is to send yourself an email containing the Universal link and open it in your email client in a web browser. Universal links work correctly when a user taps `<a href= "...">` that will drive the user to another domain. Pasting the URL into Safari won't work. Neither does following the link on the same domain or opening the URL with Javascript.

## 🔍 Tracking universal links
Update your app's App Delegate to respond to the universal link.

When iOS opens your app due to a universal link, your app receives an `NSUserActivity` object with an `activityType` value of `NSUserActivityTypeBrowsingWeb`. The activity object's `webpageURL` property contains the URL that needs to be passed on to the Bloomreach SDK's `HandleCampaignClick` method.  

#### 💻 Example

```csharp
public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
{
    var campaignUrl = userActivity.WebPageUrl?.ToString();
    if (userActivity.ActivityType == "NSUserActivityTypeBrowsingWeb" && campaignUrl != null)
    {
        BloomreachSDK.HandleCampaignClick(new Uri(campaignUrl));
        return userActivity.WebPageUrl.Host == "yourdomain.com";
    }

    return false;
}

public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
{
    BloomreachSDK.HandleCampaignClick(url);
    return url.Host == "yourdomain.com";
}
```

When an iOS app is loaded from a Universal Link, you can override the ContinueUserActivity in AppDelegate.cs to get the URL passed. From here, you can move around in your app as needed.

If your app is **not running**, `OpenUrl` method will be called instead.

> **NOTE:** Bloomreach SDK might not be configured when `HandleCampaignClick` is called. In this case, the event will be sent to Bloomreach servers **after** SDK is configured.