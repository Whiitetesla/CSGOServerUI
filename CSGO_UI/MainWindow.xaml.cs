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
        public string SteamCdmPath { get; set; }
        public UserGame GameSettings { get; set; }
        public GameMode Modes { get; set; }
        //the selected information right now
        public GameTypes SelectedType { get; set; }
        public Modes SelectedMode { get; set; }
        public DateTime LastUpdated { get; set; }

        public MainWindow()
        {
            Modes = new GameMode();
            GameSettings = new UserGame();
            SelectedType = new GameTypes();
            SelectedMode = new Modes();

            InitializeComponent();

            //auto update if the path was set to something
            if (!(SteamCdmPath == "" || SteamCdmPath == null))
            {
                DoUpdate();
            }
        }
        //gets and updates the mode for the game information
        private void SetModes()
        {

            Modes.Games = new DLL.ReadAllGames().GetGames(SteamCdmPath);

        }
        //gets the path for the steam cdm
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var temp = "";
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Programer (.exe)|*.exe|Alle filer (*.*)|*.*";

            fileDialog.FilterIndex = 1;

            fileDialog.Multiselect = false;

            bool? userClick = fileDialog.ShowDialog();

            if (userClick == true)
            {
                temp = System.IO.Path.GetFullPath(fileDialog.FileName);
                int tempIndex = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] == Char.Parse("\\"))
                    {
                        tempIndex = i;
                    }
                }
                SteamCdmPath = temp.Substring(0, tempIndex); ;
                SteamCMDName.Text = SteamCdmPath;
                try
                {
                    SetModes();
                    InitGameComboboxs();
                }
                catch (Exception)
                {
                    temp_out.Text = "the path does not match a standart steam cmd path. \n Can't read " + SteamCdmPath;
                }
            }              
        }        
        //makes sure that there only can be inserted numbers in the text field
        private void NumberOfPlayers_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //sets a template value for the max players
        private void NumberOfPlayers_Initialized(object sender, EventArgs e)
        {
            NumberOfPlayers.Text = GameSettings.GetMaxPlayers();
        }
        //when the text changes we check if we can update the max players
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
        //updates the server
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DoUpdate();
        }
        //starts the server
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if(new BLL.ServerComands().StartClient(GameSettings,SteamCdmPath))
                {
                    temp_out.Text = "server started";
                }
                else
                {
                    temp_out.Text = "error when trying to start the server";
                }
            }
            catch (Exception)
            {
                temp_out.Text = "error when trying to start the server";
            }
        }
        //stops the server
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new BLL.ServerComands().StopClient())
                {
                    temp_out.Text = "server stopped";
                }
                else
                {
                    temp_out.Text = "error when trying to stop the server";
                }
            }
            catch (Exception)
            {
                temp_out.Text = "error when trying to stop the server";
            }
        }
        //what will happen when csmaps combobox loses focus
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
        //what will happen when gamemodes combobox loses focus
        private void gameModes_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateGameModeList();
        }
        //what will happen when gametypes combobox loses focus
        private void gameTypes_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateGameTypeList();
        }
        //method for updating the csgo server
        private void DoUpdate()
        {
            try
            {
                LastUpdated = new BLL.ServerComands().UpdateClient(SteamCdmPath);

                updateButt.Content = LastUpdated.ToString();
            }
            catch (Exception)
            {
                temp_out.Text = "error with the Steam cmd Please check if you entered the right path";
            }
        }
        //updates the list of gametypes if there was a change with it
        private void UpdateGameTypeList()
        {
            foreach (var type in Modes.Games)
            {
                if (type.Name == gameTypes.SelectionBoxItem.ToString())
                {
                    SelectedType = type;
                    SetGameModesList();
                    UpdateGameModeList();
                }
            }
        }
        //updates the list of gamemodes if there was a change with it
        private void UpdateGameModeList()
        {
            foreach (var mode in SelectedType.Modes)
            {
                if (mode.Name == SelectedType.Modes[0].Name)
                {
                    GameSettings.SetGamemode(mode.Name, SelectedType, Modes.Games.IndexOf(SelectedType));
                    SelectedMode = mode;
                    SetCSMapList();
                    if (SelectedMode.MapGroups.Count > 0)
                        GameSettings.SetMap(SelectedMode.MapGroups[0].Name);
                }
            }
        }
        //sets the list of the game type
        private void SetGameTypesList()
        {
            gameTypes.ItemsSource = Modes.Games;
            gameTypes.SelectedIndex = 0;
            if (Modes.Games.Count > 0)
                SelectedType = Modes.Games[0];
        }
        //sets the list of the game modes
        private void SetGameModesList()
        {
            gameModes.ItemsSource = SelectedType.Modes;
            gameModes.SelectedIndex = 0;
            if (SelectedType.Modes.Count > 0)
                SelectedMode = SelectedType.Modes[0];
        }
        //sets the list of the game maps
        private void SetCSMapList()
        {
            CSMaps.ItemsSource = SelectedMode.MapGroups;
            CSMaps.SelectedIndex = 0;
        }
        //updates all the comboboxes with the new data
        private void InitGameComboboxs()
        {
            SetGameTypesList();
            UpdateGameTypeList();
        }
    }
}