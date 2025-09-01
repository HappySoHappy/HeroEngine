using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace HeroEngine.Util
{
    public static class JsonPropertyUpdater
    {
        public static void UpdateFields<T>(T target, dynamic data)
        {
            if (data is JObject jObjectData)
            {
                RecursiveFieldsUpdate(target, jObjectData.ToObject<Dictionary<string, object>>()!);
                return;
            }

            if (data is Dictionary<string, object> dictionaryData)
            {
                RecursiveFieldsUpdate(target, dictionaryData);
                return;
            }

            if (data is string json && IsValidJsonString(json))
            {
                RecursiveFieldsUpdate(target, JsonConvert.DeserializeObject<Dictionary<string, object>>(json)!);
                return;
            }
        }

        private static void RecursiveFieldsUpdate(object? target, Dictionary<string, object> data)
        {
            if (target == null) return;

            var type = target.GetType();

#if DEBUG
            if (type.GetFields().Count() == 0)
            {
                Console.WriteLine("| !!! | ===================================================");
                Console.WriteLine($"|      Class {type} has 0 fields!");
                Console.WriteLine("|      This might be due to the following reasons:");
                Console.WriteLine("|      - The class is empty and doesn't define any fields.");
                Console.WriteLine("|      - The class may have private or non-public fields, which are not accessible by default.");
                Console.WriteLine($"|      - The class has {type.GetProperties().Count()} properties.");
                foreach (var property in type.GetProperties())
                {
                    Console.WriteLine($"|            - #{property.Name}, Type: {property.PropertyType.Name}");
                }

                Console.WriteLine("| !!! | ===================================================");
            }
#endif

            foreach (var field in type.GetFields())
            {
                var attribute = field.GetCustomAttribute<JsonPropertyAttribute>();
                if (attribute == null) continue;

                string? name = attribute.PropertyName;
                if (string.IsNullOrEmpty(name) || !data.ContainsKey(name!))
                {
                    continue;
                }

                var value = data[name];
                /*if (value is JObject jb)
                {
                    Console.WriteLine("{0,-20} => {1,-20} = {2}", name, field.Name, jb.ToString(Formatting.None));
                }
                else
                {
                    Console.WriteLine("{0,-20} => {1,-20} = {2}", name, field.Name, value ?? "null");
                }*/

                // list type
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var listType = field.FieldType.GetGenericArguments()[0];
                    var listInstance = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType))!;

                    if (value is JArray jArrayValue)
                    {
                        foreach (var item in jArrayValue)
                        {
                            var obj = item.ToObject(listType);

                            var addMethod = listInstance.GetType().GetMethod("Add");
                            addMethod?.Invoke(listInstance, [obj]);
                        }
                    }

                    field.SetValue(target, listInstance);
                    continue;
                }

                /*
                 * if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var keyType = field.FieldType.GetGenericArguments()[0];
            var valueType = field.FieldType.GetGenericArguments()[1];
            var dictionaryInstance = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType))!;

            if (value is JObject jObjectValue)
            {
                foreach (var property in jObjectValue.Properties())
                {
                    var key = Convert.ChangeType(property.Name, keyType);
                    var val = property.Value.ToObject(valueType);
                    dictionaryInstance.GetType().GetMethod("Add")?.Invoke(dictionaryInstance, new[] { key, val });
                }
            }

            field.SetValue(target, dictionaryInstance);
            continue;
        }
                 */

                // complex type
                if (!field.FieldType.IsPrimitive && field.FieldType != typeof(string) && value is JObject jValue)
                {
                    var fieldValue = field.GetValue(target);
                    if (fieldValue == null)
                    {
                        field.SetValue(target, Activator.CreateInstance(field.FieldType));
                    }

                    RecursiveFieldsUpdate(field.GetValue(target), jValue.ToObject<Dictionary<string, object>>()!);
                    continue;
                }

                if (field.FieldType.IsAssignableFrom(value?.GetType()))
                {
                    field.SetValue(target, value);
                }
                else
                {
                    var jsonConverter = field.GetCustomAttribute<JsonConverterAttribute>();
                    if (jsonConverter != null)
                    {
                        var converterType = jsonConverter.ConverterType;
                        var converter = Activator.CreateInstance(converterType) as JsonConverter;
                        if (converter != null)
                        {
                            if ((value ?? "").ToString()!.StartsWith("{") && (value ?? "").ToString()!.EndsWith("}"))
                                value = converter.ReadJson(new JsonTextReader(new StringReader(value?.ToString() ?? "")), field.FieldType, null, new JsonSerializer());

                            if (field.FieldType.IsArray)
                            {
                                value = converter.ReadJson(new JsonTextReader(new StringReader(value?.ToString() ?? "")), field.FieldType, null, new JsonSerializer());
                            }

                            /*if ((value ?? "").ToString()!.StartsWith("[") && (value ?? "").ToString()!.EndsWith("]"))
                            {
                                //this indeed can be empty string, and not array string
                                //if (string.IsNullOrEmpty(value?.ToString())) value = "[]";

                                var serializer = new JsonSerializer();
                                //value = serializer.Deserialize(new JsonTextReader(new StringReader(value?.ToString() ?? "[]")), field.FieldType);

                                value = ((ArrayStringConverter<dynamic>)converter).ReadJson(new JsonTextReader(new StringReader(value?.ToString() ?? "")), field.FieldType, null, new JsonSerializer());
                            }*/
                        }
                    }

                    field.SetValue(target, Convert.ChangeType(value, field.FieldType));
                }
            }
        }

        private static bool IsValidJsonString(string json)
        {
            try
            {
                System.Text.Json.JsonDocument.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
