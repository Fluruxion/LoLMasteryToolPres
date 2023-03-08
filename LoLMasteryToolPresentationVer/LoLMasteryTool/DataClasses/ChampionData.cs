using LoLMasteryTool.DataClasses;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YamlDotNet.Serialization;

namespace LoLMasteryTool
{
    class ChampionData: ObservableObject
    {
        #region Properties
        [YamlIgnore]
        private string m_ChampionName { get; set; }
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

        public bool NewChamp { get; set; }
        [YamlIgnore]
        private string m_ChampionFileName { get; set; }
        [YamlIgnore]
        public string ChampionFileName
        {
            get
            {
                if (m_ChampionFileName != null) return m_ChampionFileName;
                else return ChampionName;
            }
            set
            {
                m_ChampionFileName = value;
            }
        }
        [YamlIgnore]
        public int MasteryScore { get; set; }

        [YamlIgnore]
        public int MasteryPoints { get; set; }

        [YamlIgnore]
        public string MasteryScoreDisplay
        {
            get
            {
                return String.Format("Mastery: {0}", MasteryScore);
            }
        }
        [YamlIgnore]
        public string MasteryPointDisplay
        {
            get
            {
                return string.Format("Points: {0:n0}", MasteryPoints);
                
            }
        }
        [YamlIgnore]
        public int Rekindled { get; set; }
        [YamlIgnore]
        public long PointsRemaining { get; set; }

        [YamlIgnore]
        public string RekindledDisplay
        {
            get
            {
                return String.Format("x{0}", Rekindled);
            }
        }
        [YamlIgnore]
        public int Tokens { get; set; }
        [YamlIgnore]
        public Positions Roles { get; set; }

        [YamlIgnore]
        public string TokensDisplay
        {
            get
            {
                if (MasteryScore == 7)
                {
                    return "";
                }
                else if (MasteryScore < 5)
                {
                    return String.Format("Points Until mastery {0}: {1}", MasteryScore + 1, PointsRemaining);
                }
                return String.Format("Tokens: {0}/{1}", Tokens, MaxTokens);
            }
        }

        [YamlIgnore]
        public int TokensLeft
        {
            get
            {
                return MaxTokens - Tokens;
            }
        }

        [YamlIgnore]
        public int MaxTokens
        {
            get
            {
                switch (MasteryScore)
                {
                    case 5: return 2;
                    case 6: return 3;
                    default: return 0;
                }
            }
        }

