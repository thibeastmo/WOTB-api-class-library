using System;
using FMWOTB;
using FMWOTB.Account;
using FMWOTB.Clans;
using FMWOTB.Tools;
using FMWOTB.Tools.Replays;
using FMWOTB.Tournament;

namespace LibraryTesting
{
    //JsonObjectConverter werkte tot 1.28
    class Program
    {
        public const string WG_APPLICATION_ID = "35e9c6a1ad0224a0a2950bb159522aaa";
        public const long ACCOUNT_ID = 538258181;
        static void Main(string[] args)
        {
            string account_name = "THIBEASTMO";
            //var accounts = WGAccount.searchByName(FMWOTB.Tools.SearchAccuracy.EXACT, account_name, WG_APPLICATION_ID,
            //    loadVehicles: true, loadClanMembers: false, loadClan: false, loadStatistics: false).Result;
            var accounts = WGAccount.searchByName(FMWOTB.Tools.SearchAccuracy.EXACT, account_name, WG_APPLICATION_ID,
                loadVehicles: 2, loadClanMembers: false, loadClan: false, loadStatistics: false).Result;


            //WGBattle battle = new WGBattle("https://replays.wotinspector.com/en/view/dfbb4828ae5f069de36b495d57310086");

            //var clanResults = WGClan.searchByName(SearchAccuracy.EXACT, "1-DEF", WG_APPLICATION_ID, true).Result;
            //if (clanResults != null && clanResults.Count > 0)
            //{
            //    WGClan clan = clanResults[0];
            //}

            //string tournamentsString = Tournaments.tournamentsToString(WG_APPLICATION_ID).Result;
            //Json tournamentsJson = new Json(tournamentsString, string.Empty);
            //string tournamentString = WGTournament.tournamentsToString(WG_APPLICATION_ID, long.Parse(tournamentsJson.jsonArray[0].Item2.tupleList[8].Item2.Item1)).Result;
            //WGTournament tournament = new WGTournament(new Json(tournamentString, string.Empty));

            //var accountNoVehicles = new WGAccount(WG_APPLICATION_ID, ACCOUNT_ID, loadVehicles: 0);
            //var accountAllVehicles = new WGAccount(WG_APPLICATION_ID, ACCOUNT_ID, loadVehicles: 1);
            //var accountGarageVehicles = new WGAccount(WG_APPLICATION_ID, ACCOUNT_ID, loadVehicles: 2);

            bool ok = true;
        }
    }
}
