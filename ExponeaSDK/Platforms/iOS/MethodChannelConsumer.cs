
using System.Text.Json;
using ExponeaSdkNativeiOS;

namespace ExponeaSDK.Platforms.iOS
{
    internal class MethodChannelConsumerIos : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly ExponeaSdkNativeiOS.ExponeaSDK NativeSdk = ExponeaSdkNativeiOS.ExponeaSDK.Instance;

        string IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, object? data)
        {
            var methodParams = JsonSerializer.Serialize(data);
            var result = NativeSdk.InvokeMethodWithMethod(method, methodParams);
            return result.Data + "|" + result.Error;
        }
    }
}

