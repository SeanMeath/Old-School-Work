using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a4v3_meats
{
    class Program
    {
        enum StateType { white, word, num, dble, expt };
        enum CharType { whsp, lett, expo, digit, plus, minus, point, quote, stop };

        static char[] line;
        static char ch;
        static CharType type = CharType.whsp;
        static StateType state = StateType.white;
        static int wlen;
        static int k;
        static int len;
        static bool JustChanged = false;
        static char lc = ' ';
        static int ln = 0;

        static void Main(string[] args)
        {
            int nLines = 4;
            int[] llen = new int[4];
            string[] lines = new string[] {
                "    first 123		and then -.1234 but you'll need 123.456		 and 7e-4 plus one like +321. all quite avant-",
                "garde   whereas ellen's true favourites are 123.654E-2	exponent-form which can also be -54321E-03 or this -.9E+5",
                "We'll prefer items like			fmt1-decimal		+.1234567e+05 or fmt2-dec -765.3245 or fmt1-int -837465 and vice-",
                "versa or even format2-integers -19283746   making one think of each state's behaviour for 9 or even 3471e-7 states " };

            //  PRINT OUT THE TEXT LINES AS SINGLE STRINGS FOLLOWED BY THIER LENGTH.
            Console.WriteLine("\n\nHERE ARE THE TEXT LINES PRINTED OUT AS SINGLE STRINGS ... EACH FOLLOWED BY ITS LENGTH. \n\n");
            for (int k = 0; k < nLines; k++)
            {
                Console.WriteLine(lines[k], "\n");
                llen[k] = lines[k].Length;
                Console.WriteLine(llen[k]);
            }
            Console.WriteLine("\n\n");
            Console.ReadLine();

            //NOW PRINT OUT THE LINES 1 CHARACTER AT A TIME.

            Console.WriteLine("\nHERE ARE THE SAME LINES AGAIN ... this time printed character by character.\n\n");

            for (int i = 0; i < nLines; i++)
            {

                Console.WriteLine("\n" + lines[i] + "\n");
                line = lines[i].ToCharArray();
                len = line.Length;
                k = 0;
                if (state != StateType.word || type != CharType.minus)
                    state = StateType.white;
                ch = line[0];
                type = getType(ch);


                while (k < len - 1)
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
                            DblState();
                            break;
                        case StateType.expt:
                            ExpoState();
                            break;
                    }
                }
                Console.Write("\nPress any key to continue...");
                Console.ReadLine();
            }

            Console.WriteLine("\n");
            Console.ReadLine();
        }
        /**************************************** GET TYPE **********************************************/
        static CharType getType(char ch)
        {
            CharType type = CharType.whsp;

            if (isSpace(ch))
                type = CharType.whsp;
            else if (isAlpha(ch))
                if (toUpper(ch) == 'E')
                    type = CharType.expo;
                else
                    type = CharType.lett;
            else if (isDigit(ch))
                type = CharType.digit;
            else
            {
                switch (ch)
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
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isSpace(char ch)
        {
            return (ch == ' ' || ch == '\t' || ch == '\n');
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static bool isAlpha(char ch)
        {
            return ((toUpper(ch) >= 'A' && toUpper(ch) <= 'Z'));
        }
        /************************************************************************************************/
        /************************************************************************************************/
        static char toUpper(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                ch = (char)(ch - ('a' - 'A'));
            return ch;
        }
        /************************************************************************************************/
        /*************************************** WHITE STATE ********************************************/
        static void WhiteState()
        {
            while (state == StateType.white && k < len)
            {
                switch (type)
                {
                    case CharType.lett:
                    case CharType.expo:
                        WhiteToWord();
                        break;
                    case CharType.digit:
                    case CharType.plus:
                    case CharType.minus:
                        WhiteToNum();
                        break;
                    case CharType.point:
                        WhiteToDble();
                        break;
                    default:
                        Console.Write(ch);
                        JustChanged = false;
                        if (k < len - 1)
                            ch = line[++k];
                        else
                            return;
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WHITE TO WORD ******************************************/
        static void WhiteToWord()
        {
            if (JustChanged)
                Console.Write("ST0-1\n");
            else
                Console.Write("ST0-1\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.word;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WHITE TO NUM *******************************************/
        static void WhiteToNum()
        {
            if (JustChanged)
                Console.Write("ST0-2\n");
            else
                Console.Write("ST0-2\n");
            Console.Write(ch);
            ln = k;
            JustChanged = true;
            state = StateType.num;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WHITE TO DBLE ******************************************/
        static void WhiteToDble()
        {
            if (JustChanged)
                Console.Write("ST0-3\n");
            else
                Console.Write("ST0-3\n");
            Console.Write(ch);
            ln = k;
            JustChanged = true;
            state = StateType.dble;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /*************************************** WORD STATE *********************************************/
        static void WordState()
        {
            if (getType(lc) != CharType.minus)
            {
                wlen = 1;
            }

            while (state == StateType.word && k < len)
            {
                lc = ch;
                switch (type)
                {
                    case CharType.whsp:
                        WordToWhite();
                        break;
                    default:
                        wlen++;
                        Console.Write(ch);
                        JustChanged = false;
                        if (k < len - 1)
                            ch = line[++k];
                        else
                            return;
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /*************************************** WORD TO WHITE ******************************************/
        static void WordToWhite()
        {
            if (JustChanged)
                Console.Write(" ST1-0");
            else
                Console.Write(" ST1-0");
            Console.Write("  WORD LENGTH: " + wlen + "\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** NUM STATE *********************************************/
        static void NumState()
        {
            while (state == StateType.num && k < len)
            {
                switch (type)
                {
                    case CharType.whsp:
                        NumToWhite();
                        break;
                    case CharType.point:
                        NumToDbl();
                        break;
                    case CharType.expo:
                        NumToExpo();
                        break;
                    default:
                        Console.Write(ch);
                        JustChanged = false;
                        if (k < len - 1)
                            ch = line[++k];
                        else
                            return;
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** NUM TO WHITE ******************************************/
        static void NumToWhite()
        {
            if (JustChanged)
                Console.Write(" ST2-0");
            else
                Console.Write(" ST2-0");
            Console.Write("  INTEGER VALUE: " + atof(ln) + "\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** NUM TO DBL ********************************************/
        static void NumToDbl()
        {
            if (JustChanged)
                Console.Write(" ST2-3\n");
            else
                Console.Write(" ST2-3\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.dble;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** NUM TO EXPO *******************************************/
        static void NumToExpo()
        {
            if (JustChanged)
                Console.Write(" ST2-4\n");
            else
                Console.Write(" ST2-4\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.expt;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** DBL STATE *********************************************/
        static void DblState()
        {
            while (state == StateType.dble && k < len)
            {
                switch (type)
                {
                    case CharType.whsp:
                        DblToWhite();
                        break;
                    case CharType.expo:
                        DblToExpo();
                        break;
                    default:
                        Console.Write(ch);
                        JustChanged = false;
                        if (k < len - 1)
                            ch = line[++k];
                        else
                            return;
                        type = getType(ch);
                        break;
                }
            }
        }
        /************************************************************************************************/
        /**************************************** DBL TO WHITE ******************************************/
        static void DblToWhite()
        {
            if (JustChanged)
                Console.Write(" ST3-0");
            else
                Console.Write(" ST3-0");
            Console.Write("  DOUBLE VALUE: " + atof(ln) + "\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** DBL TO EXPO *******************************************/
        static void DblToExpo()
        {
            if (JustChanged)
                Console.Write(" ST3-4\n");
            else
                Console.Write(" ST3-4\n");
            Console.Write(ch);
            JustChanged = true;
            state = StateType.expt;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** EXPO STATE ********************************************/
        static void ExpoState()
        {
            while (state == StateType.expt && k < len)
            {
                switch (type)
                {
                    case CharType.whsp:
                        ExpoToWhite();
                        break;
                    default:
                        Console.Write(ch);
                        JustChanged = false;
                        if (k < len - 1)
                            ch = line[++k];
                        else
                        {
                            ExpoToWhite();
                            return;
                        }
                        type = getType(ch);
                        break;
                }
            }
            if (k >= len)
            {
                ExpoToWhite();
            }
        }
        /************************************************************************************************/
        /**************************************** EXPO TO WHITE *****************************************/
        static void ExpoToWhite()
        {
            if (JustChanged)
                Console.Write(" ST4-0");
            else
                Console.Write(" ST4-0");
            Console.Write("  EXPONENT-FORMAT DOUBLE VALUE: " + atof(ln) + "\n");
            if (getType(ch) == CharType.whsp)
            {
                Console.Write(ch);
            }
            JustChanged = true;
            state = StateType.white;
            if (k < len - 1)
                ch = line[++k];
            else
                return;
            type = getType(ch);
        }
        /************************************************************************************************/
        /**************************************** ASCII TO FLOAT ****************************************/
        static double atof(int l)
        {
            double val = 0;
            int sign = 1, i, power = 1, expt = 0, esign = 1;
            bool access = false;
            try
            {
                for (i = l; isSpace(line[i]); i++) ;
                if (line[i] == '-')
                    sign = -1;
                if (line[i] == '-' || line[i] == '+')
                    i++;
                for (val = 0; isDigit(line[i]); i++)
                    val = 10 * val + (line[i] - '0');
                if (line[i] == '.')
                    i++;
                for (power = 1; isDigit(line[i]); i++)
                {
                    val = 10 * val + (line[i] - '0');
                    power *= 10;
                }
                val /= power;
                if (line[i] == 'E' || line[i] == 'e')
                {
                    i++;
                    esign = 1;
                    if (line[i] == '-')
                        esign = -1;
                    if (line[i] == '-' || line[i] == '+')
                        i++;
                    for (expt = 0; isDigit(line[i]); i++)
                    {
                        access = true;
                        expt = expt * 10 + (line[i] - '0');
                    }
                    if (esign == 1)
                        while (expt-- > 0)
                            val *= 10;
                    else
                        while (expt-- > 0)
                            val /= 10;
                }
                return val * sign;
            }
            catch
            {
                val = (sign * val / power);
                if (access)
                    expt++;
                if (esign == 1)
                    while (expt-- > 0)
                        val *= 10;
                else
                    while (expt-- > 0)
                        val /= 10;
                return val;
            }
        }
    }
}
