# Push notifications
Bloomreach allows you to easily create complex scenarios which you can use to send push notifications directly to your customers. The following section explains how to enable push notifications.

There is some setup required for each of the native platforms.
* [iOS Push notification setup](./IOS_PUSH.md)
* [Android Push notification setup](./ANDROID_PUSH.md)

> To test your push notification setup, send push notifications to your device from Bloomreach backend following [Sending Push notifications guide](./PUSH_SEND.md)

## Responding to push notifications
Once you perform platform setup, your application should be able to receive push notifications. To respond to push notification interaction, you can setup a listener using `BloomreachSDK.SetOpenedPushCallback()`. The SDK will hold last push notification and call the listener once it's set, but it's still recommended to set the listener as soon as possible to keep good flow of your application.
```csharp
Bloomreach.BloomreachSDK.SetOpenedPushCallback(action =>
{
    switch (action.ActionType)
    {
        case "app":
            // last push directed user to your app with no link
            // log data defined on Bloomreach backend
            Console.WriteLine(action.Attributes);
            break;
        case "deeplink":
            // last push directed user to your app with deeplink
            Console.WriteLine(action.Url);
            break;
        case "web":
            // last push directed user to web, nothing to do here
            Console.WriteLine(action.Url);
            break;
    }
});
```

## Received push notifications
You can set up a listener for received push notifications using `BloomreachSDK.SetReceivedPushCallback`, which is mostly useful for silent push notifications. The SDK will hold last push notification and call the listener once it's set, but it's still recommended to set the listener as soon as possible.

``` csharp
Bloomreach.BloomreachSDK.SetReceivedPushCallback(payload =>
{
    foreach (var entry in payload.RawData)
    {
        Console.WriteLine($"Push Extra: {entry.Key} : {entry.Value} ");
    }
});
```
