
using System.Text.Json;

namespace ExponeaSDK.Platforms.iOS
{
    internal class MethodChannelConsumerIos : IMethodChannelConsumerPlatformSpecific
    {
        string IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, object? data)
        {
            var methodParams = JsonSerializer.Serialize(data);
            return "result.Data + \"|\" + result.Error;";
        }
    }
}

