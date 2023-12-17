using System;
using System.Collections.Generic;
using System.Reflection;
using thibeastmo.Json;

namespace W0tb.Replays.Tool
{

    public class Details
    {
        public int damage_assisted_track { get; private set; }
        public int base_capture_points { get; private set; }
        public int wp_points_earned { get; private set; }
        public int time_alive { get; private set; }
        public int chassis_id { get; private set; }
        public int hits_received { get; private set; }
        public int shots_splash { get; private set; }
        public int gun_id { get; private set; }
        public int hits_pen { get; private set; }
        public int hero_bonus_credits { get; private set; }
        public int hitpoints_left { get; private set; }
        public int dbid { get; private set; }
        public int shots_pen { get; private set; }
        public int exp_for_assist { get; private set; }
        public int damage_received { get; private set; }
        public int hits_bounced { get; private set; }
        public int hero_bonus_exp { get; private set; }
        public int enemies_damaged { get; private set; }
        public IReadOnlyCollection<Achievement> achievements { get; private set; }
        public int exp_for_damage { get; private set; }
        public int damage_blocked { get; private set; }
        public int distance_travelled { get; private set; }
        public int hits_splash { get; private set; }
        public int credits { get; private set; }
        public int squad_index { get; private set; }
        public int wp_points_stolen { get; private set; }
        public int damage_made { get; private set; }
        public int vehicle_descr { get; private set; }
        public int exp_team_bonus { get; private set; }
        public int enemies_spotted { get; private set; }
        public int shots_hit { get; private set; }
        public int clanid { get; private set; }
        public int turret_id { get; private set; }
        public int enemies_destroyed { get; private set; }
        public int killed_by { get; private set; }
        public int base_defend_points { get; private set; }
        public int exp { get; private set; }
        public int damage_assisted { get; private set; }
        public int death_reason { get; private set; }
        public int shots_made { get; private set; }
        public string clan_tag { get; private set; }

        public Details(Json json)
        {
            foreach (Tuple<string, string> subHelper in json.tupleList)
            {
                string temp = subHelper.Item2.Trim(' ').Trim('\"').ToLower();
                if (!temp.ToLower().Equals("null"))
                {
                    var property = this.GetType().GetProperty(subHelper.Item1.Trim(' ').Trim('\"').ToLower());
                    var valueToSet = Convert.ChangeType(temp, property.PropertyType);
                    property.SetValue(this, valueToSet);
                }
            }
            if (json.subJsons != null)
            {
                List<Achievement> achList = new List<Achievement>();
                foreach (Json subHelper in json.subJsons)
                {
                    foreach(Json subSubHelper in subHelper.subJsons)
                    {
                        achList.Add(new Achievement(subSubHelper));
                    }
                }
                this.achievements = achList;
            }
        }
    }
}
