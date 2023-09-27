using Bloomreach;
using Foundation;
using ObjCRuntime;
using UserNotifications;

namespace ExampleApp;

[Register("ExampleNotificationService")]
public class ExampleNotificationService : UNNotificationServiceExtension
{

    protected internal ExampleNotificationService(NativeHandle handle) : base(handle)
    {
        // Note: this .ctor should not contain any initialization logic.
    }
    
    public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
    {
        if (!Bloomreach.BloomreachSDK.HandleRemoteMessage(request, contentHandler))
        {
            Console.WriteLine("Remote Message received without Bloomreach content");
        }
    }

    public override void TimeWillExpire()
    {
        Bloomreach.BloomreachSDK.HandleRemoteMessageTimeWillExpire();
    }
}