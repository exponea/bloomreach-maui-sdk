﻿
namespace Exponea
{
    public class MethodMauiResultForView
    {
        public MethodMauiResultForView(bool success, IView? data, string error)
        {
            this.Success = success;
            this.Data = data;
            this.Error = error;
        }

        public string Error { get; set; }

        public IView? Data { get; set; }

        public bool Success { get; set; }
    }

    public class MethodMauiResult
    {
        public MethodMauiResult(bool success, string? data, string error)
        {
            this.Success = success;
            this.Data = data;
            this.Error = error;
        }

        public string Error { get; set; }

        public string? Data { get; set; }

        public bool Success { get; set; }
    }
    
    internal class MethodChannelConsumer
    {

        private readonly IMethodChannelConsumerPlatformSpecific? _channelInternal = null;

        public MethodChannelConsumer() : this(
#if ANDROID
            new Platforms.Android.MethodChannelConsumerAndroid()
#elif IOS
            new Platforms.iOS.MethodChannelConsumerIos()
#else
            null
#endif
            )
        { }

        public MethodChannelConsumer(IMethodChannelConsumerPlatformSpecific? platformChannel)
        {
            if (platformChannel == null)
            {
                ExponeaSDK.ThrowOrLog(new Exception("No platform channel consumer has been created"));
            }
            _channelInternal = platformChannel;
        }

        internal virtual MethodMauiResult? InvokeMethodWithMethod(string method, string? data)
        {
            try
            {
                var result = _channelInternal?.InvokeMethod(method, data);
                return result;
            }
            catch (Exception e)
            {
                ExponeaSDK.ThrowOrLog(e);
                return null;
            }
        }

        internal virtual string? InvokeMethod(string method, string? data)
        {
            try
            {
                var result = _channelInternal?.InvokeMethod(method, data);
                if (result?.Success == false)
                {
                    ExponeaSDK.ThrowOrLog(new Exception($"Method {method} return failure status, see logs"));
                }
                return result?.Data;
            }
            catch (Exception e)
            {
                ExponeaSDK.ThrowOrLog(e);
                return null;
            }
        }

        internal virtual void InvokeMethodAsync(string method, string? data, Action<string?, Exception?> action)
        {
            _channelInternal?.InvokeMethodAsync(method, data, (result, exception) =>
            {
                try
                {
                    action.Invoke(result.Data, exception);
                }
                catch (Exception e)
                {
                    action.Invoke(null, e);
                }
            });
        }

        internal virtual IView? InvokeUiMethod(string method, string? data)
        {
            try
            {
                var result = _channelInternal?.InvokeUiMethod(method, data);
                if (result?.Success == false)
                {
                    ExponeaSDK.ThrowOrLog(new Exception($"Method {method} return failure status, see logs"));
                }
                return result?.Data;
            }
            catch (Exception e)
            {
                ExponeaSDK.ThrowOrLog(e);
                return null;
            }
        }
    }
    internal interface IMethodChannelConsumerPlatformSpecific
    {
        internal MethodMauiResult InvokeMethod(string method, string? data);
        internal void InvokeMethodAsync(string method, string? data, Action<MethodMauiResult, Exception?> action);
        internal MethodMauiResultForView InvokeUiMethod(string method, string? data);
    }
}