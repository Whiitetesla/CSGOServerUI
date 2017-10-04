using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.entitys
{
    public class UserGame
    {
        //how many players the user wich to have in a game
        private int maxplayers = 10;
        private int gameType = 0;
        private int gameMode = 0;
        private Modes selectedGame;
        private MapGroups selectedMap = new MapGroups()
        {
            Name = "de_dust2",
            Group = "inactivemaps"
        };

        public string GetMaxPlayers()
        {
            return maxplayers.ToString();
        }

        public string GetMaxplayersCode()
        {
            if (maxplayers <= selectedGame.MaxPlayers)
            {
                return " -maxplayers_override " + maxplayers;
            }
            return " -maxplayers_override " + selectedGame.MaxPlayers;
        }
        
        public bool SetMaxplayers(int value)
        {
            if (selectedGame != null)
            {
                if(value <= selectedGame.MaxPlayers)
                {
                    maxplayers = value;
                    return true;
                }
            }
            if(value <= maxplayers)
            {
                return true;
            }
            return false;
        }

        public string GetMapCode()
        {
            if (selectedMap.Name != "")
            {
                return " +mapgroup "+selectedMap.Group+" +map " + selectedMap.Name;
            }
            return " +mapgroup " + selectedMap.Group;

        }

        public bool SetMap(string value)
        {
            if(selectedGame != null)
            {
                foreach (var map in selectedGame.MapGroups)
                {
                    if(map.Name == value)
                    {
                        selectedMap = map;
                        return true;
                    }
                }
            }
            return false;

        }

        public string GetGameMode()
        {
            return " +game_type " + gameType + " +game_mode " + gameMode;
        }

        public bool SetGamemode(string value, GameTypes game, int gameType)
        {
            if(game != null)
            {
                this.gameType = gameType;
                foreach (var mode in game.Modes)
                {
                    if (mode.Name.GetGameName() == value)
                    {
                        selectedGame = mode;
                        gameMode = game.Modes.IndexOf(mode);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
