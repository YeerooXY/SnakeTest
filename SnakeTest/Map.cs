using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeTest
{
    internal class Map
    {
        public Map()
        {
            Console.Clear();
            Console.WriteLine("Give the map size between 8-35!");
            bool success = false;
            int numberToParse = 0;
            while (!success)
            {
                try
                {
                    numberToParse = int.Parse(Console.ReadLine());
                    if (numberToParse < 8 || numberToParse > 35)
                    {
                        Console.WriteLine("Invalid number!");
                    }
                    else
                    {
                        success = true;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input!");

                }
            }
            this.height = numberToParse;

        }

        public int Height { get => height; set => height = value; }
        public int Width { get => Height * 2; }
        public void SeeMap()
        {
            Console.Write("\n\n\n   ");
            for (int i = 0; i < Height; i++) //+2 due to the sides of the map taking up characters
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || i == Height - 1) // if true, then it is either the top or bottom row
                    {
                        if (j == 0 || j == Width - 1) // if true, then it is the corner of the map
                        {
                            Console.Write("+");
                        }
                        else
                        {
                            Console.Write("-");
                        }
                    }
                    else if (j == 0 || j == Width - 1) // if true, then it is one of the sides of the map
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n   ");

            }
        }
        private int height;
    }
}
