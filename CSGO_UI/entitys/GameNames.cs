using System.ComponentModel;

namespace CSGO_UI.entitys
{
    public class GameNames
    {
        private readonly string _name;
        private readonly string _gameName;

        private GameNames(string name, string gameName)
        {
            this._name = name;
            this._gameName = gameName;
        }

        public static readonly GameNames NonSet = new GameNames("Non","");
        public static readonly GameNames Casual = new GameNames("Casual", "casual");
        public static readonly GameNames Competitive = new GameNames("Competitive", "competitive");
        public static readonly GameNames Scrimcomp2V2 = new GameNames("Wingman", "scrimcomp2v2");
        public static readonly GameNames Scrimcomp5V5 = new GameNames("Weapons Expert", "scrimcomp5v5");
        public static readonly GameNames Gungameprogressive = new GameNames("Arms Race", "gungameprogressive");
        public static readonly GameNames Gungametrbomb = new GameNames("Demolition", "gungametrbomb");
        public static readonly GameNames Deathmatch = new GameNames("Deathmatch", "deathmatch");
        public static readonly GameNames Training = new GameNames("Training", "training");
        public static readonly GameNames Custom = new GameNames("Custom", "custom");
        public static readonly GameNames Cooperative = new GameNames("Cooperative", "cooperative");
        public static readonly GameNames Coopmission = new GameNames("Coopmission", "coopmission");
        public static readonly GameNames Skirmish = new GameNames("Skirmish", "skirmish");

        public static GameNames GetAGameNames(string name)
        {
            switch (name)
            {
                case "casual":
                    return Casual;
                case "competitive":
                    return Competitive;
                case "scrimcomp2v2":
                    return Scrimcomp2V2;
                case "scrimcomp5v5":
                    return Scrimcomp5V5;
                case "gungameprogressive":
                    return Gungameprogressive;
                case "gungametrbomb":
                    return Gungametrbomb;
                case "deathmatch":
                    return Deathmatch;
                case "training":
                    return Training;
                case "custom":
                    return Custom;
                case "cooperative":
                    return Cooperative;
                case "coopmission":
                    return Coopmission;
                case "skirmish":
                    return Skirmish;
                default:
                    return NonSet;
            }

            return NonSet;
        }

        public string GetGameName()
        {
            if (_gameName != null)
                return _gameName;
            return "";
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
