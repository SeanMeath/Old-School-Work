
/*********************************************************************************************************************************************************
 * 
 *      Due Date: October       22nd
 *      Software Designer:      Sean Meath
 *      Course:                 420-306-AB      Section 2
 *      Deliverable:            Assignment 3 -- Sorting and Searching
 *      
 *      Description:            This program works with two parallel arrays (one containing names and one containing weights) describing people. The
 *                              program sorts the arrays by different methods (chosen by the user of either Insert, Selection, Shell) using the name
 *                              array to compare (sorts alphabetically) any number of times. Then the user can search the now sorted array to find a
 *                              requested person.
 *                              
 ********************************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sMEAT___a3v5
{
    class Program
    {   const int NMAX = 10;          //maximum size of each name string
        const int LSIZE = 20;         //number of actual name strings in array

        //array of name strings
        static string[] nam = new string[20] { "wendy", "ellen", "freddy", "tom", "susan",
                             "dick", "harry", "aloysius", "zelda", "sammy",
                             "mary", "hortense", "georgie", "ada", "daisy",
                             "paula", "alexander", "louis", "fiona", "bessie"  };

        //array of weights corresponding to these names
        static int[] wght = new int[20] { 120, 115, 195, 235, 138, 177, 163, 150, 128, 142,
                       118, 134, 255, 140, 121, 108, 170, 225, 132, 148 };

        static void Main()                                                                                                              //Create:
        {   string[] WKnam = new string[LSIZE];                                                                                         // -Work Array (Name)
            int[] WKwght = new int[LSIZE];                                                                                              // -Work Array (Wght)
            bool done = false;                                                                                                          // -End Loop Var
            bool oneSort = false;                                                                                                       // -Allow Search
            char choice;                                                                                                                // -Chosen Sort
            OutLists("UNSORTED ARRAY DATA", "NAME", "WEIGHT", nam, wght);                                                               //Show Unsorted List
            Console.WriteLine("========================================================================");
            Console.WriteLine("YOU MUST SORT AT LEAST ONCE BEFORE SEARCHING (Press any key to continue)");                              //Show Requirement
            Console.ReadLine();                                                                                                         //Aknowleged
            while (!done)                                                                                                               //Check if Searched
            {   PutMenu();                                                                                                              // Menu
                choice = GetChoice();                                                                                                   // Menu Opt Chosen
                Console.Clear();
                if(choice < '4')                                                                                                        //To be Sorted
                {   CopyLists(WKnam, WKwght);                                                                                           //Copy Arrays
                    DoSort(choice, WKnam, WKwght);                                                                                      //Chosen Sort Exe
                    oneSort = true;                                                                                                     //Sorted (Can Srch)
                }
                else if(choice == '4' && oneSort)                                                                                       //To Search
                {   Console.WriteLine("Please enter the person you wish to search for:");                                               //Ask Name
                    string xnam = Console.ReadLine();                                                                                   //Receive Name
                    Console.Clear();
                    int place = Bsrch(WKnam, WKwght, xnam);                                                                             //Search
                    if (place >= 0)                                                                                                     //Found
                        Console.WriteLine(WKnam[place]+" was found at position "+place+" and his/her weight is "+WKwght[place]+".");    //Output Result
                    else                                                                                                                //Not Found
                        Console.WriteLine(xnam + " was not found.");                                                                    //Prompt User
                    done = true;                                                                                                        //Searched/EndLoop
                }
                else                                                                                                                    //Requirement ! Met
                {   Console.WriteLine("========================================================================");
                    Console.WriteLine("YOU MUST SORT AT LEAST ONCE BEFORE SEARCHING (Press any key to continue)");                      //Show Requirement
                    Console.ReadLine();                                                                                                 //Aknowleged
                }
            }
            Console.ReadKey();                                                                                                          //Aknowleged
        }

        static void PutMenu()                                                                                                           //Display Menu
        {   Console.Clear();
            Console.WriteLine("========================================================================");
            Console.WriteLine("     Please choose one:\n\n\n");
            Console.WriteLine("         1: Insert");
            Console.WriteLine("         2: Selection");
            Console.WriteLine("         3: Shell");
            Console.WriteLine("         4: Search (Must perform at least one sort before)\n");
            Console.WriteLine("         Empty: Close the Application");
        }

        static char GetChoice()                                                                                                         
        {   string inChoice = Console.ReadLine();                                                                                       //Input
            if(inChoice == "")                                                                                                          //Empty Input
                Environment.Exit(0);                                                                                                    //Avoid Crash
            char c = inChoice[0];                                                                                                       //Possible Choice
            while (c < '1' || c > '4')                                                                                                  //Check Requirements
            {   Console.Clear();
                Console.WriteLine("The answer provided does not fit within the range of options Press any key to continue");            //Show Requirement                                                                  
                Console.ReadLine();                                                                                                     //Aknowleged
                PutMenu();                                                                                                              //Menu
                inChoice = Console.ReadLine();                                                                                          //Input
                if (inChoice == "")                                                                                                     //Empty Input
                    Environment.Exit(0);                                                                                                //Avoid Crash
                c = inChoice[0];                                                                                                        //Possible Choice
            }
            return c;                                                                                                                   //Send Choice
        }

        static void CopyLists(string[] WKnam, int[] WKwght)
        {
            Array.Copy(nam, WKnam, LSIZE);                                                                                              //Copy Name Arr
            Array.Copy(wght, WKwght, LSIZE);                                                                                            //Copy Weight Arr
        }

        static void DoSort(char c, string[] n, int[] w)
        {   switch (c)                                                                                                                  //Choice Opt
            {   case '1':
                    {   InsertSort(n, w);                                                                                               //Sorts (Insert)
                        break;
                    }
                case '2':
                    {   SelectSort(n, w);                                                                                               //Sorts (Select)
                        break;
                    }
                case '3':
                    {   ShellSort(n, w);                                                                                                //Sorts (Shell)
                        break;
                    }
                default:
                        break;
            }
            OutLists("SORTED ARRAY DATA", "NAME", "WEIGHT", n, w);                                                                      //Output Sorted
            Console.ReadLine();                                                                                                         //Aknowleged
        }

        static void InsertSort(string[] n, int[] w)
        {   int k = 1, WH, i;
            string NH;
            bool found;
            do
            {   NH = n[k];
                WH = w[k];
                i = k - 1;
                found = false;
                while (i >= 0 && !found)
                {   if (NH.CompareTo(n[i]) < 0)
                    {   n[i + 1] = n[i];
                        w[i + 1] = w[i];
                        i--;
                    }
                    else
                        found = true;
                }
                n[i + 1] = NH;
                w[i + 1] = WH;
                k++;
            } while (k <= (LSIZE - 1));
        }

        static void SelectSort(string[] n, int[] w)
        {   int i = (LSIZE-1), where, bigW, j;                                                                                          //Create Vars
            string bigN;                                                                                                                //Big Name
            do
            {   bigN = n[0];                                                                                                            //Reset Var Big Name
                bigW = w[0];                                                                                                            //Reset Var Big Wght
                where = 0;                                                                                                              //Reset Position
                j = 1;                                                                                                                  //Reset Counter
                do
                {   if (bigN.CompareTo(n[j]) < 0)                                                                                       //Comapared is Bigger
                    {   bigN = n[j];                                                                                                    //New Big Name
                        bigW = w[j];                                                                                                    //New Big Wheight
                        where = j;                                                                                                      //New Position
                    }
                    j++;                                                                                                                //Increase Counter
                } while (j <= i);                                                                                                       //Valid Counter
                n[where] = n[i];                                                                                                        //Move Old Biggest N
                w[where] = w[i];                                                                                                        //Move Old Biggest W
                n[i] = bigN;                                                                                                            //Move New Biggest N
                w[i] = bigW;                                                                                                            //Move New Biggest W
                i--;                                                                                                                    //Lower Max Position
            } while (i > 0);                                                                                                            //All Max Position
        }

        static void ShellSort(string[] n, int[] w)
        {   int numgaps = 0;
            int[] gaplist = GetGaps(ref numgaps);
            int gap, j, k, WH, i = numgaps - 1;
            string NH;
            bool found;
            do
            {   gap = gaplist[i];
                j = gap;
                do
                {   NH = n[j];
                    WH = w[j];
                    k = j - gap;
                    found = false;
                    while (k >= 0 && !found)
                    {   if (NH.CompareTo(n[k]) < 0)
                        {   n[k + gap] = n[k];
                            w[k + gap] = w[k];
                            k -= gap;
                        }
                        else
                            found = true;
                    }
                    n[k + gap] = NH;
                    w[k + gap] = WH;
                    j += 1;
                } while (j <= (LSIZE - 1));
                i --;
            } while (i >= 0);
        }

        static int[] GetGaps(ref int n)
        {   int gap = 1;
            int[] g = new int[LSIZE];
            while (gap <= LSIZE)
            {   g[n] = gap;
                gap *= 3;
                n += 1;
            }
            return g;
        }

        static void OutLists(string t, string h1, string h2, string[] n, int[] w)
        {   Console.WriteLine(t.PadLeft(30));                                                                                           //Show Title
            Console.Write("\nNAME".PadLeft(15));                                                                                        //Show Column 1
            Console.WriteLine("WEIGHT".PadLeft(15));                                                                                    //Show Column 2
            Console.WriteLine("=======================================");
            for (int i = 0; i < LSIZE; i++)                                                                                             //For Each Row
               Console.WriteLine(n[i].PadLeft(15) + w[i].ToString().PadLeft(15));                                                       //Display Row
            Console.WriteLine("=======================================");
            Console.WriteLine("\n");
        }

        static int Bsrch(string[] WKnam, int[] WKwght, string xnam)
        {   int q = -1, a = 0, b = (LSIZE - 1), mid;                                                                                    //Create Vars
            while (a <= b)                                                                                                              //Hi is Highest
            {   mid = ((a + b) / 2);                                                                                                    //New Mid
                if(xnam.CompareTo(WKnam[mid]) == 0)                                                                                     //Search Found
                {   q = mid;                                                                                                            //Found Position
                    a = LSIZE;                                                                                                          //Force Exit Loop
                }
                else if(xnam.CompareTo(WKnam[mid]) < 0)                                                                                 //Search is Bigger
                    b = mid - 1;                                                                                                        //Reduce Max
                else
                    a = mid + 1;                                                                                                        //Increase Min
            }
            return q;                                                                                                                   //Return Position
        }
    }
}
