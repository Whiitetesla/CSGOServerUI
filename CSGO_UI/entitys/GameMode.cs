using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.entitys
{
    public class GameMode
    {
        //list of all the diffrent game types that are playable
        public ObservableCollection<GameTypes> Games { get; set; } = new ObservableCollection<GameTypes>();
    }
}
