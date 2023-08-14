namespace ExponeaSDK
{
    public enum EventType
    {
        // Install event is fired only once when the app is first installed.
        Install,

        // Session start event used to mark the start of a session, typically when an app comes to foreground.
        SessionStart,

        // Session end event used to mark the end of a session, typically when an app goes to background.
        SessionEnd,

        // Custom event tracking, used to report any custom events that you want.
        TrackEvent,

        // Tracking of customers is used to identify a current customer by some identifier.
        TrackCustomer,

        // Virtual and hard payments can be tracked to better measure conversions for example.
        Payment,

        // Event used for registering the push notifications token of the device with Exponea.
        PushToken,

        // For tracking that push notification has been delivered
        PushDelivered,

        // For tracking that a push notification has been opened.
        PushOpened,

        // For tracking that a campaign button has been clicked.
        CampaignClick,

        // For tracking in-app message related events.
        Banner
    }

    internal enum EventTypeInternal
    {
        INSTALL,

        SESSION_START,

        SESSION_END,

        TRACK_EVENT,

        TRACK_CUSTOMER,

        PAYMENT,

        PUSH_TOKEN,

        PUSH_DELIVERED,

        PUSH_OPENED,

        CAMPAIGN_CLICK,

        BANNER
    }
}