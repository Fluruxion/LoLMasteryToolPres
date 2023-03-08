using LoLMasteryTool.DataClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoLMasteryTool
{
    class TextSummaryViewModel : ObservableObject
    {
        private List<string> m_OutputOptions { get; set; }
        public List<string> OutputOptions
        {
            get
            {
                return m_OutputOptions;
            }
            set
            {
                m_OutputOptions = value;
                OnPropertyChanged("OutputOptions");
            }
        }
        private string m_OutputSelected { get; set; }
        public string OutputSelected
        {
            get
            {
                return m_OutputSelected;
            }
            set
            {
                m_OutputSelected = value;
                OnPropertyChanged("OutputSelected");
                GenerateOutput();
            }
        }
        private string m_TextOutput { get; set; }
        public string TextOutput
        {
            get
            {
                return m_TextOutput;
            }
            set
            {
                m_TextOutput = value;
                OnPropertyChanged("TextOutput");
            }
        }
        private List<ChampionData> Champions { get; set; }
        private int Mastery7s { get; set; }
        private int Mastery6s { get; set; }
        private int Mastery5s { get; set; }
        private int Mastery4s { get; set; }
        private int Mastery3s { get; set; }
        private int Mastery2s { get; set; }
        private int Mastery1s { get; set; }
        private int Mastery0s { get; set; }
        private int MasteryTop { get; set; }
        private int MasteryJungle { get; set; }
        private int MasteryMid { get; set; }
        private int MasteryADC { get; set; }
        private int MasterySupport { get; set; }



        public TextSummaryViewModel(List<ChampionData> championDatas)
        {
            // Get the list of ChampionDatas
            Champions = championDatas;

            // Populate the selection list // Add more if neccessary
            m_OutputOptions = new List<string>();
            m_OutputOptions.Add("Print Mastery Quants by mastery");
            //m_OutputOptions.Add("Print Mastery Quants by role");
            m_OutputOptions.Add("All Mastery 7s");
            m_OutputOptions.Add("All Mastery 6s");
            m_OutputOptions.Add("All Mastery 5s");
            m_OutputOptions.Add("All Mastery 4s");
            m_OutputOptions.Add("All Mastery 3s");
            m_OutputOptions.Add("All Mastery 2s");
            m_OutputOptions.Add("All Mastery 1s");
            m_OutputOptions.Add("All Mastery 0s");
            //m_OutputOptions.Add("All Top Champ Masteries");
            //m_OutputOptions.Add("All Jungle Champ Masteries");
            //m_OutputOptions.Add("All Mid Champ Masteries");
            //m_OutputOptions.Add("All ADC Champ Masteries");
            //m_OutputOptions.Add("All Support Champ Masteries");

            // Assign mastery totals
            Mastery7s = Champions.Count(p => p.MasteryScore == 7);
            Mastery6s = Champions.Count(p => p.MasteryScore == 6);
            Mastery5s = Champions.Count(p => p.MasteryScore == 5);
            Mastery4s = Champions.Count(p => p.MasteryScore == 4);
            Mastery3s = Champions.Count(p => p.MasteryScore == 3);
            Mastery2s = Champions.Count(p => p.MasteryScore == 2);
            Mastery1s = Champions.Count(p => p.MasteryScore == 1);
            Mastery0s = Champions.Count(p => p.MasteryScore == 0);

            /*MasteryTop = Champions.Count(p => p.Roles.CheckForMatches(new Positions("top")));
            MasteryJungle = Champions.Count(p => p.Roles.CheckForMatches(new Positions("jungle")));
            MasteryMid = Champions.Count(p => p.Roles.CheckForMatches(new Positions("mid")));
            MasteryADC = Champions.Count(p => p.Roles.CheckForMatches(new Positions("adc")));
            MasterySupport = Champions.Count(p => p.Roles.CheckForMatches(new Positions("support")));*/
        }

        public void GenerateOutput()
        {
            switch(OutputSelected)
            {
                case "Print Mastery Quants by mastery": TextOutput = MasteryQBM(); break;
                //case "Print Mastery Quants by role": TextOutput = MasteryQBR(); break;
                case "All Mastery 7s": TextOutput = ChampsWithMastery(7); break;
                case "All Mastery 6s": TextOutput = ChampsWithMastery(6); break;
                case "All Mastery 5s": TextOutput = ChampsWithMastery(5); break;
                case "All Mastery 4s": TextOutput = ChampsWithMastery(4); break;
                case "All Mastery 3s": TextOutput = ChampsWithMastery(3); break;
                case "All Mastery 2s": TextOutput = ChampsWithMastery(2); break;
                case "All Mastery 1s": TextOutput = ChampsWithMastery(1); break;
                case "All Mastery 0s": TextOutput = ChampsWithMastery(0); break;
                //case "All Top Champ Masteries": TextOutput = ChampMasteriesInRole("top"); break;
                //case "All Jungle Champ Masteries": TextOutput = ChampMasteriesInRole("jungle"); break;
                //case "All Mid Champ Masteries": TextOutput = ChampMasteriesInRole("mid"); break;
                //case "All ADC Champ Masteries": TextOutput = ChampMasteriesInRole("adc"); break;
                //case "All Support Champ Masteries": TextOutput = ChampMasteriesInRole("support"); break;
            }
            //TextOutput = GenerateUninstallFileOutput();
            Clipboard.SetText(TextOutput);
        }

        private string GenerateInstallFileOutput()
        {
            string filesPath = @"C:\Users\alex\Documents\GitHub\LoLMasteryTool\LoLMasteryTool\LoLMasteryTool\bin\Release";
            string allFilesOutput = "";
            foreach (string filePath in Directory.GetFiles(filesPath))
            {
                allFilesOutput += string.Format("\tfile \"bin\\Release\\{0}\"\n", Path.GetFileName(filePath));
            }
            return allFilesOutput;
        }

        private string GenerateUninstallFileOutput()
        {
            string filesPath = @"C:\Users\alex\Documents\GitHub\LoLMasteryTool\LoLMasteryTool\LoLMasteryTool\bin\Release";
            string allFilesOutput = "";
            foreach (string filePath in Directory.GetFiles(filesPath))
            {
                allFilesOutput += string.Format("\tdelete $INSTDIR\\{0}\n", Path.GetFileName(filePath));
            }
            return allFilesOutput;
        }

        private string MasteryQBM()
        {
            return String.Format("Mastery quantities sorted by mastery level:\nMastery 7: {0}\nMastery 6: {1}\nMastery 5: {2}\nMastery 4: {3}\nMastery 3: {4}\nMastery 2: {5}\nMastery 1: {6}\nMastery 0: {7}", Mastery7s, Mastery6s, Mastery5s, Mastery4s, Mastery3s, Mastery2s, Mastery1s, Mastery0s);
        }
        private string MasteryQBR()
        {
            return String.Format("Mastery quantities sorted by role:\nTop: {0}\nJungle: {1}\nMid: {2}\nAdc: {3}\nSupport: {4}", AllChampsMasteryQuantInRole("top"), AllChampsMasteryQuantInRole("jungle"), AllChampsMasteryQuantInRole("mid"), AllChampsMasteryQuantInRole("adc"), AllChampsMasteryQuantInRole("support"));
        }
        private string ChampsWithMastery(int mastery)
        {
            if (Champions.Count(p => p.MasteryScore == mastery) == 0) return string.Format("Zero Champions with mastery {0}", mastery);
            
            string returnString = string.Format("Champions with mastery score {0}:\n", mastery);

            foreach (ChampionData champ in Champions.FindAll(p => p.MasteryScore == mastery))
            {
                returnString += string.Format("{0}\n", champ.ChampionName);
            }

            return returnString;
        }
        private string ChampMasteriesInRole(string role)
        {
            if (Champions.Count(p => p.Roles.CheckForMatches(new Positions(role))) == 0) return string.Format("Zero Champions in {0}", role);

            List<ChampionData> sortedChampions = Champions.OrderByDescending(p => p.MasteryScore).ToList();

            string returnString = string.Format("Champion masteries in {0}:\n", role);

            foreach (ChampionData champ in sortedChampions.FindAll(p => p.Roles.CheckForMatches(new Positions(role))))
            {
                returnString += string.Format("{0}:  {1}\n", champ.ChampionName, champ.MasteryScore);
            }

            return returnString;
        }
        private string ChampsWithMasteryAndRole(int mastery, string role, List<ChampionData> championsInRole = null)
        {
            if (championsInRole == null) championsInRole = Champions.FindAll(p => p.Roles.CheckForMatches(new Positions(role)));

            return string.Format("\n\tMastery {0}:  {1}", mastery, championsInRole.Count(p => p.MasteryScore == mastery));
        }
        private string AllChampsMasteryQuantInRole(string role)
        {
            List<ChampionData> championsInRole = Champions.FindAll(p => p.Roles.CheckForMatches(new Positions(role)));

            string returnString = "";
            List<int> masteries = new List<int>{ 7, 6, 5, 4, 3, 2, 1, 0 };
            foreach (int mastery in masteries)
            {
                returnString += ChampsWithMasteryAndRole(mastery, role, championsInRole);
            }
            return returnString;
        }
    }
}
