using ObjCRuntime;
using UserNotifications;
using Bloomreach;
using Foundation;

namespace ExamplePushServiceExtension;

[Register("ExampleNotificationService")]
public class ExampleNotificationService : UNNotificationServiceExtension
{

    protected internal ExampleNotificationService(NativeHandle handle) : base(handle)
    {
        // Note: this .ctor should not contain any initialization logic.
    }
    
    public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
    {
        if (!Bloomreach.BloomreachSDK.HandleRemoteMessage("group.com.exponea.ExponeaSDK-Example2", request, contentHandler))
        {
            Console.WriteLine("Remote Message received without Bloomreach content");
        }
    }

    public override void TimeWillExpire()
    {
        Bloomreach.BloomreachSDK.HandleRemoteMessageTimeWillExpire();
    }
}