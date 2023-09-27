using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Bloomreach.Utils;
#if IOS
using UIKit;
using UserNotifications;
using Foundation;
#endif

namespace Bloomreach
{
	public class BloomreachSDK
	{
        
        private static bool _safeMode = true;

        public static BloomreachSDK Instance = new BloomreachSDK();

        protected internal MethodChannelConsumer Channel;

        protected internal BloomreachSDK()
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
            return ConverterUtils.ToBool(result);
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
                    ApplyResultToTask(flush, ConverterUtils.ToBool(result), exception);
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

        public static void SetFlushMode(FlushMode mode)
        {
            string flushModeString;
            switch (mode)
            {
                case FlushMode.Unknown:
                    // TODO: log or throw
                    return;
                case FlushMode.AppClose:
                    flushModeString = "app_close";
                    break;
                case FlushMode.Immediate:
                    flushModeString = "immediate";
                    break;
                case FlushMode.Manual:
                    flushModeString = "manual";
                    break;
                case FlushMode.Period:
                    flushModeString = "period";
                    break;
                default:
                    // TODO: log or throw
                    return;
            }
            Instance.Channel.InvokeMethod("SetFlushMode", flushModeString);
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
            return ConverterUtils.ToBool(result);
        }

        public static void SetAutomaticSessionTracking(bool enabled)
        {
            Instance.Channel.InvokeMethod("SetAutomaticSessionTracking", ConverterUtils.SerializeInput(enabled));
        }

        public static bool IsAutoPushNotification()
        {
            string? result = Instance.Channel.InvokeMethod("IsAutoPushNotification", null);
            return ConverterUtils.ToBool(result);
        }

