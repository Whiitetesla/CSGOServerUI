using CSGO_UI.entitys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.BLL
{
    public class ServerComands
    {
        public DateTime UpdateClient(string steamCdmPath)
        {
            Process CSUpdate = new Process();
            try
            {
                CSUpdate.StartInfo.FileName = steamCdmPath + "\\steamcmd.exe";
                CSUpdate.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                CSUpdate.StartInfo.Arguments = "+login anonymous +app_update 740 +quit";
                CSUpdate.Start();
                return DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool StartClient(UserGame gameSettings, string steamCdmPath)
        {
            Process CSStart = new Process();
            string serverString = "srcds -game csgo -console -usercon " + gameSettings.GetMaxplayersCode() +
                gameSettings.GetGameMode() + gameSettings.GetMapCode();
            try
            {
                CSStart.StartInfo.FileName = steamCdmPath + "\\steamapps\\common\\Counter-Strike Global Offensive Beta - Dedicated Server\\srcds.exe";
                CSStart.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                CSStart.StartInfo.Arguments = serverString;

                if (CSStart.Start())
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool StopClient()
        {
            Process CSStop = new Process();
            try
            {
                CSStop.StartInfo.FileName = "cmd.exe";
                CSStop.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                CSStop.StartInfo.Arguments = "cmd.exe" + String.Format("/k {0} & {1}", "TASKKILL /IM srcds.exe", "exit");
                if (CSStop.Start())
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;

            }
        }
    }
}
