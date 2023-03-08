using Json.Net;
using LoLMasteryTool.DataClasses;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YamlDotNet;
using YamlDotNet.Serialization;

namespace LoLMasteryTool.DataGrab
{
    /// <summary>
    /// This class represents any file-related reading, writing and downloading.
    /// </summary>
    class DataGrab
    {
        public string FilesPath
        {
            get
            {
                return string.Format(@"{0}\LoLMasteryTool\TestOutput.txt", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
        }
        public string DirectoryPath
        {
            get
            {
                return string.Format(@"{0}\LoLMasteryTool\", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
        }
        public string IconsPath
        {
            get
            {
                return string.Format(@"{0}\LoLMasteryTool\ChampionIcons", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            }
        }
        private List<ChampionDataAram> aramChamps { get; set; }

        public DataGrab()
        {
        }

        public List<ChampionData> GetChampionData()
        {
            List<ChampionData> ChampionList = new List<ChampionData>();

            if (!File.Exists(FilesPath)) File.Create(FilesPath);

            string fileContents = File.ReadAllText(FilesPath);
            var deserializer = new DeserializerBuilder().Build();
            ChampionList = deserializer.Deserialize<List<ChampionData>>(fileContents);
            RiotAPILink test = new RiotAPILink();
            test.GetChampionData();
            return ChampionList;
        }

        public bool SaveChampionData(List<ChampionData> championDatas)
        {
            // save them, return false if it doesn't work
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(championDatas);
            File.WriteAllText(FilesPath, yaml);
            return true;
        }

        public List<ChampionDataAram> GetChampionDataAram()
        {
            List<ChampionDataAram> ChampionList = new List<ChampionDataAram>();

            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
            if (!File.Exists(FilesPath)) File.Create(FilesPath).Close();

            string fileContents = File.ReadAllText(FilesPath);
            var deserializer = new DeserializerBuilder().Build();
            ChampionList = deserializer.Deserialize<List<ChampionDataAram>>(fileContents);
            aramChamps = ChampionList;
            return ChampionList;
        }
        public ChampionDataAram GetSpecChampionDataAram(long champID)
        {
            if (aramChamps == null || aramChamps.Count <= 0) return new ChampionDataAram(true);
            try
            {
                return aramChamps.Find(x => x.ChampionID == champID);
            }
            catch (Exception ex)
            {
                return new ChampionDataAram(true);
            }
        }
        public bool SaveChampionDataAram(List<ChampionDataAram> championDatas)
        {
            // save them, return false if it doesn't work
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(championDatas);
            File.WriteAllText(FilesPath, yaml);
            return true;
        }
        public bool IsAramDataGrabbed()
        {
            bool result = false;

            if (aramChamps != null && aramChamps.Count > 0)
            {
                result = true;
            }

            return result;
        }

        public void PopulateChampionImages(List<ChampionData> champs)
        {
            List<string> champNames = new List<string>();

            if (!Directory.Exists(IconsPath)) Directory.CreateDirectory(IconsPath);

            int counter = 0;
            foreach (ChampionData champ in champs)
            {
                if (champ.ChampionIconPath == "")
                {
                    counter++;
                }
            }

            if (counter > 0) DownloadChampImages(champs);
            return;
            //if (Directory.GetFiles(IconsPath).Length == 0) DownloadChampImages(champs);

            foreach (ChampionData champ in champs)
            {
                string[] attempt = Directory.GetFiles(IconsPath);
                foreach (string iconPath in attempt)
                {
                    if (iconPath.ToLower().Contains(champ.ChampionName.ToLower()))
                    {
                        try
                        {
                            ResizeAndSaveAsPng(iconPath, string.Format(@"{0}\{1}.png", IconsPath, champ.ChampionName));
                            //File.Copy(iconPath, string.Format(@"{0}\{1}.png", IconsPath, champ.ChampionName));
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        public void PopulateChampionImages(ChampionData champ)
        {
            List<ChampionData> FakeChamps = new List<ChampionData>();
            FakeChamps.Add(champ);
            PopulateChampionImages(FakeChamps);
        }

        private void DownloadChampImages(List<ChampionData> champNames)
        {
            RiotAPILink API = new RiotAPILink();
            DataDragonChampResponse champInfoResponse = API.GetChampInfo();

            WebClient web = new WebClient();
            foreach(ChampionData champ in champNames.Where(x => x.ChampionIconPath == ""))
            {
                if (champ.NewChamp || champ.ChampionName == "Jeff" || champ.ChampionName == "") continue;

                champInfoResponse.keys.TryGetValue(champ.ChampionID.ToString(), out string champFileName);

                string IconLocation = string.Format(@"https://ddragon.leagueoflegends.com/cdn/{0}/img/champion/{1}.png", API.GetLoLVersion(), champFileName);
                try
                {
                    ResizeAndSaveAsPng(IconLocation, string.Format(@"{0}\{1}.png", IconsPath, champ.ChampionName));
                    //web.DownloadFile(IconLocation, string.Format(@"{0}\{1}.jpg", IconsPath, champ.ChampionName));
                }catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error grabbing icon for {0} (ID: {1}) \n {2}", champ.ChampionName, champ.ChampionID, ex.ToString()));
                }
            }
            web = null;
        }

        private void ResizeAndSaveAsPng(string imagePath, string saveLocation)
        {
            using (WebClient web = new WebClient())
            {
                using (var srcImage = Image.FromStream(web.OpenRead(imagePath)))
                {
                    using (var newImage = new Bitmap(100, 100))
                    using (var graphics = Graphics.FromImage(newImage))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        graphics.DrawImage(srcImage, new Rectangle(0, 0, 100, 100));
                        newImage.Save(saveLocation);
                    }
                }
            }
        }
        /// <summary>
        /// Made redundant through increased usage of APILink and a newly discovered data source called DataDragon that contains the information this previously provided.
        /// </summary>
        /// <param name="champs"></param>
        public void GetChampionNames(List<ChampionData> champs)
        {
            /*
             * Get data from a JSON
             * Create a dictionary using the JSON data
             * replace "ChampionNameFromID" in ChampionData with this new method.
             * Either do it by passing the ID and Dict into the method and swapping contents or by creating a new method and passing only the actual data in
             * Foreach championdata champ in champlist
             *  champion.addData(dictChampId(champ.champID)) ect ect
             *  int champID, string name, string fileName
             *  
             *  public List<ChampionData> GetChampionData()
        {
            List<ChampionData> ChampionList = new List<ChampionData>();

            if (!File.Exists(FilesPath)) File.Create(FilesPath);

            string fileContents = File.ReadAllText(FilesPath);
            var deserializer = new DeserializerBuilder().Build();
            ChampionList = deserializer.Deserialize<List<ChampionData>>(fileContents);
            RiotAPILink test = new RiotAPILink();
            test.GetChampionData();
            return ChampionList;
        }

        public bool SaveChampionData(List<ChampionData> championDatas)
        {
            // save them, return false if it doesn't work
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(championDatas);
            File.WriteAllText(FilesPath, yaml);
            return true;
        }
             */
            List<ChampionDataID> champIDs = new List<ChampionDataID>();
            foreach (ChampionData champ in champs)
            {
                ChampionDataID champID = new ChampionDataID(champ.ChampionID, champ.ChampionName, champ.ChampionFileName);
                champIDs.Add(champID);
            }


            
        }

        public string GetSummonerFromRegistry()
        {
            object SummonerName = "";

            RegistryKey SumNameKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\LoLMasteryTool");
            if (SumNameKey == null) return "";
            SummonerName = SumNameKey.GetValue("DefaultSummoner");

            if (SummonerName != null) return SummonerName.ToString();

            return "";
        }

        public int GetScreenFromRegistry()
        {
            object Screen = -1;

            RegistryKey ScreenKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\LoLMasteryTool");
            if (ScreenKey == null) return -1;
            Screen = ScreenKey.GetValue("DefaultScreen");
            if (Screen != null) return (int)Screen;
            return -1;
        }

        public void SetRegistry(string SummonerNameToSave = "", int DefaultScreenToSave = -1)
        {
            RegistryKey RegSave = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\LoLMasteryTool");
            
            if (SummonerNameToSave != "") RegSave.SetValue("DefaultSummoner", SummonerNameToSave);
            if (DefaultScreenToSave != -1) RegSave.SetValue("DefaultScreen", DefaultScreenToSave);
        }

        public string GetSortByFromRegistry()
        {
            object SortBy = "";

            RegistryKey ScreenKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\LoLMasteryTool");
            if (ScreenKey == null) return "Name";
            SortBy = ScreenKey.GetValue("DefaultSort");

            if (SortBy != null) return SortBy.ToString();

            return "Name";
        }

        public void SetSortByToRegistry(string SortByToSave)
        {
            RegistryKey RegSave = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\LoLMasteryTool");
            if (SortByToSave != null) RegSave.SetValue("DefaultSort", SortByToSave);
        }

        public bool CheckMasteryIconsExist()
        {
            bool IconsExist = false;

            if (File.Exists(String.Format(@"{0}\mast1.png", IconsPath))) IconsExist = true;
            return IconsExist;
        }

        public void CopyMasteryIcons()
        {
            for (int i = 1; i < 8; i++)
            {
                string masteryLocation = string.Format(@"{0}\ChampionIcons\mast{1}.png", Directory.GetCurrentDirectory(), i);
                if (File.Exists(masteryLocation))
                {
                    string masteryCopyLocation = String.Format(@"{0}\mast{1}.png", IconsPath, i);
                    if (!Directory.Exists(IconsPath)) Directory.CreateDirectory(IconsPath);
                    if (!File.Exists(masteryCopyLocation)) File.Copy(masteryLocation, masteryCopyLocation);
                }
            }
        }
    }
}
