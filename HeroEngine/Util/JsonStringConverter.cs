using Newtonsoft.Json;

namespace HeroEngine.Util
{
    public class JsonStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // Define that this converter can handle any object type.
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // If the token is a string, parse it as JSON into the target object type.
            if (reader.TokenType == JsonToken.String)
            {
                var jsonString = (string)reader.Value!;
                return JsonConvert.DeserializeObject(jsonString, objectType)!;
            }

            // If it's not a string, handle it normally.
            return serializer.Deserialize(reader, objectType)!;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Serialize the object to a JSON string and write it.
            var jsonString = JsonConvert.SerializeObject(value);
            writer.WriteValue(jsonString);
        }
    }
}
