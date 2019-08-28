using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Data
{
    class Employee
    {
        #region Backing Fields
        private string _name;
        private string _famName;
        private uint _empNumb;
        private decimal _payRate;
        private TimeStamp _workTime;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public string FamName
        {
            get
            {
                return _famName;
            }
            set
            {
                _famName = value;
            }
        }

        public uint ID
        {
            get
            {
                return _empNumb;
            }
            set
            {
                _empNumb = value;
            }
        }

        public decimal PayRate
        {
            get
            {
                return _payRate;
            }
            set
            {
                _payRate = value;
            }
        }

        public TimeStamp TimeWorked
        {
            get
            {
                return _workTime;
            }
            set
            {
                _workTime = value;
            }
        }
        #endregion

        #region Constructors
        public Employee()
        {
            Name = "";
            FamName = "";
            ID = 0;
            PayRate = 0;
            TimeWorked = new TimeStamp();
        }

        public Employee(uint id, string fname, string name, decimal payRate)
        {
            Name = name;
            FamName = fname;
            ID = id;
            PayRate = payRate;
            TimeWorked = new TimeStamp();
        }

        public Employee(string name, string fname, uint id, decimal payRate, TimeStamp ts)
        {
            Name = name;
            FamName = fname;
            ID = id;
            PayRate = payRate;
            TimeWorked = ts;
        }
        #endregion

        #region Methods
        public decimal Pay()
        {
            decimal pay = TimeWorked * PayRate;
            return pay;
        }

        /*public override string ToString()
        {
            //Returns as a string: the current TimeStamp instance in the format HH: MM: SS
            //Usage: Console.WriteLine("The TimeStamp = ", aTS.ToString()); 

            return string.Format("{0}:{1}:{2}", Hours.ToString().PadLeft(2, '0'), Minutes.ToString().PadLeft(2, '0'), Seconds.ToString().PadLeft(2, '0'));
        }
        */
        #endregion
    }
}
