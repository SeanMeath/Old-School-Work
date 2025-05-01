

/************************************************************************************************
*************************************** ID BLOCK ************************************************
    Due Date:           November 18th, 2018
    Software Designer:  Sean Meath
    Course:             420-306-AB 
    Deliverable:        Assignment 4 (v4) -- State Analysis and Parsing

    Description:        This software goes through arrays of strings. Parses line by line, character
                        by character and analyses it using 5 states depending on the type of data
                        it is then further analysed into words and numbers that are then computed
                        upon state changes and movements. The program then outputs the distribution
                        of the word lengths as well as the integers and doubles found into labeled
                        tables.

*************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v4_meats
{
    class Program
    {
        /*************************************************************************************************
        *************************************** MY GLOBAL VARS ******************************************/
        enum StateType { white, word, num, dble, expt };                            //States
        enum CharType { whsp, lett, expo, digit, plus, minus, point, quote, stop }; //Types of Chars

        static char[] line;                                 //Line of Characters
        static char ch;                                     //Character
        static CharType type = CharType.whsp;               //Character Type
        static StateType state = StateType.white;           //State of String
        static int wlen = 0;                                //Word Length
        static int k;                                       //Position Counter
        static int len;                                     //Length of Current Line
        static int ival;                                    //Int Value
        static double val;                                  //Double Value
        static int sign;                                    //Sign of Val (+/-)
        static int esign;                                   //Sign of Exponent (+/-)
        static int expval;                                  //Value of Exponent
        static int power;                                   //Power of Decimal
        static int[] myWords;                               //Distribution of Words
        static List<int> myInts = new List<int>();          //List of Ints Calculated
        static List<double> myDoubles = new List<double>(); //List of Doubles Calculated
        public const string format = "{0,-10} {1,10}";      //Format for Display

        static void Main(string[] args)
        {
            int nLines = 4;                 //Number of Lines
            int[] llen = new int[4];        //Line Lengths
            string[] lines = new string[] { //Lines
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " };

            //  PRINT OUT THE TEXT LINES AS SINGLE STRINGS FOLLOWED BY THIER LENGTH.
            Console.WriteLine("\n\nHERE ARE THE TEXT LINES PRINTED OUT AS SINGLE STRINGS ... EACH FOLLOWED BY ITS LENGTH. \n\n");
            for (int k = 0; k < nLines; k++)                                //Loop Through Lines
            {
                Console.WriteLine(lines[k], "\n");                          //Write Line
                llen[k] = lines[k].Length;                                  //Get Length
                Console.WriteLine(llen[k]);                                 //Display Length
            }
            myWords = new int[llen.Max()];                                  //Create Word Array
            for (int i = 0; i < nLines; i++)                                //Loop Through Lines
            {
                line = lines[i].ToCharArray();                              //Get Line
                len = line.Length;                                          //Get Line Length
                k = 0;                                                      //Initionalize Counter
                if (state != StateType.word || type != CharType.minus)      //Check If Countinue
                    state = StateType.white;                                //Reset State
                ch = line[0];                                               //Get Character
                type = getType(ch);                                         //Get Character Type
                while (k < len)                                             //Loop Through Line
                {
                    switch (state)                                          //Switch States
                    {
                        case StateType.white:                               //White State
                            WhiteState();                                   //Go To White State
                            break;
                        case StateType.word:                                //Word State
                            WordState();                                    //Go To Word State
                            break;
                        case StateType.num:                                 //Number State
                            NumState();                                     //Go To Number State
                            break;
                        case StateType.dble:                                //Double State
                            DblState();                                     //Go To Double State
                            break;
                        case StateType.expt:                                //Exponent State
                            ExpoState();                                    //Go To Exponent State
                            break;
                    }
                }
                if (type != CharType.minus)                                 //Check If To Be Countinued
                {
                    if (state == StateType.word)                            //Word State
                        WordToWhite();                                      //Finalize Word
                    if (state == StateType.num)                             //If Word State
                        NumToWhite();                                       //Finalize Number
                    if (state == StateType.dble)
                        DblToWhite();                                       //Finalize Double
                    if (state == StateType.expt)
                        ExpoToWhite();                                      //Finalize Exponent
                }
            }
            Display();                                                      //Displays the Tables
            Console.ReadLine();
        }
        /**************************************** GET TYPE **********************************************/
        static CharType getType(char c)
        {
            CharType type = CharType.whsp;      //Default Type
            if (isSpace(c))                     //Check White
                type = CharType.whsp;           //Set White Type
            else if (isAlpha(c))                //Check Letter
                if (toUpper(c) == 'E')          //Check Exponent
                    type = CharType.expo;       //Set Exponent Type
                else
                    type = CharType.lett;       //Set White Type
            else if (isDigit(c))                //Check Digit
                type = CharType.digit;          //Set Digit Type
            else
            {
                switch (c)                      //Check Rest
                {
                    case '+':                   //Check Plus
                        type = CharType.plus;   //Set Plus Type
                        break;
                    case '-':                   //Check Minus
                        type = CharType.minus;  //Set Minus Type
                        break;
                    case '.':                   //Check Point
                        type = CharType.point;  //Set Point Type
                        break;
                    case '\'':                  //Check Quote
                        type = CharType.quote;  //Set Quote Type
                        break;
                }
            }
            return type;                        //Return Type
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isSpace(char c)
        {
            return (c == ' ' || c == '\t' || c == '\n');    //Check White
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isDigit(char c)
        {
            return (c >= '0' && c <= '9');  //Check Digit
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isAlpha(char c)
        {
            return ((toUpper(c) >= 'A' && toUpper(c) <= 'Z'));  //Check Letter
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static char toUpper(char c)
        {
            if (c >= 'a' && c <= 'z')           //Check Lower Case
                c = (char)(c - ('a' - 'A'));    //Set Upper Case
            return c;                           //Return Upper Case
        }
        /************************************************************************************************/
        /*************************************** WHITE STATE ********************************************/
        static void WhiteState()
        {
            while (state == StateType.white && k < len) //Loop Through Line Provided White State
            {
                switch (type)                           //Switch State
                {
                    case CharType.lett:                 //Check Letter
                    case CharType.expo:                 //Check Exponent
                        WhiteToWord();                  //State Transition (Word)
                        break;
                    case CharType.digit:                //Check Digit
                    case CharType.plus:                 //Check Plus
                    case CharType.minus:                //Check Minus
                        WhiteToNum();                   //State Transition (Number)
                        break;
                    case CharType.point:                //Check Point
                        WhiteToDble();                  //State Transition (Double)
                        break;
                    default:                            //Stay State
                        if (k < len - 1)                //Check End of Line
                            ch = line[++k];             //Next Character
                        else
                        {
                            k++;                        //Trigger Exit
                            return;
                        }
                        type = getType(ch);             //Get New Character Type
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WHITE TO WORD ******************************************/
        static void WhiteToWord()
        {
            wlen = 0;               //Start New Word
            state = StateType.word; //Go To Word State
        }
        /************************************************************************************************/
        /*************************************** WHITE TO NUM *******************************************/
        static void WhiteToNum()
        {
            type = getType(ch);     //Get Current Char Type
            sign = CaptureSign(ch); //Get Sign
            ival = 0;               //Initionalize Int Value
            state = StateType.num;  //Go To Num State
        }
        /************************************************************************************************/
        /*************************************** WHITE TO DBLE ******************************************/
        static void WhiteToDble()
        {
            if (k < len)            //Check End Of Line
                ch = line[++k];     //Next Character
            else
            {
                k++;                //Trigger Exit
                return;
            }
            type = getType(ch);     //Get Current Char Type
            val = 0;                //Initionalize Double Value
            sign = 1;               //Initionalize Sign
            power = 1;              //Initionalize Power
            state = StateType.dble; //Go To Double State

        }
        /************************************************************************************************/
        /*************************************** WORD STATE *********************************************/
        static void WordState()
        {
            while (state == StateType.word && k < len)  //Loop Through Line Provided Word State
            {
                switch (type)               //Switch State
                {
                    case CharType.whsp:     //Check White
                        WordToWhite();      //State Transition(White)
                        break;
                    default:                //Stay State
                        wlen++;             //Increase Word Length
                        if (k < len - 1)    //Check End Of Line
                            ch = line[++k]; //Next Character
                        else
                        {
                            k++;            //Trigger Exit
                            return;
                        }
                        type = getType(ch); //Get Current Char Type
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WORD TO WHITE ******************************************/
        static void WordToWhite()
        {
            myWords[wlen]++;            //Increase Distribution of Size
            state = StateType.white;    //Go To White State
        }
        /************************************************************************************************/
        /**************************************** NUM STATE *********************************************/
        static void NumState()
        {
            while (state == StateType.num && k < len)   //Loop Through Line Provided Num State
            {
                switch (type)                           //Switch State
                {
                    case CharType.whsp:                 //Check White
                        NumToWhite();                   //State Transition(White)
                        break;
                    case CharType.point:                //Check Point
                        NumToDbl();                     //State Transition(Double)
                        break;
                    case CharType.expo:                 //Check Exponent
                        NumToExpo();                    //State Transition(Exponent)
                        break;
                    default:                            //Stay State
                        ival = ival * 10 + (ch - '0');  //Add Character to end of Int Value
                        if (k < len - 1)                //Check End Of Line
                            ch = line[++k];             //Next Character
                        else
                        {
                            k++;                        //Trigger Exit
                            return;
                        }
                        type = getType(ch);             //Get Current Char Type
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** NUM TO WHITE ******************************************/
        static void NumToWhite()
        {
            ival = ival * sign;         //Assign Sign to Int Value
            myInts.Add(ival);           //Adds Int Value to Int List
            state = StateType.white;    //Go To White State
        }
        /************************************************************************************************/
        /**************************************** NUM TO DBL ********************************************/
        static void NumToDbl()
        {
            if (k < len - 1)        //Check End Of Line
                ch = line[++k];     //Next Character
            else
            {
                k++;                //Trigger Exit
                return;
            }
            type = getType(ch);     //Get Current Char Type
            val = ival;             //Initionalize Double Value
            power = 1;              //Initionalize Power
            state = StateType.dble; //Go To Double State
        }
        /************************************************************************************************/
        /**************************************** NUM TO EXPO *******************************************/
        static void NumToExpo()
        {   //Not needed to check bounds because e of expo can't be the last char line
            val = ival * sign;       //Assigns Sign to Int Value
            ch = line[++k];          //Next Character
            type = getType(ch);      //Get Current Char Type
            esign = CaptureSign(ch); //Get Exponent Sign
            expval = 0;              //Initionalize Exponent Value
            state = StateType.expt;  //Go To Exponent State
        }
        /************************************************************************************************/
        /**************************************** DBL STATE *********************************************/
        static void DblState()
        {
            while (state == StateType.dble && k < len)  //Loop Through Line Provided Double State
            {
                switch (type)                           //Switch State
                {
                    case CharType.whsp:                 //Check White
                        DblToWhite();                   //State Transition(White)
                        break;
                    case CharType.expo:                 //Check Exponent
                        DblToExpo();                    //State Transition(Exponent)
                        break;
                    default:                            //Stay State
                        val = val * 10 + ch - '0';      //Add Character to end of Double Value
                        power *= 10;                    //Increase Power Value by 1 Position
                        if (k < len - 1)                //Check End Of Line
                            ch = line[++k];             //Next Character
                        else
                        {
                            k++;                        //Trigger Exit
                            return;
                        }
                        type = getType(ch);             //Get Current Char Type
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** DBL TO WHITE ******************************************/
        static void DblToWhite()
        {
            val = val * sign / power;   //Assign Sign, Power to Int Value
            myDoubles.Add(val);         //Adds Double Value to Double List
            state = StateType.white;    //Go To White State
        }
        /************************************************************************************************/
        /**************************************** DBL TO EXPO *******************************************/
        static void DblToExpo()
        {
            val = val * sign / power;   //Assign Sign, Power to Int Value
            ch = line[++k];             //Next Character
            type = getType(ch);         //Get Current Char Type
            esign = CaptureSign(ch);    //Get Exponent Sign
            expval = 0;                 //Initionalize Exponent Value
            state = StateType.expt;     //Go To Exponent State
        }
        /************************************************************************************************/
        /**************************************** EXPO STATE ********************************************/
        static void ExpoState()
        {
            while (state == StateType.expt && k < len)      //Loop Through Line Provided Exponent State
            {
                switch (type)                               //Switch State
                {
                    case CharType.whsp:                     //Check White
                        ExpoToWhite();                      //State Transition(White)
                        break;
                    default:
                        expval = expval * 10 + (ch - '0');  //Add Character to end of Exponent Value
                        if (k < len - 1)                    //Check End Of Line
                            ch = line[++k];                 //Next Character
                        else
                        {
                            k++;                            //Trigger Exit
                            return;
                        }
                        type = getType(ch);                 //Get Current Char Type
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** EXPO TO WHITE *****************************************/
        static void ExpoToWhite()
        {
            if (esign == -1)                        //Check Exponent Sign
                for (int i = 0; i < expval; i++)    //Loop Through Exponent Val
                    val = val / 10;                 //Moving Right by 1 Position
            else
                for (int i = 0; i < expval; i++)    //Loop Through Exponent Val
                    val = val * 10;                 //Moving Left by 1 Position
            myDoubles.Add(val);                     //Adds Double Value to Double List
            state = StateType.white;                //Go To White State
        }
        /************************************************************************************************/
        /**************************************** CAPTURE SIGN ******************************************/
        static int CaptureSign(char c)
        {
            int s;                                                  //Initionalize Sign
            if (type == CharType.minus || type == CharType.plus)    //Check Sign
            {
                if (type == CharType.minus)                         //Check Minus
                    s = -1;                                         //Set Sign Minus
                else
                    s = 1;                                          //Set Sign Plus
                ch = line[++k];                                     //Next Character
                type = getType(ch);                                 //Get Current Char Type
            }
            else
                s = 1;                                              //Set Sign Plus
            return s;                                               //Return Sign
        }
        /************************************************************************************************/
        /**************************************** DISLPLAY **********************************************/
        static void Display()
        {
            Console.WriteLine("\n\n     WORD RESULTS:");                                    //Display Table Name
            Console.WriteLine("╔═════════════════════╗");                                   //Display Table Top
            Console.WriteLine("║" + string.Format(format, "LENGTH", "FREQUENCY") + "║");    //Display Table Columns
            for (int w = 0; w < myWords.Length; w++)                                        //Loop Through Words
            {
                if (myWords[w] != 0)                                                        //Not Empty
                {
                    Console.WriteLine("╠═════════════════════╣");                           //Display Seperator
                    Console.WriteLine("║" + string.Format(format, w, myWords[w]) + "║");    //Word Data Column
                }
            }
            Console.WriteLine("╚═════════════════════╝");                                   //Display End of Table                          

            Console.WriteLine("\n\n     INTEGER RESULTS:");                                 //Display Table Name
            Console.WriteLine("╔═════════════════════╗");                                   //Display Table Top
            Console.WriteLine("║" + string.Format(format, "INDEX", "VALUE") + "║");         //Display Table Columns
            for (int i = 0; i < myInts.Count; i++)                                          //Loop Through Ints
            {
                Console.WriteLine("╠═════════════════════╣");                               //Display Seperator
                Console.WriteLine("║" + string.Format(format, i, myInts[i]) + "║");         //Int Data Column
            }
            Console.WriteLine("╚═════════════════════╝");                                   //Display End of Table 

            string toBeDisplayed;                                                           //Create Display String
            Console.WriteLine("\n\n     DOUBLE RESULTS:");                                  //Display Table Name
            Console.WriteLine("╔═════════════════════╗");                                   //Display Table Top
            Console.WriteLine("║" + string.Format(format, "INDEX", "VALUE") + "║");         //Display Table Columns
            for (int d = 0; d < myDoubles.Count; d++)                                       //Loop Through Doubles
            {
                Console.WriteLine("╠═════════════════════╣");                               //Display Seperator
                if (myDoubles[d] % 1 == 0)                                                  //No Decimals
                    toBeDisplayed = String.Format("{0:0.00}", myDoubles[d]);                //Prepare Display Added Decimal
                else
                    toBeDisplayed = myDoubles[d].ToString();                                //Prepare Display
                Console.WriteLine("║" + string.Format(format, d, toBeDisplayed) + "║");     //Double Data Column
            }
            Console.WriteLine("╚═════════════════════╝");                                   //Display End of Table 
        }
    }
}