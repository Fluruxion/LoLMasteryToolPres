using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LoLMasteryTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // This gives us a simple messagebox detailing any crash, useful for any crashes happening out of the dev environment.
            AppDomain currentDomain = default(AppDomain);
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            System.Windows.Forms.Application.ThreadException += GlobalThreadExceptionHandler;

            DataGrab.DataGrab RegReader = new DataGrab.DataGrab();
            string name = RegReader.GetSummonerFromRegistry();
            int screen = RegReader.GetScreenFromRegistry();
            if (name.Length <= 0 || screen == -1)
            {
                FirstTimeStartupView firstTimeView = new FirstTimeStartupView();
                firstTimeView.Show();
            }
            else
            {
                MasteryPageView masteryPageView = new MasteryPageView();
                MasteryPageViewModel masteryPageViewModel = new MasteryPageViewModel(name, screen);
                masteryPageView.DataContext = masteryPageViewModel;
                masteryPageView.Show();
                //masteryPageViewModel.Start();
            }
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            MessageBox.Show(ex.ToString());
        }

        private static void GlobalThreadExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = e.Exception;
            MessageBox.Show(ex.ToString());
        }
    }
}
