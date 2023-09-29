#if IOS
using Bloomreach.Platforms.iOS;
using Foundation;
#endif
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bloomreach.Utils;

public static class ConverterUtils
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
        Converters = new List<JsonConverter>()
        {
#if IOS
            new NSObjectConverter(),
#endif
            new DictionaryConverter(),
            new StringEnumConverter()
        },
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public static string? SerializeInput(object? data)
    {
        if (data == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(data, JsonSerializerSettings);
    }

    public static T? DeserializeOutput<T>(string? data)
    {
        if (data == null)
        {
            return (T?)(null as object);
        }
        return JsonConvert.DeserializeObject<T>(data, JsonSerializerSettings);
    }

    public static string? TrimQuotes(string? source)
    {
        return source?.Trim().Trim('"');
    }

    public static double GetNowInSeconds()
    {
        return Convert.ToDouble(DateTimeOffset.Now.ToUnixTimeMilliseconds()) / 1000;
    }

    public static IDictionary<string, object> NormalizeDictionary(IDictionary<string,string> source)
    {
        return source.ToDictionary<KeyValuePair<string, string>, string, object>(
            pair => pair.Key,
            pair => pair.Value
        );
    }

#if IOS
    public static IDictionary<string, object> NormalizeDictionary(NSDictionary source)
    {
#pragma warning disable CA1416
        var result = source.ToDictionary<KeyValuePair<NSObject, NSObject>, string, object>(
            item => item.Key as NSString,
            item => item.Value
        );
        return result;
#pragma warning restore CA1416
    }
#endif

    public static bool ToBool(string? result) => result?.ToLower()?.Equals("true") ?? false;
}