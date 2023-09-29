
using Com.Bloomreach.Sdk.Maui.Android;
using Kotlin.Jvm.Functions;
using Object = Java.Lang.Object;

namespace Bloomreach.Platforms.Android
{
	internal class MethodChannelConsumerAndroid : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly BloomreachSdkAndroid NativeSdk = new(
            Platform.CurrentActivity ?? Platform.AppContext
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
            try
            {
                NativeSdk.InvokeMethodAsync(method, data, new KotlinCallback<MethodResult>((nativeResult) =>
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
                }));
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
            var nativeResult = NativeSdk.InvokeMethodForUI(method, data);
            return new MethodMauiResultForView(
                nativeResult.Success,
                nativeResult.Data,
                nativeResult.Error
            );
        }
    }
    
    internal class KotlinCallback<TArg> : Object, IFunction1
        where TArg: Object?
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

