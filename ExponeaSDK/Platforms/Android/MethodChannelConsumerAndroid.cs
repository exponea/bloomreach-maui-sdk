
using System.Text.Json;

namespace Exponea.Platforms.Android
{
	internal class MethodChannelConsumerAndroid : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly Com.Exponea.Sdk.Maui.Android.ExponeaSDK NativeSdk = new(
            Platform.AppContext
        );

        MethodMauiResult IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, string? data)
        {
            var nativeResult = NativeSdk.InvokeMethod(method, data);
            var mauiResult = new MethodMauiResult(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }

        void IMethodChannelConsumerPlatformSpecific.InvokeMethodAsync(string method, string? data, Action<MethodMauiResult, Exception?> action)
        {
            throw new NotImplementedException();
        }

        MethodMauiResultForView IMethodChannelConsumerPlatformSpecific.InvokeUiMethod(string method, string? data)
        {
            throw new NotImplementedException();
        }
    }
}

