using System;
using System.Collections.Generic;

namespace ExponeaSDK
{
    public class Configuration
    {

        public Configuration(string projectToken, string authorization, string baseUrl)
        {
            ProjectToken = projectToken;
            Authorization = authorization;
            BaseUrl = baseUrl;
        }

        public string ProjectToken { get; set; }
        public string Authorization { get; set; }
        public string BaseUrl { get; set; }
        public IDictionary<EventType, IList<Project>>? ProjectRouteMap { get; set; }
        public int? MaxTries { get; set; }
        public double? SessionTimeout { get; set; }
        public bool? AutomaticSessionTracking { get; set; }
        public Dictionary<string, Object>? DefaultProperties { get; set; }
        public TokenTrackFrequency TokenTrackFrequency { get; set; } = TokenTrackFrequency.OnTokenChange;
        public HttpLoggingLevel? HttpLoggingLevel { get; set; }
        // ReSharper disable once InconsistentNaming
        public double? CampaignTTL { get; set; }
        public bool AllowDefaultCustomerProperties { get; set; } = true;
        public bool AdvancedAuthEnabled { get; set; } = false;
        public AndroidConfiguration? AndroidConfiguration { get; set; }
        // ReSharper disable once InconsistentNaming
        public iOSConfiguration? IOsConfiguration { get; set; }
    }

    public class AndroidConfiguration
    {

        public AndroidConfiguration(
            bool automaticPushNotification = true,
            string? pushIcon = null,
            int? pushAccentColor = null,
            string? pushChannelName = null,
            string? pushChannelDescription = null,
            string? pushChannelId = null,
            int? pushNotificationImportance = null
            )
        {
            AutomaticPushNotification = automaticPushNotification;
            PushIcon = pushIcon;
            PushAccentColor = pushAccentColor;
            PushChannelName = pushChannelName;
            PushChannelDescription = pushChannelDescription;
            PushChannelId = pushChannelId;
            PushNotificationImportance = pushNotificationImportance;
        }

        public bool AutomaticPushNotification { get; set; } = true;
        public string? PushIcon { get; set; }
        public int? PushAccentColor { get; set; }
        public string? PushChannelName { get; set; }
        public string? PushChannelDescription { get; set; }
        public string? PushChannelId { get; set; }
        public int? PushNotificationImportance { get; set; }
    }

    public class iOSConfiguration
    {
        public iOSConfiguration(
            bool? requirePushAuthorization = null,
            string? appGroup = null
            )
        {
            RequirePushAuthorization = requirePushAuthorization;
            AppGroup = appGroup;
        }
        public bool? RequirePushAuthorization { get; set; }
        public string? AppGroup { get; set; }
    }
}

