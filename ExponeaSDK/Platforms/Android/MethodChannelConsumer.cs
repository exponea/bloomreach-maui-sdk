
using System.Text.Json;
using Com.Exponea.Sdk.Maui.Android;
using Kotlin.Jvm.Functions;
using Object = Java.Lang.Object;

namespace ExponeaSDK.Platforms.Android
{
	internal class MethodChannelConsumerAndroid : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly Com.Exponea.Sdk.Maui.Android.ExponeaSDK NativeSdk = new(
            Platform.AppContext
        );

        string IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, object? data)
        {
            var methodParams = JsonSerializer.Serialize(data);
            var result = NativeSdk.InvokeMethod(method, methodParams);
            return result.Data;
        }

        void IMethodChannelConsumerPlatformSpecific.InvokeMethodAsync(string method, object? data, Action<string> action)
        {
            var methodParams = JsonSerializer.Serialize(data);
            NativeSdk.InvokeMethodAsync(method, methodParams, new KotlinCallback<MethodResult>((result) =>
            {
                action.Invoke(result.Data);
            }));
        }

        View IMethodChannelConsumerPlatformSpecific.InvokeUIMethod(string method, object? data)
        {
            throw new NotImplementedException();
        }
    }
    
    internal class KotlinCallback<TArg> : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
        where TArg: Java.Lang.Object?
    {
        private readonly Action<TArg> _callback;

        public KotlinCallback(Action<TArg> callback)
        {
            _callback = callback;
        }

        public Object? Invoke(Object? p0)
        {
            if (p0 is TArg result)
            {
                _callback.Invoke(result);
            }
            return null;
        }
    }
}

