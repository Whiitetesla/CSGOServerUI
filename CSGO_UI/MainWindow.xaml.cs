using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace CSGO_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SteamCDMPath { get; set; }
        public string MapName { get; set; }
        public int MaxPlayers { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            Process myProcess = new Process();
        }

        //creates a string for the given gamemode
        public string getModes(string mode)
        {
            string temp = "";
            switch (mode)
            {
                case "": 
                    break;
                case "1":
                    break;
                case "2":
                    break;
                default:
                    break;
            }
            return temp;
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