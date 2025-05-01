using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Programming_Project
{

    class Program
    {
        const int MENUCHOICES = 2;
        const int CARDCHOICES = 5;
        const int TESTCHOICES = 9;

        const int STARTINGVALUE = 1000;
        const int HANDSIZE = 5;

        static void Main(string[] args)
        {
            DisplayMenu();
            int choice = GetUserChoice(MENUCHOICES);
            Clear();
            switch (choice)
            {
                case 1:
                    {
                        Play();
                        break;
                    }
                case 2:
                    {
                        Test();
                        break;
                    }
                case 0:
                    {
                        break;
                    }
            }
        }

        #region Menu
        static void DisplayMenu()
        {
            WriteLine("~~~~~~~~~~~~~~~Poker Game~~~~~~~~~~~~~~~");
            WriteLine("\n   1: Play\n   2: Test");
            WriteLine("\nPlease enter your choice or press 0 to exit.");
        }
        static int GetUserChoice(int numberOfAcceptableChoices)
        {
            bool success = int.TryParse(ReadLine(), out int choice);
            while (success == false || choice < 0 || choice > numberOfAcceptableChoices)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Error the input received does not fit the desired range. Please try again.");
                ResetColor();
                WriteLine("\n");
                DisplayMenu();
                success = int.TryParse(ReadLine(), out choice);
            }
            return choice;
        }
        #endregion

        #region Play Methods
        static void Play()
        {
            double balance = STARTINGVALUE;
            Card[] hand = new Card[HANDSIZE];
            Deck deck = new Deck();

            double bet;
            string proceed;

            do
            {
                WriteLine("Current Balance: " + balance.ToString("c"));
                WriteLine("\nHow much do you wish to bet in this round? (1.." + balance + ") :");
                bet = ReceiveBet(balance);
                balance -= bet;
                deck.Shuffle();
                PopulateHand(ref hand, deck);
                SortHand(ref hand);
                DisplayHand(hand);
                SwapTime(ref hand, deck);
                bet = DetectOutcome(hand, bet);
                balance += bet;
                WriteLine("Current Balance: "+ balance.ToString("c"));
                if (balance <= 0)
                {
                    WriteLine("You lost all your money. You cannot play with no money to bet. Press any key to continue");
                    ReadKey();
                    proceed = "";
                }
                else
                {
                    WriteLine("\nDo you wish to continue? (1 for yes or anything else for no)");
                    proceed = ReadLine();
                    Clear();
                }
            } while (proceed=="1" && balance > 0);
        }

        #region Swapping
        static void SwapTime(ref Card[] hand, Deck deck)
        {
            const int MAXSWAPS = 4;

            bool[] swaps = new bool[hand.Length];
            bool success;
            int choice;
            int i = 0;
            while (i < MAXSWAPS || i == -1)
            {
                WriteLine("Which card would you wish to swap? (Press 0 if you don't wish to swap)");
                success = int.TryParse(ReadLine(), out choice);
                while (!success || choice < 0 || choice > hand.Length)
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("Error the answer provided does not consist of a valid response. Please try again.\n");
                    ResetColor();
                    WriteLine("Which card would you wish to swap? (Press 0 if you don't wish to swap)");
                    success = int.TryParse(ReadLine(), out choice);
                }

                choice--;

                if (choice >= 0)
                {
                    if (swaps[choice] == false)
                    {
                        swaps[choice] = true;
                        i++;
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine("Card successfully swapped");
                        ResetColor();
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("Error the answer provided was already swapped. Please try again.\n");
                        ResetColor();
                    }
                }
                else
                {
                    break;
                }
            }
            for (int s = 0; s < swaps.Length; s++)
            {
                if (swaps[s] == true)
                {
                    hand[s] = deck.DealACard();
                }
            }
            SortHand(ref hand);
            Clear();
            DisplayHand(hand);
        }
        #endregion

        #region Hand Functions (Populate, Sort)
        static void PopulateHand(ref Card[] hand, Deck deck)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = deck.DealACard();
            }
        }

        static void SortHand(ref Card[] hand)
        {
            int startScan, maxIndex;
            Card maxValue;
            for (startScan = 0; startScan < (hand.Length - 1); startScan++)
            {
                maxIndex = startScan;
                maxValue = hand[startScan];

                for (int index = startScan + 1; index < hand.Length; index++)
                {
                    if (hand[index].GetFaceValue() < maxValue.GetFaceValue())
                    {
                        maxValue = hand[index];      //FIND SMALLEST VALUE
                        maxIndex = index;
                    }
                }
                hand[maxIndex] = hand[startScan];     //SWAP
                hand[startScan] = maxValue;
            }
        }
        #endregion
        #endregion

        #region Test Methods
        static void Test()
        {
            string choice = "0";
            double balance = 1000;
            Card[] hand = new Card[HANDSIZE];
            bool leave = false;

            do
            {
                WriteLine("You currently have a wallet of: " + balance.ToString("c"));
                DisplayChoices();
                choice = ReadLine();
                Clear();
                switch (choice)
                {
                    default:
                        {
                            leave = true;
                            break;
                        }
                    case "1":
                        {
                            hand[0] = new Card(Suit.Spades, FaceValue.Ten);
                            hand[1] = new Card(Suit.Spades, FaceValue.Jack);
                            hand[2] = new Card(Suit.Spades, FaceValue.Queen);
                            hand[3] = new Card(Suit.Spades, FaceValue.King);
                            hand[4] = new Card(Suit.Spades, FaceValue.Ace);
                            balance=TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "2":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Hearts, FaceValue.Three);
                            hand[2] = new Card(Suit.Hearts, FaceValue.Four);
                            hand[3] = new Card(Suit.Hearts, FaceValue.Five);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Six);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "3":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Four);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Four);
                            hand[2] = new Card(Suit.Spades, FaceValue.Four);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Four);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Six);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "4":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Two);
                            hand[2] = new Card(Suit.Spades, FaceValue.Two);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Three);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Three);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "5":
                        {
                            hand[0] = new Card(Suit.Diamonds, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Three);
                            hand[2] = new Card(Suit.Diamonds, FaceValue.Five);
                            hand[3] = new Card(Suit.Diamonds, FaceValue.Seven);
                            hand[4] = new Card(Suit.Diamonds, FaceValue.Nine);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "6":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Three);
                            hand[2] = new Card(Suit.Hearts, FaceValue.Four);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Five);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Six);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "7":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Two);
                            hand[2] = new Card(Suit.Spades, FaceValue.Two);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Three);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Four);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "8":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Two);
                            hand[2] = new Card(Suit.Spades, FaceValue.Three);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Three);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Ace);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                    case "9":
                        {
                            hand[0] = new Card(Suit.Hearts, FaceValue.Two);
                            hand[1] = new Card(Suit.Diamonds, FaceValue.Three);
                            hand[2] = new Card(Suit.Spades, FaceValue.Four);
                            hand[3] = new Card(Suit.Clubs, FaceValue.Ace);
                            hand[4] = new Card(Suit.Hearts, FaceValue.Ace);
                            balance = TestContinued(hand, balance);
                            WriteLine("\nPress any key to continue");
                            ReadKey();
                            Clear();
                            break;
                        }
                }

            }
            while (!leave);
        }

        static double TestContinued(Card[] hand, double balance)
        {
            WriteLine(balance.ToString("c"));
            WriteLine("\nHow much do you wish to bet in this round? (1.." + balance + ") :");
            double bet = ReceiveBet(balance);
            balance -= bet;
            DisplayHand(hand);
            bet = DetectOutcome(hand, bet);
            balance += bet;
            return balance;
        }

        static void DisplayChoices()
        {
            WriteLine("~~~~~~~~~~~~~~~Test Game~~~~~~~~~~~~~~~");
            WriteLine("\n   1: Royal Flush\n   2: Straight Flush\n   3: Four-of-a-Kind\n   4: Full House\n   5: Flush\n   6: Straight\n   7: Three-of-a-Kind\n   8: 2 Pair\n   9: Pair\n   Anything: Quit");
            WriteLine("\nPlease enter your choice.");
        }
        #endregion

        #region Universal Methods
        static void DisplayHand(Card[] hand)
        {
            for (int i = 0; i < hand.Length; i++)
            {
                WriteLine((i + 1) + ": " + hand[i].ToString());
            }
            WriteLine();
        }

        static double ReceiveBet(double max)
        {
            bool success = double.TryParse(ReadLine(), out double choice);
            while (success == false || choice <= 0 || choice > max)
            {
                Clear();
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Error the input received does not fit the desired range. Please try again.");
                ResetColor();
                WriteLine("\nHow much do you wish to bet in this round? (1.." + max + ") :");
                success = double.TryParse(ReadLine(), out choice);
            }
            Clear();
            return choice;
        }

        #region Win Checker
        static double DetectOutcome(Card[] hand, double bet)
        {
            const int NUMFACES = 13;
            const int NUMSUITS = 4;

            const int ROYALEPAYOUT = 250;
            const int TRAIGHTFLUSHPAYOUT = 50;
            const int STRAIGHTPAYOUT = 4;
            const int FLUSHPAYOUT = 6;
            const int FOURPAYOUT = 25;
            const int FULLHOUSEPAYOUT = 9;
            const int THREEPAYOUT = 3;
            const int TWOPAIRPAYOUT = 2;
            const int PAIRPAYOUT = 1;

            int[] face = new int[NUMFACES];
            int[] suit = new int[NUMSUITS];

            for (int i = 0; i < hand.Length; i++)
            {
                face[(int)hand[i].GetFaceValue()]++;
                suit[(int)hand[i].GetSuit()]++;
            }

            bool f = Flush(suit);
            bool s = Straight(face);
            bool r = Royale(face);
            bool four = FourOfAKind(face);
            bool full = FullHouse(face);
            bool three = ThreeOfAKind(face);
            bool tp = TwoPair(face);
            bool p = Pair(face);

            if (f && s && r)
            {
                WriteLine("Winning Hand: Royal Flush");
                WriteLine("Money Won: "+ (ROYALEPAYOUT * bet).ToString("c"));
                return ROYALEPAYOUT * bet;
            }
            else if (f && s)
            {
                WriteLine("Winning Hand: Straight Flush");
                WriteLine("Money Won: " + (TRAIGHTFLUSHPAYOUT * bet).ToString("c"));
                return TRAIGHTFLUSHPAYOUT * bet;
            }
            else if (s)
            {
                WriteLine("Winning Hand: Straight");
                WriteLine("Money Won: " + (STRAIGHTPAYOUT * bet).ToString("c"));
                return STRAIGHTPAYOUT * bet;
            }
            else if (f)
            {
                WriteLine("Winning Hand: Flush");
                WriteLine("Money Won: " + (FLUSHPAYOUT * bet).ToString("c"));
                return FLUSHPAYOUT * bet;
            }
            else if (four)
            {
                WriteLine("Winning Hand: Four of a Kind");
                WriteLine("Money Won: " + (FOURPAYOUT * bet).ToString("c"));
                return FOURPAYOUT * bet;
            }
            else if (full)
            {
                WriteLine("Winning Hand: Full House");
                WriteLine("Money Won: " + (FULLHOUSEPAYOUT * bet).ToString("c"));
                return FULLHOUSEPAYOUT * bet;
            }
            else if (three)
            {
                WriteLine("Winning Hand: Three of a Kind");
                WriteLine("Money Won: " + (THREEPAYOUT * bet).ToString("c"));
                return THREEPAYOUT * bet;
            }
            else if (tp)
            {
                WriteLine("Winning Hand: Two Pairs");
                WriteLine("Money Won: " + (TWOPAIRPAYOUT * bet).ToString("c"));
                return TWOPAIRPAYOUT * bet;
            }
            else if (p)
            {
                WriteLine("Winning Hand: Pair");
                WriteLine("Money Won: " + (PAIRPAYOUT * bet).ToString("c"));
                return PAIRPAYOUT * bet;
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Losing Hand :(");
                ResetColor();
                WriteLine("Money Lost: " + (bet).ToString("c"));
                return 0;
            }
        }

        #region Individual Checks
        static bool Flush (int[] suit)
        {
            for (int i = 0; i < suit.Length; i++)
            {
                if (suit[i] == 5)
                {
                    return true;
                }
            }
            return false;
        }

        static bool Straight(int[] face)
        {
            const int SLENGTH = 5;

            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] > 1)
                {
                    return false;
                }
                else if (face[i] == 1)
                {
                    int start = i;
                    break;
                }
            }

            for (int i = 0; i < SLENGTH-1; i++)
            {
                if (face[i] != face[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        static bool Royale(int[] face)
        {
            if (face[face.Length - 1] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool FourOfAKind(int[] face)
        {
            const int SEARCHNUM = 4;

            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] == SEARCHNUM)
                {
                    return true;
                }
            }
            return false;
        }

        static bool FullHouse(int[] face)
        {
            const int SEARCHNUM_1 = 2;
            const int SEARCHNUM_2 = 3;

            bool two = false;
            bool three = false;
            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] == SEARCHNUM_1)
                {
                    two = true;
                }
                else if (face[i] == SEARCHNUM_2)
                {
                    three = true;
                }
            }

            if(two && three)
            {
                return true;
            }

            return false;
        }

        static bool ThreeOfAKind(int[] face)
        {
            const int SEARCHNUM = 3;

            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] == SEARCHNUM)
                {
                    return true;
                }
                else if (face[i] != 1 || face[i] != 0)
                {
                    return false;
                }
            }
            return false;
        }

        static bool TwoPair(int[] face)
        {
            const int SEARCHNUM = 2;

            int p = 0;

            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] == SEARCHNUM)
                {
                    p++;
                }
            }

            if (p == SEARCHNUM)
            {
                return true;
            }
            return false;
        }

        static bool Pair(int[] face)
        {
            const int SEARCHNUM = 2;

            for (int i = 0; i < face.Length; i++)
            {
                if (face[i] == SEARCHNUM)
                {
                    if (i >= (int)FaceValue.Jack)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #endregion
        #endregion
    }
}
