﻿using JsonObjectConverter;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FMWOTB.Account.Statistics
{
    public class Statistics
    {
        public List<Tuple<object, object>> frags { get; private set; }
        public int trees_cut { get; private set; }
        public All all { get; private set; }
        public Clan clan { get; private set; }
        public Rating rating { get; private set; }
        public Statistics(Json json)
        {
            setValues(json);
            bool ok = true;
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
                        switch (subJson.head.ToLower())
                        {
                            case "all": this.all = new All(subJson); break;
                            case "clan": this.clan = new Clan(subJson); break;
                            case "rating": this.rating = new Rating(subJson); break;
                            default:
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
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        bool ook = true;
                    }
                    //setValues(subJson);
                }
                bool ok = true;
            }
        }
    }
}
