using ObjCRuntime;
using UserNotifications;
using UserNotificationsUI;

namespace ExamplePushContentExtension;

[Register("NotificationViewController")]
public class NotificationViewController : UIViewController, IUNNotificationContentExtension
{

    protected internal NotificationViewController(NativeHandle handle) : base(handle)
    {
        // Note: this .ctor should not contain any initialization logic.
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        // Do any required interface initialization here.
    }

    public void DidReceiveNotification(UNNotification notification)
    {
        Console.WriteLine("APNS-BR Push notification received");
        Bloomreach.BloomreachSDK.HandleNotificationReceived(notification, ExtensionContext, this);
    }
    
}