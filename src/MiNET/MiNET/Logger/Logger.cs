using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiNET
{
    class Logger
    {
        private static string CurrentFile;
        public static void Initialize()
        {
            string LogDir = (Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs"));
            if (!Directory.Exists(LogDir))
                Directory.CreateDirectory(LogDir);

            CurrentFile = Path.Combine(LogDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture) + ".txt");
            File.WriteAllText(CurrentFile, "#MiNET Server log" + Environment.NewLine);
        }

        private static void Log(string text)
        {
            File.AppendAllText(CurrentFile,
                   DateTime.Now.ToString() + ">> " + text + Environment.NewLine);
        }

        public static void WriteServerLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SERVER] ");
            Console.ResetColor();
            Console.WriteLine(text);

            if (log)
            {
                Log("[SERVER] " + text);
            }
        }
        public static void WriteWarningLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[WARNING] ");
            Console.ResetColor();
            Console.WriteLine(text);

            if (log)
            {
                Log("[WARNING] " + text);
            }
        }

        public static void WriteInfoLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[INFO] ");
            Console.ResetColor();
            Console.WriteLine(text);

            if (log)
            {
                Log("[INFO] " + text);
            }
        }
    }
}
