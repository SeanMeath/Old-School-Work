using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace TimeStamp__Structure_
{
    class Program
    {
        //info for later

        // Writeline(number.ToString().PadLeft(Number, Filler)
        //For the 00

        public const int AMOUNTOFSECONDSINADAY = 86400;
        public const int AMOUNTOFSECONDSINANHOUR = 3600;
        public const int AMOUNTOFSECONDSINAMINUTE = 60;
        public const int MAXHOURS = 24;
        public const int MAX_MINUTES_SECONDS=60;

        public static TimeStamp tsOne;
        public static TimeStamp tsTwo;
        public static int someSeconds;

        public struct TimeStamp
        {
            public int hours;
            public int minutes;
            public int seconds;
        }

        static void Main(string[] args)
        {
            Write("Please enter the number of seconds to convert:   ");
            someSeconds = GetPositiveInt();
            WriteLine(someSeconds + " seconds converted to the following TimeStamp:");
            PrintTS(ConvertSecondsToTS(someSeconds));
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            WriteLine("Please enter a TimeStamp");
            tsOne = ReadTSFromUser();
            PrintTS(tsOne);
            Write(" converted to " + ConvertTSToSeconds(tsOne) + " seconds");
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            Write("Please enter the number of seconds to add to ");
            PrintTS(tsOne);
            Write(":   ");
            someSeconds = GetPositiveInt();
            PrintTS(tsOne);
            Write(" + " + someSeconds + " seconds = ");
            PrintTS(AddSecondsToTS(tsOne, someSeconds));
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

            WriteLine("Please enter a TimeStamp");
            tsOne = ReadTSFromUser();
            WriteLine("Please enter another TimeStamp");
            tsTwo = ReadTSFromUser();
            PrintTS(tsOne);
            Write(" + ");
            PrintTS(tsTwo);
            Write(" = ");
            PrintTS(AddTwoTS(tsOne, tsTwo));
            WriteLine("\nPress any key to continue.");
            ReadKey();
            Clear();

        }

        #region Functions
        static TimeStamp ConvertSecondsToTS(int SecondsToConvert)
        {
            #region Populate the TS
            TimeStamp TheTimeStamp = new TimeStamp();
            while (SecondsToConvert >= AMOUNTOFSECONDSINADAY)
            {
                SecondsToConvert -= AMOUNTOFSECONDSINADAY;
            }
            TheTimeStamp.hours = SecondsToConvert / AMOUNTOFSECONDSINANHOUR;
            SecondsToConvert = SecondsToConvert % AMOUNTOFSECONDSINANHOUR;
            TheTimeStamp.minutes = SecondsToConvert / AMOUNTOFSECONDSINAMINUTE;
            TheTimeStamp.seconds = SecondsToConvert % AMOUNTOFSECONDSINAMINUTE;
            #endregion

            return TheTimeStamp;
        }

        static int ConvertTSToSeconds(TimeStamp TheTimeStamp)
        {
            #region Calculate the number of seconds
            int ConvertedSeconds = TheTimeStamp.hours * AMOUNTOFSECONDSINANHOUR;
            ConvertedSeconds += TheTimeStamp.minutes * AMOUNTOFSECONDSINAMINUTE;
            ConvertedSeconds += TheTimeStamp.seconds;
            #endregion

            return ConvertedSeconds;
        }

        static void PrintTS(TimeStamp TheTimeStamp)
        {
            Write("{0}:{1}:{2}", TheTimeStamp.hours.ToString().PadLeft(2, '0'), TheTimeStamp.minutes.ToString().PadLeft(2, '0'), TheTimeStamp.seconds.ToString().PadLeft(2, '0'));
        }

        static TimeStamp AddSecondsToTS(TimeStamp TheTimeStamp, int TheSeconds)
        {
            #region Add to the TS
            int Seconds = ConvertTSToSeconds(TheTimeStamp);
            Seconds += TheSeconds;
            TheTimeStamp = ConvertSecondsToTS(Seconds);
            #endregion

            return TheTimeStamp;
        }

        static TimeStamp ReadTSFromUser()
        {
            bool Success;
            TimeStamp TheTimeStamp = new TimeStamp();

            #region Populate the TimeStamp
            Write("Please enter the hours (0.." + (MAXHOURS - 1) + "):   ");
            Success = int.TryParse(ReadLine(), out TheTimeStamp.hours);
            #region Hour validation
            while (!Success || TheTimeStamp.hours < 0 || TheTimeStamp.hours >= MAXHOURS)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than "+MAXHOURS+":   ");
                Success = int.TryParse(ReadLine(), out TheTimeStamp.hours);
            }
            Clear();
            #endregion
            WriteLine("Please enter a TimeStamp");
            Write("Please enter the minutes (0.." + (MAX_MINUTES_SECONDS) + "):   ");
            Success = int.TryParse(ReadLine(), out TheTimeStamp.minutes);
            #region Minute validation
            while (!Success || TheTimeStamp.minutes < 0 || TheTimeStamp.minutes >= MAX_MINUTES_SECONDS)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than "+MAX_MINUTES_SECONDS+":   ");
                Success = int.TryParse(ReadLine(), out TheTimeStamp.minutes);
            }
            Clear();
            #endregion
            WriteLine("Please enter a TimeStamp");
            Write("Please enter the seconds (0.." + (MAX_MINUTES_SECONDS) + "):   ");
            Success = int.TryParse(ReadLine(), out TheTimeStamp.seconds);
            #region Second validation
            while (!Success || TheTimeStamp.seconds < 0 || TheTimeStamp.seconds >= MAX_MINUTES_SECONDS)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than " + MAX_MINUTES_SECONDS + ":   ");
                Success = int.TryParse(ReadLine(), out TheTimeStamp.seconds);
            }
            Clear();
            #endregion
            #endregion

            return TheTimeStamp;
        }

        static TimeStamp AddTwoTS(TimeStamp TimeStampOne, TimeStamp TimeStampTwo)
        {
            #region Add the TimeStamps
            int Seconds = ConvertTSToSeconds(TimeStampOne);
            Seconds += ConvertTSToSeconds(TimeStampTwo);
            TimeStampOne = ConvertSecondsToTS(Seconds);
            #endregion

            return TimeStampOne;
        }

        static int GetPositiveInt()
        {
            bool Success = int.TryParse(ReadLine(), out int TheNumber);

            #region Validate integer

            while (!Success || TheNumber <= 0)
            {
                Write("Error! Please enter a positive integer:   ");
                Success = int.TryParse(ReadLine(), out TheNumber);
            }
            #endregion

            return TheNumber;
        }
        #endregion
    }
}
