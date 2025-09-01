using Newtonsoft.Json;

namespace HeroEngine.Desktop.Interface
{
    public class ColorConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string colorString = (string)reader.Value;
                return ColorTranslator.FromHtml(colorString);
            }
            throw new JsonSerializationException("Invalid color format");
        }

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue(ColorTranslator.ToHtml(value));
        }
    }
}
