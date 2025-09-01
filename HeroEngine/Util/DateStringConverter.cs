using Newtonsoft.Json;

namespace HeroEngine.Util
{
    public class DateStringConverter : JsonConverter
    {
        private readonly string _format;

        public DateStringConverter()
        {
            _format = "yyyy-MM-dd HH:mm:ss";
        }

        public DateStringConverter(string format = "yyyy-MM-dd HH:mm:ss")
        {
            _format = format;
        }

        public override bool CanConvert(Type objectType)
        {
            // Define that this converter can handle any object type.
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string? dateString = reader.Value?.ToString();
            return DateTime.TryParseExact(dateString, _format, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var result)
                ? result
                : throw new JsonSerializationException($"Invalid date format: {dateString}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(_format));
        }
    }
}
