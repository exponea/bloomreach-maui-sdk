using Newtonsoft.Json;

namespace Bloomreach.Utils;

public class DictionaryConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        var dictionary = (IDictionary<string, object?>)value;
        writer.WriteStartObject();
        foreach (var pair in dictionary)
        {
            if (pair.Value != null)
            {
                writer.WritePropertyName(pair.Key);
                serializer.Serialize(writer, pair.Value);
            }
        }
        writer.WriteEndObject();
    }
    
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var result = new Dictionary<string, object>();
        string? key = null;
        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.EndObject)
            {
                break;
            }
            if (reader.TokenType == JsonToken.PropertyName)
            {
                key = (string?) reader.Value;
            }
            else
            {
                var value = serializer.Deserialize(reader, objectType.GetGenericArguments()[1]);
                if (key != null && value != null)
                {
                    result[key] = value;
                }
                key = null;
            }
        }
        return result;
    }
    
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<string, object?>)
               || objectType == typeof(Dictionary<string, object>)
               || objectType == typeof(IDictionary<string, object?>)
               || objectType == typeof(IDictionary<string, object>);
    }
}