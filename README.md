
# MAUI Bloomreach SDK
MAUI Bloomreach SDK allows your application to interact with the [Bloomreach](https://bloomreach.com/) Customer Data & Experience Platform. Bloomreach empowers B2C marketers to raise conversion rates, improve acquisition ROI, and maximize customer lifetime value.


SDK is created as .net wrapper for binding libraries of [native Android SDK](https://github.com/exponea/exponea-android-sdk) and [native iOS SDK](https://github.com/exponea/exponea-ios-sdk).


## Getting started

 - Add BloomreachSDK NuGet as a dependency
 - Just use BloomreachSDK methods by calling 

 ```csharp
   Bloomreach.BloomreachSDK.Configure(yourConfig);
 ```

Bloomreach SDK for multi-platform App UI (.NET MAUI) supports the following platforms:
  * Android 5.0 (API 21) or higher
  * iOS 11 or higher

## Documentation
  * [Configuration](./Documentation/CONFIG.md)
  * [Tracking](./Documentation/TRACK.md)
  * [Tracking Campaigns(Android App Links)](./Documentation/ANDROID_UNIVERZAL_LINKS.md)
  * [Tracking Campaigns(Universal links)](./Documentation/IOS_UNIVERSAL_LINKS.md)
  * [Flushing](./Documentation/FLUSH.md)
  * [Push notifications](./Documentation/PUSH.md)
  * [Anonymize customer](./Documentation/ANONYMIZE.md)
  * [Payment tracking](./Documentation/PAYMENT.md)
  * [Project mapping](./Documentation/PROJECT_MAPPING.md)
  
## Release Notes

[Release notes](./Documentation/RELEASE_NOTES.md) for the SDK.

## Support

Are you a Bloomreach customer and dealing with some issues on mobile SDK? You can reach the official Engagement Support [via these recommended ways](https://documentation.bloomreach.com/engagement/docs/engagement-support#contacting-the-support).
Note that Github repository issues and PRs will also be considered but with the lowest priority and without guaranteed output.
