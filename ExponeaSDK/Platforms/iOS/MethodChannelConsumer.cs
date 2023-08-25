
using System.Text.Json;
using ExponeaSdkNativeiOS;
using UIKit;

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

        void IMethodChannelConsumerPlatformSpecific.InvokeMethodAsync(string method, object? data, Action<string> action)
        {
            var methodParams = JsonSerializer.Serialize(data);
            NativeSdk.InvokeMethodAsyncWithMethod(method, methodParams, delegate (MethodResult result)
            {
                action.Invoke(result.Data + "|" + result.Error);
            });
        }

        View IMethodChannelConsumerPlatformSpecific.InvokeUIMethod(string method, object? data)
        {
            var methodParams = JsonSerializer.Serialize(data);
            var result = NativeSdk.InvokeMethodForUIWithMethod(method, methodParams);
            ContentView wrapper = new ContentView();
            StackLayout stackLayout = new StackLayout();
            if (result.Data != null)
            {
                //stackLayout.Children.Add((UIView)result.Data);
            }
            return wrapper;
        }
    }
}

