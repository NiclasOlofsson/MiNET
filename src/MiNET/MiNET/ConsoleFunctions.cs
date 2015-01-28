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
    class ConsoleFunctions
    {
        public static void WriteServerLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[SERVER] ");
            Console.ResetColor();
            Console.WriteLine(text);
        }
        public static void WriteWarningLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[WARNING] ");
            Console.ResetColor();
            Console.WriteLine(text);
        }

        public static void WriteInfoLine(string text, bool log = true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[INFO] ");
            Console.ResetColor();
            Console.WriteLine(text);
        }
    }
}
