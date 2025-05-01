using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;

namespace Employee_Data
{
    class Program
    {
        static void Main(string[] args)
        {

            //fill these in
            const string FILENAME = "employees.txt";
            const string OTHERFILENAME = "employeeHours.txt";
            const string FIRSTREPORTFILENAME = "lastNameReport.txt";
            const string SECONDREPORTFILENAME = "timeWorkedReport.txt";
            const string THIRDREPORTFILENAME = "payReport.txt";

            int ARRAYSIZE = GetEmployeeListSize(FILENAME);
            Employee[] employeeArray = new Employee[ARRAYSIZE];

            GenerateEmployeeListFromFile(employeeArray, FILENAME);
            ProcessTimeWorkedFile(employeeArray, OTHERFILENAME);

            BubbleSortEmployeeListByLastName(employeeArray);
            PrintReport(employeeArray, FIRSTREPORTFILENAME);
            WriteLine("\n\n");

            SelectionSortEmployeeListByTimeWorkedDesc(employeeArray);
            PrintReport(employeeArray, SECONDREPORTFILENAME);
            WriteLine("\n\n");

            SortEmployeeListByPay(employeeArray);
            PrintReport(employeeArray, THIRDREPORTFILENAME);

            ReadKey();

        }

        #region Populate the Array
        static int GetEmployeeListSize(string file)
        {
            const int MAXEMPLOYEES = 100;

            StreamReader reader = null;
            string content;
            int counter = 0;
            try
            {
                reader = new StreamReader(file);

                while ((content = reader.ReadLine()) != null)
                {
                    counter++;
                }

                if (counter > MAXEMPLOYEES)
                {
                    counter = -1;
                }

                reader.Close();
                return counter;
            }
            catch
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return -1;
            }
        }

        static void GenerateEmployeeListFromFile(Employee[] empArray, string file)
        {
            string[] values;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(file);

                for (int i = 0; i < empArray.Length; i++)
                {
                    values = reader.ReadLine().Split('|');
                    empArray[i] = new Employee(uint.Parse(values[0]), values[1], values[2], decimal.Parse(values[3]));
                }
            }
            catch (ArgumentException ex)
            {
                WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
        #endregion

        #region Add Time Worked
        static void ProcessTimeWorkedFile(Employee[] empArray, string file)
        {
            string[] values;
            int index;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(file);

                while ((values = reader.ReadLine().Split('|')) != null)
                {
                    index = FindEmployeeIndex(empArray, uint.Parse(values[0]));
                    empArray[index].TimeWorked += TimeStamp.ReadFromString(values[1]);
                }
            }
            catch
            {

            }
        }

        static int FindEmployeeIndex(Employee[] empArray, uint value)
        {
            for (int i = 0; i < empArray.Length; i++)
            {
                if (empArray[i].ID == value)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        #region Print Report
        static void PrintReport(Employee[] empArray, string file)
        {
            WriteToFile(empArray, file);
            const string format = "{0,5} {1,15} {2,15} {3,11} {4,11} {5,9}";
            WriteLine(format, "Emp #", "Last Name", "First Name", "Time Worked", "Hourly Wage", "Pay");
            WriteLine(format, "-----", "---------------", "---------------", "-----------", "-----------", "---------");
            for (int i = 0; i < empArray.Length; i++)
            {
                WriteLine(format, empArray[i].ID, empArray[i].FamName, empArray[i].Name, empArray[i].TimeWorked, empArray[i].PayRate.ToString("c"), Math.Round(CalculateEmpPay(empArray[i]), 2).ToString("c"));
            }

            TimeStamp totalTime = CalculateTotalTimeWorked(empArray);
            decimal totalPay = CalculateTotalPay(empArray);

            WriteLine("\nTotal Time Worked = " + totalTime);
            WriteLine("Total Pay = " + Math.Round(totalPay, 2));
        }

        static void WriteToFile(Employee[] empArray, string file)
        {
            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter(file);

                const string format = "{0,5} {1,15} {2,15} {3,11} {4,11} {5,9}";
                writer.WriteLine(format, "Emp #", "Last Name", "First Name", "Time Worked", "Hourly Wage", "Pay");
                writer.WriteLine(format, "-----", "---------------", "---------------", "-----------", "-----------", "---------");
                for (int i = 0; i < empArray.Length; i++)
                {
                    writer.WriteLine(format, empArray[i].ID, empArray[i].FamName, empArray[i].Name, empArray[i].TimeWorked, empArray[i].PayRate.ToString("c"), Math.Round(CalculateEmpPay(empArray[i]), 2).ToString("c"));
                }

                TimeStamp totalTime = CalculateTotalTimeWorked(empArray);
                decimal totalPay = CalculateTotalPay(empArray);

                writer.WriteLine("\nTotal Time Worked = " + totalTime);
                writer.WriteLine("Total Pay = " + Math.Round(totalPay, 2));

                writer.Close();
            }
            catch
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        static double CalculateEmpPay(Employee emp)
        {
            return (double)emp.Pay();
        }

        static decimal CalculateTotalPay(Employee[] empArray)
        {
            decimal total = 0;
            for (int i = 0; i < empArray.Length; i++)
            {
                total += empArray[i].Pay();
            }
            return total;
        }

        static TimeStamp CalculateTotalTimeWorked(Employee[] empArray)
        {
            TimeStamp total = new TimeStamp();
            for (int i = 0; i < empArray.Length; i++)
            {
                total += empArray[i].TimeWorked;
            }
            return total;
        }
        #endregion

        #region Sort
        static void BubbleSortEmployeeListByLastName(Employee[] arr)
        {
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if ((arr[sort].FamName.CompareTo(arr[sort + 1].FamName)) == 1)
                    {
                        Swap(ref arr[sort], ref arr[sort + 1]);
                    }
                }
            }
        }

        static void SelectionSortEmployeeListByTimeWorkedDesc(Employee[] arr)
        {
            int startScan, maxIndex;
            Employee maxValue;
            for (startScan = 0; startScan < (arr.Length - 1); startScan++)
            {
                maxIndex = startScan;
                maxValue = arr[startScan];

                for (int index = startScan + 1; index < arr.Length; index++)
                {
                    if (arr[index].TimeWorked > maxValue.TimeWorked)
                    {
                        maxValue = arr[index];      //FIND SMALLEST VALUE
                        maxIndex = index;
                    }
                }
                arr[maxIndex] = arr[startScan];     //SWAP
                arr[startScan] = maxValue;

            }
        }

        static void SortEmployeeListByPay(Employee[] arr)
        {
            int startScan, minIndex;
            Employee minValue;
            for (startScan = 0; startScan < (arr.Length - 1); startScan++)
            {
                minIndex = startScan;
                minValue = arr[startScan];

                for (int index = startScan + 1; index < arr.Length; index++)
                {
                    if (arr[index].Pay() < minValue.Pay())
                    {
                        minValue = arr[index];      //FIND SMALLEST VALUE
                        minIndex = index;
                    }
                }
                arr[minIndex] = arr[startScan];     //SWAP
                arr[startScan] = minValue;

            }
        }

        static void Swap(ref Employee a, ref Employee b)
        {
            Employee temp = a;
            a = b;
            b = temp;
        }
        #endregion
    }
}
