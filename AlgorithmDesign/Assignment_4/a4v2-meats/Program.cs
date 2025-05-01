using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v2_seanM
{   class Program
    {
        #region Global Vars
        const int nLines = 4;
        static int[] llen = new int[4];
        static string[] lines = new string[] {
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " };

        public enum StateType { white, word, num, dble, expo };
        public enum CharType { whsp, lett, expo, digit, plus, minus, point, quote, endstr };
        static CharType chType;
        static StateType state;
        static char[] line;           // current line array
        static char ch;
        static int wlen;
        static int k;
        static int len;
        #endregion

        static void Main(string[] args)
        {   //  PRINT OUT THE TEXT LINES AS SINGLE STRINGS FOLLOWED BY THIER LENGTH.
            #region NOT YET
            Console.WriteLine("\n\nHERE ARE THE TEXT LINES PRINTED OUT AS SINGLE STRINGS ... EACH FOLLOWED BY ITS LENGTH. \n\n");
            for (int n = 0; n < nLines; n++)
            {   Console.WriteLine(lines[n], "\n");
                llen[n] = lines[n].Length;
                Console.WriteLine(llen[n]);
            }
            Console.WriteLine("\n\n");
            Console.ReadLine();
            #endregion
            //NOW PRINT OUT THE LINES 1 CHARACTER AT A TIME. 
            #region Char/Char
            Console.WriteLine("\nHERE ARE THE SAME LINES AGAIN ... this time printed character by character.\n\n");
            for (int n = 0; n < nLines; n++)
            {   ///Console.WriteLine("CURRENT LINE ... printed character by character.");
                ///state = StateType.white;
                Console.WriteLine(lines[n] + "\n");
                line = lines[n].ToCharArray();  // change kth string into an array-of-characters
                len = line.Length;              // grab length of this array
                ch = line[0];                   // Grab 1st character from current line array.
                chType = getType(ch);
                while (k < len)
                {
                    switch (state)
                    {
                        case StateType.white:
                            WhiteState();
                            break;
                        case StateType.word:
                            WordState();
                            break;
                        case StateType.num:
                            NumState();
                            break;
                        case StateType.dble:
                            FloatState();
                            break;
                        case StateType.expo:
                            ExpoState();
                            break;
                    }
                }
                k = 0;
                Console.Write("\nPress Enter to Continue");
                Console.ReadLine();
            }
            #endregion
            Console.WriteLine("\n");
            Console.ReadLine();
        }

        #region Char Type Things
        static CharType getType(char c)
        {   CharType type = CharType.whsp;
            if (isSpace(c))
                type = CharType.whsp;
            else if (isAlpha(c))
            {
                if (toUpper(c) == 'E')
                    type = CharType.expo;
                else
                    type = CharType.lett;
            }
            else if (isDigit(c))
                type = CharType.digit;
            else
            {
                switch (c)
                {
                    case '+':
                        type = CharType.plus;
                        break;
                    case '-':
                        type = CharType.minus;
                        break;
                    case '.':
                        type = CharType.point;
                        break;
                    case '\'':
                        type = CharType.quote;
                        break;
                }
            }
            return type;
        }

        static bool isSpace(char c)
        {   return (c == ' ' || c == '\t' || c == '\n');
        }

        static bool isAlpha(char c)
        {   return (toUpper(c) >= 'A' && toUpper(c) <= 'Z');
        }

        static bool isDigit(char c)
        {   return (c >= '0' && c <= '9');
        }

        static char toUpper(char c)
        {   if (c >= 'a' && c <= 'z')
                c = (char)(c - ('a' - 'A'));
            return c;
        }
        #endregion

        #region States
        static void WhiteState()
        {   while (state == StateType.white && k < len)
            {   switch (chType)
                {
                    case CharType.lett:
                    case CharType.expo:
                    case CharType.quote:
                        WhiteToWord();
                        break;
                    case CharType.plus:
                    case CharType.minus:
                    case CharType.digit:
                        WhiteToNum();
                        break;
                    case CharType.point:
                        WhiteToDouble();
                        break;
                    default:
                        if (k < len - 1)
                        {
                            ch = line[++k];
                        }
                        else
                            k++;
                        chType = getType(ch);
                        break;
                }
            }
        }

        static void WordState()
        {
            while (state == StateType.word && k < len)
            {
                switch (chType)
                {
                    case CharType.whsp:
                        ToWhite();
                        break;
                    default:
                        if (k < len - 1)
                        {
                            if (k == 0)
                            {
                                Console.Write(ch);
                            }
                            ch = line[++k];
                            Console.Write(ch);
                        } 
                        else
                            k++;
                        chType = getType(ch);
                        break;
                }

            }
        }

        static void NumState()
        {
            while (state == StateType.num && k < len)
            {
                switch (chType)
                {
                    case CharType.whsp:
                        ToWhite();
                        break;
                    case CharType.expo:
                        ToExpo();
                        break;
                    case CharType.point:
                        ToDouble();
                        break;
                    default:
                        if (k < len - 1)
                        {
                            if (k == 0)
                            {
                                Console.Write(ch);
                            }
                            ch = line[++k];
                            if (!(getType(ch) == CharType.point || getType(ch) == CharType.expo))
                                {
                                Console.Write(ch);
                            }
                        }
                        else
                            k++;
                        chType = getType(ch);
                        break;
                }

            }
        }

        static void FloatState()
        {
            while (state == StateType.dble && k < len)
            {
                switch (chType)
                {
                    case CharType.whsp:
                        ToWhite();
                        break;
                    case CharType.expo:
                        ToExpo();
                        break;
                    default:
                        if (k < len - 1)
                        {
                            ch = line[++k];
                            if (!(getType(ch) == CharType.expo))
                            {
                                if (k == 0)
                                {
                                    Console.Write(ch);
                                }
                                Console.Write(ch);
                            }
                        }
                        else
                            k++;
                        chType = getType(ch);
                        break;
                }

            }
        }

        static void ExpoState()
        {
            while (state == StateType.expo && k < len)
            {
                switch (chType)
                {
                    case CharType.whsp:
                        ToWhite();
                        break;
                    default:
                        if (k < len - 1)
                        {
                            if (k == 0)
                            {
                                Console.Write(ch);
                            }
                            ch = line[++k];
                            Console.Write(ch);
                        }
                        else
                            k++;
                        chType = getType(ch);
                        break;
                }

            }
        }
        #endregion

        #region State Changes
        static void WhiteToWord()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.word;
            Console.Write((int)state + "\n");
        }

        static void WhiteToNum()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.num;
            Console.Write((int)state + "\n");
        }

        static void WhiteToDouble()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.dble;
            Console.Write((int)state + "\n");
        }

        static void ToWhite()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.white;
            Console.Write((int)state);
        }

        static void ToDouble()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.dble;
            Console.Write((int)state + "\n");
        }

        static void ToExpo()
        {
            Console.WriteLine();
            Console.Write(ch);
            Console.Write("ST" + (int)state + "-");
            state = StateType.expo;
            Console.Write((int)state + "\n");
        }
        #endregion

    }
}


