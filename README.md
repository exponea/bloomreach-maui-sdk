
# MAUI Exponea SDK
MAUI Exponea SDK allows your application to interact with the [Exponea](https://exponea.com/) Customer Data & Experience Platform. Exponea empowers B2C marketers to raise conversion rates, improve acquisition ROI, and maximize customer lifetime value.


SDK is created as .net wrapper for binding libraries of [native Android SDK](https://github.com/exponea/exponea-android-sdk) and [native iOS SDK](https://github.com/exponea/exponea-ios-sdk).


## Getting started

 - Add ExponeaSDK NuGet as a dependency
 - Just use ExponeaSDK methods by calling 

 ```csharp
   Exponea.ExponeaSDK.Configure(yourConfig);
 ```

Exponea SDK for multi-platform App UI (.NET MAUI) supports the following platforms:
  * Android 5.0 (API 21) or higher
  * iOS 11 or higher

## Documentation
  * [Configuration](./Documentation/CONFIG.md)
  * [Tracking](./Documentation/TRACK.md)
  * [Flushing](./Documentation/FLUSH.md)
  * [Anonymize customer](./Documentation/ANONYMIZE.md)
  * [Payment tracking](./Documentation/PAYMENT.md)
  * [Project mapping](./Documentation/PROJECT_MAPPING.md)
  
## Release Notes

[Release notes](./Documentation/RELEASE_NOTES.md) for the SDK.

## Support

Are you a Bloomreach customer and dealing with some issues on mobile SDK? You can reach the official Engagement Support [via these recommended ways](https://documentation.bloomreach.com/engagement/docs/engagement-support#contacting-the-support).
Note that Github repository issues and PRs will also be considered but with the lowest priority and without guaranteed output.
