
/*************************************************** ID BLOCK******************************************************************
 * Software Designer: Sean Meath                                                                                              *
 * Course 420-306-AB                                                                                                          *
 * Assignment 5v5                                                                                                             *
 *                                                                                                                            *
 * Description:     This program changes infix functions into postfix functions in order to evaluate and process them         *
 *                  to calculate the end value of each expression                                                             *
 *                                                                                                                            *
 *                  The infix and postfix expressions are seperated into arrays for character to character computing.         *
 *                  The function uses stacks to push, pop and popandtest the operators and operands for better usage of data. *
 *                  The function computes the postfix expression into its numerical value.                                    *
 *                                                                                                                            *
 *                  The program outputs the infix expression as well as the postfix expression and the resulting value        *
 *                  in a neat table display for comparison. As well as the symbols or variables used in the caculations       *
 *                  and their data values                                                                                     *
 *                                                                                                                            *
 ******************************************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a5v5_meats
{   class Program
    {   const int NMAX = 5;        //maximum size of each name string
        const int LSIZE = 5;       //actual number of infix strings in the data array
        const int NOPNDS = 10;     //number of operand symbols in the operand array
        static int IDX;                   //index used to implement conversion stub
        static char[] opnd = new char[NOPNDS] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' }; //operands symbols
        static double[] opndval = new double[NOPNDS] { 3, 1, 2, 5, 2, 4, -1, 3, 7, 187 };           //operand values
        static List<double> OPNDstack = new List<double>();                                         //operand stack
        static List<char> OPRstack = new List<char>();                                              //operator stack

        static void Main()
        {   Console.WindowWidth = 120;
            Console.WindowHeight = Console.WindowWidth * 9 / 25;

            /*************************************************************************
                                      KEY DECLARATIONS            
            *************************************************************************/
            string[] infix = new string[LSIZE] { "C$A$E",    //array of infix strings
                             "(A+B)*(C-D)",
                             "A$B*C-D+E/F/(G+H)",
                             "((A+B)*C-(D-E))$(F+G)",
                             "A-B/(C*D$E)"  };

            /*************************************************************************
                   PRINT OUT THE OPERANDS AND THEIR VALUES            
             *************************************************************************/
            Console.WriteLine("\nOPERAND SYMBOLS USED:\n");   //title
            for (int i = 0; i < NOPNDS; i++)                  //loop through operands
                Console.Write(opnd[i].ToString().PadLeft(5)); //write operand
            Console.WriteLine("\n\n\nCORRESPONDING OPERAND VALUES:\n");   //title
            for (int i = 0; i < NOPNDS; i++)                     //loop through operands
                Console.Write(opndval[i].ToString().PadLeft(5)); //write operand value
            Console.WriteLine("\n\n");

            /*************************************************************************
                                            OUTPUT LINES
            *************************************************************************/
            Console.WriteLine("Infix Expression".PadRight(30) + "Postfix Expression".PadRight(30) + "Value".PadRight(20));  //header
            OutLine(70, '=');                                                                                               //print '=' * 70
            for (IDX = 0; IDX < LSIZE; IDX++)                                                                               //loop through infix strings
            {   string postfix = ConvertToPostfix(infix[IDX]);                                                              //converts infix to postfix
                Console.WriteLine(infix[IDX].PadRight(30) + postfix.PadRight(30) + EvaluatePostfix(postfix));               //display infix, postfix, value
            }
            Console.ReadLine();
        }

        /***************************************************************************************** 
                FUNCTION OutLine:   formatting function to print n repetitions of char ch
        ******************************************************************************************/
        static void OutLine(int n, char ch)
        {   for (int q = 0; q < n; q++)         //loops n times
                Console.Write(ch.ToString());   //print the character
            Console.WriteLine("\n");
        }

        /*************************************************************************
                                CONVERSION FUNCTION     
        *************************************************************************/
        static string ConvertToPostfix(string infix)
        {   string postfixString = "";                      //stores postfix string
            char topsym = '+';                              //holds current highest order char
            char currentChar;                               //holds current char
            bool und;                                       //if stack empty
            char[] infixfixArray = infix.ToCharArray();     //create char array of infix
            currentChar = infixfixArray[0];                 //gets first char
            for (int i = 0; i < infixfixArray.Length; i++)
            {   currentChar = infixfixArray[i];             //gets current char
                if (IsOperand(currentChar))                 //check type
                    postfixString += currentChar;           //add operand to postfix stack
                else
                {   und = OPRpopandtest(ref topsym);        //gets new topsim + und
                    while(!und && prcd(topsym, currentChar))//presedence and !empty stack
                    {   postfixString += topsym;            //add operator to postfix stack
                        und = OPRpopandtest(ref topsym);    //gets new topsim + und
                    }
                    if (!und)                               //!empty stack
                        OPRpush(topsym);                    //add highest order char to  operator stack
                    if (und || currentChar != ')')          //!empty stack and not empty parenthesis
                        OPRpush(currentChar);               //add current char to  operator stack
                    else
                        topsym = OPRpop();                  //get new topsim
                }
            }
            while (!OPRpopandtest(ref currentChar))         //while not empty, pop
                postfixString += currentChar;               //add current char to  operator stack
            return postfixString;
        }

        /*************************************************************************
                                EVALUATION FUNCTION  
        *************************************************************************/
        static double EvaluatePostfix(string postfix)
        {   char currentChar;                               //holds current char
            double op1, op2, val = 0;                       //calculator placeholders
            char[] postfixArray = postfix.ToCharArray();    //create char array of postfix
            for (int i = 0; i < postfixArray.Length; i++)   //loop through postfix array
            {   currentChar = postfixArray[i];              //get current char
                if (IsOperand(currentChar))                 //is operand
                {   for (int c = 0; c < opnd.Length; c++)   //loop through operands
                    {   if (currentChar == opnd[c])         //if same operand
                        {   OPNDpush(opndval[c]);           //push operand value into operand list
                            break;                          //quit loop
                        }
                    }
                }
                else
                {   op2 = OPNDpop();                        //get one value to compute
                    op1 = OPNDpop();                        //get other value to compute
                    switch (currentChar)                    //switch on operator
                    {   case '+':                           //is +
                            val = op1 + op2;                //apply operator
                            break;
                        case '-':                           //is -
                            val = op1 - op2;                //apply operator
                            break;
                        case '*':                           //is *
                            val = op1 * op2;                //apply operator
                            break;
                        case '/':                           //is /
                            val = op1 / op2;                //apply operator
                            break;
                        case '$':                           //is $
                            val = Math.Pow(op1, op2);       //apply operator
                            break;
                    }
                    OPNDpush(val);                          //push computed value to stack
                }
            }
            val = OPNDpop();                                //get final val
            return (val);
        }

        /*************************************************************************
                                OTHER FUNCTIONS  
        *************************************************************************/

        static bool IsOperand(char test)
        {   return (test >= 'A' && test <= 'J');    //check if operand
        }

        static int getRank(char c)
        {
            int rank = 0;       //initionalise rank
            switch (c)          //switch on c
            {   case '+':       //is +
                case '-':       //is -
                    rank = 3;   //set low priority
                    break;
                case '*':       //is *
                case '/':       //is /
                    rank = 2;   //set medium priority
                    break;
                case '$':       //is $
                    rank = 1;   //set high priority
                    break;
            }
            return rank;
        }

        static bool prcd(char oldChar, char newChar)
        {   bool precedence;                            //is precedent
            if (oldChar == '(')
                precedence = false;                     //not prcd
            else if (newChar == '(' && oldChar != ')')
                precedence = false;                     //not prcd
            else if (newChar == '(')
                precedence = true;                      //prcd
            else if (newChar == ')' && oldChar != '(')
                precedence = true;                      //prcd
            else if (newChar == ')')
                precedence = false;                     //not prcd
            else if (oldChar == '$' && newChar == '$')
                precedence = false;                     //not prcd
            else if (getRank(oldChar) <= getRank(newChar))  //topsym ranks lower
                precedence = true;                      //prcd
            else
                precedence = false;                     //not prcd
            return precedence;
        }

        /*************************************************************************
                            OPND STACK FUNCTIONS:
		- the global object "OPNDstack" is an instance of the class "List"
		- the contents of "OPNDstack" are doubles
		- see its declaration immediately before the Main block

        *************************************************************************/
        static void OPNDpush(double opnd)
        {   OPNDstack.Add(opnd);    //adds/pushes operand to operand stack
        }

        static double OPNDpop()
        {   double last = OPNDstack[OPNDstack.Count - 1];   //get last operand
            OPNDstack.RemoveAt(OPNDstack.Count - 1);        //remove last operand
            return last;
        }

        static void dumpOPNDstack()
        {   foreach (double value in OPNDstack)     //loop through operand stack
                Console.Write(value + " | ");       //display operand value
            if (OPNDstack.Count == 0)               //is empty
                Console.Write("EMPTY");             //display empty
        }

        /*************************************************************************
                            OPR STACK FUNCTIONS:
		- the global object "OPRstack" is an instance of the class "List"
		- the contents of "OPRstack" are characters
		- see its declaration immediately before the Main block

        *************************************************************************/
        static void OPRpush(char opr)
        {   OPRstack.Add(opr);      //adds/pushes operator to operator stack
        }

        static char OPRpop()
        {   char last = OPRstack[OPRstack.Count - 1];   //get last operator
            OPRstack.RemoveAt(OPRstack.Count - 1);      //remove last operator
            return last;
        }

        static bool OPRpopandtest(ref char next)
        {   bool und = false;           //initionalize und
            if (OPRstack.Count == 0)    //if empty stack
                und = true;             //underflow is true
            else
                next = OPRpop();        //get next character
            return und;
        }

        static void dumpOPRstack()
        {   foreach (double value in OPRstack)  //loop through operator stack
                Console.Write(value + " | ");   //write each operator
            if (OPRstack.Count == 0)            //if empty stack
                Console.Write("EMPTY");         //display empty
        }
    }
}