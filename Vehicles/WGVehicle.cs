using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using thibeastmo.Json;

namespace W0tb.Replay.Tool
{
    public class WGVehicle
    {
        public string description { get; private set; }
        public List<int> engines { get; private set; }
        public List<int> guns { get; private set; }
        public bool is_premium { get; private set; }
        public string name { get; private set; }
        public string nation { get; private set; }
        public List<int> next_tanks { get; private set; }
        public List<int> prices_xp { get; private set; }
        public List<int> suspensions { get; private set; }
        public long tank_id { get; private set; }
        public int tier { get; private set; }
        public List<int> turrets { get; private set; }
        public string type { get; private set; }
        public WGVehicle(Json json)
        {
            setValues(json);
        }
        private void setValues(Json helper)
        {
            foreach (Tuple<string, string> tuple in helper.tupleList)
            {
                string temp = tuple.Item2.Trim(' ').Trim('\"').ToLower();
                if (!temp.ToLower().Equals("null"))
                {
                    string item1 = tuple.Item1.Trim(' ').Trim('\"').ToLower();
                    var property = this.GetType().GetProperty(item1);
                    object valueToSet;
                    if (item1.StartsWith("["))
                    {
                        valueToSet = convertStringToList(item1, property.PropertyType);
                    }
                    else
                    {
                        valueToSet = convertStringToType(temp, property.PropertyType);
                    }
                    property.SetValue(this, valueToSet);
                }
            }
        }
        private List<object> convertStringToList(string text, Type type)
        {
            List<object> objectList = new List<object>();
            string[] splitted = text.TrimStart('[').TrimEnd(']').Split(',');
            foreach(string item in splitted)
            {
                string tempItem = item.Trim(' ').Trim('\"');
                objectList.Add(convertStringToType(tempItem, type));
            }
            return objectList;
        }
        private object convertStringToType(string text, Type type)
        {
            return Convert.ChangeType(text, type);
        }
        public static async Task<string> vehiclesToString(string key)
        {
            string url = @"https://api.wotblitz.eu/wotb/encyclopedia/vehicles/?application_id=" + key;
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form1 = new MultipartFormDataContent();
            HttpResponseMessage response = await httpClient.PostAsync(url, form1);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