        [YamlIgnore]
        public string ChampionIconPath
        {
            get
            {
                string temp = string.Format("{0}{1}{2}.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"\LolMasteryTool\ChampionIcons\", ChampionName);
                if (File.Exists(temp)) return temp;
                return "";
            }
            set
            {
                // do nothing
            }
        }

        [YamlIgnore]
        public string ChampionMasteryIcon
        {
            get
            {
                return string.Format("{0}{1}{2}.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"\LolMasteryTool\ChampionIcons\Mast", MasteryScore);
            }
        }
        [YamlIgnore]
        public Bitmap ChampionIcon
        {
            get
            {
                try
                {
                    Bitmap temp = new Bitmap(new Bitmap(ChampionIconPath), 50, 50);
                    return temp;
                }
                catch
                {
                    return new Bitmap(50,50);
                }
            }
            set
            {
                // do nothing
            }
        }

        #endregion

        #region Constructors

        public ChampionData()
        {
            ChampionName = "";
            MasteryScore = -1;
            Roles = new Positions();
        }

        public ChampionData(string champion, int mastery, Positions roles)
        {
            ChampionName = champion;
            MasteryScore = mastery;
            Rekindled = 0;
            Roles = roles;
        }

        public ChampionData(string champion, int mastery, int tokens, Positions roles)
        {
            ChampionName = champion;
            MasteryScore = mastery;
            Tokens = tokens;
            Roles = roles;
        }

        // This is the main one
        public ChampionData(string champion, int mastery, int tokens, int rekindled, Positions roles)
        {
            ChampionName = champion;
            MasteryScore = mastery;
            Tokens = tokens;
            Rekindled = rekindled;
            Roles = roles;
        }

        public ChampionData(ChampionData championData)
        {
            if (championData == null) championData = new ChampionData();
            ChampionName = championData.ChampionName;
            MasteryScore = championData.MasteryScore;
            Rekindled = championData.Rekindled;
            Tokens = championData.Tokens;
            Roles = championData.Roles;
        }

        public ChampionData(ChampionsResponse APIChampion, DataDragonChampResponse champNames)
        {
            ChampionNameFromID(APIChampion.ChampionID, champNames);
            Tokens = APIChampion.TokensEarned;
            MasteryPoints = APIChampion.ChampionPoints;
            MasteryScore = APIChampion.ChampionLevel;
            PointsRemaining = APIChampion.championPointsUntilNextLevel;
        }
        #endregion

        #region Methods

        public void IncreaseMastery()
        {
            if (MasteryScore == 7) return;
            MasteryScore += 1;
            Tokens = 0;
        }

        public void DecreaseMastery()
        {
            if (MasteryScore == 0) return;
            MasteryScore -= 1;
            Tokens = 0;
        }

        public void IncreaseRekindled()
        {
            if (Rekindled == 2) return;
            Rekindled += 1;
        }

        public void DecreaseRekindled()
        {
            if (Rekindled == 0) return;
            Rekindled -= 1;
        }

        public void IncreaseToken()
        {
            if (Tokens < MaxTokens) Tokens++;
        }

        public void DecreaseToken()
        {
            if (Tokens > 0) Tokens--;
        }

        public void AddAramData(ChampionDataAram champ)
        {
            //ChampionName = champ.ChampionName;
            if (champ != null && ChampionID == champ.ChampionID) AramCompleted = champ.AramCompleted;
        }
        public ChampionDataAram GetAramData()
        {
            return new ChampionDataAram(this);
        }
        public void ClickChamp()
        {
            if (AramCompleted == null) AramCompleted = false;
            else if (AramCompleted == false) AramCompleted = true;
            else if (AramCompleted == true) AramCompleted = false;
            OnPropertyChanged("AramCompleted");
        }

        /// <summary>
        /// The large comment containing all the names and IDs has been left simply for viewing purposes as I find it useful to have.
        /// There is NO intention to return to using that switch statement.
        /// </summary>
        /// <param name="champID"></param>
        /// <param name="champNames"></param>
        private void ChampionNameFromID(long champID, DataDragonChampResponse champNames)
        {
            ChampionID = champID;
            string testResult = "";
            champNames.keys.TryGetValue(champID.ToString(), out testResult);
            ChampionName = testResult;
            return; // Just to be sure we're skipping the switch statement if it gets un-commented.
                    // Realistically it should be moved elsewhere and the return removed but this works for now. (Read as 'Likely to remain permanently')

            /*switch (champID)
            {
                case 266: ChampionName = "Aatrox"; break;
                case 103: ChampionName = "Ahri"; break;
                case 84: ChampionName = "Akali"; break;
                case 166: ChampionName = "Akshan"; break;
                case 12: ChampionName = "Alistar"; break;
                case 32: ChampionName = "Amumu"; break;
                case 34: ChampionName = "Anivia"; break;
                case 1: ChampionName = "Annie"; break;
                case 523: ChampionName = "Aphelios"; break;
                case 22: ChampionName = "Ashe"; break;
                case 136: ChampionName = "Aurelion Sol"; ChampionFileName = "AurelionSol"; break;
                case 268: ChampionName = "Azir"; break;
                case 432: ChampionName = "Bard"; break;
                case 53: ChampionName = "Blitzcrank"; break;
                case 63: ChampionName = "Brand"; break;
                case 201: ChampionName = "Braum"; break;
                case 51: ChampionName = "Caitlyn"; break;
                case 164: ChampionName = "Camille"; break;
                case 69: ChampionName = "Cassiopeia"; break;
                case 31: ChampionName = "Cho'Gath"; ChampionFileName = "Chogath"; break;
                case 42: ChampionName = "Corki"; break;
                case 122: ChampionName = "Darius"; break;
                case 131: ChampionName = "Diana"; break;
                case 119: ChampionName = "Draven"; break;
                case 36: ChampionName = "Dr. Mundo"; ChampionFileName = "DrMundo"; break;
                case 245: ChampionName = "Ekko"; break;
                case 60: ChampionName = "Elise"; break;
                case 28: ChampionName = "Evelynn"; break;
                case 81: ChampionName = "Ezreal"; break;
                case 9: ChampionName = "Fiddlesticks"; ChampionFileName = "FiddleSticks"; break;
                case 114: ChampionName = "Fiora"; break;
                case 105: ChampionName = "Fizz"; break;
                case 3: ChampionName = "Galio"; break;
                case 41: ChampionName = "Gangplank"; break;
                case 86: ChampionName = "Garen"; break;
                case 150: ChampionName = "Gnar"; break;
                case 79: ChampionName = "Gragas"; break;
                case 104: ChampionName = "Graves"; break;
                case 887: ChampionName = "Gwen"; break;
                case 120: ChampionName = "Hecarim"; break;
                case 74: ChampionName = "Heimerdinger"; break;
                case 420: ChampionName = "Illaoi"; break;
                case 39: ChampionName = "Irelia"; break;
                case 427: ChampionName = "Ivern"; break;
                case 40: ChampionName = "Janna"; break;
                case 59: ChampionName = "Jarvan IV"; ChampionFileName = "JarvanIV"; break;
                case 24: ChampionName = "Jax"; break;
                case 126: ChampionName = "Jayce"; break;
                case 202: ChampionName = "Jhin"; break;
                case 222: ChampionName = "Jinx"; break;
                case 145: ChampionName = "Kai'Sa"; ChampionFileName = "Kaisa"; break;
                case 429: ChampionName = "Kalista"; break;
                case 43: ChampionName = "Karma"; break;
                case 30: ChampionName = "Karthus"; break;
                case 38: ChampionName = "Kassadin"; break;
                case 55: ChampionName = "Katarina"; break;
                case 10: ChampionName = "Kayle"; break;
                case 141: ChampionName = "Kayn"; break;
                case 85: ChampionName = "Kennen"; break;
                case 121: ChampionName = "Kha'Zix"; ChampionFileName = "Khazix"; break;
                case 203: ChampionName = "Kindred"; break;
                case 240: ChampionName = "Kled"; break;
                case 96: ChampionName = "Kog'Maw"; ChampionFileName = "KogMaw"; break;
                case 7: ChampionName = "LeBlanc"; ChampionFileName = "Leblanc"; break;
                case 64: ChampionName = "Lee Sin"; ChampionFileName = "LeeSin"; break;
                case 89: ChampionName = "Leona"; break;
                case 876: ChampionName = "Lillia"; break;
                case 127: ChampionName = "Lissandra"; break;
                case 236: ChampionName = "Lucian"; break;
                case 117: ChampionName = "Lulu"; break;
                case 99: ChampionName = "Lux"; break;
                case 54: ChampionName = "Malphite"; break;
                case 90: ChampionName = "Malzahar"; break;
                case 57: ChampionName = "Maokai"; break;
                case 11: ChampionName = "Master Yi"; ChampionFileName = "MasterYi"; break;
                case 21: ChampionName = "Miss Fortune"; ChampionFileName = "MissFortune"; break;
                case 62: ChampionName = "Wukong"; ChampionFileName = "MonkeyKing"; break;
                case 82: ChampionName = "Mordekaiser"; break;
                case 25: ChampionName = "Morgana"; break;
                case 267: ChampionName = "Nami"; break;
                case 75: ChampionName = "Nasus"; break;
                case 111: ChampionName = "Nautilus"; break;
                case 518: ChampionName = "Neeko"; break;
                case 76: ChampionName = "Nidalee"; break;
                case 56: ChampionName = "Nocturne"; break;
                case 20: ChampionName = "Nunu & Willump"; ChampionFileName = "Nunu"; break;
                case 2: ChampionName = "Olaf"; break;
                case 61: ChampionName = "Orianna"; break;
                case 516: ChampionName = "Ornn"; break;
                case 80: ChampionName = "Pantheon"; break;
                case 78: ChampionName = "Poppy"; break;
                case 555: ChampionName = "Pyke"; break;
                case 246: ChampionName = "Qiyana"; break;
                case 133: ChampionName = "Quinn"; break;
                case 497: ChampionName = "Rakan"; break;
                case 33: ChampionName = "Rammus"; break;
                case 421: ChampionName = "Rek'Sai"; ChampionFileName = "RekSai"; break;
                case 526: ChampionName = "Rell"; break;
                case 888: ChampionName = "Renata Glasc"; ChampionFileName = "Renata"; break;
                case 58: ChampionName = "Renekton"; break;
                case 107: ChampionName = "Rengar"; break;
                case 92: ChampionName = "Riven"; break;
                case 68: ChampionName = "Rumble"; break;
                case 13: ChampionName = "Ryze"; break;
                case 360: ChampionName = "Samira"; break;
                case 113: ChampionName = "Sejuani"; break;
                case 235: ChampionName = "Senna"; break;
                case 147: ChampionName = "Seraphine"; break;
                case 875: ChampionName = "Sett"; break;
                case 35: ChampionName = "Shaco"; break;
                case 98: ChampionName = "Shen"; break;
                case 102: ChampionName = "Shyvana"; break;
                case 27: ChampionName = "Singed"; break;
                case 14: ChampionName = "Sion"; break;
                case 15: ChampionName = "Sivir"; break;
                case 72: ChampionName = "Skarner"; break;
                case 37: ChampionName = "Sona"; break;
                case 16: ChampionName = "Soraka"; break;
                case 50: ChampionName = "Swain"; break;
                case 517: ChampionName = "Sylas"; break;
                case 134: ChampionName = "Syndra"; break;
                case 223: ChampionName = "Tahm Kench"; ChampionFileName = "TahmKench"; break;
                case 163: ChampionName = "Taliyah"; break;
                case 91: ChampionName = "Talon"; break;
                case 44: ChampionName = "Taric"; break;
                case 17: ChampionName = "Teemo"; break;
                case 412: ChampionName = "Thresh"; break;
                case 18: ChampionName = "Tristana"; break;
                case 48: ChampionName = "Trundle"; break;
                case 23: ChampionName = "Tryndamere"; break;
                case 4: ChampionName = "Twisted Fate"; ChampionFileName = "TwistedFate"; break;
                case 29: ChampionName = "Twitch"; break;
                case 77: ChampionName = "Udyr"; break;
                case 6: ChampionName = "Urgot"; break;
                case 110: ChampionName = "Varus"; break;
                case 67: ChampionName = "Vayne"; break;
                case 45: ChampionName = "Veigar"; break;
                case 161: ChampionName = "Vel'Koz"; ChampionFileName = "Velkoz"; break;
                case 711: ChampionName = "Vex"; break;
                case 254: ChampionName = "Vi"; break;
                case 234: ChampionName = "Viego"; break;
                case 112: ChampionName = "Viktor"; break;
                case 8: ChampionName = "Vladimir"; break;
                case 106: ChampionName = "Volibear"; break;
                case 19: ChampionName = "Warwick"; break;
                case 498: ChampionName = "Xayah"; break;
                case 101: ChampionName = "Xerath"; break;
                case 5: ChampionName = "Xin Zhao"; ChampionFileName = "XinZhao"; break;
                case 157: ChampionName = "Yasuo"; break;
                case 777: ChampionName = "Yone"; break;
                case 83: ChampionName = "Yorick"; break;
                case 350: ChampionName = "Yuumi"; break;
                case 154: ChampionName = "Zac"; break;
                case 238: ChampionName = "Zed"; break;
                case 221: ChampionName = "Zeri"; break;
                case 115: ChampionName = "Ziggs"; break;
                case 26: ChampionName = "Zilean"; break;
                case 142: ChampionName = "Zoe"; break;
                case 143: ChampionName = "Zyra"; break;
                case 200: ChampionName = "Bel'Veth"; ChampionFileName = "Belveth"; break;
                case 895: ChampionName = "Nilah"; break;
                //default: ChampionName = champID.ToString(); NewChamp = true; break;
            }*/
        }
        #endregion
    }
}
