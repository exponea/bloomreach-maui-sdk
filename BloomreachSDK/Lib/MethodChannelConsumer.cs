#if IOS
using Bloomreach.Platforms.iOS;
using Foundation;
using UIKit;
using UserNotifications;
#endif

namespace Bloomreach
{
    public class MethodMauiResultForView
    {
        public MethodMauiResultForView(bool success, object? data, string error)
        {
            this.Success = success;
            this.Data = data;
            this.Error = error;
        }

        public string Error { get; set; }

        public object? Data { get; set; }

        public bool Success { get; set; }
    }

    public class MethodMauiResult
    {
        public MethodMauiResult(bool success, string? data, string error)
        {
            this.Success = success;
            this.Data = data;
            this.Error = error;
        }

        public string Error { get; set; }

        public string? Data { get; set; }

        public bool Success { get; set; }
    }

    public class MethodChannelConsumer
    {

        private readonly IMethodChannelConsumerPlatformSpecific? _channelInternal = null;

        public MethodChannelConsumer() : this(
#if ANDROID
            new Platforms.Android.MethodChannelConsumerAndroid()
#elif IOS
            new Platforms.iOS.MethodChannelConsumerIos()
#else
            null
#endif
            )
        { }

        public MethodChannelConsumer(IMethodChannelConsumerPlatformSpecific? platformChannel)
        {
            if (platformChannel == null)
            {
                BloomreachSDK.ThrowOrLog(new Exception("No platform channel consumer has been created"));
            }
            _channelInternal = platformChannel;
        }

        internal virtual string? InvokeMethod(string method, string? data)
        {
            try
            {
                var result = _channelInternal?.InvokeMethod(method, data);
                if (result?.Success == false)
                {
                    BloomreachSDK.ThrowOrLog(new Exception($"Method {method} return failure status, see logs"));
                }
                return result?.Data;
            }
            catch (Exception e)
            {
                BloomreachSDK.ThrowOrLog(e);
                return null;
            }
        }

        internal virtual void InvokeMethodAsync(string method, string? data, Action<string?, Exception?> action)
        {
            _channelInternal?.InvokeMethodAsync(method, data, (result, exception) =>
            {
                try
                {
                    action.Invoke(result.Data, exception);
                }
                catch (Exception e)
                {
                    action.Invoke(null, e);
                }
            });
        }

        internal virtual object? InvokeUiMethod(string method, string? data)
        {
            try
            {
                var result = _channelInternal?.InvokeUiMethod(method, data);
                if (result?.Success == false)
                {
                    BloomreachSDK.ThrowOrLog(new Exception($"Method {method} return failure status, see logs"));
                }
                return result?.Data;
            }
            catch (Exception e)
            {
                BloomreachSDK.ThrowOrLog(e);
                return null;
            }
        }

        internal virtual string? HandleRemoteMessage(params object[] args)
        {
#if ANDROID
            return InvokeMethod("HandleRemoteMessage", (string?)args[0]);
#elif IOS
            try
            {
                var result = ((MethodChannelConsumerIos)_channelInternal!).HandleRemoteMessage(
                    (string)args[0],
                    (UNNotificationRequest)args[1],
                    (Action<UNNotificationContent>)args[2]
                );
                if (result?.Success == false)
                {
                    BloomreachSDK.ThrowOrLog(
                        new Exception($"Method HandleRemoteMessage return failure status, see logs: {result?.Error}")
                    );
                }
                return result?.Data;
            }
            catch (Exception e)
            {
                BloomreachSDK.ThrowOrLog(e);
                return null;
            }
#else
            return null;
#endif
        }

        public void HandleNotificationReceived(params object[] args)
        {
#if ANDROID
            BloomreachSDK.ThrowOrLog(new Exception($"Method HandleNotificationReceived is not supported on Android"));
#elif IOS
            try
            {
                var result = ((MethodChannelConsumerIos)_channelInternal!).HandleNotificationReceived(
                    (UNNotification)args[0],
                    (NSExtensionContext?)args[1],
                    (UIViewController)args[2]
                );
                if (result?.Success == false)
                {
                    BloomreachSDK.ThrowOrLog(
                        new Exception($"Method HandleNotificationReceived return failure status, see logs: {result?.Error}")
                    );
                }
            }
            catch (Exception e)
            {
                BloomreachSDK.ThrowOrLog(e);
            }
#else
            BloomreachSDK.ThrowOrLog(new Exception($"Method HandleNotificationReceived is working only on iOS platform"));
#endif
        }
    }

    public interface IMethodChannelConsumerPlatformSpecific
    {
        MethodMauiResult InvokeMethod(string method, string? data);
        void InvokeMethodAsync(string method, string? data, Action<MethodMauiResult, Exception?> action);
        MethodMauiResultForView InvokeUiMethod(string method, string? data);
    }
}