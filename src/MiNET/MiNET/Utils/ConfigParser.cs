using System;

namespace MiNET.Utils
{
    public class ConfigParser
    {
        public static string ConfigFile = string.Empty;
        private static string FileContents = string.Empty;
        public static string[] InitialValue;

        public ConfigParser ()
        {

        }

        public static bool Check()
        {
            if (!System.IO.File.Exists (ConfigFile))
            {
                System.IO.File.WriteAllLines (ConfigFile, InitialValue);
                return Check ();
            } 
            else
            {
                FileContents = System.IO.File.ReadAllText (ConfigFile);
                if (!FileContents.Contains ("#DO NOT REMOVE THIS LINE - MiNET Config"))
                {
                    System.IO.File.Delete (ConfigFile);
                    return Check ();
                } 
                else
                {
                    return true;
                }
            }
        }

        public static string ReadString(string Rule)
        {
            foreach (string Line in FileContents.Split(new string[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.None))
            {
                if (Line.StartsWith(Rule + "="))
                {
                    string Value = Line.Split ('=') [1];
                    return Value;
                }
            }
            return "";
        }

        public static int ReadInt(string Rule)
        {
            foreach (string Line in FileContents.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (Line.StartsWith(Rule + "="))
                {
                    string Value = Line.Split ('=') [1];
                    return Convert.ToInt32(Value);
                }
            }
            return 0;
        }

        public static bool ReadBoolean(string Rule)
        {
            string D = ReadString (Rule);
            if (D == "true")
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}