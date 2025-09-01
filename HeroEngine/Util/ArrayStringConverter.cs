using Newtonsoft.Json;

namespace HeroEngine.Util
{
    public class ArrayStringConverter<T> : JsonConverter<T[]>
    {
        public override T[] ReadJson(JsonReader reader, Type objectType, T[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Read() && reader.TokenType == JsonToken.StartArray)
            {
                var list = new List<T>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    if (reader.Value != null)
                    {
                        list.Add((T)Convert.ChangeType(reader.Value, typeof(T)));
                    }
                }
                return list.ToArray();
            }

            return Array.Empty<T>();

            /*var str = reader.Value?.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                return str.Trim('[', ']')
                          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => (T)Convert.ChangeType(s.Trim(), typeof(T)))
                          .ToArray();
            }

            return Array.Empty<T>();*/
        }


        public override void WriteJson(JsonWriter writer, T[] value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var joined = "[" + string.Join(",", value.Select(v => v.ToString())) + "]";
                writer.WriteValue(joined);
            }
            else
            {
                writer.WriteValue("[]");
            }
        }
    }

}
