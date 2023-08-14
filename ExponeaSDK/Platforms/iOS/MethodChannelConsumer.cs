
namespace ExponeaSDK
{
	internal partial class MethodChannelConsumerIOS : IMethodChannelConsumerPlatformSpecific
	{
        string IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, object? data)
        {
            return "iOS with " + method;
        }
    }
}

