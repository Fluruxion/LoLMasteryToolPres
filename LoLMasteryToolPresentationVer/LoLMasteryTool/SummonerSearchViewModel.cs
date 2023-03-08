using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LoLMasteryTool
{
    class SummonerSearchViewModel
    {
        private string m_SummonerInput { get; set; }
        public string SummonerInput
        {
            get
            {
                return m_SummonerInput;
            }
            set
            {
                m_SummonerInput = value;
            }
        }
        
        public ICommand SearchSummoner { get; set; }



        public SummonerSearchViewModel(ICommand Search)
        {
            SearchSummoner = Search;
        }
    }
}
