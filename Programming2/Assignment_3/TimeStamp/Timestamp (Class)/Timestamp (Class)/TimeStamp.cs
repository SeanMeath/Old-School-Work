using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Timestamp__Class_
{
    class TimeStamp
    {
        private const int AMOUNTOFSECONDSIN_MINUTE = 60;
        private const int AMOUNTOFSECONDSIN_HOUR = 3600;
        private const int AMOUNTOFSECONDSIN_DAY = 86400;
        private const int AMOUNTOFHOURSIN_DAY = 24;

        #region Backing Fields
        private int _hours;
        private int _minutes;
        private int _seconds;
        #endregion

        #region Properties
        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                if (value < 0 || value >= AMOUNTOFHOURSIN_DAY)
                {
                    throw new ArgumentException("Hours does not fit in range (0<=X>" + AMOUNTOFHOURSIN_DAY + ").");
                }
                else
                {
                    _hours = value;
                }
            }
        }

        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (value < 0 || value >= AMOUNTOFSECONDSIN_MINUTE)
                {
                    throw new ArgumentException("Minutes does not fit in range (0<=X>" + AMOUNTOFSECONDSIN_MINUTE + ").");
                }
                else
                {
                    _minutes = value;
                }
            }
        }

        public int Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                if (value < 0 || value >= AMOUNTOFSECONDSIN_MINUTE)
                {
                    throw new ArgumentException("Seconds does not fit in range (0<=X>" + AMOUNTOFSECONDSIN_MINUTE + ").");
                }
                else
                {
                    _seconds = value;
                }
            }
        }
        #endregion

        #region Constructors
        public TimeStamp()
        {
            Hours = 0;
            Minutes = 0;
            Seconds = 0;
        }

        public TimeStamp(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }
        #endregion

        #region Other Methods
        public void ConvertFromSeconds(int SecondsToConvert)
        {
            //Set hours, minutes and seconds of the current TimeStamp instance based on secondsToConvert.

            while (SecondsToConvert >= AMOUNTOFSECONDSIN_DAY)
            {
                SecondsToConvert -= AMOUNTOFSECONDSIN_DAY;
            }

            Hours = SecondsToConvert / AMOUNTOFSECONDSIN_HOUR;
            SecondsToConvert = SecondsToConvert % AMOUNTOFSECONDSIN_HOUR;
            Minutes = SecondsToConvert / AMOUNTOFSECONDSIN_MINUTE;
            Seconds = SecondsToConvert % AMOUNTOFSECONDSIN_MINUTE;
        }

        public int ConvertToSeconds()
        {
            //Return total number of seconds in the current TimeStamp instance.

            int convertedSeconds = Hours * AMOUNTOFSECONDSIN_HOUR;
            convertedSeconds += Minutes * AMOUNTOFSECONDSIN_MINUTE;
            convertedSeconds += Seconds;
            return convertedSeconds;
        }

        public void ReadFromUser()
        {
            //Ask the user to enter the 3 parts of the TimeStamp. Validate that each is in the correct range.Set the current TimeStamp instance to the value read in. 

            Write("Please enter a TimeStamp");
            Write("Please enter the hours (0.." + (AMOUNTOFHOURSIN_DAY - 1) + "):   ");
            bool success = int.TryParse(ReadLine(), out int answer);
            #region Hour validation
            while (!success || answer < 0 || answer >= AMOUNTOFHOURSIN_DAY)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than " + AMOUNTOFHOURSIN_DAY + ":   ");
                success = int.TryParse(ReadLine(), out answer);
            }
            #endregion
            Hours = answer;
            Clear();

            WriteLine("Please enter a TimeStamp");
            Write("Please enter the minutes (0.." + (AMOUNTOFSECONDSIN_MINUTE) + "):   ");
            success = int.TryParse(ReadLine(), out answer);
            #region Minute validation
            while (!success || answer < 0 || answer >= AMOUNTOFSECONDSIN_MINUTE)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than " + AMOUNTOFSECONDSIN_MINUTE + ":   ");
                success = int.TryParse(ReadLine(), out answer);
            }
            #endregion
            Minutes = answer;
            Clear();

            WriteLine("Please enter a TimeStamp");
            Write("Please enter the seconds (0.." + (AMOUNTOFSECONDSIN_MINUTE) + "):   ");
            success = int.TryParse(ReadLine(), out answer);
            #region Second validation
            while (!success || answer < 0 || answer >= AMOUNTOFSECONDSIN_MINUTE)
            {
                Clear();
                WriteLine("Please enter a TimeStamp");
                Write("Error! Please enter a positive integer smaller than " + AMOUNTOFSECONDSIN_MINUTE + ":   ");
                success = int.TryParse(ReadLine(), out answer);
            }
            #endregion
            Seconds = answer;
            Clear();

        }

        public override string ToString()
        {
            //Returns as a string: the current TimeStamp instance in the format HH: MM: SS
            //Usage: Console.WriteLine("The TimeStamp = ", aTS.ToString()); 

            return string.Format("{0}:{1}:{2}", Hours.ToString().PadLeft(2, '0'), Minutes.ToString().PadLeft(2, '0'), Seconds.ToString().PadLeft(2, '0'));
        }

        public static TimeStamp AddTwoTS(TimeStamp ts1, TimeStamp ts2)
        {
            //Return a new TimeStamp, which is the sum of TimeStampOne and TimeStampTwo . Note: the current TimeStamp instance does not change.

            TimeStamp nts = new TimeStamp();
            int Seconds = ts1.ConvertToSeconds();
            Seconds += ts2.ConvertToSeconds();
            nts.ConvertFromSeconds(Seconds);

            return nts;
        }

        public void AddSeconds(int secondsToAdd)
        {
            //Add TheSeconds to current TimeStamp instance. 

            int seconds = ConvertToSeconds();
            seconds += secondsToAdd;
            ConvertFromSeconds(seconds);
        }
        #endregion

        #region Operators
        #region Equals/Not Equals
        public static bool operator ==(TimeStamp ts1, TimeStamp ts2)
        {
            return ts1.Hours == ts2.Hours && ts1.Minutes == ts2.Minutes && ts1.Seconds == ts2.Seconds;
        }

        public static bool operator !=(TimeStamp ts1, TimeStamp ts2)
        {
            return !(ts1.Hours == ts2.Hours && ts1.Minutes == ts2.Minutes && ts1.Seconds == ts2.Seconds);
        }
        #endregion

        #region Bigger/Smaller than...
        public static bool operator >(TimeStamp ts1, TimeStamp ts2)
        {
            return ts1.ConvertToSeconds() > ts2.ConvertToSeconds();
        }

        public static bool operator >=(TimeStamp ts1, TimeStamp ts2)
        {
            return ts1.ConvertToSeconds() >= ts2.ConvertToSeconds();
        }

        public static bool operator <(TimeStamp ts1, TimeStamp ts2)
        {
            return ts1.ConvertToSeconds() < ts2.ConvertToSeconds();
        }

        public static bool operator <=(TimeStamp ts1, TimeStamp ts2)
        {
            return ts1.ConvertToSeconds() <= ts2.ConvertToSeconds();
        }
        #endregion

        #region Plus
        public static TimeStamp operator +(TimeStamp ts1, TimeStamp ts2)
        {
            TimeStamp nts = new TimeStamp();
            nts.ConvertFromSeconds((ts1.ConvertToSeconds() + ts2.ConvertToSeconds()));

            return nts;
        }

        public static TimeStamp operator +(TimeStamp ts1, int seconds)
        {
            TimeStamp nts = new TimeStamp();
            nts.ConvertFromSeconds((ts1.ConvertToSeconds() + seconds));

            return nts;
        }
        #endregion
        #endregion
    }
}
