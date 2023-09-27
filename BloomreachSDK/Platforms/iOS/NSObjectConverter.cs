using Foundation;
using Newtonsoft.Json;

namespace Bloomreach.Platforms.iOS;

public class NSObjectConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(NSObject).IsAssignableFrom(objectType);
    }

    public override bool CanRead => false;

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            return;
        }
        if (value is NSDictionary dictionary)
        {
            writer.WriteStartObject();
            foreach (var pair in dictionary)
            {
                if (pair.Key == null || pair.Value == null) continue;
                writer.WritePropertyName(pair.Key.ToString());
                serializer.Serialize(writer, pair.Value);
            }
            writer.WriteEndObject();
            return;
        }
        if (value is NSArray array)
        {
            writer.WriteStartArray();
            foreach (var item in array)
            {
                if (item == null) continue;
                serializer.Serialize(writer, item);
            }
            writer.WriteEndArray();
            return;
        }
        if (value is NSNull nsNull)
        {
            writer.WriteNull();
            return;
        }
        writer.WriteValue(value.ToString());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}