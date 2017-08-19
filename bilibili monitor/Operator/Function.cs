using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Monitor.Operator
{
    class Function
    {
        public static void ConsoleWrite(string String, ConsoleColor ForeColor = ConsoleColor.White, ConsoleColor BackColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = ForeColor;
            Console.BackgroundColor = BackColor;
            Console.Write(String);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void ConsoleWriteLine(string String = null, ConsoleColor ForeColor = ConsoleColor.White, ConsoleColor BackColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = ForeColor;
            Console.BackgroundColor = BackColor;
            Console.WriteLine(String);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
