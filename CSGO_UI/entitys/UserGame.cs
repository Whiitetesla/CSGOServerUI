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
        private int maxplayers = 0;
        private int gameType = 0;
        private int gameMode = 1;
        private Modes selectedGame;
        private MapGroups selectedMap = new MapGroups()
        {
            Name = "de_dust2",
            Group = ""
        };

        public string GetMaxPlayers()
        {
            return maxplayers.ToString();
        }

        public string GetMaxplayersCode()
        {
            return " -maxplayers_override " + maxplayers;
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
            return false;
        }

        public string GetMapCode()
        {
            return " +mapgroup "+selectedMap.Group+" +map " + selectedMap.Name;
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
                    if (mode.Name == value)
                    {
                        selectedGame = mode;
                        gameMode = game.Modes.IndexOf(mode);
                    }
                }
            }
            return false;
        }
    }
}
