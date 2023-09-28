

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
                NativeSdk.InvokeMethodAsyncWithMethod(method, data, delegate (BloomreachSdkNativeiOS.MethodResult nativeResult)
                {
                    try
                    {
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
            var nativeResult = NativeSdk.HandleRemoteMessageWithAppGroup(
                appGroup,
                notificationRequest,
                handler
            );
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }

        internal MethodMauiResult HandleNotificationReceived(
            UNNotification notification,
            NSExtensionContext? context,
            UIViewController viewController
        )
        {
            var nativeResult = NativeSdk.HandleRemoteMessageContentWithNotification(
                notification,
                context,
                viewController
            );
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }
    }
}

