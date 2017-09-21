using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using CSGO_UI.entitys;

namespace CSGO_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SteamCdmPath { get; set; } = "C:\\Servers\\CSGo";
        public UserGame GameSettings { get; set; }
        public GameMode Modes { get; set; }
        public GameTypes SelectedType { get; set; }
        public Modes SelectedMode { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public ObservableCollection<string> GameModes { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Maps { get; set; } = new ObservableCollection<string>();
        

        public MainWindow()
        {
            Modes = new GameMode();
            GameSettings = new UserGame();
            SelectedType = new GameTypes();
            SelectedMode = new Modes();

            InitializeComponent();
        }

        private void setMapsAndModes()
        {
            string text = System.IO.File.ReadAllText(SteamCdmPath + "\\steamapps\\common\\Counter-Strike Global Offensive Beta - Dedicated Server\\csgo\\gamemodes.txt");
            //string text = System.IO.File.ReadAllText(@"C:\Users\NHL\Source\Repos\CSGOServerUI\CSGO_UI\gamemodes.txt");
            string replacement = Regex.Replace(text, @"\t|\n|\r| ", "");

            replacement.Trim();
            var topString = replacement.Split(new string[] { "gameTypes" }, StringSplitOptions.None);
            var endString = topString[1].Split(new string[] { "}}}}}" }, StringSplitOptions.None);
            var mainString = endString[0];
            mainString += "}}}}}";

            Modes.Games = getAllGameTypes(mainString);

            Console.WriteLine(Modes);

        }

        public static string RemoveNONCharactor(string str)
        {
            var temp = str;
            try
            {
                while (!Char.IsLetterOrDigit(temp[0]))
                {
                    temp = temp.Remove(0, 1);
                }
                while (!Char.IsLetterOrDigit(temp[temp.Length - 1]))
                {
                    temp = temp.Remove(temp.Length - 1, 1);
                }
            }
            catch (Exception)
            {
                return "";
            }
            return temp;
        }

        public static string RemoveAllNonLetters(string str)
        {
            var temp = str;
            try
            {
                while (!Char.IsLetter(temp[0]))
                {
                    temp = temp.Remove(0, 1);
                }
                while (!Char.IsLetter(temp[temp.Length - 1]))
                {
                    temp = temp.Remove(temp.Length - 1, 1);
                }
            }
            catch (Exception)
            {
            }
            return temp;
        }

        public ObservableCollection<GameTypes> getAllGameTypes(string mainString)
        {
            ObservableCollection<GameTypes> gameTypes = new ObservableCollection<GameTypes>();

            List<string> gameTypesString = mainString.Split(new string[] { "}}}}" }, StringSplitOptions.None).ToList();
            gameTypesString[0] = gameTypesString[0].Remove(0, 2);
            gameTypesString.RemoveAt(gameTypesString.Count - 1);

            foreach (var obj in gameTypesString)
            {
                var tempArray = obj.Split(new string[] { "{" }, StringSplitOptions.None).ToList();
                var temp = RemoveNONCharactor(tempArray[0]);

                ObservableCollection<Modes> gameModes = GetGameTypeModes(temp, string.Join(",", tempArray.ToArray()));

                gameTypes.Add(new GameTypes()
                {
                    Name = temp,
                    Modes = gameModes
                });
            }
            return gameTypes;
        }

        public ObservableCollection<Modes> GetGameTypeModes(string gameType, string mainString)
        {
            ObservableCollection<Modes> gameModes = new ObservableCollection<Modes>();
            ObservableCollection<MapGroups> gameMaps = new ObservableCollection<MapGroups>();

            List<string> modestring = mainString.Split(new string[] { "}}" }, StringSplitOptions.None).ToList();
            var maxPlayersStr = mainString.Split(new string[] { "maxplayers" }, StringSplitOptions.None);
            List<int> mPlayers = new List<int>();

            foreach (var p in maxPlayersStr)
            {
                var temp = RemoveNONCharactor(p);
                var maxPstr = temp.Substring(0, 4);
                try
                {
                    mPlayers.Add(int.Parse(RemoveNONCharactor(maxPstr)));
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

            var cookie = modestring[0].Split(new string[] { "," }, StringSplitOptions.None);
            var cake = RemoveNONCharactor(cookie[2]);
            gameMaps = GetGameModeMaps(modestring[0]);
            gameModes.Add(new Modes()
            {
                Name = cake,
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
                        temp = RemoveNONCharactor(temp);

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

        public ObservableCollection<MapGroups> GetGameModeMaps(string mainString)
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
                            var imp = RemoveAllNonLetters(maps[i]);

                            tempMap = imp;
                        }
                        else
                        {
                            tempGroup = RemoveNONCharactor(maps[i]);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Programer (.exe)|*.exe|Alle filer (*.*)|*.*";

            fileDialog.FilterIndex = 1;

            fileDialog.Multiselect = false;

            bool? userClick = fileDialog.ShowDialog();

            if (userClick == true)
            {
                string temp = System.IO.Path.GetFullPath(fileDialog.FileName);
                int tempIndex = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i]== Char.Parse("\\"))
                    {
                        tempIndex = i;
                    }
                }

                SteamCdmPath = temp.Substring(0, tempIndex);
                SteamCMDName.Text = SteamCdmPath;
                try
                {
                    setMapsAndModes();

                    gameTypes.ItemsSource = Modes.Games;
                    gameTypes.SelectedIndex = 0;
                    if (Modes.Games.Count > 0)
                        SelectedType = Modes.Games[0];

                    gameModes.ItemsSource = SelectedType.Modes;
                    gameModes.SelectedIndex = 0;
                    if (SelectedType.Modes.Count > 0)
                        SelectedMode = SelectedType.Modes[0];

                    CSMaps.ItemsSource = SelectedMode.MapGroups;
                    CSMaps.SelectedIndex = 0;
                }
                catch (Exception)
                {
                    temp_out.Text = "the path does not match a standart steam cmd path. \n Can't read \\steamapps\\common\\Counter-Strike Global Offensive Beta - Dedicated Server\\csgo\\gamemodes.txt";
                }
            }
        }

        private void NumberOfPlayers_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberOfPlayers_Initialized(object sender, EventArgs e)
        {
            NumberOfPlayers.Text = GameSettings.GetMaxPlayers();
        }

        private void NumberOfPlayers_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var tempInt = Int32.Parse(NumberOfPlayers.Text);
                if(!GameSettings.SetMaxplayers(tempInt))
                {
                    temp_out.Text = "number Of player not matching with maximun for map the max is " + SelectedMode.MaxPlayers;

                }
                else
                {
                    temp_out.Text = "";
                }
            }
            catch (Exception)
            {
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateClient();
        }

        private void UpdateClient()
        {
            Process CSUpdate = new Process();

            try
            {
                CSUpdate.StartInfo.FileName = SteamCdmPath + "\\steamcmd.exe";
                CSUpdate.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                CSUpdate.StartInfo.Arguments = "+login anonymous +app_update 740 +quit";
                CSUpdate.Start();
                LastUpdated = DateTime.Now;
                updateButt.Content = LastUpdated.ToString();
            }
            catch (Exception)
            {
                temp_out.Text = "error with the Steam cmd Please check if you entered the right path";
            }
        }

        private void StartClient()
        {
            Process CSStart = new Process();
            string serverString = "srcds -game csgo -console -usercon "+ GameSettings.GetMaxplayersCode() + 
                GameSettings.GetGameMode() + GameSettings.GetMapCode();

            temp_out.Text = serverString;
            try
            {
                CSStart.StartInfo.FileName = SteamCdmPath + "\\steamapps\\common\\Counter-Strike Global Offensive Beta - Dedicated Server\\srcds.exe";
                CSStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                CSStart.StartInfo.Arguments = serverString;

                if (CSStart.Start())
                {
                    //temp_out.Text = "server started";
                }
                temp_out.Text = serverString;
            }
            catch (Exception)
            {
                //temp_out.Text = "error when trying to start the server";

            }
        }

        private void StopClient()
        {
            Process CSStop = new Process();
            try
            {
                CSStop.StartInfo.FileName = "cmd.exe";
                CSStop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                CSStop.StartInfo.Arguments = "cmd.exe" + String.Format("/k {0} & {1}", "TASKKILL /IM srcds.exe", "exit");
                if (CSStop.Start())
                {
                    temp_out.Text = "server stopped";
                }
            }
            catch (Exception)
            {
                temp_out.Text = "error when trying to stop the server";

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            StartClient();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StopClient();
        }

        private void CSMaps_Initialized(object sender, EventArgs e)
        {
            CSMaps.ItemsSource = SelectedMode.MapGroups;
            CSMaps.SelectedIndex = 0;
        }

        private void gameModes_Initialized(object sender, EventArgs e)
        {
            gameModes.ItemsSource = SelectedType.Modes;
            gameModes.SelectedIndex = 0;
            if (SelectedType.Modes.Count > 0)
                SelectedMode = SelectedType.Modes[0];
        }

        private void gameTypes_Initialized(object sender, EventArgs e)
        {
            gameTypes.ItemsSource = Modes.Games;
            gameTypes.SelectedIndex = 0;
            if (Modes.Games.Count > 0)
                SelectedType = Modes.Games[0];
        }

        private void CSMaps_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var map in SelectedMode.MapGroups)
            {
                if (map.Name == CSMaps.SelectionBoxItem.ToString())
                {
                    GameSettings.SetMap(map.Name);
                }
            }
        }

        private void gameModes_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var mode in SelectedType.Modes)
            {
                if (mode.Name == gameModes.SelectionBoxItem.ToString())
                {
                    GameSettings.SetGamemode(mode.Name, SelectedType, Modes.Games.IndexOf(SelectedType));
                    SelectedMode = mode;
                    CSMaps.ItemsSource = SelectedMode.MapGroups;
                    CSMaps.SelectedIndex = 0;
                }
            }
        }

        private void gameTypes_LostFocus(object sender, RoutedEventArgs e)
        {
            foreach (var type in Modes.Games)
            {
                if(type.Name == gameTypes.SelectionBoxItem.ToString())
                {
                    SelectedType = type;
                    gameModes.ItemsSource = SelectedType.Modes;
                    gameModes.SelectedIndex = 0;
                    SelectedMode = SelectedType.Modes[0];


                    foreach (var mode in SelectedType.Modes)
                    {
                        if (mode.Name == SelectedType.Modes[0].Name)
                        {
                            GameSettings.SetGamemode(mode.Name, SelectedType, Modes.Games.IndexOf(SelectedType));
                            SelectedMode = mode;
                            CSMaps.ItemsSource = SelectedMode.MapGroups;
                            CSMaps.SelectedIndex = 0;
                            if(SelectedMode.MapGroups.Count > 0)
                                GameSettings.SetMap(SelectedMode.MapGroups[0].Name);
                        }
                    }
                }
            }

        }
    }
}