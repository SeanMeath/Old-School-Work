using System;

namespace Programming_Project
{
    class Deck
    {
        #region Constants
        public const int NUM_OF_CARDS_IN_DECK = 52;
        private const int TIMES_TO_SHUFFLE = 3;
        #endregion

        #region Backing Fields
        private Card[] deck = new Card[NUM_OF_CARDS_IN_DECK];
        private int currentCard = 0;
        #endregion

        #region Constructor
        public Deck()
        {
            currentCard = 0;

            foreach (Suit s in Enum.GetValues(typeof(Suit)))
            {
                foreach (FaceValue f in Enum.GetValues(typeof(FaceValue)))
                {
                    deck[currentCard++] = new Card(s, f);
                }
            }
			
			currentCard = 0;
        }
        #endregion

        #region Methods
        public void Shuffle()
        {
            currentCard = 0;

            Random random = new Random();
            Card tempCard;

            for (int shuffle = 0; shuffle < TIMES_TO_SHUFFLE; shuffle++)
            {
                for (int loop = 0; loop < NUM_OF_CARDS_IN_DECK; loop++)
                {
                    int rand = random.Next(NUM_OF_CARDS_IN_DECK);

                    tempCard = deck[rand];
                    deck[rand] = deck[loop];
                    deck[loop] = tempCard;
                }
            }
        }


        // Pre-Condtion - the deck cannot be empty.
        // IsEmpty is verified in the application before calling dealACard()
        public Card DealACard()
        {
            return this.deck[currentCard++];
        }

        public bool IsEmpty()
        {
            return currentCard >= NUM_OF_CARDS_IN_DECK;
        }

        //Used only for testing
        //Comment it before putting it in production/submitting it. 
		public override string ToString()
		{
            string output = "";

            foreach (Card c in deck)
            {
                output += c.ToString() + "\n";
            }

            return output;
		}
        #endregion
    }
}
