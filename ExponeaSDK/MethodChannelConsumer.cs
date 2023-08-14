
namespace ExponeaSDK
{
    internal class MethodChannelConsumer
    {
        internal string InvokeMethod(string method, object? data)
        {
            IMethodChannelConsumerPlatformSpecific channelInternal;
#if ANDROID
            channelInternal = new Platforms.Android.MethodChannelConsumerAndroid();
#else
            channelInternal = null;
#endif
            return channelInternal?.InvokeMethod(method, data);
        }
    }
    internal interface IMethodChannelConsumerPlatformSpecific
    {
        internal string InvokeMethod(string method, object? data);
    }
}