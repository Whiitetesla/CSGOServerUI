using CSGO_UI.entitys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSGO_UI.DLL
{
    public class ReadAllGames
    {

        public ObservableCollection<GameTypes> GetGames(string steamCdmPath)
        {
            string text = System.IO.File.ReadAllText(steamCdmPath + "\\steamapps\\common\\Counter-Strike Global Offensive Beta - Dedicated Server\\csgo\\gamemodes.txt");
            //string text = System.IO.File.ReadAllText(@"C:\Users\NHL\Source\Repos\CSGOServerUI\CSGO_UI\gamemodes.txt");
            string replacement = Regex.Replace(text, @"\t|\n|\r| ", "");

            replacement.Trim();
            var topString = replacement.Split(new string[] { "gameTypes" }, StringSplitOptions.None);
            var endString = topString[1].Split(new string[] { "}}}}}" }, StringSplitOptions.None);
            var mainString = endString[0];
            mainString += "}}}}}";
            return getAllGameTypes(mainString);
        }

        private ObservableCollection<GameTypes> getAllGameTypes(string mainString)
        {
            ObservableCollection<GameTypes> gameTypes = new ObservableCollection<GameTypes>();

            List<string> gameTypesString = mainString.Split(new string[] { "}}}}" }, StringSplitOptions.None).ToList();
            gameTypesString[0] = gameTypesString[0].Remove(0, 2);
            gameTypesString.RemoveAt(gameTypesString.Count - 1);

            foreach (var obj in gameTypesString)
            {
                var tempArray = obj.Split(new string[] { "{" }, StringSplitOptions.None).ToList();
                var temp = StringEditor.RemoveNONCharactor(tempArray[0]);

                ObservableCollection<Modes> gameModes = GetGameTypeModes(temp, string.Join(",", tempArray.ToArray()));

                gameTypes.Add(new GameTypes()
                {
                    Name = temp,
                    Modes = gameModes
                });
            }
            return gameTypes;
        }
        private ObservableCollection<Modes> GetGameTypeModes(string gameType, string mainString)
        {
            ObservableCollection<Modes> gameModes = new ObservableCollection<Modes>();
            ObservableCollection<MapGroups> gameMaps = new ObservableCollection<MapGroups>();

            List<string> modestring = mainString.Split(new string[] { "}}" }, StringSplitOptions.None).ToList();
            var maxPlayersStr = mainString.Split(new string[] { "maxplayers" }, StringSplitOptions.None);
            List<int> mPlayers = new List<int>();

            foreach (var p in maxPlayersStr)
            {
                var temp = StringEditor.RemoveNONCharactor(p);
                var maxPstr = temp.Substring(0, 4);
                try
                {
                    mPlayers.Add(int.Parse(StringEditor.RemoveNONCharactor(maxPstr)));
                }
                catch (Exception)
                {
                }
            }

            for (int i = 0; i < modestring.Count; i++)
            {
                List<int> removeable = new List<int>();
                if (modestring[i].StartsWith("/"))
                {
                    modestring[i - 1] += modestring[i];
                    removeable.Add(i);
                }

                foreach (var remove in removeable)
                {
                    modestring.RemoveAt(remove);
                }
            }

            for (int i = 0; i < modestring.Count; i++)
            {
                List<int> removeable = new List<int>();
                if (modestring[i].Contains("weaponprogression_t"))
                {
                    modestring[i - 1] += modestring[i];
                    removeable.Add(i);
                }

                foreach (var remove in removeable)
                {
                    modestring.RemoveAt(remove);
                }
            }

            gameMaps = GetGameModeMaps(modestring[0]);
            gameModes.Add(new Modes()
            {
                Name = StringEditor.RemoveNONCharactor(modestring[0].Split(new string[] { "," }, StringSplitOptions.None)[2]),
                MaxPlayers = mPlayers[0],
                MapGroups = gameMaps
            });



            for (int i = 1; i < modestring.Count; i++)
            {
                var temp = modestring[i].Split(new string[] { "," }, StringSplitOptions.None)[0];
                if (!temp.StartsWith("//"))
                {
                    if (!temp.Contains("weaponprogression_t"))
                    {
                        temp = StringEditor.RemoveNONCharactor(temp);

                        gameMaps = GetGameModeMaps(modestring[i]);

                        gameModes.Add(new Modes()
                        {
                            Name = temp,
                            MaxPlayers = mPlayers[i],
                            MapGroups = gameMaps
                        });
                    }
                }
            }

            return gameModes;
        }

        private ObservableCollection<MapGroups> GetGameModeMaps(string mainString)
        {
            ObservableCollection<MapGroups> gameMaps = new ObservableCollection<MapGroups>();
            var tempGroup = "";
            var tempMap = "";

            var gameMap = mainString.Split(new string[] { "mapgroupsSP", "//Mapgroupsforonlinemodes" }, StringSplitOptions.None);
            if (gameMap.Length > 1)
            {
                var gameMapSlit = gameMap[1].Split(new string[] { "//" }, StringSplitOptions.None);
                foreach (var item in gameMapSlit)
                {
                    var maps = item.Split(new string[] { "mg" }, StringSplitOptions.None);

                    for (int i = 0; i < maps.Length; i++)
                    {
                        if (maps[i].Contains("_"))
                        {
                            var imp = StringEditor.RemoveAllNonLetters(maps[i]);

                            tempMap = imp;
                        }
                        else
                        {
                            tempGroup = StringEditor.RemoveNONCharactor(maps[i]);
                        }

                        if (tempMap != "")
                        {
                            if (gameMaps.FirstOrDefault(obj => obj.Group == "inactivemaps" && obj.Name == "de_dust") != null && tempMap == "de_dust")
                            {
                                tempMap += "2";
                            }
                            gameMaps.Add(new MapGroups()
                            {
                                Name = tempMap,
                                Group = tempGroup
                            });
                        }

                        tempMap = "";
                    }
                }
            }

            return gameMaps;
        }
    }
}
