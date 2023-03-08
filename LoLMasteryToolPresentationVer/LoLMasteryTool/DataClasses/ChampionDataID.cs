using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLMasteryTool.DataClasses
{
    class ChampionDataID
    {
        public long ChampionID { get; set; }
        public string ChampionName { get; set; }
        public string ChampionFileName { get; set; }

        public ChampionDataID()
        {
            // Default values used to clearly show a failure. Will show a picture of a blobfish if the data is invalid in my test environment (Shows as a black square on installed version of app).
            ChampionID = -1;
            ChampionName = "Jeff";
            ChampionFileName = "Jeff";
        }

        public ChampionDataID(long champID, string name, string fileName)
        {
            ChampionID = champID;
            ChampionName = name;
            ChampionFileName = fileName;
        }
    }
}
