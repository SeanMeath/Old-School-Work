using System;
using static System.Console;
using System.IO;

namespace Weather_Stats
{
    class Program
    {

        struct Month
        {
            public int totalFall;
            public int highestTemp;
            public int lowestTemp;
            public double avgTemp;
        }

        public enum Months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            Novenber,
            December
        }

        public enum Split
        {
            Falls,
            HTemps,
            LTemps
        }

        const string filename = "../../weatherStats.CSV";
        public const int ARRAYSIZE = 12;

        static void Main(string[] args)
        {
            int totalFall=0;
            double avg=0;
            Months highestMonth=0;
            Months lowestMonth=0;
            Month[] monthHolder = new Month[ARRAYSIZE];

            bool success = PopulateMonth(monthHolder);
            if (success)
            {
                GetStats(monthHolder, ref totalFall, ref highestMonth, ref lowestMonth, ref avg);
                DisplayMonthlyData(monthHolder);
                DisplayData(monthHolder, totalFall, highestMonth, lowestMonth, avg);
            }
            else
            {
                WriteLine("     The program was not able to complete its reading of the file and therefore cannot work proprely. Please fix the file and try again.");
            }
            WriteLine("\n     Press any key to end the program");
            ReadKey();
        }

        #region Populate

        static bool PopulateMonth(Month[] holder)
        {
            string[] values;
            bool success = true;
            StreamReader stream = null;

            try
            {
                stream = new StreamReader(filename);

                for (Months i = Months.January; i <= Months.December; i++)
                {

                    values = (stream.ReadLine()).Split(',');
                    success = int.TryParse(values[(int)Split.Falls], out holder[(int)i].totalFall);
                    while(!success|| holder[(int)i].totalFall<0)
                    {
                        throw new Exception();
                    }
                    success = int.TryParse(values[(int)Split.HTemps], out holder[(int)i].highestTemp);
                    while (!success)
                    {
                        throw new Exception();
                    }
                    success = int.TryParse(values[(int)Split.LTemps], out holder[(int)i].lowestTemp);
                    while (!success)
                    {
                        throw new Exception();
                    }
                    holder[(int)i].avgTemp = (holder[(int)i].highestTemp + holder[(int)i].lowestTemp) / 2;
                    success = true;
                }
            }
            catch(Exception ex)
            {
                success = false;
                WriteLine(ex.Message);
                ReadKey();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return success;
        }

        #endregion

        #region Compute

        static void GetStats(Month[] holder,ref int total, ref Months big, ref Months small, ref double avg)
        {
            for (Months i = Months.January; i <= Months.December; i++)
            {
                total += holder[(int)i].totalFall;
                if (holder[(int)i].highestTemp > holder[(int)big].highestTemp)
                {
                    big = i;
                }
                if (holder[(int)i].lowestTemp < holder[(int)small].lowestTemp)
                {
                    small = i;
                }
            }
            avg = total / ARRAYSIZE;
        }

        #endregion

        #region Display Methods

        static void DisplayMonthlyData(Month[] holder)
        {
            for (Months i = Months.January; i <= Months.December; i++)
            {
                WriteLine("     " + i + ":\n");
                WriteLine("     Total Precipitation: " + holder[(int)i].totalFall);
                WriteLine("     Highest Temperature: " + holder[(int)i].highestTemp);
                WriteLine("     Lowest Temperature: " + holder[(int)i].lowestTemp);
                WriteLine("     Average Temperature: " + holder[(int)i].avgTemp);
                WriteLine("***************************************************************");
            }
            WriteLine("     Press any key to show the computed stats.");
            ReadKey();
            Clear();
        }

        static void DisplayData(Month[] holder, int total, Months highest, Months lowest, double avg)
        {
            WriteLine("     Statistics Data:\n");
            WriteLine("     Total precipitation for the year: " + total + "cm");
            WriteLine("     The monthly average precipitation comes to: " + Math.Round(avg, 2) + "cm");
            WriteLine("     Highest Temperature goes to " + highest + " with " + holder[(int)highest].highestTemp + "C");
            WriteLine("     Lowest Temperature Month goes to " + lowest + " with " + holder[(int)lowest].lowestTemp + "C");
        }

        #endregion

        //Tested by James Lee
    }
}
