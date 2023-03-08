using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json;

namespace LoLMasteryTool.DataClasses
{
    /// <summary>
    /// Data classes used to parse API responses quickly in a way that makes sense.
    /// </summary>
    class AccountResponse
    {
        public string ID { get; set; }
        public string AccountID { get; set; }
        public string PUUID { get; set; }
        public string Name { get; set; }
        public int ProfileIconID { get; set; }
        public long RevisionDate { get; set; }
        public long SummonerLevel { get; set; }
        public AccountResponse()
        {

        }
    }

    class ChampionsResponse
    {
        public long ChampionID { get; set; }
        public int ChampionLevel { get; set; }
        public int ChampionPoints { get; set; }
        public long LastPlayTime { get; set; }
        public long PointsSinceLastLevel { get; set; }
        public long championPointsUntilNextLevel { get; set; }
        public bool ChestGranted { get; set; }
        public int TokensEarned { get; set; }
        public ChampionsResponse()
        {

        }
    }

    class DataDragonChampResponse
    {
        public string type { get; set; }
        public string format { get; set; }
        public string version { get; set; }
        public Dictionary<string, DataDragonChampDataReponse> data { get; set; }
        public Dictionary<string, string> keys { get; set; }

    }

    class DataDragonChampDataReponse
    {
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public Dictionary<string, object> image { get; set; }
    }
}
