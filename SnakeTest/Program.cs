using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SnakeTest
{

    internal class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        static bool NewGame()
        {
            SnakeGame newGame = new SnakeGame();
            Map map = new Map();
            newGame.InitializeGame(map);
            do
            {
                newGame.SetDirection();
            } while (!newGame.GameOver(map));
            Thread.Sleep(500);
            bool another = true;
            string input = Console.ReadLine();
            switch (input)
            {
                case "n":
                    another = false;
                    break;
                default:
                    another = true;
                    break;
            }
            return another;
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
            Console.ReadLine();
            do
            {

            } while (NewGame());
           
        }
    }
}
