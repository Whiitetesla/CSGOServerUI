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
            string temp = "";
            switch (mode)
            {
                case "Dust 2":
                    temp = "2Dust";
                    break;
                case "Dust":
                    temp = "1Dust";
                    break;
                case "Nuke":
                    temp = "deNuke";
                    break;
                default:
                    temp = "ckae";
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
    }
}


//Sub StartServer()
//        SaveSettings()
//        Dim ARKSO_Launch As System.Diagnostics.Process
//        Try
//            ARKSO_Launch = New System.Diagnostics.Process()
//            ARKSO_Launch.StartInfo.FileName = My.Settings.SteamCMDPath & "\steamapps\common\ARK Survival Evolved Dedicated Server\ShooterGame\Binaries\Win64\ShooterGameServer.exe"
//            If CheckBox2.Checked = True Then
//                ARKSO_Launch.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
//            Else
//                ARKSO_Launch.StartInfo.WindowStyle = ProcessWindowStyle.Normal
//            End If
//            ARKSO_Launch.StartInfo.Arguments = Chr(34) & ComboBox1.Text & "?listen?SessionName=" & TextBox2.Text & "?ServerAdminPassword=" & TextBox4.Text & "?ServerPassword=" & TextBox3.Text & "?Port=" & TextBox5.Text & "?QueryPort=" & TextBox6.Text & "?MaxPlayers=" & NumericUpDown1.Value & Chr(34) & " -" & ComboBox2.Text & " -" & ComboBox3.Text
//            ARKSO_Launch.Start()
//        Catch
//            MessageBox.Show("Could not start process " & "ShooterGameServer.exe", "Error")
//        End Try
//    End Sub
//    Sub StopServer()
//        Dim ARKSO_Stop As System.Diagnostics.Process
//        Try
//            ARKSO_Stop = New System.Diagnostics.Process()
//            ARKSO_Stop.StartInfo.FileName = "cmd.exe"
//            ARKSO_Stop.StartInfo.WindowStyle = ProcessWindowStyle.Normal
//            ARKSO_Stop.StartInfo.Arguments = "cmd.exe" & String.Format("/k {0} & {1}", "TASKKILL /IM ShooterGameServer.exe", "exit")
//            ARKSO_Stop.Start()
//        Catch
//            MessageBox.Show("Could not stop process " & "ShooterGame.exe", "Error")
//        End Try
//    End Sub
//    Sub UpdateServer()
//        Dim ARKSO_Update As System.Diagnostics.Process
//        Try
//            ARKSO_Update = New System.Diagnostics.Process()
//            ARKSO_Update.StartInfo.FileName = My.Settings.SteamCMDPath & "\steamcmd.exe"
//            ARKSO_Update.StartInfo.WindowStyle = ProcessWindowStyle.Normal
//            ARKSO_Update.StartInfo.Arguments = "+login anonymous +app_update 376030 +quit"
//            ARKSO_Update.Start()
//            My.Settings.Button3 = "Update " & "(Last: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm") & ")"
//            Button3.Text = "Update " & "(Last: " & DateTime.Now.ToString("yyyy/MM/dd HH:mm") & ")"
//        Catch
//            MessageBox.Show("Could not start process " & "SteamCMD", "Error")
//        End Try
//    End Sub