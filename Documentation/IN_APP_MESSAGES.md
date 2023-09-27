## In-app messages
Bloomreach SDK allows you to display native In-app messages based on definitions set up on the Bloomreach web application. You can find information on how to create your messages in [Bloomreach documentation](https://docs.Bloomreach.com/docs/in-app-messages).

In-app messages do not require any development work. They work automatically after proper SDK initialization.

### Logging
The SDK logs a lot of useful information about presenting In-app messages on the default `Info` level. To see why each message was/wasn't displayed, make sure your logger level is `Info` at most. You can set the logger level before initializing the SDK using `Bloomreach.BloomreachSDK.SetLogLevel(LogLevel.Verbose)`

### Displaying In-app messages
In-app messages are triggered when an event is tracked based on conditions setup on the Bloomreach backend. Once a message passes those filters, the SDK will try to present the message.

Message is able to be shown only if it is loaded and also its image is loaded too. In case that message is not yet fully loaded (including its image) then the request-to-show is registered in SDK for that message so SDK will show it after full load.
Due to prevention of unpredicted behaviour (i.e. image loading takes too long) that request-to-show has timeout of 3 seconds.

> If message loading hits timeout of 3 seconds then message will be shown on 'next request'. For example the 'session_start' event triggers a showing of message that needs to be fully loaded but it timeouts, then message will not be shown. But it will be ready for next `session_start` event so it will be shown on next 'application run'.

#### On Android

The SDK hooks into the application lifecycle, and every time an activity is resumed, it will remember it and use it for presenting an In-app message. Messages are displayed in a new Activity that is started for them (except for the slide-in message that is directly injected into the currently running Activity).

#### On iOS

Once a message passes the filters, the SDK will try to present the message in the top-most `presentedViewController` (except for the slide-in message that uses `UIWindow` directly).

### In-app messages loading
In-app messages reloading is triggered by any case of:
- when `BloomreachSDK.IdentifyCustomer` is called
- when `BloomreachSDK.Anonymize` is called
- when any event is tracked (except Push clicked, opened or session ends) and In-app messages cache is older then 30 minutes from last load
  Any In-app message images are preloaded too so message is able to be shown after whole process is finished. Please considers it while testing of In-app messages feature.
  It is common behaviour that if you change an In-app message data on platform then this change is reflected in SDK after 30 minutes due to usage of messages cache. Do call `BloomreachSDK.IdentifyCustomer` or `BloomreachSDK.Anonymize` if you want to reflect changes immediately.

### Custom In-app message actions
If you want to override default SDK behavior, when In-app message action is performed (button is clicked, a message is closed), or you want to add your code to be performed along with code executed by the SDK, you can set up InAppMessageDelegate by calling `SetInAppMessageActionCallback` on Bloomreach instance.

```csharp
Bloomreach.BloomreachSDK.SetInAppMessageActionCallback(
    false,  //If overrideDefaultBehavior is set to true, default In-app action will not be performed ( e.g. deep link )
    false,  // If trackActions is set to false, click and close In-app events will not be tracked automatically
    (message, text, url, interaction) =>
    {
        // Here goes the code you want to be executed on In-app message action
        // On In-app click, the button contains button text and button URL, and the interaction is true
        // On In-app close by user interaction, the button is null and the interaction is true
        // On In-app close by non-user interaction (i.e. timeout), the button is null and the interaction is false
        if (url == null)
        {
            Bloomreach.BloomreachSDK.TrackInAppMessageClose(message, interaction);
        }
        else
        {
            Bloomreach.BloomreachSDK.TrackInAppMessageClick(message, text, url);
        }
    }
);
```
If you set `trackActions` to **false** but you still want to track click/close event under some circumstances, you can call Bloomreach methods `TrackInAppMessageClick` or `TrackInAppMessageClose`.

```csharp
Bloomreach.BloomreachSDK.SetInAppMessageActionCallback(false, false, (message, text, url, interaction) =>
    {
        if (<your-special-condition>)
        {
            if (interaction && url != null)
            {
                Bloomreach.BloomreachSDK.TrackInAppMessageClick(message, text, url);
            }
            else
            {
                Bloomreach.BloomreachSDK.TrackInAppMessageClose(message, interaction);
            }
        }
    }
);
```

Method `TrackInAppMessageClose` will track a 'close' event with 'interaction' field of TRUE value by default. You are able to use a optional parameter 'interaction' of this method to override this value.

> The behaviour of `TrackInAppMessageClick` and `TrackInAppMessageClose` may be affected by the tracking consent feature, which in enabled mode considers the requirement of explicit consent for tracking. Read more in [tracking consent documentation](./TRACKING_CONSENT.md).
