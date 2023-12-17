using thibeastmo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace W0tb.Replays.Tool
{
    public class WGBattle /* : Battle*/
    {
        public string view_url { get; private set; }
        public string download_url { get; private set; }
        public int winner_team { get; private set; }
        public long uploaded_by { get; private set; }
        public int credits_total { get; private set; }
        public int exp_base { get; private set; }
        public string player_name { get; private set; }
        public string title { get; private set; }
        public Details details { get; private set; }
        public List<Details> fullDetails { get; private set; }
        public string vehicle { get; private set; }
        public List<long> enemies { get; private set; }
        public string description { get; private set; }
        public double battle_duration { get; private set; }
        public ulong arena_unique_id { get; private set; }
        public int vehicle_tier { get; private set; }
        public DateTime battle_start_time { get; private set; }
        public int mastery_badge { get; private set; }
        public long protagonist { get; private set; }
        public int battle_type { get; private set; }
        public int exp_total { get; private set; }
        public List<long> allies { get; private set; }
        public int vehicle_type { get; private set; }
        public double battle_start_timestamp { get; private set; }
        public int credits_base { get; private set; }
        public int protagonist_team { get; private set; }
        public string map_name { get; private set; }
        public int room_type { get; private set; }
        public int battle_result { get; private set; }
        public string error { get; private set; }
        public string hexKey { get; private set; }

        /// <summary>
        /// Instantiate this object with the the replay url. This url is from a downloadable place such as discord or ...
        /// </summary>
        /// <param name="replayUrl"></param>
        public WGBattle(string replayUrlOrJson, List<long> clanIDList)
        {
            if (replayUrlOrJson != null)
            {
                if (replayUrlOrJson.StartsWith("http"))
                {
                    getBattle(replayUrlOrJson, true).Wait();
                }
                else
                {
                    getBattle(replayUrlOrJson, false).Wait();
                    //jsonToBattle(replayUrlOrJson);
                }
                if (this.details == null)
                {
                    if (this.fullDetails != null)
                    {
                        foreach (Details detail in this.fullDetails)
                        {
                            if (clanIDList.Contains(detail.clanid) || clanIDList.Contains(detail.clanid))
                            {
                                if (detail.dbid.Equals(this.protagonist))
                                {
                                    this.details = detail;
                                }
                            }
                        }
                    }
                }
            }
        }
        private async Task getBattle(string pathOrJson, bool isHTTP)
        {
            string jsonText = pathOrJson;
            if (isHTTP)
            {
                jsonText = await replayToString(pathOrJson, null);
            }
            if (jsonText == null)
            {
                throw new JsonNotFoundException();
            }
            else if (jsonText.Length == 0)
            {
                throw new JsonNotFoundException();
            }

            jsonToBattle(jsonText);
        }
        private async Task<string> replayToString(string path, string title)
        {
            if (title == null)
            {
                title = Path.GetFileName(path);
            }
            string url = @"https://wotinspector.com/api/replay/upload?url=";
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form1 = new MultipartFormDataContent();
            String AsBase64String = null;
            if (path.StartsWith("http"))
            {
                AsBase64String = Convert.ToBase64String(await httpClient.GetByteArrayAsync(path));
            }
            else
            {
                AsBase64String = Convert.ToBase64String(File.ReadAllBytes(path));
            }

            form1.Add(new StringContent(Path.GetFileName(path)), "filename");
            form1.Add(new StringContent(AsBase64String), "file");

            HttpResponseMessage response = await httpClient.PostAsync(url, form1);
            return await response.Content.ReadAsStringAsync();
        }
        private void jsonToBattle(string jsonText)
        {
            Json jsonHelper = new Json(jsonText, "WGBattle");
            if (jsonHelper.subJsons != null)
            {
                foreach (Json helper in jsonHelper.subJsons)
                {
                    //WGBattle
                    setValues(helper);
                    if (helper.subJsons != null)
                    {
                        foreach (Json subHelper in helper.subJsons)
                        {
                            setValues(subHelper);
                            if (helper.subJsons != null)
                            {
                                foreach (Json subSubHelper in subHelper.subJsons)
                                {
                                    //details
                                    //enemies
                                    //alies
                                    if (subSubHelper.head.ToLower().Equals("details"))
                                    {
                                        if (subSubHelper.subJsons.Count > 1)
                                        {
                                            this.fullDetails = new List<Details>();
                                            foreach (Json detailJson in subSubHelper.subJsons)
                                            {
                                                this.fullDetails.Add(new Details(detailJson));
                                            }
                                        }
                                        else if (subSubHelper.subJsons.Count > 0)
                                        {
                                            this.details = new Details(subSubHelper.subJsons[0]);
                                        }
                                    }
                                    else
                                    {
                                        string[] splitted = subSubHelper.jsonText.Trim(' ').TrimStart('[').TrimEnd(']').Split(',');
                                        foreach (string item in splitted)
                                        {
                                            if (subSubHelper.head.ToLower().Equals("enemies"))
                                            {
                                                if (this.enemies == null)
                                                {
                                                    this.enemies = new List<long>();
                                                }
                                                this.enemies.Add(Convert.ToInt64(item));
                                            }
                                            else
                                            {
                                                if (this.allies == null)
                                                {
                                                    this.allies = new List<long>();
                                                }
                                                this.allies.Add(Convert.ToInt64(item));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                    var valueToSet = new object();
                    if (property.PropertyType.Name.ToLower().Equals("datetime"))
                    {
                        valueToSet = convertStringToDateTime(temp);
                    }
                    else
                    {
                        valueToSet = Convert.ChangeType(temp, property.PropertyType);
                    }
                    property.SetValue(this, valueToSet);
                    if (property.Name.ToLower().Contains("url") && this.hexKey == null)
                    {
                        this.hexKey = Path.GetFileName((string)valueToSet);
                    }
                }
            }
        }
        private DateTime convertStringToDateTime(string date)
        {
            date = convertToDate(date);
            string[] splitted = date.Split(' ');
            string[] firstPart = splitted[0].Split('-');
            string[] secondPart = splitted[1].Split(':');
            return new DateTime(Convert.ToInt32(firstPart[0]), Convert.ToInt32(firstPart[1]), Convert.ToInt32(firstPart[2]), Convert.ToInt32(secondPart[0]), Convert.ToInt32(secondPart[1]), Convert.ToInt32(secondPart[2]));
        }
        private string convertToDate(string date)
        {
            string[] splitted = date.Replace('/', '-').Split(' ');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < splitted.Length; i++)
            {
                if (i < 2)
                {
                    if (i > 0)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(splitted[i]);
                }
            }
            return sb.ToString();
        }
    }
}