﻿using JsonObjectConverter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FMWOTB.Tournament
{
    public class Groups
    {
        public long group_id { get; set; }
        public int group_order { get; set; }
        public Groups(Json json)
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
                        if (property != null)
                        {
                            object valueToSet;
                            if (item1.StartsWith("[") || item1.StartsWith("{"))
                            {
                                valueToSet = Json.convertStringToList(item1, property.PropertyType);
                            }
                            else if (property.PropertyType == typeof(DateTime?))
                            {
                                if (property.GetValue(this) == null)
                                {
                                    property.SetValue(this, new DateTime());
                                }
                                valueToSet = Json.convertStringToDateTime(temp);
                            }
                            else
                            {
                                valueToSet = Json.convertStringToType(temp, property.PropertyType);
                            }
                            property.SetValue(this, valueToSet);
                        }
                    }
                }
            }
            if (helper.subJsons != null)
            {
                foreach (Json subJson in helper.subJsons)
                {
                    try
                    {
                        if (subJson.tupleList == null)
                        {
                            var property = this.GetType().GetProperty(subJson.head);
                            if (property != null)
                            {
                                string item1 = subJson.jsonText.Trim(' ').Trim('\"');
                                var valueToSet = Json.convertStringToList(item1, property.PropertyType);
                                object instance = Activator.CreateInstance(property.PropertyType);
                                // List<T> implements the non-generic IList interface
                                IList list = (IList)instance;
                                foreach (object item in valueToSet)
                                {
                                    list.Add(item);
                                }
                                property.SetValue(this, list, null);
                            }
                        }
                        else
                        {
                            var property = this.GetType().GetProperty(subJson.head.Trim(' ').Trim('\"'));
                            if (property != null)
                            {
                                List<Tuple<string, string>> tupleList = new List<Tuple<string, string>>();
                                foreach (var item in subJson.tupleList)
                                {
                                    tupleList.Add(new Tuple<string, string>(item.Item1.Trim(' ').Trim('\"'), item.Item2.Item1.Trim(' ').Trim('\"')));
                                }
                                object instance = Activator.CreateInstance(property.PropertyType);
                                // List<T> implements the non-generic IList interface
                                IList list = (IList)instance;
                                foreach (object item in tupleList)
                                {
                                    list.Add(item);
                                }
                                property.SetValue(this, list, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        bool ok = true;
                    }
                    setValues(subJson);
                }
            }
        }
    }
}
