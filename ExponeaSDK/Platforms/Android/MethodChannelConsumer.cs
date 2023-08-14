﻿
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;

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
            return result.Data + "|" + result.Error;
        }
    }
}

