using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.entitys
{
    public class GameTypes
    {
        //the name of the game type
        public string Name { get; set; }
        //what modes are in that game type
        public ObservableCollection<Modes> Modes { get; set; } = new ObservableCollection<entitys.Modes>();

        public override string ToString()
        {
            return Name;
        }

    }
}
