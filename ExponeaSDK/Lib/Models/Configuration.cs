using System;
using System.Collections.Generic;

namespace Exponea
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
        public bool AutomaticPushNotification { get; set; } = true;
        public string? PushIcon { get; set; }
        public int? PushAccentColor { get; set; }
        public string? PushChannelName { get; set; }
        public string? PushChannelDescription { get; set; }
        public string? PushChannelId { get; set; }
        public int? PushNotificationImportance { get; set; }
        // ReSharper disable once InconsistentNaming
        public bool? RequirePushAuthorization { get; set; }
        public string? AppGroup { get; set; }
    }
}

