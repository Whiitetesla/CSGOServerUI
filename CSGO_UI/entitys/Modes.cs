using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.entitys
{
    public class Modes
    {
        //the name of this mode
        public string Name { get; set; }
        //how many players that max is alowed in that mode
        public int MaxPlayers { get; set; }
        //gets an list of all the maps that is in that mode
        public ObservableCollection<MapGroups> MapGroups { get; set; } = new ObservableCollection<entitys.MapGroups>();

        public override string ToString()
        {
            return Name;
        }
    }
}
