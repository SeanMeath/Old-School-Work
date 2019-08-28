using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Thing
{
    class Program
    {
        static void Main(string[] args)
        {
            string player = "X";
            int choice;
            bool win = false;
            int ARRAYSIZE = 9;
            string[] quadrant = new string[ARRAYSIZE];
            int counter = 0;
            quadrant[0] = "1";
            quadrant[1] = "2";
            quadrant[2] = "3";
            quadrant[3] = "4";
            quadrant[4] = "5";
            quadrant[5] = "6";
            quadrant[6] = "7";
            quadrant[7] = "8";
            quadrant[8] = "9";
            while (win == false && counter < ARRAYSIZE)
            {
                printBoard(quadrant);
                if (counter % 2 == 0)
                {
                    player = "X";
                }
                else
                {
                    player = "O";
                }
                choice = validation(quadrant, player);
                quadrant[choice] = player;
                win = testwin(quadrant);
                counter = counter + 1;
            }
            printBoard(quadrant);
            if (win)
            {
                Console.WriteLine("Congratulations! Player " + player + " won! Press enter to close the program.");
            }
            else
            {
                Console.WriteLine("Good game it's a tie! Press enter to close the program.");
            }
            Console.ReadLine();
        }

        static void printBoard(string[] quadrant)
        {
            Console.Clear();
            Console.WriteLine(quadrant[0] + "|" + quadrant[1] + "|" + quadrant[2]);
            Console.WriteLine(quadrant[3] + "|" + quadrant[4] + "|" + quadrant[5]);
            Console.WriteLine(quadrant[6] + "|" + quadrant[7] + "|" + quadrant[8]);
        }

        static int validation(string[] quadrant, string player)
        {
            int pass = 0;
            bool success;
            int choice;
            Console.WriteLine("Player " + player + " please enter your quadrant you wish to fill.");
            success = int.TryParse(Console.ReadLine(), out choice);
            while (success == false)
            {
                Console.Clear();
                printBoard(quadrant);
                Console.WriteLine("Error. The answer given is not a valid integer. Please try again.");
                success = int.TryParse(Console.ReadLine(), out choice);
            }
            choice = choice - 1;
            do
            {
                if (pass == 1)
                {
                    Console.Clear();
                    printBoard(quadrant);
                    Console.WriteLine("The number entered was already filled. Please try again.");
                    success = int.TryParse(Console.ReadLine(), out choice);
                    while (success == false)
                    {
                        Console.Clear();
                        printBoard(quadrant);
                        Console.WriteLine("Error. The answer given is not a valid integer. Please try again.");
                        success = int.TryParse(Console.ReadLine(), out choice);
                    }
                    choice = choice - 1;
                }
                while (choice < 0 || choice > 8)
                {
                    Console.Clear();
                    printBoard(quadrant);
                    Console.WriteLine("The number entered is not in range. Please try again.");
                    success = int.TryParse(Console.ReadLine(), out choice);
                    while (success == false)
                    {
                        Console.Clear();
                        printBoard(quadrant);
                        Console.WriteLine("Error. The answer given is not a valid integer. Please try again.");
                        success = int.TryParse(Console.ReadLine(), out choice);
                    }
                    choice = choice - 1;
                }
                pass = 1;

            }
            while (quadrant[choice] == "X" || quadrant[choice] == "O");
            return choice;
        }

        static bool testwin(string[] quadrant)
        {
            bool win = false;
            if ((quadrant[0] == quadrant[1] && quadrant[1] == quadrant[2]) ||
               (quadrant[3] == quadrant[4] && quadrant[4] == quadrant[5]) ||
               (quadrant[6] == quadrant[7] && quadrant[7] == quadrant[8]) ||
               (quadrant[0] == quadrant[3] && quadrant[3] == quadrant[6]) ||
               (quadrant[1] == quadrant[4] && quadrant[4] == quadrant[7]) ||
               (quadrant[2] == quadrant[5] && quadrant[5] == quadrant[8]) ||
               (quadrant[0] == quadrant[4] && quadrant[4] == quadrant[8]) ||
               (quadrant[2] == quadrant[4] && quadrant[4] == quadrant[6]))
            {
                win = true;
            }
            return win;
        }

    }
}


