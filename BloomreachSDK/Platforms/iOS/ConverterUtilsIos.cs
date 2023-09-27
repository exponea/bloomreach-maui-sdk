using System.Runtime.Versioning;
using Foundation;

namespace Bloomreach.Platforms.iOS;

[SupportedOSPlatform("ios")]
public static class ConverterUtilsIos
{
    public static IDictionary<string, object> NormalizeDictionary(NSDictionary source) =>
        source.ToDictionary<KeyValuePair<NSObject, NSObject>, string, object>(
            pair => pair.Key as NSString,
            pair => pair.Value
        );
}