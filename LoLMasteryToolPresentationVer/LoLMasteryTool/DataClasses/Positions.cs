using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLMasteryTool.DataClasses
{
    /// <summary>
    /// Previously used when entering championData manually. Looking into reintroducing these features through API usage in the future.
    /// </summary>
    class Positions
    {
        public bool Top { get; set; }
        public bool Jungle { get; set; }
        public bool Mid { get; set; }
        public bool Adc { get; set; }
        public bool Support { get; set; }

        public Positions()
        {
            Top = false;
            Jungle = false;
            Mid = false;
            Adc = false;
            Support = false;
        }
        public Positions(string role)
        {
            Top = false;
            Jungle = false;
            Mid = false;
            Adc = false;
            Support = false;

            switch (role.ToLower())
            {
                case "top": Top = true; break;
                case "jungle": Jungle = true; break;
                case "mid": Mid = true; break;
                case "adc": Adc = true; break;
                case "support": Support = true; break;
            }
            
        }
        public Positions(List<string> roleList)
        {
            Top = false;
            Jungle = false;
            Mid = false;
            Adc = false;
            Support = false;

            foreach (string role in roleList)
            {
                switch(role.ToLower())
                {
                    case "top": Top = true; break;
                    case "jungle": Jungle = true; break;
                    case "mid": Mid = true; break;
                    case "adc": Adc = true; break;
                    case "support": Support = true; break;
                }
            }
        }

        public Positions(bool top, bool jungle, bool mid, bool adc, bool support)
        {
            Top = top;
            Jungle = jungle;
            Mid = mid;
            Adc = adc;
            Support = support;
        }

        public bool CheckForMatches(Positions roles)
        {
            bool check = false;

            if (Top && roles.Top) check = true;
            if (Jungle && roles.Jungle) check = true;
            if (Mid && roles.Mid) check = true;
            if (Adc && roles.Adc) check = true;
            if (Support && roles.Support) check = true;

            return check;
        }
    }
}
