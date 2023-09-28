using Android.App;
using Bloomreach;
using Huawei.Hms.Push;

namespace ExampleApp;

[Service(Name = "ExampleApp.ExampleHuaweiMessageService", Exported = false)]
[IntentFilter(new[] { "com.huawei.push.action.MESSAGING_EVENT" })]
public class ExampleHuaweiMessageService : HmsMessageService
{
    public override void OnMessageReceived(RemoteMessage message)
    {
        Bloomreach.BloomreachSDK.SetReceivedPushCallback(payload =>
        {
            foreach (var entry in payload.RawData)
            {
                Console.WriteLine($"Push Extra: {entry.Key} : {entry.Value} ");
            }
        });
        if (!Bloomreach.BloomreachSDK.HandleRemoteMessage(NotificationPayload.Parse(message.DataOfMap)))
        {
            Console.WriteLine("Remote Message received without Bloomreach content");
        }
    }

    public override void OnNewToken(string token)
    {
        Bloomreach.BloomreachSDK.HandleHmsPushToken(token);
    }
}