        public static bool IsConfigured()
        {
            string? result = Instance.Channel.InvokeMethod("IsConfigured", null);
            return ConverterUtils.ToBool(result);
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

        public static void TrackPaymentEvent(Payment payment, double? timestamp = null)
        {
            Instance.Channel.InvokeMethod(
                "TrackPaymentEvent",
                ConverterUtils.SerializeInput(new Dictionary<string, object?>
                {
                    {"payment", payment},
                    {"timestamp", timestamp ?? ConverterUtils.GetNowInSeconds()}
                })
            );
        }

        public static void TrackEvent(Event evt, double? timestamp = null)
        {
            Instance.Channel.InvokeMethod(
                "TrackEvent",
                ConverterUtils.SerializeInput(new Dictionary<string, object?>
                {
                    {"event", evt},
                    {"timestamp", timestamp ?? ConverterUtils.GetNowInSeconds()}
                })
            );
        }

        public static void TrackSessionEnd()
        {
            Instance.Channel.InvokeMethod("TrackSessionEnd", null);
        }

        public static void TrackSessionStart()
        {
            Instance.Channel.InvokeMethod("TrackSessionStart", null);
        }

        [SupportedOSPlatform("android")]
        public static bool HandleRemoteMessage(NotificationPayload payload)
        {
            var serializedPayload = ConverterUtils.SerializeInput(payload.RawData);
            if (serializedPayload == null)
            {
                return false;
            }
            string? result = Instance.Channel.HandleRemoteMessage(serializedPayload);
            return ConverterUtils.ToBool(result);
        }

#if IOS
        [SupportedOSPlatform("ios")]
        public static bool HandleRemoteMessage(
            string appGroup,
            UNNotificationRequest request,
            Action<UNNotificationContent> contentHandler
        )
        {
            string? result = Instance.Channel.HandleRemoteMessage(appGroup, request, contentHandler);
            return ConverterUtils.ToBool(result);
        }

        [SupportedOSPlatform("ios")]
        public static void HandleRemoteMessageTimeWillExpire()
        {
            Instance.Channel.InvokeMethod("HandleRemoteMessageTimeWillExpire", null);
        }
        
        [SupportedOSPlatform("ios")]
        public static void HandleNotificationReceived(
            UNNotification notification,
            NSExtensionContext? extensionContext,
            UIViewController notificationViewController
        )
        {
            Instance.Channel.HandleNotificationReceived(notification, extensionContext, notificationViewController);
        }
#endif

        public static void HandlePushNotificationOpened(NotificationAction action)
        {
            Console.WriteLine("APNS-BR Calling HandlePushNotificationOpened");
            Instance.Channel.InvokeMethod("HandlePushNotificationOpened", ConverterUtils.SerializeInput(action));
            Console.WriteLine("APNS-BR Calling HandlePushNotificationOpened is DONE");
        }

        public static void HandlePushNotificationOpenedWithoutTrackingConsent(NotificationAction action)
        {
            Instance.Channel.InvokeMethod("HandlePushNotificationOpenedWithoutTrackingConsent", ConverterUtils.SerializeInput(action));
        }

        public static void HandleCampaignClick(Uri url)
        {
            Instance.Channel.InvokeMethod("HandleCampaignClick", url.ToString());
        }

        public static void HandleHmsPushToken(string pushToken)
        {
            Instance.Channel.InvokeMethod("HandleHmsPushToken", pushToken);
        }

        public static void HandlePushToken(string pushToken)
        {
            Instance.Channel.InvokeMethod("HandlePushToken", pushToken);
        }

#if IOS
        public static void HandlePushToken(NSData deviceToken)
        {
            var bytes = deviceToken.ToArray<byte>();
            var hexArray = bytes.Select(b => b.ToString("X2")).ToArray();
            var deviceTokenString = string.Join(string.Empty, hexArray);
            Console.WriteLine("APNS-BR Push token received");
            Console.WriteLine("APNS-BR Push token " + deviceTokenString);
            Console.WriteLine("APNS-BR Push token printed");
            Instance.Channel.InvokeMethod("HandlePushToken", deviceTokenString);
        }
#endif

        public static bool IsBloomreachNotification(NotificationPayload notificationPayload)
        {
            var result = Instance.Channel.InvokeMethod(
                "IsBloomreachNotification",
                ConverterUtils.SerializeInput(notificationPayload.RawData)
            );
            return ConverterUtils.ToBool(result);
        }

        public static void TrackClickedPush(NotificationAction action)
        {
            Instance.Channel.InvokeMethod("TrackClickedPush", ConverterUtils.SerializeInput(action));
        }

        public static void TrackClickedPushWithoutTrackingConsent(NotificationAction action)
        {
            Instance.Channel.InvokeMethod(
                "TrackClickedPushWithoutTrackingConsent",
                ConverterUtils.SerializeInput(action)
            );
        }

        public static void TrackPushToken(string pushToken)
        {
            Instance.Channel.InvokeMethod("TrackPushToken", pushToken);
        }

        public static void TrackDeliveredPush(NotificationPayload payload)
        {
            Instance.Channel.InvokeMethod(
                "TrackDeliveredPush",
                ConverterUtils.SerializeInput(payload.RawData)
            );
        }

        public static void TrackDeliveredPushWithoutTrackingConsent(NotificationPayload payload)
        {
            Instance.Channel.InvokeMethod(
                "TrackDeliveredPushWithoutTrackingConsent",
                ConverterUtils.SerializeInput(payload.RawData)
            );
        }

        public static void TrackHmsPushToken(string pushToken)
        {
            Instance.Channel.InvokeMethod(
                "TrackHmsPushToken",
                pushToken
            );
        }

        public static void RequestPushAuthorization()
        {
            Instance.Channel.InvokeMethodAsync("RequestPushAuthorization", null, (s, exception) =>
            {
                Console.WriteLine("APNS-BR Requested permission with result " + s);
            });
        }

        public static void SetReceivedPushCallback(Action<NotificationPayload> listener)
        {
            Instance.Channel.InvokeMethodAsync("SetReceivedPushCallback", null, (result, exception) =>
            {
                if (exception != null)
                {
                    ThrowOrLog(exception);
                    return;
                }
                if (result == null)
                {
                    Log("ReceivedPushCallback got empty notification data");
                    return;
                }
                try
                {
                    var rawPayload = ConverterUtils.DeserializeOutput<Dictionary<string, string>>(result);
                    if (rawPayload == null)
                    {
                        ThrowOrLog(BloomreachException.Common("ReceivedPushCallback got invalid notification data"));
                        return;
                    }
                    var payload = NotificationPayload.Parse(rawPayload);
                    listener.Invoke(payload);
                }
                catch (Exception e)
                {
                    ThrowOrLog(e);
                }
            });
        }

        public static void SetOpenedPushCallback(Action<NotificationAction> listener)
        {
            Instance.Channel.InvokeMethodAsync("SetOpenedPushCallback", null, (result, exception) =>
            {
                if (exception != null)
                {
                    ThrowOrLog(exception);
                    return;
                }
                if (result == null)
                {
                    Log("SetOpenedPushCallback got empty notification action");
                    return;
                }
                try
                {
                    var payload = ConverterUtils.DeserializeOutput<NotificationAction>(result);
                    if (payload == null)
                    {
                        ThrowOrLog(BloomreachException.Common("SetOpenedPushCallback got invalid notification action"));
                        return;
                    }
                    listener.Invoke(payload);
                }
                catch (Exception e)
                {
                    ThrowOrLog(e);
                }
            });
        }

        internal static void ThrowOrLog(Exception exception)
        {
            Log(exception);
            if (!IsSafeMode())
            {
                throw exception;
            }
        }

        private static void Log(string message)
        {
            // TODO: log
            Console.WriteLine(message);
        }

        private static void Log(Exception exception)
        {
            // TODO: log
            Console.WriteLine(exception);
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

        public static void TrackInAppMessageClick(InAppMessage message, string buttonText, string buttonLink)
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>
            {
                {"message", ConverterUtils.SerializeInput(message) },
                {"buttonText", buttonText },
                {"buttonLink", buttonLink }
            };
            Instance.Channel.InvokeMethod(
                "TrackInAppMessageClick",
                ConverterUtils.SerializeInput(data)
            );
        }

        public static void TrackInAppMessageClickWithoutTrackingConsent(InAppMessage message, string buttonText, string buttonLink)
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>
            {
                {"message", ConverterUtils.SerializeInput(message) },
                {"buttonText", buttonText },
                {"buttonLink", buttonLink }
            };
            Instance.Channel.InvokeMethod(
                "TrackInAppMessageClickWithoutTrackingConsent",
                ConverterUtils.SerializeInput(data)
            );
        }

