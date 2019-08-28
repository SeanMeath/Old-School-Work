using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Timestamp__Class_
{
    class Program
    {

        public static TimeStamp tsOne = new TimeStamp();
        public static TimeStamp tsTwo = new TimeStamp();
        public static TimeStamp tsThree = new TimeStamp();
        public static int someSeconds;

        static void Main(string[] args)
        {
            Write("Please enter the number of seconds to convert:   ");
            someSeconds = GetPositiveInt();
            WriteLine(someSeconds + " seconds converted to the following TimeStamp:");
            tsOne.ConvertFromSeconds(someSeconds);
            WriteLine(tsOne.ToString());
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            tsOne.ReadFromUser();
            tsOne.ToString();
            WriteLine(" converted to " + tsOne.ConvertToSeconds() + " seconds");
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            Write("Please enter the number of seconds to add to ");
            Write(tsOne.ToString());
            Write(":   ");
            someSeconds = GetPositiveInt();
            Write(tsOne.ToString());
            Write(" + " + someSeconds + " seconds = ");
            tsOne += someSeconds;
            WriteLine(tsOne.ToString());
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            WriteLine("Please enter a TimeStamp");
            tsOne.ReadFromUser();
            WriteLine("Please enter another TimeStamp");
            tsTwo.ReadFromUser();
            #region Relations Between tsOne and tsTwo
            if (tsOne == tsTwo)
            {
                WriteLine("{0} == {1}", tsOne.ToString(), tsTwo.ToString());
            }
            else
            {
                WriteLine("{0} != {1}", tsOne.ToString(), tsTwo.ToString());

                if (tsOne > tsTwo)
                {
                    WriteLine("{0} > {1}", tsOne.ToString(), tsTwo.ToString());
                }
                else
                {
                    WriteLine("{0} < {1}", tsOne.ToString(), tsTwo.ToString());
                }
            }
            #endregion
            ReadKey();
            Clear();

            Write(tsOne.ToString());
            Write(" + ");
            Write(tsTwo.ToString());
            Write(" = ");
            tsThree = TimeStamp.AddTwoTS(tsOne, tsTwo);
            Write(tsThree.ToString());
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();
        }

        static int GetPositiveInt()
        {
            bool Success = int.TryParse(ReadLine(), out int TheNumber);

            #region Validate integer

            while (!Success || TheNumber <= 0)
            {
                Clear();
                Write("Error! Please enter a positive integer:   ");
                Success = int.TryParse(ReadLine(), out TheNumber);
            }
            #endregion

            return TheNumber;
        }
    }
}
