using Exponea.Utils;

namespace Exponea
{
	public class ExponeaSDK
	{
        
        private static bool _safeMode = true;
        
        internal static ExponeaSDK Instance = new ExponeaSDK();

        internal MethodChannelConsumer Channel;

        internal ExponeaSDK()
        {
            Channel = new MethodChannelConsumer();
        }

        public static void Anonymize(Project? project = null, IDictionary<EventType, IList<Project>>? projectMapping = null)
        {
            Instance.Channel.InvokeMethod("Anonymize", ConverterUtils.SerializeInput(new Dictionary<string, object?>()
            {
                { "project", project },
                { "projectMapping", projectMapping }
            }));
        }

        public static void SetCheckPushSetup(bool enable)
        {
            Instance.Channel.InvokeMethod("SetCheckPushSetup", ConverterUtils.SerializeInput(enable));
        }

        public static bool GetCheckPushSetup()
        {
            string? result = Instance.Channel.InvokeMethod("GetCheckPushSetup", null);
            return result?.ToLower()?.Equals("true") ?? false;
        }

        public static void Configure(Configuration config)
        {
            Instance.Channel.InvokeMethod("Configure", ConverterUtils.SerializeInput(config));
        }

        public static string? GetCustomerCookie()
        {
            return Instance.Channel.InvokeMethod("GetCustomerCookie", null);
        }

        public static IDictionary<string, object> GetDefaultProperties()
        {
            string? result = Instance.Channel.InvokeMethod("GetDefaultProperties", null);
            Dictionary<string, object> emptyDic = new();
            if (result == null)
            {
                return emptyDic;
            }
            try
            {
                return ConverterUtils.DeserializeOutput<IDictionary<string, object>>(result) ?? emptyDic;
            }
            catch
            {
                // TODO: log or throw
                return emptyDic;
            }
        }

        public static void SetDefaultProperties(IDictionary<string, object> defaultProperties)
        {
            Instance.Channel.InvokeMethod("SetDefaultProperties", ConverterUtils.SerializeInput(defaultProperties));
        }

        public static Task<bool> FlushData()
        {
            var flush = new TaskCompletionSource<bool>();
            try
            {
                Instance.Channel.InvokeMethodAsync("FlushData", null, (result, exception) =>
                {
                    ApplyResultToTask(flush, result?.ToLower()?.Equals("true") ?? false, exception);
                });
            }
            catch (Exception e)
            {
                ThrowOrLog(e);
                flush.SetResult(false);
            }
            return flush.Task;
        }

        public static FlushMode GetFlushMode()
        {
            string? result = Instance.Channel.InvokeMethod("GetFlushMode", null);
            if (result == null)
            {
                return FlushMode.Unknown;
            }
            switch (result.ToLower()) {
                case "app_close":
                case "automatic":
                    return FlushMode.AppClose;
                case "immediate":
                    return FlushMode.Immediate;
                case "manual":
                    return FlushMode.Manual;
                case "period":
                case "periodic":
                    return FlushMode.Period;
                default:
                    // TODO: log or throw
                    return FlushMode.Unknown;
            }
        }

        public static TimeSpan GetFlushPeriod()
        {
            string? result = Instance.Channel.InvokeMethod("GetFlushPeriod", null);
            long periodMillis = 0;
            if (result != null)
            {
                try
                {
                    periodMillis = Convert.ToInt64(result);
                }
                catch
                {
                    // TODO: log or throw
                }
            }
            return TimeSpan.FromMilliseconds(periodMillis);
        }

        public static void SetFlushPeriod(TimeSpan period)
        {
            Instance.Channel.InvokeMethod("SetFlushPeriod", ConverterUtils.SerializeInput(period.Ticks / TimeSpan.TicksPerMillisecond));
        }

        public static void IdentifyCustomer(Customer customer)
        {
            Instance.Channel.InvokeMethod("IdentifyCustomer", ConverterUtils.SerializeInput(customer));
        }

