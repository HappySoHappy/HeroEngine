using System.Collections.Specialized;
using System.Collections;
using Newtonsoft.Json;

namespace HeroEngine.Framework
{
    public class RequestData
    {
        private OrderedDictionary _data;
        public RequestData()
        {
            _data = new OrderedDictionary();
        }

        public object? this[string key]
        {
            get => _data[key];
            set => _data[key] = value;
        }

        public void SetData(string key, object? value)
        {
            _data[key] = value;
        }

        public object? GetData(string key)
        {
            return _data.Contains(key) ? _data[key] : null;
        }

        public void RemoveData(string key)
        {
            _data.Remove(key);
        }

        public bool Contains(string key)
        {
            return _data.Contains(key);
        }

        public override string ToString()
        {
            List<string> keyValuePairs = new List<string>();

            foreach (DictionaryEntry entry in _data)
            {
                string key = entry.Key.ToString()!;
                string value = entry.Value?.ToString() ?? "";
                keyValuePairs.Add($"{key}={value}");
            }

            return string.Join("&", keyValuePairs);
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(_data);
        }
    }
}
