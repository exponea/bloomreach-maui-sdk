## App Inbox

Bloomreach SDK feature App Inbox allows you to use message list in your app. You can find information on creating your messages in [Bloomreach documentation](https://documentation.bloomreach.com/engagement/docs/app-inbox).

### Using App Inbox

Only required step to use App Inbox in your application is to add a button into your screen. Messages are then displayed by clicking on a button:

```xaml
<!-- Somewhere in your layout -->
<StackLayout x:Name="AppInboxButtonHere"/>
```

```csharp
var button = Bloomreach.BloomreachSDK.GetAppInboxButton();
AppInboxButtonHere.Children.Add(button);
```

App Inbox button has registered a click action to show an screen with App Inbox list.

> Always check for retrieved button instance nullability. Button cannot be build for non-initialized Bloomreach SDK.

No more work is required for showing App Inbox but may be customized in multiple ways.

## Default App Inbox behavior

Bloomreach SDK is fetching and showing an App Inbox for you automatically in default steps:

1. Shows a button to access App Inbox list (need to be done by developer)
2. Shows a screen for App Inbox list. Each item is shown with:
    1. Flag if message is read or unread
    2. Delivery time in human-readable form (i.e. `2 hours ago`)
    3. Single-lined title of message ended by '...' for longer value
    4. Two-lined content of message ended by '...' for longer value
    5. Squared image if message contains any
    6. Shows a loading state of list (indeterminate progress)
    7. Shows an empty state of list with title and message
    8. Shows an error state of list with title and description
3. Screen for App Inbox list calls a `Bloomreach.BloomreachSDK.TrackAppInboxOpened` on item click and marks message as read automatically
4. Shows a screen for App Inbox message detail that contains:
    1. Large squared image. A gray placeholder is shown if message has no image
    2. Delivery time in human-readable form (i.e. `2 hours ago`)
    3. Full title of message
    4. Full content of message
    5. Buttons for each reasonable action (actions to open browser link or invoking of universal link). Action that just opens current app is meaningless so is not listed
5. Screen for message detail calls `Bloomreach.BloomreachSDK.TrackAppInboxClick` on action button click automatically

> The behavior of `TrackAppInboxOpened` and `TrackAppInboxClick` may be affected by the tracking consent feature, which in enabled mode considers the requirement of explicit consent for tracking. Read more in [tracking consent documentation](./TRACKING_CONSENT.md).

### Localization

Bloomreach SDK contains only texts in EN translation. To modify this or add a localization, you are able to define customized strings in your `strings.xml` files for Android:

```xml
<string name="Bloomreach_inbox_button">Inbox</string>
<string name="Bloomreach_inbox_title">Inbox</string>
<string name="Bloomreach_inbox_defaultTitle">Inbox message</string>
<string name="Bloomreach_inbox_emptyTitle">Empty Inbox</string>
<string name="Bloomreach_inbox_emptyMessage">You have no messages yet.</string>
<string name="Bloomreach_inbox_errorTitle">Something went wrong :(</string>
<string name="Bloomreach_inbox_errorMessage">We could not retrieve your messages.</string>
<string name="Bloomreach_inbox_mainActionTitle">See more</string>
```

or in your `Localizable.string` files for iOS:

```text
"Bloomreach.inbox.button" = "Inbox";
"Bloomreach.inbox.title" = "AppInbox";
"Bloomreach.inbox.emptyTitle" = "Empty Inbox";
"Bloomreach.inbox.emptyMessage" = "You have no messages yet.";
"Bloomreach.inbox.errorTitle" = "Something went wrong :(";
"Bloomreach.inbox.errorMessage" = "We could not retrieve your messages.";
"Bloomreach.inbox.defaultTitle" = "Message";
"Bloomreach.inbox.mainActionTitle" = "See more";
```

### UI components styling

App Inbox screens are designed with love and to fulfill customers needs but may not fit design of your application. You are able to customize multiple colors and text appearances with simple configuration.

```csharp
Bloomreach.BloomreachSDK.SetAppInboxProvider(new AppInboxStyle()
  {
      AppInboxButton = new ButtonStyle()
      {
          TextOverride = "text value",
          TextColor = "color",
          BackgroundColor = "color",
          ShowIcon = true,
          TextSize = "12px",
          Enabled = true,
          BorderRadius = "5px",
          TextWeight = "bold|normal|100..900",
      },
      DetailView = new DetailViewStyle()
      {
          Title = new TextViewStyle()
          {
              Visible = true,
              TextColor = "color",
              TextSize = "12px",
              TextWeight = "bold|normal|100..900",
              TextOverride = "text"
          },
          Content = new TextViewStyle() { ... },
          ReceivedTime = new TextViewStyle() { ... },
          Image = new ImageViewStyle()
          {
              Visible = true,
              BackgroundColor = "color"
          },
          Button = new ButtonStyle() { ... }
      },
      ListView = new ListScreenStyle()
      {
          EmptyTitle = new TextViewStyle() { ... },
          EmptyMessage = new TextViewStyle() { ... },
          ErrorTitle = new TextViewStyle() { ... },
          ErrorMessage = new TextViewStyle() { ... },
          Progress = new ProgressBarStyle()
          {
              Visible = true,
              ProgressColor = "color",
              BackgroundColor = "color"
          },
          List = new AppInboxListViewStyle()
          {
              BackgroundColor = "color",
              Item = new AppInboxListItemStyle()
              {
                  BackgroundColor = "color",
                  ReadFlag = new ImageViewStyle() { ... },
                  ReceivedTime = new TextViewStyle() { ... },
                  Title = new TextViewStyle() { ... },
                  Content = new TextViewStyle() { ... },
                  Image = new ImageViewStyle() { ... },
              }
          }
      }
  });
```

Supported colors formats are:
* Short hex `#rgb` or with alpha `#rgba`
* Hex format `#rrggbb` or `#rrggbbaa`
* RGB format `rgb(255, 255, 255)`
* RGBA format `rgba(255, 255, 255, 1.0)` or `rgba(255 255 255 / 1.0)`
* ARGB format `argb(1.0, 255, 255, 255)`
* name format `yellow` (names has to be supported by Android/iOS platform)

Supported size formats are:
* Pixels `12px` or `12`
* Scaleable Pixels `12sp`
* Density-independent Pixels `12dp` or `12dip`
* Points `12pt`
* Inches `12in`
* Millimeters `12mm`

Supported text weight formats are:
* 'normal' - normal/regular style on both platforms
* 'bold' - bold style on both platforms
* Number from `100` to `900` - mainly usable on iOS platform. Can be used also on Android but with limitation (100-600 means 'normal'; 700-900 means 'bold')

> You may register your own provider at any time - before Bloomreach SDK init or later in some of your screens. Every action in scope of App Inbox is using currently registered provider instance. Nevertheless, we recommend to set your provider right after Bloomreach SDK initialization.

## App Inbox data API

Bloomreach SDK provides methods to access App Inbox data directly without accessing UI layer at all. This allows you to create your UI completely and only data would be fetched from SDK.

### App Inbox load

App Inbox is assigned to existing customer account (defined by hardIds) so App Inbox is cleared in case of:

- calling any `BloomreachSDK.IdentifyCustomer` method
- calling any `BloomreachSDK.Anonymize` method

To prevent a large data transferring on each fetch, App Inbox are stored locally and next loading is incremental. It means that first fetch contains whole App Inbox but next requests contain only new messages. You are freed by handling such a behavior, result data contains whole App Inbox but HTTP request in your logs may be empty for that call.
List of assigned App Inbox is done by

```csharp
var res = await Bloomreach.BloomreachSDK.FetchAppInbox();
```

Bloomreach SDK provides API to get single message from App Inbox. To load it you need to pass a message ID:

```csharp
var res = await Bloomreach.BloomreachSDK.FetchAppInboxItem(messageId);
```
Fetching of single message is still requesting for fetch of all messages (including incremental loading). But message data are returned from local repository in normal case (due to previous fetch of messages).

### App Inbox message read state

To set an App Inbox message read flag you need to pass a message:
```csharp
bool markedAsRead = await Bloomreach.BloomreachSDK.MarkAppInboxAsRead(message);
```
> Marking a message as read by `MarkAppInboxAsRead` method is not invoking a tracking event for opening a message. To track an opened message, you need to call `BloomreachSDK.TrackAppInboxOpened` method.

## Tracking events for App Inbox

Bloomreach SDK default behavior is tracking the events for you automatically. In case of your custom implementation, please use tracking methods in right places.

### Tracking opened App Inbox message

To track an opening of message detail, you should use method `BloomreachSDK.TrackAppInboxOpened` with opened message data.
The behaviour of `BloomreachSDK.TrackAppInboxOpened` may be affected by the tracking consent feature, which in enabled mode considers the requirement of explicit consent for tracking. Read more in [tracking consent documentation](./TRACKING_CONSENT.md).
If you want to avoid to consider tracking, you may use `BloomreachSDK.TrackAppInboxOpenedWithoutTrackingConsent` instead. This method will do track event ignoring tracking consent state.

### Tracking clicked App Inbox message action

To track an invoking of action, you should use method `BloomreachSDK.TrackAppInboxClick` with clicked message action and data.
The behaviour of `BloomreachSDK.TrackAppInboxClick` may be affected by the tracking consent feature, which in enabled mode considers the requirement of explicit consent for tracking. Read more in [tracking consent documentation](./TRACKING_CONSENT.md).
If you want to avoid to consider tracking, you may use `BloomreachSDK.TrackAppInboxClickWithoutTrackingConsent` instead. This method will do track event ignoring tracking consent state.
