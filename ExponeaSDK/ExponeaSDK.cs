using System;
namespace ExponeaSDK
{
	public sealed class ExponeaSDK
	{
        private static readonly Lazy<ExponeaSDK> lazy = new(() => new ExponeaSDK());

        public static ExponeaSDK Instance { get { return lazy.Value; } }

        private MethodChannelConsumer channel;

        private ExponeaSDK()
		{
			channel = new MethodChannelConsumer();
		}

        public static void Configure(Configuration config)
        {
            Instance.channel.InvokeMethod("configure", config);
        }

        public static string ConfigureWithResult(Configuration config)
        {
            return Instance.channel.InvokeMethod("configureWithResult", config);
        }
    }
}

