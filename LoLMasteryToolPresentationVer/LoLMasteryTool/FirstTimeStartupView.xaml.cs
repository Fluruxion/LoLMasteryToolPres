using LoLMasteryTool.DataGrab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoLMasteryTool
{
    /// <summary>
    /// Interaction logic for FirstTimeStartupView.xaml
    /// </summary>
    public partial class FirstTimeStartupView : Window
    {
        private DataGrab.DataGrab dataGrabber;
        private bool configMode { get; set; }
        public FirstTimeStartupView()
        {
            dataGrabber = new DataGrab.DataGrab();

            InitializeComponent();
        }

        public FirstTimeStartupView(bool boolConfig)
        {
            dataGrabber = new DataGrab.DataGrab();
            configMode = boolConfig;
            InitializeComponent();
            DefaultNameTextBox.Text = dataGrabber.GetSummonerFromRegistry();
            switch (dataGrabber.GetScreenFromRegistry())
            {
                case 1: CheckMastery.IsChecked = true; break;
                case 2: CheckAram.IsChecked = true; break;
                default: break;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string summoner = DefaultNameTextBox.Text;
            int screen = -1;
            bool incomplete = false;
            if (CheckMastery.IsChecked == true)
            {
                screen = 1;
            }
            if (CheckAram.IsChecked == true)
            {
                screen = 2;
            }
            if (summoner.Length < 1 || !CheckValidSummoner(summoner))
            {
                incomplete = true;
                WarningSummoner.Visibility = Visibility.Visible;
            }
            else
            {
                WarningSummoner.Visibility = Visibility.Hidden;
            }
            if (screen == -1)
            {
                incomplete = true;
                WarningScreen.Visibility = Visibility.Visible;
            }
            else
            {
                WarningScreen.Visibility = Visibility.Hidden;
            }
            if (!incomplete)
            {
                dataGrabber.SetRegistry(summoner, screen);
                LaunchMain(summoner, screen);
            }
        }

        private void CheckAram_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAram.IsChecked == true && CheckMastery.IsChecked == true)
            {
                CheckMastery.IsChecked = false;
            }
        }

        private void CheckMastery_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAram.IsChecked == true && CheckMastery.IsChecked == true)
            {
                CheckAram.IsChecked = false;
            }
        }

        private bool CheckValidSummoner(string name)
        {
            RiotAPILink APICall = new RiotAPILink();
            return APICall.GetSummonerID(name);
        }

        private void LaunchMain(string name, int screen)
        {
            if (configMode)
            {
                Close();
                return;
            }
            MessageBox.Show("The application will now download all champion icons, this may take a couple of minutes. You can find the images in Appdata local.");
            MasteryPageView masteryPageView = new MasteryPageView();
            MasteryPageViewModel masteryPageViewModel = new MasteryPageViewModel(name, screen);
            masteryPageView.DataContext = masteryPageViewModel;
            masteryPageView.Show();
            //masteryPageViewModel.Start();
            Close();
        }
    }
}
