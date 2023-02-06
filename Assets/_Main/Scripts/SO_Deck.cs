using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "IGDF/New Deck")]
    public class SO_Deck : ScriptableObject
    {
        public Card[] cards_Production;
        public Card[] cards_Design;
        public Card[] cards_Art;
        public Card[] cards_Code;
    }

    [System.Serializable]
    public class Card
    {
        public CardType cardType;
        public string cardName;
        public Sprite cardImage;
        public int cardValue;
    }

    public enum CardType
    {
        Production, Design, Art, Code
    }
}

