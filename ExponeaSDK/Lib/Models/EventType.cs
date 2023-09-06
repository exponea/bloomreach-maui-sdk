using System.Runtime.Serialization;

namespace Exponea
{
    public enum EventType
    {
        // Install event is fired only once when the app is first installed.
        [EnumMember(Value = "install")] 
        Install,

        // Session start event used to mark the start of a session, typically when an app comes to foreground.
        [EnumMember(Value = "session_start")] 
        SessionStart,

        // Session end event used to mark the end of a session, typically when an app goes to background.
        [EnumMember(Value = "session_end")] 
        SessionEnd,

        // Custom event tracking, used to report any custom events that you want.
        [EnumMember(Value = "track_event")] 
        TrackEvent,

        // Tracking of customers is used to identify a current customer by some identifier.
        [EnumMember(Value = "track_customer")] 
        TrackCustomer,

        // Virtual and hard payments can be tracked to better measure conversions for example.
        [EnumMember(Value = "payment")] 
        Payment,

        // Event used for registering the push notifications token of the device with Exponea.
        [EnumMember(Value = "push_token")] 
        PushToken,

        // For tracking that push notification has been delivered
        [EnumMember(Value = "push_delivered")] 
        PushDelivered,

        // For tracking that a push notification has been opened.
        [EnumMember(Value = "push_opened")] 
        PushOpened,

        // For tracking that a campaign button has been clicked.
        [EnumMember(Value = "campaign_click")] 
        CampaignClick,

        // For tracking in-app message related events.
        [EnumMember(Value = "banner")] 
        Banner
    }
}