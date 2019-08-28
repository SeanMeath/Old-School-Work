using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sMEAT___a3v3
{
    class Program
    {
        const int NMAX = 10;          //maximum size of each name string
        const int LSIZE = 20;         //number of actual name strings in array

        //array of name strings
        static string[] nam = new string[20] { "wendy", "ellen", "freddy", "tom", "susan",
                             "dick", "harry", "aloysius", "zelda", "sammy",
                             "mary", "hortense", "georgie", "ada", "daisy",
                             "paula", "alexander", "louis", "fiona", "bessie"  };

        //array of weights corresponding to these names
        static int[] wght = new int[20] { 120, 115, 195, 235, 138, 177, 163, 150, 128, 142,
                       118, 134, 255, 140, 121, 108, 170, 225, 132, 148 };


        static void Main()
        {
            string[] WKnam = new string[LSIZE];
            int[] WKwght = new int[LSIZE];

            bool done = false;
            bool oneSort = false;
            char choice;

            OutLists("UNSORTED ARRAY DATA", "NAME", "WEIGHT", nam, wght);

            Console.WriteLine("==============================================================================");
            Console.WriteLine("     YOU MUST SORT AT LEAST ONCE BEFORE SEARCHING (Press any key to continue)");
            Console.ReadLine();

            while (!done)
            {
                PutMenu();
                choice = GetChoice();
                Console.Clear();

                if(choice < '4')
                {
                    CopyLists(WKnam, WKwght);
                    DoSort(choice, WKnam, WKwght);
                    oneSort = true;
                }
                else if(choice == '4' && oneSort)
                {
                    Bsrch(WKnam, WKwght, "Some Name");
                    done = true;
                }
                else
                {
                    Console.WriteLine("==============================================================================");
                    Console.WriteLine("     YOU MUST SORT AT LEAST ONCE BEFORE SEARCHING (Press any key to continue)");
                    Console.ReadLine();
                }
            }

            Console.ReadKey();

        }

        static void PutMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================================================================");
            Console.WriteLine("     Please choose one:");
            Console.WriteLine("\n\n\n");
            Console.WriteLine("         1: Shell");
            Console.WriteLine("         2: Insert");
            Console.WriteLine("         3: Selection\n");
            Console.WriteLine("         4: Search (Must perform at least one sort before)\n");
        }

        static char GetChoice()
        {
            string inChoice;
            inChoice = Console.ReadLine();
            char c = inChoice[0];
            while (c < '1' || c > '4')
            {
                Console.Clear();
                Console.WriteLine("The answer provided does not fit within the range of options");
                Console.WriteLine("Press any key to continue");
                Console.ReadLine();
                PutMenu();
                inChoice = Console.ReadLine();
                c = inChoice[0];
            }
            return c;
        }

        static void CopyLists(string[] WKnam, int[] WKwght)
        {
            WKnam = nam;
            WKwght = wght;
        }

        #region Sorts
        static void DoSort(char c, string[] n, int[] w)
        {
            switch (c)
            {
                case '1':
                    {
                        ShellSort(n, w);
                        break;
                    }
                case '2':
                    {
                        InsertSort(n, w);
                        break;
                    }
                case '3':
                    {
                        SelectSort(n, w);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        static void InsertSort(string[] n, int[] w)
        {
            Console.WriteLine("InsertSort (Out of Order)");
            Console.ReadLine();
        }

        static void SelectSort(string[] n, int[] w)
        {
            Console.WriteLine("SelectSort (Out of Order)");
            Console.ReadLine();
        }

        static void ShellSort(string[] n, int[] w)
        {
            Console.WriteLine("ShellSort (Out of Order)");
            Console.ReadLine();
        }
        #endregion

        static void OutLists(string t, string h1, string h2, string[] n, int[] w)
        {
            Console.WriteLine(t.PadLeft(30));
            Console.WriteLine();

            Console.Write("NAME".PadLeft(15));
            Console.WriteLine("WEIGHT".PadLeft(15));
            Console.WriteLine("=======================================");

            for (int i = 0; i < LSIZE; i++)
            {
                Console.WriteLine(n[i].PadLeft(15) + w[i].ToString().PadLeft(15));
            }

            Console.WriteLine("=======================================");


            Console.WriteLine();
            Console.WriteLine();
        }

        static int Bsrch(string[] WKnam, int[] WKwght, string s)
        {
            Console.WriteLine("Search (Out of Order)");
            return 0;
        }

    }
}
