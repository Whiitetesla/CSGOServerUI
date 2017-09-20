using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.entitys
{
    public class MapGroups
    {
        //name of witch group the map is in
        public string Group { get; set; } = "";
        //the name of the map
        public string Name { get; set; } = "random_classic";

        public override string ToString()
        {
            return Name;
        }
    }
}
