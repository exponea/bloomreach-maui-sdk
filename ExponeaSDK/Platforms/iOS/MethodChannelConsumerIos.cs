

namespace Exponea.Platforms.iOS
{
    internal class MethodChannelConsumerIos : IMethodChannelConsumerPlatformSpecific
    {
        private static readonly ExponeaSdkNativeiOS.ExponeaSDK NativeSdk = ExponeaSdkNativeiOS.ExponeaSDK.Instance;

        MethodMauiResult IMethodChannelConsumerPlatformSpecific.InvokeMethod(string method, string? data)
        {
            var nativeResult = NativeSdk.InvokeMethodWithMethod(method, data);
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
                NativeSdk.InvokeMethodAsyncWithMethod(method, data, delegate (ExponeaSdkNativeiOS.MethodResult nativeResult)
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
                });
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
            var nativeResult = NativeSdk.InvokeMethodForUIWithMethod(method, data);
            var mauiResult = new MethodMauiResultForView(
                nativeResult.Success,
                null,
                // nativeResult.Data,
                nativeResult.Error
            );
            return mauiResult;
        }
    }
}

