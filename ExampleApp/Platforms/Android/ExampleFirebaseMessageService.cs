using Android.App;
using Bloomreach;
using Firebase.Messaging;

namespace ExampleApp;

[Service(Name = "ExampleApp.ExampleFirebaseMessageService", Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
public class ExampleFirebaseMessageService : FirebaseMessagingService
{
    public override void OnMessageReceived(RemoteMessage message)
    {
        Bloomreach.BloomreachSDK.SetOpenedPushCallback(action =>
        {
            switch (action.ActionType)
            {
                case "app":
                    Console.WriteLine(action.Attributes);
                    break;
                case "deeplink":
                    Console.WriteLine(action.Url);
                    break;
                case "web":
                    Console.WriteLine(action.Url);
                    break;
            }
        });
        Bloomreach.BloomreachSDK.SetReceivedPushCallback(payload =>
        {
            foreach (var entry in payload.RawData)
            {
                Console.WriteLine($"Push Extra: {entry.Key} : {entry.Value} ");
            }
        });
        if (!Bloomreach.BloomreachSDK.HandleRemoteMessage(NotificationPayload.Parse(message.Data)))
        {
            Console.WriteLine("Remote Message received without Bloomreach content");
        }
    }

    public override void OnNewToken(string token)
    {
        Bloomreach.BloomreachSDK.HandlePushToken(token);
    }
}