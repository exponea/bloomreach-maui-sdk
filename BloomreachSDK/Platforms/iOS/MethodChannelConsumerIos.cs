

using Foundation;
using UIKit;
using UserNotifications;

namespace Bloomreach.Platforms.iOS
{
    internal class MethodChannelConsumerIos : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly BloomreachSdkNativeiOS.BloomreachSdkIOS NativeSdk = BloomreachSdkNativeiOS.BloomreachSdkIOS.Instance;

        MethodMauiResult IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, string? data)
        {
            var nativeResult = NativeSdk.InvokeMethodWithMethod(method, data);
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }

        void IMethodChannelConsumerPlatformSpecific.InvokeMethodAsync(string method, string? data, Action<MethodMauiResult, Exception?> action)
        {
            try
            {
                if (method == "RequestAuthorization")
                {
                    Console.WriteLine("APNS-BR PushRequest requested");
                }
                NativeSdk.InvokeMethodAsyncWithMethod(method, data, delegate (BloomreachSdkNativeiOS.MethodResult nativeResult)
                {
                    try
                    {
                        if (method == "RequestAuthorization")
                        {
                            Console.WriteLine("APNS-BR PushRequest responded " + nativeResult.Success);
                        }
                        var mauiResult = new MethodMauiResult(
                            nativeResult.Success,
                            nativeResult.Data,
                            nativeResult.Error
                        );
                        action.Invoke(mauiResult, null);
                    }
                    catch (Exception e)
                    {
                        action.Invoke(
                            new MethodMauiResult(false, "", $"Native {method} failed, see logs"),
                            e
                        );
                    }
                });
            }
            catch (Exception e)
            {
                action.Invoke(
                    new MethodMauiResult(false, "", $"Native {method} failed, see logs"),
                    e
                );
            }
        }

        MethodMauiResultForView IMethodChannelConsumerPlatformSpecific.InvokeUiMethod(string method, string? data)
        {
            var nativeResult = NativeSdk.InvokeMethodForUIWithMethod(method, data);
            var mauiResult = new MethodMauiResultForView(
                nativeResult.Success,
                null,
                // nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }

        internal MethodMauiResult HandleRemoteMessage(
            string appGroup,
            UNNotificationRequest notificationRequest,
            Action<UNNotificationContent> handler
        )
        {
            Console.WriteLine("APNS-BR Native Handle message consumer starts");
            var nativeResult = NativeSdk.HandleRemoteMessageWithAppGroup(
                appGroup,
                notificationRequest,
                handler
            );
            Console.WriteLine("APNS-BR Native handle message consumer done");
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            Console.WriteLine("APNS-BR Native handle message consumer mapped");
            return mauiResult;
        }

        internal MethodMauiResult HandleNotificationReceived(
            UNNotification notification,
            NSExtensionContext? context,
            UIViewController viewController
        )
        {
            Console.WriteLine("APNS-BR Native HandleNotificationReceived starts");
            var nativeResult = NativeSdk.HandleRemoteMessageContentWithNotification(
                notification,
                context,
                viewController
            );
            Console.WriteLine("APNS-BR Native HandleNotificationReceived done");
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            Console.WriteLine("APNS-BR Native HandleNotificationReceived mapped");
            return mauiResult;
        }
    }
}

