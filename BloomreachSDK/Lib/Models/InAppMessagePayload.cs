using Bloomreach.Utils;

#if IOS
using Bloomreach.Platforms.iOS;
using Foundation;
#endif

namespace Bloomreach;

public class InAppMessagePayload
{
    public InAppMessagePayload(IDictionary<string, object> rawData)
    {
        RawData = rawData;
    }

    public static InAppMessagePayload Parse(IDictionary<string, string> messageData)
    {
        return new InAppMessagePayload(ConverterUtils.NormalizeDictionary(messageData));
    }

#if IOS
    public static InAppMessagePayload Parse(NSDictionary messageData)
    {
        return new InAppMessagePayload(ConverterUtilsIos.NormalizeDictionary(messageData));
    }
#endif

    public IDictionary<string, object> RawData { get; set; }
}