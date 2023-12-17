﻿using JsonObjectConverter;
using System;
using System.Collections;

namespace FMWOTB.Vehicles
{
    public class Gun
    {
        public float aim_time { get; private set; }
        public int caliber { get; private set; }
        public int clip_capacity { get; private set; }
        public float clip_reload_time { get; private set; }
        public float dispersion { get; private set; }
        public float fire_rate { get; private set; }
        public int move_down_arc { get; private set; }
        public int move_up_arc { get; private set; }
        public string name { get; private set; }
        public float reload_time { get; private set; }
        public int tier { get; private set; }
        public float traverse_speed { get; private set; }
        public int weight { get; private set; }

        public Gun(Json json)
        {
            setValues(json);
        }
        private void setValues(Json helper)
        {
            if (helper.tupleList != null)
            {
                foreach (var tuple in helper.tupleList)
                {
                    string temp = tuple.Item2.Item1.Trim(' ').Trim('\"');
                    if (!temp.ToLower().Equals("null"))
                    {
                        string item1 = tuple.Item1.Trim(' ').Trim('\"');
                        var property = this.GetType().GetProperty(item1);
                        if (item1.StartsWith("[") || item1.StartsWith("{"))
                        {
                            var valueToSet = Json.convertStringToList(item1, property.PropertyType);
                            object instance = Activator.CreateInstance(property.PropertyType);
                            IList list = (IList)instance;
                            foreach (object item in valueToSet)
                            {
                                list.Add(item);
                            }
                            property.SetValue(this, list, null);
                        }
                        else
                        {
                            var valueToSet = Json.convertStringToType(temp, property.PropertyType);
                            property.SetValue(this, valueToSet);
                        }
                    }
                }
            }
        }
    }
}
