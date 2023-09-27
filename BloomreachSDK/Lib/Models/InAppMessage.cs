using System;

namespace Bloomreach
{
	public class InAppMessage
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string RawMessageType { get; set; }
        public string RawFrequency { get; set; }
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public string EventType { get; set; }
        public int Priority { get; set; }
        public int DelayMS { get; set; }
        public int TimeoutMS { get; set; }
        public string PayloadHtml { get; set; }
        public bool IsHtml { get; set; }
        public bool RawHasTrackingConsent { get; set; }
        public string ConsentCategoryTracking { get; set; }

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
            Id = data["id"].ToString() ?? "";
            Name = data["name"].ToString() ?? "";
            RawMessageType = data["rawMessageType"].ToString() ?? "";
            RawFrequency = data["rawFrequency"].ToString() ?? "";
            VariantId = int.Parse(data["variantId"].ToString() ?? "0");
            VariantName = data["variantName"].ToString() ?? "";
            EventType = data["eventType"].ToString() ?? "";
            Priority = int.Parse(data["priority"].ToString() ?? "0");
            DelayMS = int.Parse(data["delayMS"].ToString() ?? "0");
            TimeoutMS = int.Parse(data["timeoutMS"].ToString() ?? "0");
            PayloadHtml = data["payloadHtml"].ToString() ?? "";
            IsHtml = (data["isHtml"].ToString() ?? "") == "true" ? true : false;
            RawHasTrackingConsent = (data["rawHasTrackingConsent"].ToString() ?? "") == "true" ? true : false;
            ConsentCategoryTracking = data["consentCategoryTracking"].ToString() ?? "";
        }
    }
}

