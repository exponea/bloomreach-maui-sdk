## 🌋 Project Mapping

Exponea SDK can track specified event types into multiple projects.

The configuration contains **ProjectRouteMap**, a map of event types and projects into which you'd like to track events. A project is identified by its baseUrl, project token, and authorization token.

Events are always tracked into default project and to all projects that you specify in the mapping.

E.g.:

If you want to track push notification opened events to projects `project-a` and `project-b`, you should configure the `ProjectRouteMap` in the configuration object as:

#### Example

``` csharp


var config = new Configuration("default-project", "some-token", "https://api.exponea.com");

config.ProjectRouteMap = new Dictionary<EventType, IList<Project>>() {

        {
            EventType.PushOpened,
                new List<Project>() {
                        new Project(
                            projectToken: "project-a",
                            authorization: "token-a",
                            baseUrl: "https://api.exponea.com"
                        ),
                        new Project(
                            projectToken: "project-b",
                            authorization: "token-b",
                            baseUrl: "https://api.exponea.com"
                        )
                }
        }
};
```

When a push notification is opened, Exponea SDK will track the event three times with the same parameters, just changing the project. That means that you will see the same event in the projects `default-project`, `project-a`, and `project-b`.

Project mapping can be used for these specific event types:

```
 public enum EventType
    {
        // Install event is fired only once when the app is first installed.
        Install,

        // Session start event used to mark the start of a session, typically when an app comes to the foreground.
        SessionStart,

        // Session end event used to mark the end of a session, typically when an app goes to background.
        SessionEnd,

        // Custom event tracking is used to report any custom events that you want.
        TrackEvent,

        // Tracking of customers is used to identify a current customer by some identifier.
        TrackCustomer,

        // Virtual and hard payments can be tracked to better measure conversions, for example.
        Payment,

        // Event used for registering the push notifications token of the device with Exponea.
        PushToken,

        // For tracking that push notification has been delivered
        PushDelivered,

        // For tracking that a push notification has been opened.
        PushOpened,

        // For tracking that a campaign button has been clicked.
        CampaignClick,

        // For tracking in-app message-related events.
        Banner
    }

```
