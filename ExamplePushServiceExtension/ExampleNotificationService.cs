using Foundation;
using ObjCRuntime;
using UserNotifications;
using Bloomreach;

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
        Console.WriteLine("APNS-BR Push notification received");
        if (!Bloomreach.BloomreachSDK.HandleRemoteMessage("group.com.exponea.ExponeaSDK-Example2", request, contentHandler))
        {
            Console.WriteLine("Remote Message received without Bloomreach content");
        }
    }

    public override void TimeWillExpire()
    {
        Console.WriteLine("APNS-BR Push notification time will expire");
        Bloomreach.BloomreachSDK.HandleRemoteMessageTimeWillExpire();
    }
}