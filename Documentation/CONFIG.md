


## 🔍 Configuration

Before using most of the SDK functionality, you'll need to configure Exponea to connect it to the backend application. Configuration consists of these properties:

#### ProjectToken

* Is your project token which can be found in the Exponea APP ```Project``` -> ```Overview```
* If you need to switch project settings during runtime of the application, you can use [Anonymize feature](./ANONYMIZE.md)

#### Authorization

* Exponea **public** key.
* For more information, please see [Exponea API documentation](https://docs.exponea.com/reference#access-keys)

#### BaseURL

* Base URL of your Exponea deployment.
* Default value `https://api.exponea.com`

#### ProjectRouteMap

* If you have more than one project to track for one event, you should specify the "Routes" for tracking.

For detailed information, please go to [Project Mapping documentation](./PROJECT_MAPPING.md)

#### MaxTries

* Maximum number of retries to flush data to Exponea API.
* SDK will consider the value flushed if this number is exceeded and delete it from the queue.

#### SessionTimeout

When the application is closed, the SDK doesn't track the end of the session right away but waits a bit for the user to come back before doing so. You can configure the timeout by setting this property.

#### AutomaticSessionTracking

* Flag to control the automatic tracking of user sessions.
* When set to true, the SDK will
automatically send `session_start` and `session_end` events to Exponea API
* You can opt out by setting this flag to false and implement your own session tracking.

#### DefaultProperties

* The properties defined here will always be sent with all triggered tracking events. 

#### TokenTrackFrequency

* You can define your policy for tracking push notification token. Default value `OnTokenChange` is recommended. 

>When changing this value, consider wisely all consequences. E.g., if token tracking frequency is set to DAILY, and you use "Anonymize" method for changing the customer, push token for the new customer can not be tracked immediately, but only after another day.

#### AllowDefaultCustomerProperties
* If true, default properties are applied also for 'identifyCustomer' event.

#### AutomaticPushNotification (Working on Android)

* Controls if the SDK will handle push notifications automatically.

#### PushIcon (Working on Android)

* Icon to be displayed when showing a push notification, specified by resource name. Place icon into resouces/drawable folder of your app, and enter it's name into PushIcon property when configuring the SDK.

#### PushAccentColor (Working on Android)

* Accent color of push notification. It changes the color of the small icon and notification buttons.
    > This is a color id, not a resource id. When using colors from resources, you have to get the resource first, and convert it to uint value, or get color directly from argb, e.g. `System.Drawing.Color.FromArgb(0, 0, 255).ToInt();` for blue.

#### PushChannelName (Working on Android)

* Name of the Channel to be created for the push notifications.
* Only available for API level 26+. More info [here](https://developer.android.com/training/notify-user/channels)

#### PushChannelDescription (Working on Android)

* Description of the Channel to be created for the push notifications.
* Only available for API level 26+. More info [here](https://developer.android.com/training/notify-user/channels)

#### PushChannelId (Working on Android)

* Channel ID for push notifications.
* Only available for API level 26+. More info [here](https://developer.android.com/training/notify-user/channels)

#### PushNotificationImportance (Working on Android)

* Notification importance for the notification channel.
* Only available for API level 26+. More info [here](https://developer.android.com/training/notify-user/channels)

#### RequirePushAuthorization (Working on iOS)
If true, push notification registration and push token tracking are only done if the device is authorized to display push notifications. Unless you're using silent notifications, keep the default value `true`.

#### AppGroup (Working on iOS)
 App group used for communication between the main app and notification extensions. AppGroup is a required field for Rich push notification setup.


#### Example
``` csharp
var config = new Configuration("project-token", "your-auth-token", "https://api.exponea.com");
config.AutomaticSessionTracking = false;
config.DefaultProperties = new Dictionary<string, object>()
            {
                { "thisIsADefaultStringProperty", "This is a default string value" },
                { "thisIsADefaultIntProperty", 1},
                { "thisIsADefaultDoubleProperty", 12.53623}

            };
config.AutomaticPushNotification = false;
ExponeaSDK.Configure(config);
```