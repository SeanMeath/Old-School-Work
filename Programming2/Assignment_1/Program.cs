using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Assignment_1_Part_2
{
    class Program
    {
        
        /*Question 0:
         * The Main Menu
         * Displays the choices of functions and allows the user to navigate between them.
         * The user is returned to this screen after each function is finished.
         * 
         * Tester: MOM
         */

        #region MainMenu
        static void Main(string[] args)
        {
            int Choice;
            bool Success;

            do
            {
                Clear();
                Menu();
                WriteLine("Please enter the program you wish to run.\n");

                Success = int.TryParse(ReadLine(), out Choice);

                while (Success == false)
                {
                    Clear();
                    WriteLine("Error! The answer given is not a valid response. Please try again.");
                    Menu();
                    Success = int.TryParse(ReadLine(), out Choice);
                }

                Clear();

                switch (Choice)
                {
                    case 1:
                        {
                            RectangleArea();
                            break;
                        }
                    case 2:
                        {
                            SafestDrivingArea();
                            break;
                        }
                    case 3:
                        {
                            Population();
                            break;
                        }
                    case 4:
                        {
                            PaintJobEstimator();
                            break;
                        }
                    case 5:
                        {
                            Prime();
                            break;
                        }
                    case 6:
                        {
                            RockPaperScisors();
                            break;
                        }
                    case 7:
                        {
                            break;
                        }
                    default:
                        {
                            WriteLine("The answer given does not constitute an option on the menu. Please try again.");
                            ReadKey();
                            break;
                        }
                }
            }
            while (Choice != 7);
        }
        static void Menu()
        {
            WriteLine("Main Menu:\n");
            WriteLine("1: Rectangle Area\n2: Safest Driving Area\n3: Population\n4: Paint Job Estimator\n5: Is Prime\n6: Rock, Paper, Scisors\n7: Exit \n");
        }
        #endregion MainMenu

        /*Question 1:
         * Rectangle Area
         * Asks the user for the dimentions of the rectangle and computes the area of it.
         * It then displays this for the user.
         * 
         * Tester: MOM
         */

        #region RectangleArea
        static void RectangleArea()
        {
            double length;
            double width;
            double area;

            length = GetLength();
            width = GetWidth();
            area = GetArea(length, width);

            DisplayData(length, width, area);

        }
        static double GetLength()
        {
            bool success;
            double length;
            bool pass=false;
            WriteLine("What is the length of the rectangle?\n");
            success = double.TryParse(ReadLine(), out length);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nWhat is the length of the rectangle?\n");
                    success = double.TryParse(ReadLine(), out length);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nWhat is the length of the rectangle?\n");
                    success = double.TryParse(ReadLine(), out length);
                }
                pass = true;
            }
            while (length<=0);
            return length;
        }
        static double GetWidth()
        {
            bool success;
            double width;
            bool pass = false;
            Clear();
            WriteLine("What is the width of the rectangle?\n");
            success = double.TryParse(ReadLine(), out width);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nWhat is the width of the rectangle?\n");
                    success = double.TryParse(ReadLine(), out width);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nWhat is the width of the rectangle?\n");
                    success = double.TryParse(ReadLine(), out width);
                }
                pass = true;
            }
            while (width <= 0);
            return width;
        }
        static double GetArea(double l, double w)
        {
            double area;
            area = l * w;
            return area;
        }
        static void DisplayData(double l, double w, double a)
        {
            Clear();
            WriteLine("The length of the rectangle comes to:" + l);
            WriteLine("The width of the rectangle comes to:" + w);
            WriteLine("The area of the rectangle comes to:" + a);
            WriteLine("\nPress any key to return to the main menu.");
            ReadKey();
        }
        #endregion RectangleArea

        /*Question 2:
         * Safest Driving Area
         * Asks the user to enter in the number of accidents. It then computes the safest area.
         * It then displays this for the user.
         * 
         * Tester: MOM
         */

        #region SafestDrivingArea
        static void SafestDrivingArea()
        {
            const int ARRAYSIZE = 5;
            int lowest;
            string[] region = new string[ARRAYSIZE];
            int[] accidents = new int[ARRAYSIZE];
            region[0] = "north";
            region[1] = "south";
            region[2] = "east";
            region[3] = "south";
            region[4] = "central";
            for (int counter = 0; counter < ARRAYSIZE; counter += 1)
            {
                accidents[counter] = GetNumAccidents(region[counter]);
            }
            lowest = FindLowest(accidents, ARRAYSIZE);
            WriteLine("The safest region would be " + region[lowest] + ".");
            WriteLine("It only had " + accidents[lowest] + " accident(s).\n\nPress any key to return to the main menu.");
            ReadKey();
        }
        static int GetNumAccidents(string region)
        {
            bool success;
            bool pass = false;
            int accidents;
            Clear();
            WriteLine("What was the total number of reported accidents in the " + region + " region.");
            success = int.TryParse(ReadLine(), out accidents);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (0 and above). Please try again.\nWhat was the total number of reported accidents in the " + region + " region.\n");
                    success = int.TryParse(ReadLine(), out accidents);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid integer. Please try again.\nWhat was the total number of reported accidents in the " + region + " region.\n");
                    success = int.TryParse(ReadLine(), out accidents);
                }
                pass = true;
            }
            while (accidents < 0);
            return accidents;     

        }
        static int FindLowest(int[] accidents, int ARRAYSIZE)
        {
            int lowest = accidents[0];
            int min=0;
            for(int counter=0; counter<ARRAYSIZE; counter += 1)
            {
                if (accidents[counter] < lowest)
                {
                    lowest = accidents[counter];
                    min = counter;
                }
            }
            return min;
        }
        #endregion SafestDrivingArea

        /*Question 3:
         * Population
         * Asks the user for the population, birth rate, death rate and the number of years and computes the population over time.
         * It then displays this to the user.
         * 
         * Tester: MOM
         */

        #region Population
        static void Population()
        {
            bool success;
            bool pass = false;
            int population;
            int years;
            double currentPop;
            double deathRate;
            double birthRate;
            const string format = "{0, -10}|{1, -25}";

            Clear();
            WriteLine("How many people are their in the starting population pool?\n");
            success = int.TryParse(ReadLine(), out population);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nHow many people are their in the starting population pool?\n");
                    success = int.TryParse(ReadLine(), out population);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid integer. Please try again.\nHow many people are their in the starting population pool?\n");
                    success = int.TryParse(ReadLine(), out population);
                }
                pass = true;
            }
            while (population <= 0);
            pass = false;
            Clear();
            WriteLine("What is the death rate of the population/year?\n");
            success = double.TryParse(ReadLine(), out deathRate);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nWhat is the death rate of the population/year?\n");
                    success = double.TryParse(ReadLine(), out deathRate);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nWhat is the death rate of the population/year?\n");
                    success = double.TryParse(ReadLine(), out deathRate);
                }
                pass = true;
            }
            while (deathRate <= 0);
            pass = false;
            Clear();
            WriteLine("What is the birth rate of the population/year?\n");
            success = double.TryParse(ReadLine(), out birthRate);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nWhat is the birth rate of the population/year?\n");
                    success = double.TryParse(ReadLine(), out birthRate);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nWhat is the birth rate of the population/year?\n");
                    success = double.TryParse(ReadLine(), out birthRate);
                }
                pass = true;
            }
            while (birthRate <= 0);
            pass = false;
            Clear();
            WriteLine("How many years do you wish to calculate?");
            success = int.TryParse(ReadLine(), out years);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nHow many years do you wish to calculate?\n");
                    success = int.TryParse(ReadLine(), out years);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid integer. Please try again.\nHow many years do you wish to calculate?\n");
                    success = int.TryParse(ReadLine(), out years);
                }
                pass = true;
            }
            while (years <= 0);
            Clear();
            currentPop = population;
            WriteLine(format, "Year", "Population");
            for (int counter = 1; counter <= years; counter++)
            {
                currentPop = GetCurrentPop(currentPop, deathRate, birthRate);
                currentPop = Math.Round(currentPop);
                WriteLine(format, counter, currentPop);
            }
            WriteLine("\nThe program has ended press any key to return to the main menu.");
            ReadKey();
        }
        static double GetCurrentPop(double population, double deathrate, double birthrate)
        {
            double curentPop=population+birthrate*population-deathrate*population;
            return curentPop;
        }
        #endregion Population

        /*Question 4:
         * Paint Job Estimator
         * Asks the user for the number of rooms, the area of each rooms and the cost for the paint used and computes the total cost.
         * It then displays this to the user.
         * 
         * Tester: MOM
         */

        #region PaintJobEstimator
        static void PaintJobEstimator()
        {
            const string format = "{0,10}|{1,10}|{2,10}|{3,10}|{4,10}";
            const int minCost=10;
            const int laborPerHour=18;
            int rooms;
            double gallons;
            double hours;
            double paintCost;
            double paintTotal;
            double laborCost;
            double totalCost;
            bool success;
            bool pass=false;
            WriteLine("How many rooms need to be painted?\n");
            success = int.TryParse(ReadLine(), out rooms);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nHow many rooms need to be painted?\n");
                    success = int.TryParse(ReadLine(), out rooms);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid integer. Please try again.\nHow many rooms need to be painted?\n");
                    success = int.TryParse(ReadLine(), out rooms);
                }
                pass = true;
            }
            while (rooms <= 0);
            double[] squareFeet = new double [rooms];
            for (int counter = 0; counter < rooms; counter++)
            {
                pass = false;
                Clear();
                WriteLine("How many sqaure feet need to be painted in the #"+(counter+1)+" room?\n");
                success = double.TryParse(ReadLine(), out squareFeet[counter]);
                do
                {
                    if (pass == true)
                    {
                        Clear();
                        WriteLine("Error! The answer given does not consist of a valid response (above 0). Please try again.\nHow many sqaure feet need to be painted in the #" + (counter+1) + " room?\n");
                        success = double.TryParse(ReadLine(), out squareFeet[counter]);
                    }
                    while (!success)
                    {
                        Clear();
                        WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nHow many sqaure feet need to be painted in the #" + (counter+1) + " room?\n");
                        success = double.TryParse(ReadLine(), out squareFeet[counter]);
                    }
                    pass = true;
                }
                while (squareFeet[counter] <= 0);
            }
            pass = false;
            Clear();
            WriteLine("How much does the paint used cost?\n");
            success = double.TryParse(ReadLine(), out paintCost);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid response (10$ or above). Please try again.\nHow much does the paint used cost?\n");
                    success = double.TryParse(ReadLine(), out paintCost);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The answer given does not consist of a valid double. Please try again.\nHow much does the paint used cost?\n");
                    success = double.TryParse(ReadLine(), out paintCost);
                }
                pass = true;
            }
            while (paintCost < minCost);
            Clear();
            hours = GetHours(rooms, squareFeet);
            gallons = Math.Ceiling(hours);
            paintTotal = gallons * paintCost;
            laborCost = hours * laborPerHour;
            totalCost = paintTotal + laborCost;
            WriteLine(format, "gallons", "work hours", "paint cost", "work charge", "total cost");
            WriteLine(format, gallons, Math.Round(hours, 2), Math.Round(paintTotal, 2), Math.Round(laborCost, 2), Math.Round(totalCost, 2));
            WriteLine("\nPress any key to return to the main menu.");
            ReadKey();
        }
        static double GetHours(int rooms, double[] squareFeet)
        {
            const int squareFeetPerGallon = 115;
            double hours;
            double sum = 0;
            for (int counter = 0; counter < rooms; counter++)
            {
                sum += squareFeet[counter];
            }
            hours = sum / squareFeetPerGallon;
            return hours;
        }
        #endregion PaintJobEstimator

        /*Question 5:
         * IsPrime
         * Asks the user for a number and determines wether or not the number is prime.
         * It then displays this as well as all the prime numbers up to 100.
         * 
         * Tester: MOM
         */

        #region IsPrime
        static void Prime()
        {
            int numb;
            bool isPrime;
            bool success;
            bool pass = false;
            WriteLine("Please enter a number to see if it is Prime.\n");
            success=int.TryParse(ReadLine(), out numb);
            do
            {
                if (pass == true)
                {
                    Clear();
                    WriteLine("Error! The response given must be greater than 0. Please try again./nPlease enter a number to see if it is Prime.\n");
                    success = int.TryParse(ReadLine(), out numb);
                }
                while (!success)
                {
                    Clear();
                    WriteLine("Error! The response given must consist of an integer. Please try again./nPlease enter a number to see if it is Prime.\n");
                    success = int.TryParse(ReadLine(), out numb);
                }
            }
            while (numb <= 0);
            Clear();
            isPrime = IsPrime(numb);
            if (isPrime)
            {
                WriteLine("The number " + numb + " is a prime number.");
                ReadKey();
            }
            else
            {
                WriteLine("The number " + numb + " is not a prime number.");
                ReadKey();
            }
            WriteLine("\nPress any key to print all primes bellow 100.");
            Clear();
            PrintPrimes();
            WriteLine("\nPress any key to return to the main menu.");
            ReadKey();
        }
        static bool IsPrime(int numb)
        {
            const int start = 2;
            bool isPrime=true;
            for (int counter = start; counter < numb; counter++)
            {
                if ((numb % counter) == 0)
                {
                    isPrime = false;
                }
            }
            return isPrime;
        }
        static void PrintPrimes()
        {
            bool isPrime = true;
            const int start = 2;
            const int max = 100;
            for (int c = start; c < max; c++)
            {
                for (int i = start; i < c; i++)
                {
                    if (c % i == 0)
                    {
                        isPrime = false;
                    }
                }
                if (isPrime)
                {
                    WriteLine(c);
                }
                isPrime = true;
            }
        }
        #endregion IsPrime

        /*Question 6:
         * Rock, Paper, Scisors
         * Asks the user for their option between the three choices and randomly assigns one to the computer.
         * Displays a winner.
         * 
         * Tester: MOM
         */

        #region Rock,Paper,Scisors
        static void RockPaperScisors()
        {
            Clear();
            string choice;
            int numbOfChoice;
            const int rock = 1;
            const int paper = 2;
            const int scisors = 3;
            const int randomMax = 4;
            Random Rand = new Random();
            int randomNumb = Rand.Next(1, randomMax);
            WriteLine("Please enter your choice of either ''rock'', ''paper'' or ''scisors''.");
            choice = ReadLine();
            switch (choice)
            {
                case "rock":
                    {
                        DisplayCompChoice(randomNumb, rock, paper, scisors);
                        numbOfChoice = rock;
                        DisplayWinner(randomNumb, numbOfChoice, rock, paper, scisors);
                        break;
                    }
                case "paper":
                    {
                        DisplayCompChoice(randomNumb, rock, paper, scisors);
                        numbOfChoice = paper;
                        DisplayWinner(randomNumb, numbOfChoice, rock, paper, scisors);
                        break;
                    }
                case "scisors":
                    {
                        DisplayCompChoice(randomNumb, rock, paper, scisors);
                        numbOfChoice = scisors;
                        DisplayWinner(randomNumb, numbOfChoice, rock, paper, scisors);
                        break;
                    }
                default:
                    {
                        Clear();
                        WriteLine("Error! The answer given does not fit into any of the 3 choices. Please try again.");
                        ReadKey();
                        RockPaperScisors();
                        break;
                    }
            }
        }
        static void DisplayCompChoice(int randomNumb, int rock, int paper, int scisors)
        {
            Clear();
            if (randomNumb == rock)
            {
                WriteLine("The computer chose rock.");
            }
            else if (randomNumb == paper)
            {
                WriteLine("The computer chose paper.");
            }
            else
            {
                WriteLine("The computer chose scisors.");
            }
        }
        static void DisplayWinner(int randomNumb, int numbOfChoice, int rock, int paper, int scisors)
        {
            switch (numbOfChoice)
            {
                case 1:
                    {
                        if (randomNumb == rock)
                        {
                            WriteLine("The game came to a tie.\nPlay again to determine the true winner.");
                            ReadKey();
                            RockPaperScisors();
                        }
                        else if (randomNumb == paper)
                        {
                            WriteLine("The paper wraps the rock with its strong grip!");
                            WriteLine("The computer wins!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        else
                        {
                            WriteLine("With its massive mass the rock smashes the scisors to smitherines!");
                            WriteLine("You win!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        break;
                    }
                case 2:
                    {
                        if (randomNumb == rock)
                        {
                            WriteLine("The paper wraps the rock with its strong grip!");
                            WriteLine("You win!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        else if (randomNumb == paper)
                        {
                            WriteLine("The game came to a tie.\nPlay again to determine the true winner.");
                            ReadKey();
                            RockPaperScisors();
                        }
                        else
                        {
                            WriteLine("The scisors slice the valiant paper in half!");
                            WriteLine("The computer wins!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        break;
                    }
                case 3:
                    {
                        if (randomNumb == rock)
                        {
                            WriteLine("With its massive mass the rock smashes the scisors to smitherines!");
                            WriteLine("The computer wins!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        else if (randomNumb == paper)
                        {
                            WriteLine("The scisors slice the valiant paper in half!");
                            WriteLine("You win!");
                            WriteLine("\nPress any key to return to the main menu.");
                            ReadKey();
                            Clear();
                        }
                        else
                        {
                            WriteLine("The game came to a tie.\nPlay again to determine the true winner.");
                            ReadKey();
                            RockPaperScisors();
                        }
                        break;
                    }
            }
        }
        #endregion Rock,Paper,Scisors
    }
}
 