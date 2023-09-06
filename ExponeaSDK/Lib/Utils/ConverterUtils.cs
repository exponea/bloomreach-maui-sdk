using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Exponea.Utils;

public static class ConverterUtils
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
        Converters = new List<JsonConverter>()
        {
            new DictionaryConverter(),
            new StringEnumConverter()
        },
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static string? SerializeInput(object? data)
    {
        if (data == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(data, JsonSerializerSettings);
    }

    public static T? DeserializeOutput<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data, JsonSerializerSettings);
    }

    public static string? TrimQuotes(string? source)
    {
        return source?.Trim().Trim('"');
    }
}