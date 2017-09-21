using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGO_UI.DLL
{
    public class StringEditor
    {
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
    }
}
