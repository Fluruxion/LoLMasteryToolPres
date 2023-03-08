using LoLMasteryTool.DataClasses;
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
    /// <summary>
    /// Now unused
    /// </summary>
    class AddChampionScreenViewModel
    {
        private ICommand m_CommandCreateChampion {get; set; }
        public ICommand CommandCreateChampion
        {
            get
            {
                if (m_CommandCreateChampion == null)
                {
                    m_CommandCreateChampion = new DelegateCommand(p => CreateChampion());
                }

                return m_CommandCreateChampion;
            }
        }
        public ICommand CommandSaveChampion { get; set; }
        public bool Top { get; set; }
        public bool Jungle { get; set; }
        public bool Mid { get; set; }
        public bool Adc { get; set; }
        public bool Support { get; set; }
        public string ChampionName { get; set; }
        public int Mastery { get; set; }
        public int Tokens { get; set; }
        public int Rekindled { get; set; }

        public AddChampionScreenViewModel(ICommand commandSaveChampion)
        {
            CommandSaveChampion = commandSaveChampion;
            Top = false;
            Jungle = false;
            Mid = false;
            Adc = false;
            Support = false;
        }

        public void CreateChampion()
        {
            Positions roles = new Positions(Top, Jungle, Mid, Adc, Support);
            ChampionData champion = new ChampionData(ChampionName, Mastery, Tokens, Rekindled, roles);
            CommandSaveChampion.Execute(champion);
        }
    }
}
