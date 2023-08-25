
namespace ExponeaSDK
{
    internal class MethodChannelConsumer
    {
        internal string InvokeMethod(string method, object? data)
        {
            IMethodChannelConsumerPlatformSpecific? channelInternal = null;
#if ANDROID
            channelInternal = new Platforms.Android.MethodChannelConsumerAndroid();
#elif IOS
            channelInternal = new Platforms.iOS.MethodChannelConsumerIos();
#endif
            return channelInternal?.InvokeMethod(method, data) ?? "NO_PLATFORM";
        }
    }
    internal interface IMethodChannelConsumerPlatformSpecific
    {
        internal string InvokeMethod(string method, object? data);
        internal void InvokeMethodAsync(string method, object? data, Action<string> action);
        internal View InvokeUIMethod(string method, object? data);
    }
}