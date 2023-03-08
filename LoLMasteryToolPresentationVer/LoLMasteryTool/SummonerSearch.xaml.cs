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
    /// Interaction logic for SummonerSearch.xaml
    /// </summary>
    public partial class SummonerSearch : Window
    {
        public SummonerSearch()
        {
            InitializeComponent();
            SummonerInputBox.Focus();
        }
    }
}
