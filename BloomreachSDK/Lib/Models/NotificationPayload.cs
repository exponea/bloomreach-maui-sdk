using Bloomreach.Utils;

#if IOS
using Bloomreach.Platforms.iOS;
using Foundation;
#endif

namespace Bloomreach;

public class NotificationPayload
{
    public NotificationPayload(IDictionary<string, object> rawData)
    {
        RawData = rawData;
    }

    public static NotificationPayload Parse(IDictionary<string, string> messageData)
    {
        return new NotificationPayload(ConverterUtils.NormalizeDictionary(messageData));
    }

#if IOS
    public static NotificationPayload Parse(NSDictionary messageData)
    {
        return new NotificationPayload(ConverterUtilsIos.NormalizeDictionary(messageData));
    }
#endif
    
    public IDictionary<string,object> RawData { get; set; }
}