        public static bool IsAutomaticSessionTracking()
        {
            string? result = Instance.Channel.InvokeMethod("IsAutomaticSessionTracking", null);
            return result?.ToLower()?.Equals("true") ?? false;
        }

        public static void SetAutomaticSessionTracking(bool enabled)
        {
            Instance.Channel.InvokeMethod("SetAutomaticSessionTracking", ConverterUtils.SerializeInput(enabled));
        }

        public static bool IsAutoPushNotification()
        {
            string? result = Instance.Channel.InvokeMethod("IsAutoPushNotification", null);
            return result?.ToLower()?.Equals("true") ?? false;
        }

        public static bool IsConfigured()
        {
            string? result = Instance.Channel.InvokeMethod("IsConfigured", null);
            return result?.ToLower()?.Equals("true") ?? false;
        }

        public static LogLevel GetLogLevel()
        {
            string? result = Instance.Channel.InvokeMethod("GetLogLevel", null);
            if (result == null)
            {
                return LogLevel.Unknown;
            }
            switch (result.ToLower())
            {
                case "none":
                case "off":
                    return LogLevel.Off;
                case "error":
                    return LogLevel.Error;
                case "warning":
                case "warn":
                    return LogLevel.Warn;
                case "info":
                    return LogLevel.Info;
                case "debug":
                    return LogLevel.Debug;
                case "verbose":
                    return LogLevel.Verbose;
                default:
                    // TODO: log or throw
                    return LogLevel.Unknown;
            }
        }

        public static void SetLogLevel(LogLevel level)
        {
            var serializedLogLevel = ConverterUtils.SerializeInput(level);
            var normalizedLogLevel = ConverterUtils.TrimQuotes(serializedLogLevel);
            Instance.Channel.InvokeMethod("SetLogLevel", normalizedLogLevel);
        }

        public static TimeSpan GetSessionTimeout()
        {
            string? result = Instance.Channel.InvokeMethod("GetSessionTimeout", null);
            long timeoutMillis = 0;
            if (result != null)
            {
                try
                {
                    timeoutMillis = Convert.ToInt64(result);
                }
                catch
                {
                    // TODO: log or throw
                }
            }
            return TimeSpan.FromMilliseconds(timeoutMillis);
        }

        public static void SetSessionTimeout(TimeSpan timeout)
        {
            Instance.Channel.InvokeMethod("SetSessionTimeout", ConverterUtils.SerializeInput(timeout.Ticks / TimeSpan.TicksPerMillisecond));
        }

        public static TokenTrackFrequency GetTokenTrackFrequency()
        {
            string? result = Instance.Channel.InvokeMethod("GetTokenTrackFrequency", null);
            if (result == null)
            {
                // TODO: log or throw
                return TokenTrackFrequency.OnTokenChange;
            }
            switch (result.ToLower())
            {
                case "on_token_change":
                case "ontokenchange":
                    return TokenTrackFrequency.OnTokenChange;
                case "every_launch":
                case "everylaunch":
                    return TokenTrackFrequency.EveryLaunch;
                case "daily":
                    return TokenTrackFrequency.Daily;
                default:
                    // TODO: log or throw
                    return TokenTrackFrequency.OnTokenChange;
            }
        }

        public static void SetSafeMode(bool enabled)
        {
            _safeMode = enabled;
        }

        public static bool IsSafeMode()
        {
            return _safeMode;
        }

        internal static void ThrowOrLog(Exception exception)
        {
            // TODO: log
            Console.WriteLine(exception);
            if (!IsSafeMode())
            {
                throw exception;
            }
        }
        
        private static void ApplyResultToTask<T>(TaskCompletionSource<T> task, T resultOrDefault, Exception? exception)
        {
            if (exception == null || IsSafeMode())
            {
                task.TrySetResult(resultOrDefault);
            }
            else
            {
                task.TrySetException(exception);
            }
        }
    }
}