        public static void TrackInAppMessageClose(InAppMessage message, bool? isUserInteraction = null)
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>
            {
                {"message", ConverterUtils.SerializeInput(message) },
                {"isUserInteraction", isUserInteraction }
            };
            Instance.Channel.InvokeMethod(
                "TrackInAppMessageClose",
                ConverterUtils.SerializeInput(data)
            );
        }

        public static void TrackInAppMessageCloseWithoutTrackingConsent(InAppMessage message, bool? isUserInteraction = null)
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>
            {
                {"message", ConverterUtils.SerializeInput(message) },
                {"isUserInteraction", isUserInteraction }
            };
            Instance.Channel.InvokeMethod(
                "TrackInAppMessageCloseWithoutTrackingConsent",
                ConverterUtils.SerializeInput(data)
            );
        }

        public static void SetInAppMessageActionCallback(
            bool overrideDefaultBehavior,
            bool trackActions,
            Action<InAppMessage, string?, string?, bool> listener
        )
        {
            var inputFlags = new Dictionary<string, bool>()
            {
                { "overrideDefaultBehavior", overrideDefaultBehavior },
                { "trackActions", trackActions }
            };
            Instance.Channel.InvokeMethodAsync(
                "SetInAppMessageActionCallback",
                ConverterUtils.SerializeInput(inputFlags),
                (result, exception) =>
                {
                    if (exception != null)
                    {
                        ThrowOrLog(exception);
                        return;
                    }
                    if (result == null)
                    {
                        Log("ReceivedPushCallback got empty in app message data");
                        return;
                    }
                    try
                    {
                        var actionPayload = ConverterUtils.DeserializeOutput<InAppMessageAction>(result);
                        if (actionPayload?.Message == null)
                        {
                            ThrowOrLog(BloomreachException.Common("Received In App message got invalid data"));
                            return;
                        }
                        var messageData = ConverterUtils.DeserializeOutput<IDictionary<string, object>>(actionPayload.Message);
                        if (messageData == null)
                        {
                            ThrowOrLog(BloomreachException.Common("Received In App message got invalid message"));
                            return;
                        }
                        var message = new InAppMessage(messageData);
                        listener.Invoke(message, actionPayload.ButtonText, actionPayload.ButtonLink, actionPayload.IsUserInteraction ?? false);
                    }
                    catch (Exception e)
                    {
                        ThrowOrLog(e);
                    }
                }
            );
        }
    }
}

