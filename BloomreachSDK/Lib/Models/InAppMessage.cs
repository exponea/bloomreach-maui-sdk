using System;

namespace Bloomreach
{
	public class InAppMessage
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string RawMessageType { get; set; }
        public string? RawFrequency { get; set; }
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public string? EventType { get; set; }
        public int? Priority { get; set; }
        public int? DelayMS { get; set; }
        public int? TimeoutMS { get; set; }
        public string? PayloadHtml { get; set; }
        public bool? IsHtml { get; set; }
        public bool RawHasTrackingConsent { get; set; }
        public string? ConsentCategoryTracking { get; set; }

        public InAppMessage(
            string id,
            string name,
            string rawMessageType,
            string rawFrequency,
            int variantId,
            string variantName,
            string eventType,
            int priority,
            int delayMs,
            int timeoutMs,
            string payloadHtml,
            bool isHtml,
            bool rawHasTrackingConsent,
            string consentCategoryTracking
            )
        {
            Id = id;
            Name = name;
            RawMessageType = rawMessageType;
            RawFrequency = rawFrequency;
            VariantId = variantId;
            VariantName = variantName;
            EventType = eventType;
            Priority = priority;
            DelayMS = delayMs;
            TimeoutMS = timeoutMs;
            PayloadHtml = payloadHtml;
            IsHtml = isHtml;
            RawHasTrackingConsent = rawHasTrackingConsent;
            ConsentCategoryTracking = consentCategoryTracking;
        }

        public InAppMessage(IDictionary<string, object> data)
        {
            Id = ParseNullSafe<string>(data, "id", "")!;
            Name = ParseNullSafe<string>(data, "name", "")!;
            RawMessageType = ParseNullSafe<string>(data, "rawMessageType", "modal")!;
            RawFrequency = ParseNullSafe<string>(data, "rawFrequency", null);
            VariantId = (int)ParseNullSafe<int>(data, "variantId", 0)!;
            VariantName = ParseNullSafe<string>(data, "variantName", "")!;
            EventType = ParseNullSafe<string>(data, "eventType", null);
            Priority = ParseNullSafe<int>(data, "priority", null);
            DelayMS = ParseNullSafe<int>(data, "delayMs", null);
            TimeoutMS = ParseNullSafe<int>(data, "timeoutMs", null);
            PayloadHtml = ParseNullSafe<string>(data, "payloadHtml", null);
            IsHtml = ParseNullSafe<bool>(data, "isHtml", null);
            RawHasTrackingConsent = (bool)ParseNullSafe<bool>(data, "rawHasTrackingConsent", true)!;
            ConsentCategoryTracking = ParseNullSafe<string>(data, "consentCategoryTracking", null);
        }

        private T? ParseNullSafe<T>(IDictionary<string, object> source, string key, T? defaultValue) where T : class
        {
            source.TryGetValue(key, out var tmp);
            if (tmp is T value)
            {
                return value;
            }
            return defaultValue;
        }

        private T? ParseNullSafe<T>(IDictionary<string, object> source, string key, T? defaultValue) where T : struct
        {
            source.TryGetValue(key, out var tmp);
            if (tmp is T value)
            {
                return value;
            }
            return defaultValue;
        }
    }
}

