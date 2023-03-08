using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using LoLMasteryTool.DataClasses;

namespace LoLMasteryTool.DataGrab
{
    /// <summary>
    /// This handles all links to the Riot API
    /// Uses the APIResponse dataclasses to handle return data
    /// </summary>
    class RiotAPILink
    {
        private string SummonerID { get; set; }
        private string SummonerName { get; set; }
        private string APIKey { get; set; }
        public int Icon { get; set; }
        Dictionary<string, string> m_ChampIDDict { get; set; }
        Dictionary<string, string> ChampIDDict
        {
            get
            {
                if (m_ChampIDDict.Count > 5) return m_ChampIDDict;
                else
                {
                    GetChampInfo();
                    return m_ChampIDDict;
                }
            }
            set
            {
                m_ChampIDDict = value;
            }
        }


        public RiotAPILink()
        {
            APIKey = Environment.GetEnvironmentVariable("turtle");
            //SummonerName = "";
            //GetSummonerID(SummonerName);
        }

        public bool GetSummonerID(string summonerName)
        {
            SummonerName = summonerName;
            string request = string.Format(@"https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/{0}?api_key={1}", SummonerName, APIKey);
            WebClient webClient = new WebClient();
            string response;
            try
            {
                response = webClient.DownloadString(request);
            }
            catch (Exception ex)
            {
                return false;
            }

            AccountResponse account = new AccountResponse();

            account = JsonNet.Deserialize<AccountResponse>(response);

            SummonerID = account.ID;
            SummonerName = account.Name;
            //Icon = account.ProfileIconID;
            //string iconPath = string.Format(@"https://ddragon.leagueoflegends.com/cdn/10.23.1/img/profileicon/{0}.png", Icon);
            //webClient.DownloadFile(iconPath, string.Format(@"{0}\LoLMasteryTool\PlayerIcon.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
            webClient.Dispose();
            webClient = null;
            return true;
        }

        public List<ChampionData> GetChampionData(DataGrab saveData = null)
        {
            DataDragonChampResponse champNames = GetChampInfo();
            //GetSummonerID("Fluruxion");
            List<ChampionData> champions = new List<ChampionData>();
            string request = string.Format(@"https://euw1.api.riotgames.com/lol/champion-mastery/v4/champion-masteries/by-summoner/{0}?api_key={1}", SummonerID, APIKey);
            //string test = File.ReadAllText(request);
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            WebClient webClient = new WebClient();
            string response = webClient.DownloadString(request);

            List<ChampionsResponse> championsResponse = new List<ChampionsResponse>();

            championsResponse = JsonNet.Deserialize<List<ChampionsResponse>>(response);

            foreach (ChampionsResponse APIChampion in championsResponse)
            {
                champions.Add(new ChampionData(APIChampion, champNames));
            }
            webClient = null;

            int counter = 0;
            if (saveData != null && champions != null && champions.Count > 0)
            {
                foreach (ChampionData champ in champions)
                {
                    if (champ.ChampionIconPath == "")
                    {
                        counter++;
                    }
                }

                if (counter > 0) saveData.PopulateChampionImages(champions);
            }


            return champions;
        }

        public string GetLoLVersion()
        {
            string version = "";
            List<String> versionsResponse = new List<string>();
            string response = DownloadPage(@"https://ddragon.leagueoflegends.com/api/versions.json");
            versionsResponse = JsonNet.Deserialize<List<String>>(response);
            version = versionsResponse.First();
            return version;
        }

        public string DownloadPage(string request)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(request);
            }
        }

        public DataDragonChampResponse GetChampInfo()
        {
            string Version = GetLoLVersion();
            DataDragonChampResponse champInfoResponse = new DataDragonChampResponse();
            string response = DownloadPage(string.Format(@"https://ddragon.leagueoflegends.com/cdn/{0}/data/en_GB/championFull.json", Version));
            champInfoResponse = JsonNet.Deserialize<DataDragonChampResponse>(response);

            return champInfoResponse;
        }

        public string GetChampIconDownloadPath(string champID)
        {
            string champPath = "";



            return champPath;
        }
    }
}
