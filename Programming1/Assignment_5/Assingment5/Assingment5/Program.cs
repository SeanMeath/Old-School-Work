using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            do
            {
                bool success = false;
                Console.WriteLine("Main Menu");
                Console.WriteLine("1.   Students & Grades");
                Console.WriteLine("2.   Rainfall Statistics");
                Console.WriteLine("3.   Payroll");
                Console.WriteLine("4.   Quit this menu");
                Console.WriteLine("");
                Console.WriteLine("Enter youre choice");
                success = int.TryParse(Console.ReadLine(), out choice);
                while (success == false)
                {
                    Console.WriteLine("Error! Invalid number was entered please try again.");
                    success = int.TryParse(Console.ReadLine(), out choice);
                }
                switch (choice)

                {
                    case 1:
                        {
                            StudentsAndTests();
                            break;
                        }
                    case 2:
                        {
                            Rainfall();
                            break;
                        }
                    case 3:
                        {
                            Payroll();
                            break;
                        }
                    case 4:
                        {
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Error! Invalid number was entered please try again.");
                            break;

                        }

                }
            } while (choice != 4);






        }

        static void StudentsAndTests()                                          //Case1
        {
            Console.Clear();
            int StudentsNumb;
            int NumbTests;
            double Sum = 0;
            double Avg;
            bool success = false;
            int Counter;
            int Counter2;
            int Max = 100;
            int TestAnswer;
            Console.WriteLine("Please enter the number of students.");
            success = int.TryParse(Console.ReadLine(), out StudentsNumb);
            while (success = false || StudentsNumb <= 0)
            {
                Console.WriteLine("Error! Answer entered is not valid. Please try again.");
                success = int.TryParse(Console.ReadLine(), out StudentsNumb);
            }
            Console.WriteLine("Please enter the number of tests.");
            success = int.TryParse(Console.ReadLine(), out NumbTests);
            while (success = false || NumbTests <= 0)
            {
                Console.WriteLine("Error! Answer entered is not valid. Please try again.");
                success = int.TryParse(Console.ReadLine(), out NumbTests);
            }
            for (Counter = 0; Counter < StudentsNumb; Counter++)
            {
                for (Counter2 = 0; Counter2 < NumbTests; Counter2++)
                {
                    Console.WriteLine("Please enter the grade obtained by student #" + (Counter + 1) + " in test #" + (Counter2 + 1) + ".");
                    success = int.TryParse(Console.ReadLine(), out TestAnswer);
                    while (success == false || TestAnswer < 0 || TestAnswer > Max)
                    {
                        Console.WriteLine("Error! Answer entered is not valid. Please try again.");
                        success = int.TryParse(Console.ReadLine(), out TestAnswer);
                    }
                    Sum = TestAnswer + Sum;
                }
                Avg = Sum / NumbTests;
                Console.WriteLine("For student #" + (Counter + 1) + ", the average is " + Avg + "% and the sum of his grades is " + Sum + ".");
                Sum = 0;
                TestAnswer = 0;
                Avg = 0;
            }
            Console.WriteLine("The case you chose is now over. Please press <enter> in order to return to the menu.");
            Console.ReadLine();
            Console.Clear();
        }

        static void Rainfall()                                                  //Case2
        {
            Console.Clear();
            double AvgRain;
            int MaxRain;
            int MinRain;
            int TotalRain;
            const int ARRAYSIZE = 12;
            int Counter;
            int[] RainFall = new int[ARRAYSIZE];
            bool success = false;
            for (Counter = 0; Counter < ARRAYSIZE; Counter++)
            {
                Console.WriteLine("Please enter the amount of rain that fell in month #" + (Counter + 1) + " in milimeters.");
                success = int.TryParse(Console.ReadLine(), out RainFall[Counter]);
                while (success == false || RainFall[Counter] < 0)
                {
                    Console.WriteLine("Error! Answer entered is not valid. Please try again.");
                    success = int.TryParse(Console.ReadLine(), out RainFall[Counter]);
                }
            }
            TotalRain = GetRain(RainFall, ARRAYSIZE);
            AvgRain = GetAvg(TotalRain, ARRAYSIZE);
            MaxRain = GetMax(RainFall, ARRAYSIZE);
            MinRain = GetMin(RainFall, ARRAYSIZE);
            Console.WriteLine("This year the average rainfall for a month is of " + AvgRain + ".");
            Console.WriteLine("The total rainfall of the year was " + TotalRain + ".");
            Console.WriteLine("The month with the least amount of rain was #" + (MinRain + 1) + ".");
            Console.WriteLine("The month with the least amount of rain was #" + (MaxRain + 1) + ".");
            Console.WriteLine();
            Console.WriteLine("The case you chose is now over. Please press <enter> in order to return to the menu.");
            Console.ReadLine();
            Console.Clear();
        }

        static int GetRain(int[] RainFall, int da)
        {
            int Total = 0;
            int Counter;
            for (Counter = 0; Counter < da; Counter++)
            {
                Total = Total + RainFall[Counter];
            }
            return Total;
        }

        static double GetAvg(double da, double db)
        {
            double Avg;
            Avg = da / db;
            return Avg;
        }

        static int GetMax(int[] RainFall, int da)
        {
            int Max = 0;
            int Counter;
            for (Counter = 0; Counter < da; Counter++)
            {
                if (RainFall[Counter] > Max)
                {
                    Max = Counter;
                }
            }
            return Max;
        }

        static int GetMin(int[] RainFall, int da)
        {
            int Min = RainFall[0];
            int Counter;
            for (Counter = 0; Counter < da; Counter++)
            {
                if (RainFall[Counter] <= Min)
                {
                    Min = Counter;
                }
            }
            return Min;
        }

        static void Payroll()                                                   //Case3
        {
            Console.Clear();
            int ARRAYSIZE = 7;
            int[] IDs = new int[ARRAYSIZE];
            int[] Hours = new int[ARRAYSIZE];
            double[] PayRate = new double[ARRAYSIZE];
            double[] GrossWages = new double[ARRAYSIZE];
            bool success = false;
            int Counter = 0;
            IDs[Counter] = 56588;
            Counter = (Counter + 1);
            IDs[Counter] = 45201;
            Counter = (Counter + 1);
            IDs[Counter] = 78951;
            Counter = (Counter + 1);
            IDs[Counter] = 87775;
            Counter = (Counter + 1);
            IDs[Counter] = 84512;
            Counter = (Counter + 1);
            IDs[Counter] = 13028;
            Counter = (Counter + 1);
            IDs[Counter] = 75804;
            for (Counter = 0; Counter < ARRAYSIZE; Counter++)
            {
                Console.WriteLine("Please enter the hours worked by employee #" + (IDs[Counter]) + ".");
                success = int.TryParse(Console.ReadLine(), out Hours[Counter]);
                while (success == false || Hours[Counter] < 0)
                {
                    Console.WriteLine("Error! The number entered is not  valid answer please try again.");
                    success = int.TryParse(Console.ReadLine(), out Hours[Counter]);
                }
            }
            for (Counter = 0; Counter < ARRAYSIZE; Counter++)
            {
                Console.WriteLine("Please enter the payrate of employee #" + (IDs[Counter]) + ".");
                success = double.TryParse(Console.ReadLine(), out PayRate[Counter]);
                while (success == false || PayRate[Counter] <= 0)
                {
                    Console.WriteLine("Error! The number entered is not  valid answer please try again.");
                    success = double.TryParse(Console.ReadLine(), out PayRate[Counter]);
                }
            }
            for (Counter = 0; Counter < ARRAYSIZE; Counter++)
            {
                GrossWages[Counter] = Hours[Counter] * PayRate[Counter];
            }
            PrintInfo(IDs, GrossWages, ARRAYSIZE);
        }
        static void PrintInfo(int[] Ids, double[] GrossWages, int ARRAYSIZE)
        {
            int Counter;
            for (Counter = 0; Counter < ARRAYSIZE; Counter++)
            {
                Console.WriteLine("The employee #" + Ids[Counter] + " earned a gross pay of $" + GrossWages[Counter] + ".");
            }
            Console.WriteLine();
            Console.WriteLine("The case you chose is now over. Please press <enter> in order to return to the menu.");
            Console.ReadLine();
            Console.Clear();
        }

    }

}


