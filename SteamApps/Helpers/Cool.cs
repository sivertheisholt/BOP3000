using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace SteamApps.Helpers
{
    public class Cool
    {
        public static Task ColorfulAnimation()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    Console.Clear();

                    // steam
                    Console.Write("       . . . . o o o o o o", Console.ForegroundColor = ConsoleColor.Gray);
                    for (int s = 0; s < j / 2; s++)
                    {
                        Console.Write(" o", Console.ForegroundColor = ConsoleColor.Gray);
                    }
                    Console.WriteLine();

                    var margin = "".PadLeft(j);
                    Console.WriteLine(margin + "                _____      o", Console.ForegroundColor = ConsoleColor.Gray);
                    Console.WriteLine(margin + "       ____====  ]OO|_n_n__][.", Console.ForegroundColor = ConsoleColor.Cyan);
                    Console.WriteLine(margin + "      [________]_|__|________)< ", Console.ForegroundColor = ConsoleColor.Cyan);
                    Console.WriteLine(margin + "       oo    oo  'oo OOOO-| oo\\_", Console.ForegroundColor = ConsoleColor.Blue);
                    Console.WriteLine("   +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+", Console.ForegroundColor = ConsoleColor.Gray);

                    Thread.Sleep(200);
                }
            }
            return Task.CompletedTask;
        }

        public static void WriteProgressBar(int percent, bool update = false)
        {
            const char _block = '■';
            const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
            const string _twirl = "-\\|/";
            if (update)
                Console.Write(_back);
            Console.Write("[");
            var p = (int)((percent / 10f) + .5f);
            for (var i = 0; i < 10; ++i)
            {
                if (i >= p)
                    Console.Write(' ');
                else
                    Console.Write(_block);
            }
            Console.Write("] {0,3:##0}%", percent);
        }
    }
}