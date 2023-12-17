using System;
using thibeastmo.Json;

namespace W0tb.Replays.Tool
{

    public class Achievement
    {
        public int t { get; private set; }
        public int v { get; private set; }

        public Achievement(Json json)
        {
            foreach (Tuple<string, string> tuple in json.tupleList)
            {
                string temp = tuple.Item2.Trim(' ').Trim('\"').ToLower();
                if (!temp.ToLower().Equals("null"))
                {
                    var property = this.GetType().GetProperty(tuple.Item1.Trim(' ').Trim('\"').ToLower());
                    var valueToSet = Convert.ChangeType(temp, property.PropertyType);
                    property.SetValue(this, valueToSet);
                }
            }
        }
    }
}
