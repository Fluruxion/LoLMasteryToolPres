using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace LoLMasteryTool.DataClasses
{
    /// <summary>
    /// This is an alternative variant of the original ChampionData that is used simply for storing and reading ARAMViewMode related data.
    /// Acts mostly as a middleman between the ChampionData class and the Yaml file we're storing/reading AramViewMode related data to/from.
    /// [YamlIgnore] specifies that the property is not saved or read when using this class in a Yaml parser.
    /// </summary>
    class ChampionDataAram
    {
        [YamlIgnore]
        private string m_ChampionName { get; set; }
        [YamlIgnore]
        public string ChampionName
        {
            get
            {
                if (m_ChampionName == "") return ChampionID.ToString();
                return m_ChampionName;
            }
            set
            {
                m_ChampionName = value;
            }
        }
        public long ChampionID { get; set; }
        [YamlIgnore]
        private bool m_AramCompleted { get; set; }
        public bool AramCompleted
        {
            get
            {
                return m_AramCompleted;
            }
            set
            {
                m_AramCompleted = value;
            }
        }
        public ChampionDataAram()
        {
        }

        public ChampionDataAram(ChampionData champ)
        {
            ChampionName = champ.ChampionName;
            ChampionID = champ.ChampionID;
            AramCompleted = champ.AramCompleted;
        }

        public ChampionDataAram(ChampionDataAram champ)
        {
            ChampionName = champ.ChampionName;
            ChampionID = champ.ChampionID;
            AramCompleted = champ.AramCompleted;
        }
        public ChampionDataAram(bool invalid)
        {
            if (invalid)
            {
                ChampionName = "Error";
                ChampionID = -1;
            }
        }
    }
}
