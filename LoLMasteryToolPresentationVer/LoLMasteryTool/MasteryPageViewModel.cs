using LoLMasteryTool.DataClasses;
using LoLMasteryTool.DataGrab;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LoLMasteryTool

{
    class MasteryPageViewModel : ObservableObject
    {
        #region Properties

        private List<ChampionData> m_championList { get; set; }
        /// <summary>
        /// SaveData is used for any data saved to or loaded from the computer or downloaded from online as a file.
        /// </summary>
        private DataGrab.DataGrab SaveData { get; set; }
        /// <summary>
        /// OnlineData is the API link, this gets the actual information on champions and such.
        /// </summary>
        private RiotAPILink OnlineData { get; set; }
        public List<ChampionData> ChampionList
        {
            get
            {
                return m_championList;
            }
            set
            {
                m_championList = value;
                ChampListFiltered = value;
                OnPropertyChanged("ChampionList");
                OnPropertyChanged("ChampListFiltered");
            }
        }
        private string m_IconPath { get; set; }
        public string IconPath
        {
            get
            {
                if (m_IconPath != null && File.Exists(m_IconPath)) return m_IconPath;
                return "";
            }
            set
            {
                m_IconPath = value;
            }
        }
        private List<ChampionData> m_ChampListFiltered { get; set; }
        /// <summary>
        /// This is the list used for displaying on the screen, the rest are manipulated to present data as desired or keep a persistent & true data source to revert to.
        /// </summary>
        public List<ChampionData> ChampListFiltered
        {
            get
            {
                return m_ChampListFiltered;
            }
            set
            {
                m_ChampListFiltered = value;
                OnPropertyChanged("ChampListFiltered");
            }
        }

        /// <summary>
        /// All references to role were removed when the program moved to using the APILink as it was previously populated when entering champions manually, the API handles this differently.
        /// </summary>
        /*private List<string> roleFilter { get; set; }
        public List<string> RoleFilter
        {
            get
            {
                return roleFilter;
            }
            set
            {
                roleFilter = value;
                OnPropertyChanged("RoleFilter");
            }
        }*/
        private List<string> m_MasteryFilter { get; set; }
        public List<string> MasteryFilter
        {
            get
            {
                return m_MasteryFilter;
            }
            set
            {
                m_MasteryFilter = value;
                OnPropertyChanged("MasteryFilter");
            }
        }
        private List<string> m_TokenFilter { get; set; }
        public List<string> TokenFilter
        {
            get
            {
                return m_TokenFilter;
            }
            set
            {
                m_TokenFilter = value;
                OnPropertyChanged("TokenFilter");
            }
        }
        /*private string roleSelected { get; set; }
        public string RoleSelected
        {
            get
            {
                return roleSelected;
            }
            set
            {
                roleSelected = value;
                Refresh();
                OnPropertyChanged("RoleSelected");
            }
        }*/
        private string m_MasterySelected { get; set; }
        public string MasterySelected
        {
            get
            {
                return m_MasterySelected;
            }
            set
            {
                m_MasterySelected = value;
                OnPropertyChanged("MasterySelected");
            }
        }
        public string MasterySelectedWithRefresh
        {
            get
            {
                return m_MasterySelected;
            }
            set
            {
                m_MasterySelected = value;
                Refresh();
                OnPropertyChanged("MasterySelected");
            }
        }
        private string m_TokenSelected { get; set; }
        public string TokenSelected
        {
            get
            {
                return m_TokenSelected;
            }
            set
            {
                m_TokenSelected = value;
                OnPropertyChanged("TokenSelected");
            }
        }
        public string TokenSelectedWithRefresh
        {
            get
            {
                return m_TokenSelected;
            }
            set
            {
                m_TokenSelected = value;
                Refresh();
                OnPropertyChanged("TokenSelected");
            }
        }

        private string m_SearchChampNameString { get; set; }
        public string SearchChampNameString
        {
            get
            {
                return m_SearchChampNameString;
            }
            set
            {
                m_SearchChampNameString = value;
                OnPropertyChanged("SearchChampNameString");
            }
        }
        private List<string> m_SortByList { get; set; }
        public List<string> SortByList
        {
            get
            {
                return m_SortByList;
            }
            set
            {
                m_SortByList = value;
                OnPropertyChanged("SortByList");
            }
        }
        private string m_SortBySelected { get; set; }
        public string SortBySelected
        {
            get
            {
                return m_SortBySelected;
            }
            set
            {
                m_SortBySelected = value;
                OnPropertyChanged("SortBySelected");
                Refresh();
            }
        }
        private int m_AramChampsCompleted { get; set; }

        public int AramChampsCompleted
        {
            get
            {
                return m_AramChampsCompleted;
            }
            set
            {
                m_AramChampsCompleted = value;
            }
        }

        public string AramChampsCompletedString
        {
            get
            {
                return String.Format("{0}/{1}", AramChampsCompleted, ChampionList.Count);
            }
        }
        SummonerSearch summonerSearch { get; set; }
        SummonerSearchViewModel searchData { get; set; }
        // The following ICommands handle all interaction between the backend and the UI
        private ICommand m_CommandIncreaseMastery { get; set; }
        private ICommand m_CommandDecreaseMastery { get; set; }
        private ICommand m_CommandIncreaseToken { get; set; }
        private ICommand m_CommandDecreaseToken { get; set; }
        private ICommand m_CommandRefresh { get; set; }
        private ICommand m_CommandClose { get; set; }
        private ICommand m_CommandDeleteChampion { get; set; }
        private ICommand m_CommandFilterChampionList { get; set; }
        private ICommand m_CommandCreateChampion { get; set; }
        private ICommand m_CommandLaunchAddChampionScreen { get; set; }
        private ICommand m_CommandLoadIcons { get; set; }
        private ICommand m_CommandSearchChampName { get; set; }
        private ICommand m_CommandDecreaseRekindle { get; set; }
        private ICommand m_CommandIncreaseRekindle { get; set; }
        private ICommand m_CommandTextSummary { get; set; }
        private ICommand m_SearchSummoner { get; set; }
        private ICommand m_LaunchSummonerSearchScreenCommand { get; set; }
        private ICommand m_CommandSwap { get; set; }
        private ICommand m_ChampClicked { get; set; }
        private ICommand m_CommandLaunchConfig { get; set; }
        public ICommand LaunchSummonerSearchScreenCommand
        {
            get
            {
                if (m_LaunchSummonerSearchScreenCommand == null)
                {
                    m_LaunchSummonerSearchScreenCommand = new DelegateCommand(p => LaunchSummonerSearchScreen());
                }

                return m_LaunchSummonerSearchScreenCommand;
            }
        }
        public ICommand SearchSummoner
        {
            get
            {
                if (m_SearchSummoner == null)
                {
                    m_SearchSummoner = new DelegateCommand<string>(p => GetSummoner(p));
                }

                return m_SearchSummoner;
            }
        }
        public ICommand CommandRefresh
        {
            get
            {
                if (m_CommandRefresh == null)
                {
                    m_CommandRefresh = new DelegateCommand(p => Update());
                }

                return m_CommandRefresh;
            }
        }
        public ICommand CommandIncreaseMastery
        {
            get
            {
                if (m_CommandIncreaseMastery == null)
                {
                    m_CommandIncreaseMastery = new DelegateCommand<ChampionData>(p => IncreaseMastery(p));
                }

                return m_CommandIncreaseMastery;
            }
        }
        public ICommand CommandDecreaseMastery
        {
            get
            {
                if (m_CommandDecreaseMastery == null)
                {
                    m_CommandDecreaseMastery = new DelegateCommand<ChampionData>(p => DecreaseMastery(p));
                }

                return m_CommandDecreaseMastery;
            }
        }
        public ICommand CommandIncreaseToken
        {
            get
            {
                if (m_CommandIncreaseToken == null)
                {
                    m_CommandIncreaseToken = new DelegateCommand<ChampionData>(p => IncreaseToken(p));
                }

                return m_CommandIncreaseToken;
            }
        }
        public ICommand CommandDecreaseToken
        {
            get
            {
                if (m_CommandDecreaseToken == null)
                {
                    m_CommandDecreaseToken = new DelegateCommand<ChampionData>(p => DecreaseToken(p));
                }

                return m_CommandDecreaseToken;
            }
        }
        public ICommand CommandClose
        {
            get
            {
                if (m_CommandClose == null)
                {
                    m_CommandClose = new DelegateCommand(p => InterruptClosing());
                }

                return m_CommandClose;
            }
        }

        public ICommand CommandDeleteChampion
        {
            get
            {
                if (m_CommandDeleteChampion == null)
                {
                    m_CommandDeleteChampion = new DelegateCommand<ChampionData>(p => DeleteChampion(p));
                }

                return m_CommandDeleteChampion;
            }
        }

        public ICommand CommandFilterChampionList
        {
            get
            {
                if (m_CommandFilterChampionList == null)
                {
                    m_CommandFilterChampionList = new DelegateCommand(p => Refresh());
                }

                return m_CommandFilterChampionList;
            }
        }

        public ICommand CommandCreateChampion
        {
            get
            {
                if (m_CommandCreateChampion == null)
                {
                    m_CommandCreateChampion = new DelegateCommand<ChampionData>(p => CreateChampion(p));
                }

                return m_CommandCreateChampion;
            }
        }

        public ICommand CommandLaunchAddChampionScreen
        {
            get
            {
                if (m_CommandLaunchAddChampionScreen == null)
                {
                    m_CommandLaunchAddChampionScreen = new DelegateCommand(p => LaunchAddChampionScreen());
                }

                return m_CommandLaunchAddChampionScreen;
            }
        }
        public ICommand CommandLoadIcons
        {
            get
            {
                if (m_CommandLoadIcons == null)
                {
                    m_CommandLoadIcons = new DelegateCommand(p => RefreshIcons());
                }

                return m_CommandLoadIcons;
            }
        }
        public ICommand CommandSearchChampName
        {
            get
            {
                if (m_CommandSearchChampName == null)
                {
                    m_CommandSearchChampName = new DelegateCommand(p => SearchChamp());
                }

                return m_CommandSearchChampName;
            }
        }
        public ICommand CommandIncreaseRekindle
        {
            get
            {
                if (m_CommandIncreaseRekindle == null)
                {
                    m_CommandIncreaseRekindle = new DelegateCommand<ChampionData>(p => IncreaseRekindle(p));
                }

                return m_CommandIncreaseRekindle;
            }
        }
        public ICommand CommandDecreaseRekindle
        {
            get
            {
                if (m_CommandDecreaseRekindle == null)
                {
                    m_CommandDecreaseRekindle = new DelegateCommand<ChampionData>(p => DecreaseRekindle(p));
                }

                return m_CommandDecreaseRekindle;
            }
        }
        public ICommand CommandTextSummary
        {
            get
            {
                if (m_CommandTextSummary == null)
                {
                    m_CommandTextSummary = new DelegateCommand(p => LaunchTextOutputScreen());
                }

                return m_CommandTextSummary;
            }
        }
        public ICommand CommandSwap
        {
            get
            {
                if (m_CommandSwap == null)
                {
                    m_CommandSwap = new DelegateCommand(p => SwapScreens());
                }

                return m_CommandSwap;
            }
        }
        public ICommand ChampClicked
        {
            get
            {
                if (m_ChampClicked == null)
                {
                    m_ChampClicked = new DelegateCommand<long>(p => ClickChamp(p));
                }

                return m_ChampClicked;
            }
        }
        public ICommand CommandLaunchConfig
        {
            get
            {
                if (m_CommandLaunchConfig == null)
                {
                    m_CommandLaunchConfig = new DelegateCommand(p => LaunchConfig());
                }

                return m_CommandLaunchConfig;
            }
        }

        private int m_ItemWidth { get; set; }
        private int m_ItemHeight { get; set; }
        private int m_gridBackWidth { get; set; }
        private int m_gridBackHeight { get; set; }
        private int m_colDefWidth1 { get; set; }
        private int m_colDefWidth2 { get; set; }
        private int m_colDefWidth3 { get; set; }
        private int m_champNameSize { get; set; }
        private int m_currentScreen { get; set; }  

        public int ItemWidth {
            get
            {
                return m_ItemWidth;
            }
            set
            {
                m_ItemWidth = value;
            }
        }
        public int ItemHeight
        {
            get
            {
                return m_ItemHeight;
            }
            set
            {
                m_ItemHeight = value;
            }
        }
        public int gridBackWidth
        {
            get
            {
                return m_gridBackWidth;
            }
            set
            {
                m_gridBackWidth = value;
            }
        }
        public int gridBackHeight
        {
            get
            {
                return m_gridBackHeight;
            }
            set
            {
                m_gridBackHeight = value;
            }
        }
        public int colDefWidth1
        {
            get
            {
                return m_colDefWidth1;
            }
            set
            {
                m_colDefWidth1 = value;
            }
        }
        public int colDefWidth2
        {
            get
            {
                return m_colDefWidth2;
            }
            set
            {
                m_colDefWidth2 = value;
            }
        }
        public int colDefWidth3
        {
            get
            {
                return m_colDefWidth3;
            }
            set
            {
                m_colDefWidth3 = value;
            }
        }
        public int champNameSize
        {
            get
            {
                return m_champNameSize;
            }
            set
            {
                m_champNameSize = value;
            }
        }
        public int currentScreen
        {
            get
            {
                return m_currentScreen;
            }
            set
            {
                m_currentScreen = value;
            }
        }
        public string SwapScreenText
        {
            get
            {
                switch (currentScreen)
                {
                    case 1: return "ARAM Screen"; break;
                    case 2: return "Mastery Screen"; break;
                    default: return "Swap Screen"; break;
                }
            }
        }
        #endregion

        #region Constructors
        public MasteryPageViewModel(string StartupSummonerName, int StartupScreen)
        {
            SharedConstructorCode(false);
            GetSummoner(StartupSummonerName);
            SetScreen(StartupScreen);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Originally moved to a seperate function due to the existence of more than one constructor to handle manually entered saveData during original implementation.
        /// Kept it seperate in case another constructor is required in the future.
        /// </summary>
        /// <param name="getChampData">Defaults to true, when true will use the APILink to get data. Added in case this code has to run but we already have data.</param>
        private void SharedConstructorCode(bool getChampData = true)
        {
            summonerSearch = new SummonerSearch();
            searchData = new SummonerSearchViewModel(SearchSummoner);
            ChampionList = new List<ChampionData>();
            SaveData = new DataGrab.DataGrab();
            OnlineData = new RiotAPILink();
            MasteryFilter = new List<string>
            {
                "ALL","7","6","5","4","3","2","1","0"
            };
            TokenFilter = new List<string>
            {
                "ALL","3","2","1"
            };
            MasterySelected = "ALL";
            TokenSelected = "ALL";
            SearchChampNameString = "";
            SortByList = new List<string>();
            SortByList.Add("Name");
            SortByList.Add("Mastery");
            SortByList.Add("Remaining");
            SortByList.Add("Tokens Left");
            SortBySelected = GetSortByDefault();
            CheckIcons();
            if (getChampData) ChampionList = OnlineData.GetChampionData(SaveData);
        }

        /// <summary>
        /// Refreshes the screen, applies any filters and searches as required before repopulating the list of champions.
        /// </summary>
        /// <param name="ignoreCheck">If true then do not search for a champion regardless of the search bar contents</param>
        public void Refresh(bool ignoreCheck = false)
        {
            if (m_championList == null) return;

            ChampListFiltered = ChampionList;

            if (!ignoreCheck && SearchChampNameString != "")
            {
                SearchChamp();
            }

            switch(SortBySelected)
            {
                case "Name": ChampListFiltered = ChampListFiltered.OrderBy(p => p.ChampionName).ToList(); break;
                case "Mastery": ChampListFiltered = ChampListFiltered.OrderByDescending(p => p.MasteryScore).ThenByDescending(p => p.MasteryPoints).ToList(); break;
                case "Remaining": ChampListFiltered = ChampListFiltered.OrderBy(p => p.PointsRemaining).Where(p => p.MasteryScore < 5).ToList(); break;
                case "Tokens Left": ChampListFiltered = ChampListFiltered.OrderBy(p => p.TokensLeft).Where(p => p.MasteryScore == 5 || p.MasteryScore == 6).ToList(); break;
            }
            SaveSortByDefault();

            if (MasterySelected != "ALL" && MasterySelected != null)
            {
                List<ChampionData> temp = ChampListFiltered.FindAll(p => p.MasteryScore == Int32.Parse(MasterySelected));
                ChampListFiltered = temp;
            }
            if (TokenSelected != "ALL" && TokenSelected != null)
            {
                List<ChampionData> temp = ChampListFiltered.FindAll(p => p.TokensLeft == Int32.Parse(m_TokenSelected));
                ChampListFiltered = temp;
            }

            OnPropertyChanged("ChampionList");
        }
        public void RefreshIcons()
        {
            SaveData.PopulateChampionImages(ChampionList);
        }
        public void LoadChampData()
        {
            ChampionList = SaveData.GetChampionData();
        }

        public void SaveAllChampData()
        {
            SaveData.SaveChampionData(ChampionList);
        }

        public void IncreaseMastery(ChampionData champion)
        {
            champion.IncreaseMastery();
            Refresh();
        }
        public void DecreaseMastery(ChampionData champion)
        {
            champion.DecreaseMastery();
            Refresh();
        }
        public void IncreaseToken(ChampionData champion)
        {
            champion.IncreaseToken();
            Refresh();
        }
        public void DecreaseToken(ChampionData champion)
        {
            champion.DecreaseToken();
            Refresh();
        }
        public void IncreaseRekindle(ChampionData champion)
        {
            champion.IncreaseRekindled();
            Refresh();
        }
        public void DecreaseRekindle(ChampionData champion)
        {
            champion.DecreaseRekindled();
            Refresh();
        }
        public void InterruptClosing()
        {
            SaveData.SaveChampionData(ChampionList);
        }

        public void DeleteChampion(ChampionData champ)
        {
            ChampionList.Remove(champ);
            SaveData.SaveChampionData(ChampionList);
            Refresh();
        }
        /// <summary>
        /// Sort the champions alphabetically.
        /// </summary>
        public void SortChampionList()
        {
            foreach (ChampionData champ in ChampionList)
            {
                if (champ == null) return;
            }
            List<ChampionData> temp = m_championList.OrderBy(p => p.ChampionName).ToList();
            m_championList = temp;
        }
        /// <summary>
        /// This was the original implementation to allow the user to add champion data to their tool, this was done through manual entry. The ICommand was typically passed into another ViewModel.
        /// </summary>
        /// <param name="champion"></param>
        public void CreateChampion(ChampionData champion)
        {
            if (champion == null) return;
            if (m_championList == null) m_championList = new List<ChampionData>();
            ChampionList.Add(champion);
            SortChampionList();
            SaveAllChampData();
            Refresh();
        }
        /// <summary>
        /// Same as above.
        /// </summary>
        public void LaunchAddChampionScreen()
        {
            AddChampionScreen addChampionScreen = new AddChampionScreen();
            AddChampionScreenViewModel championContext = new AddChampionScreenViewModel(CommandCreateChampion);
            addChampionScreen.DataContext = championContext;
            addChampionScreen.Show();
        }
        /// <summary>
        /// Search for a specified champion within the list.
        /// </summary>
        public void SearchChamp()
        {
            if (SearchChampNameString == "")
            {
                ChampListFiltered = ChampionList;
                Refresh();
                return;
            }
            ChampListFiltered = ChampionList;
            List<ChampionData> temp = ChampListFiltered.FindAll(p => p.ChampionName.ToLower().Contains(SearchChampNameString.ToLower()));
            ChampListFiltered = temp;
        }
        /// <summary>
        /// This launches a screen that lets the user see summaries of their data, like the amount of 'Mastery 7' champions they have.
        /// </summary>
        public void LaunchTextOutputScreen()
        {
            TextSummaryView textSummaryView = new TextSummaryView();
            List<ChampionData> champs = new List<ChampionData>();
            if (m_championList != null && m_ChampListFiltered != null && m_championList.Count>m_ChampListFiltered.Count)
            {
                champs = m_championList;
            }
            else
            {
                if (m_ChampListFiltered != null)
                {
                    champs = m_ChampListFiltered;
                }
                else
                {
                    MessageBox.Show("No champion data!");
                    return;
                }
            }

            TextSummaryViewModel textSummaryViewModel = new TextSummaryViewModel(m_championList.OrderBy(p => p.ChampionName).ToList());
            textSummaryView.DataContext = textSummaryViewModel;
            textSummaryView.Show();
        }
        /// <summary>
        /// Allows the user to check the mastery scores of another account.
        /// This simply opens the screen that the user interacts with to search for the account name.
        /// The ICommand passed into the new ViewModel points at GetSummoner(string summonerName)
        /// </summary>
        public void LaunchSummonerSearchScreen()
        {
            summonerSearch = new SummonerSearch();
            searchData = new SummonerSearchViewModel(SearchSummoner);
            summonerSearch.DataContext = searchData;
            summonerSearch.Show();
        }
        /// <summary>
        /// PENDING REMOVAL
        /// Assumedly redundant way of calling the above method with a wrapper ontop.
        /// </summary>
        /// <param name="summonerName"></param>
        public void GetNewSummonerChampList(string summonerName)
        {
            if (!GetSummoner(summonerName)) return;
            UpdateChampionList();
        }
        /// <summary>
        /// This is the method that looks up the account name on the API, then uses the API again to get data associated with the account.
        /// This is called by the SearchSummoner ICommand used in LaunchSummonerSearchScreen.
        /// </summary>
        /// <param name="summonerName"></param>
        /// <returns></returns>
        public bool GetSummoner(string summonerName)
        {
            if (summonerName == null || summonerName == "") return false;

            if (!OnlineData.GetSummonerID(summonerName))
            {
                MessageBox.Show("Invalid summoner name. Please try again.");
                return false;
            }
            //IconPath = string.Format(@"{0}\LoLMasteryTool\PlayerIcon.png", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            //OnPropertyChanged("IconPath");
            ChampionList = OnlineData.GetChampionData();
            summonerSearch = null;
            searchData = null;

            Refresh();

            return true;
        }
        /// <summary>
        /// PENDING REMOVAL
        /// Assumedly redundant method of updating champion list, called by another redundant method. Likely leftover from the manual-entry version.
        /// </summary>
        public void UpdateChampionList()
        {
            ChampionList = OnlineData.GetChampionData(SaveData);
            //summonerSearch.Close();
            summonerSearch = null;
            searchData = null;
            Refresh();
        }
        /// <summary>
        /// Method used by an ICommand that the UI points at to update the list of champions.
        /// (So the user can see changes in mastery after finishing a game without restarting the application)
        /// </summary>
        public void Update()
        {
            ChampionList = OnlineData.GetChampionData();
            Refresh();
            GetAramDataForChamps(true);
        }

        /// <summary>
        /// Method used to set values that control UI sizing and position.
        /// This makes the ChampionData items within the listview take up more space and show more information and represents the MasteryViewMode.
        /// </summary>
        public void MasteryMode()
        {
            ItemWidth = 350;
            ItemHeight = 300;
            gridBackWidth = 200;
            gridBackHeight = 220;
            colDefWidth1 = 50;
            colDefWidth2 = 100;
            colDefWidth3 = 50;
            champNameSize = 1;
            currentScreen = 1;

        }
        /// <summary>
        /// Method used to set values that control UI sizing and position.
        /// This makes the ChampionData items within the listview take up less space and show no information, it represents the ARAMViewMode.
        /// </summary>
        public void ARAMMode()
        {
            ItemWidth = 150;
            ItemHeight = 150;
            gridBackWidth = 100;
            gridBackHeight = 140;
            colDefWidth1 = 0;
            colDefWidth2 = 100;
            colDefWidth3 = 0;
            champNameSize = 2;
            currentScreen = 2;
        }
        /// <summary>
        /// Used when starting the application to set the screen to begin with. All future screen changes need only care about swapping to the opposite as there are only two options.
        /// </summary>
        /// <param name="screen"></param>
        public void SetScreen(int screen)
        {
            GetAramDataForChamps();

            switch (screen)
            {
                case 1: MasteryMode(); break;
                case 2: ARAMMode(); break;
                default:
                    MasteryMode();
#if DEBUG
                    MessageBox.Show("Error selecting screen");
#endif                    
                    break;
            }

            OnPropertyChanged("ItemWidth");
            OnPropertyChanged("ItemHeight");
            OnPropertyChanged("gridBackWidth");
            OnPropertyChanged("gridBackHeight");
            OnPropertyChanged("colDefWidth1");
            OnPropertyChanged("colDefWidth2");
            OnPropertyChanged("colDefWidth3");
            OnPropertyChanged("champNameSize");
            OnPropertyChanged("currentScreen");
            OnPropertyChanged("ChampListFiltered");
        }
        /// <summary>
        /// Used to change which screen / viewmode is currently being used during standard usage as opposed to on launch. As there are only two screens this will simply swap to the other option.
        /// </summary>
        public void SwapScreens()
        {
            // Will require extra work if a 3rd screen is added. Possible new screen or dropdown box selector. Would be worth overhauling UI as a whole if this occurs anyway.
            switch (currentScreen)
            {
                case 1: ARAMMode(); break;
                case 2: MasteryMode(); break;
                default:
                    MasteryMode();
#if DEBUG
                    MessageBox.Show("Error selecting screen");
#endif
                    break;
            }
            OnPropertyChanged("ItemWidth");
            OnPropertyChanged("ItemHeight");
            OnPropertyChanged("gridBackWidth");
            OnPropertyChanged("gridBackHeight");
            OnPropertyChanged("colDefWidth1");
            OnPropertyChanged("colDefWidth2");
            OnPropertyChanged("colDefWidth3");
            OnPropertyChanged("champNameSize");
            OnPropertyChanged("currentScreen");
            OnPropertyChanged("ChampListFiltered");
            OnPropertyChanged("SwapScreenText");
        }
        /// <summary>
        /// This is part of the ARAMViewMode behaviour.
        /// Clicking a ChampionData item should cause it to swap states between being "complete" and "incomplete" for the sake of tracking progress by the user.
        /// This should only occur during ARAMViewMode, though that is handled within the View.xaml code.
        /// Possible future change: investigate looking into passing SaveData into ChampionData and making this a method of ChampionData instead. - Removes need for champID param.
        /// </summary>
        /// <param name="champID">The ID of the champion that has been clicked.</param>
        public void ClickChamp(long champID)
        {
            ChampListFiltered.Find(x => x.ChampionID == champID).ClickChamp();
            List<ChampionDataAram> AramChampList = new List<ChampionDataAram>();
            AramChampsCompleted = 0;
            foreach (ChampionData champ in ChampionList)
            {
                AramChampList.Add(champ.GetAramData());
                if (champ.AramCompleted == true) AramChampsCompleted++;
            }
            SaveData.SaveChampionDataAram(AramChampList);
            //ChampListFiltered = new List<ChampionData>(ChampListFiltered);
            OnPropertyChanged("ChampListFiltered");
            OnPropertyChanged("AramChampsCompletedString");

        }
        /// <summary>
        /// Loads the data associated with the ARAMViewMode screen.
        /// This data is not saved whatsoever within the API so much be saved locally. We only care whether or not each champion is 'Completed' so it's not complex.
        /// </summary>
        /// <param name="ForceCheck"></param>
        public void GetAramDataForChamps(bool ForceCheck = false)
        {
            if (!SaveData.IsAramDataGrabbed() || ForceCheck) SaveData.GetChampionDataAram();
            AramChampsCompleted = 0;
            foreach (ChampionData champ in ChampionList)
            {
                ChampionDataAram tempResponse = SaveData.GetSpecChampionDataAram(champ.ChampionID);
                if (tempResponse != null && tempResponse.ChampionID != -1 && tempResponse.ChampionName != "Error") champ.AddAramData(tempResponse);
                if (champ.AramCompleted == true) AramChampsCompleted++;
            }
            OnPropertyChanged("AramChampsCompletedString");

        }

        public string GetSortByDefault()
        {
            return SaveData.GetSortByFromRegistry();
        }

        public void SaveSortByDefault()
        {
            SaveData.SetSortByToRegistry(SortBySelected);
        }
        /// <summary>
        /// Launches the configuration screen where the user can change their default screen and summoner name.
        /// </summary>
        public void LaunchConfig()
        {
            FirstTimeStartupView firstTimeView = new FirstTimeStartupView(true);
            firstTimeView.Show();
        }
        /// <summary>
        /// Check if we have the icons associated with mastery level available. If not, populate them.
        /// </summary>
        public void CheckIcons()
        {
            if (!SaveData.CheckMasteryIconsExist()) SaveData.CopyMasteryIcons();
        }
        #endregion
    }
}
