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

namespace CSGO_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SteamCdmPath { get; set; }
        public string MapCode { get; set; }
        public string GameMode { get; set; }
        public int MaxPlayers { get; set; } = 10;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public ObservableCollection<string> Maps { get; set; } = new ObservableCollection<string>();
        

        public MainWindow()
        {
            InitializeComponent();
            UpdateClient();

            setMaps();
            MapCode = GetModes(Maps.First());
        }

        private void setMaps()
        {
            Maps.Add("Dust 2");
            Maps.Add("Dust");
            Maps.Add("Nuke");

        }

        //creates a string for the given gamemode
        public string GetModes(string mode)
        {
            string temp = "+map";
            switch (mode)
            {
                case "Dust 2":
                    temp += "+de_dust2";
                    break;
                case "Dust":
                    temp += "de_dust";
                    break;
                case "Nuke":
                    temp += "de_nuke";
                    break;
                default:
                    temp += "de_dust2";
                    break;
            }
            return temp;
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
            }
        }

        private void NumberOfPlayers_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CSMaps_Initialized(object sender, EventArgs e)
        {
            CSMaps.ItemsSource = Maps;
            CSMaps.SelectedIndex = 0;
        }

        private void CSMaps_GotFocus(object sender, RoutedEventArgs e)
        {
            MapCode = GetModes(CSMaps.SelectionBoxItem.ToString());
        }

        private void NumberOfPlayers_Initialized(object sender, EventArgs e)
        {
            NumberOfPlayers.Text = MaxPlayers.ToString();
        }

        private void NumberOfPlayers_TextChanged(object sender, TextChangedEventArgs e)
        {
            MaxPlayers = Int32.Parse(NumberOfPlayers.Text);
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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void StartClient()
        {
            Process CSStart = new Process();
            try
            {
                CSStart.StartInfo.FileName = SteamCdmPath + "\\steamapps\\common\\ARK Survival Evolved Dedicated Server\\ShooterGame\\Binaries\\Win64\\ShooterGameServer.exe";
                if (HiddenConsole.IsChecked.Value == true)
                {
                    CSStart.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }
                else
                {
                    CSStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                }
                CSStart.StartInfo.Arguments = "srcds -game csgo -usercon +game_type 0 +game_mode 0 + mapgrout mg_active + map de_dust2";
                CSStart.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void StopClient()
        {
            Process CSStop = new Process();
            try
            {
                CSStop.StartInfo.FileName = "cmd.exe";
                CSStop.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                CSStop.StartInfo.Arguments = "cmd.exe" + String.Format("/k {0} & {1}", "TASKKILL /IM srcds.exe", "exit");
                CSStop.Start();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
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
    }
}