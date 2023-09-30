## Authorization

Data between SDK and BE are delivered trough authorized HTTP/HTTPS communication. Level of security has to be defined by developer.
All authorization modes are used to set `Authorization` HTTP/HTTPS header.

### Token authorization

This mode is required and has to be set by `Authorization` parameter in `Configuration`. See [configuration](./CONFIG.md).

Token authorization mode has to be given in `Token <value>` format. It is used for all public API access by default:

* `POST /track/v2/projects/<projectToken>/customers` as tracking of customer data
* `POST /track/v2/projects/<projectToken>/customers/events` as tracking of event data
* `POST /track/v2/projects/<projectToken>/campaigns/clicks` as tracking campaign events
* `POST /data/v2/projects/<projectToken>/customers/attributes` as fetching customer attributes
* `POST /data/v2/projects/<projectToken>/consent/categories` as fetching consents
* `POST /webxp/s/<projectToken>/inappmessages?v=1` as fetching InApp messages
* `POST /webxp/projects/<projectToken>/appinbox/fetch` as fetching of AppInbox data
* `POST /webxp/projects/<projectToken>/appinbox/markasread` as marking of AppInbox message as read
* `POST /campaigns/send-self-check-notification?project_id=<projectToken>` as part of SelfCheck push notification flow

Please check more details about [Public API](https://documentation.bloomreach.com/engagement/reference/authentication#public-api-access).

``` csharp
var config = new Configuration("project-token", "your-auth-token", "https://api.exponea.com");
Bloomreach.BloomreachSDK.Configure(config);
```

> There is no change nor migration needed in case of using of `Authorization` parameter if you are already using SDK in your app.

### Customer Token authorization

JSON Web Tokens are an open, industry standard [RFC 7519](https://tools.ietf.org/html/rfc7519) method for representing claims securely between SDK and BE. We recommend this method to be used according to higher security.

This mode is optional and may be set by `AdvancedAuthEnabled` parameter in `Configuration`. See [configuration](./CONFIG.md).

Authorization value is used in `Bearer <value>` format. Currently it is supported for listed API (for others Token authorization is used when `AdvancedAuthEnabled = true`):

* `POST /webxp/projects/<projectToken>/appinbox/fetch` as fetching of AppInbox data
* `POST /webxp/projects/<projectToken>/appinbox/markasread` as marking of AppInbox message as read

To activate this mode you need to set `advancedAuthEnabled` parameter `true` in `Configuration` as in given example:

``` csharp
var config = new Configuration("project-token", "your-auth-token", "https://api.exponea.com");
config.AdvancedAuthEnabled = true;
Bloomreach.BloomreachSDK.Configure(config);
```

### Android authorization provider
First step is to register your AuthorizationProvider to AndroidManifest.xml file as:

```xml
<application
    ...
    <meta-data
        android:name="ExponeaAuthProvider"
        android:value="RegisteredClassName"
        />
</application>
```

>Note: AndroidManifest.xml meta-data are typically registered with `AssemblyInfo.cs` so just put this line `[assembly: MetaData("ExponeaAuthProvider", Value = "RegisteredClassName")]` into file.

Second step is to implement an Authorization provider. That part of code will be requested from Android native part of SDK.
To create an Authorization provider that will be requested from Android native code, you'll provide implementation:

``` csharp
using Android.Runtime;

namespace YourApplicationNamespace.Droid
{
    [Register("RegisteredClassName")]
    public class ExampleAuthProvider : Java.Lang.Object, IAuthorizationProvider
    {
        public ExampleAuthProvider()
        {
        }

        public string AuthorizationToken => ...
    }
}
```

> Please notice the requirements of class definition. Your class has to be registered with name (see `[Register("RegisteredClassName")]`) that will be searched by SDK. Also, let a class to inherit from `Java.Lang.Object` and `IAuthorizationProvider` that has to be defined to support valid communication between native and xamarin implementations.

If you define AuthorizationProvider but is not working correctly, SDK initialization will fail. Please check for logs.
1. If you enable Customer Token authorization by configuration flag `AdvancedAuthEnabled` and implementation has not been found, you'll see log
   `Advanced auth has been enabled but provider has not been found`. Please repeat and validate the first step.
2. If you register class in AndroidManifest.xml but it cannot been found, you'll see log
   `Registered <your class> class has not been found` with detailed info. Please repeat and validate the first step.
3. If you register class in AndroidManifest.xml but it is not implementing auth interface, you will see log
   `Registered <your class> class has to implement com.exponea.sdk.services.AuthorizationProvider`.
   Please repeat and validate second step, check that class implements `IAuthorizationProvider`. (Log seems misleading but that is caused by differences between Java/Kotlin and #C languages)

AuthorizationProvider is loaded while SDK is initializing or after `BloomreachSDK.Configure()` is called; so you're able to see these logs in that time in case of any problem.

### iOS authorization provider
Only step is to implement a protocol of `MauiAuthorizationProvider` with `[Register("MauiAuthProvider")]` that will be requested from iOS native part of SDK. See given example:

```csharp
using Foundation;

namespace YourApplicationNamespace.iOS
{
    [Register("MauiAuthProvider")]
    public class ExampleAuthProvider : MauiAuthorizationProvider
    {
        public ExampleAuthProvider()
        {
        }

        public override string AuthorizationToken => ...
    }

}
```

> Please notice the requirements of class definition. Your class has to be registered with name (see `[Register("MauiAuthProvider")]`) that will be searched by SDK. Also, let a class to inherit from `MauiAuthorizationProvider` that has to be defined to support valid communication between native and MAUI implementations.

If you define MauiAuthProvider but is not working, please check for logs.
1. If you enable Customer Token authorization by configuration flag `AdvancedAuthEnabled` and implementation has not been found, you'll see log
   `Advanced authorization flag has been enabled without provider`
2. Registered class has to extend NSObject otherwise you'll see log
   `Class MauiAuthProvider does not conform to NSObject`
   This should be already provided by `MauiAuthorizationProvider` itself, but we note this log for any case.
3. Registered class has to conform to MauiAuthorizationProvider otherwise you'll see log
   `Class MauiAuthProvider does not conform to AuthorizationProviderType`
   Log seems misleading but that is caused by differences between Swift and #C languages. Class `MauiAuthorizationProvider` already implements this protocol, so if you see this logs, your class is not extending a class `MauiAuthorizationProvider` for some reason.

#### Asynchronous implementation of AuthorizationProvider

Token value is requested for every HTTP call in runtime. Value from `AuthorizationToken` is requested in background thread. Therefore, you are able to block any asynchronous token retrieval (i.e. other HTTP call) and waits for result by blocking this thread. In case of error result of your token retrieval you may return NULL value but request will automatically fail.

#### Customer Token retrieval policy

Token value is requested for every HTTP call (listed previously in this doc) that requires it.
As it is common thing that JWT tokens have own expiration lifetime so may be used multiple times. Thus information cannot be read from JWT token value directly so SDK is not storing token in any cache. As developer you may implement any type of token cache behavior you want.

> Please consider to store your cached token in more secured way. Android and iOS offers you multiple options such as using [KeyStore](https://developer.android.com/training/articles/keystore) or use [Encrypted Shared Preferences](https://developer.android.com/reference/androidx/security/crypto/EncryptedSharedPreferences)

> :warning: Customer Token is valid until its expiration and is assigned to current customer IDs. Bear in mind that if customer IDs have changed (during `identifyCustomer` or `anonymize` method) your Customer token is invalid for future HTTP requests invoked for new customer IDs.