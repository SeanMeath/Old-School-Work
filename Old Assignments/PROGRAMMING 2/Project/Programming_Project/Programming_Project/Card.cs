using System;

namespace Programming_Project
{
    #region Enums
    public enum Suit { Hearts, Diamonds, Clubs, Spades };
    public enum FaceValue { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace };
    #endregion

    public class Card
    {
        #region Backing Fields
        private FaceValue _faceValue;
        private Suit _suit;
        #endregion

        #region Constructors
        public Card()
        {
            this._suit = Suit.Spades;
            this._faceValue = FaceValue.Ace;
        }

        public Card(Suit theSuit, FaceValue theFaceValue)
        {
            this._suit = theSuit;
            this._faceValue = theFaceValue;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            return this._faceValue + " of " + this._suit;
        }
        #endregion

        #region Properties
        public Suit GetSuit()
        {
            return this._suit;
        }

        public FaceValue GetFaceValue()
        {
            return this._faceValue;
        }
        #endregion

    }
}
