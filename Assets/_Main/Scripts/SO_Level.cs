using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "IGDF/New Deck")]
    public class SO_Level : ScriptableObject
    {
        public LevelType levelType;
        public string levelName;
        public Sprite levelButtonImage;
        [Header("Deck")]
        public Card[] cards_Production;
        public Card[] cards_Design;
        public Card[] cards_Art;
        public Card[] cards_Code;
        [Header("Product Level")]
        public Product[] productLevels;
    }

    [System.Serializable]
    public class Card
    {
        public CardType cardType;
        public string cardName;
        public Sprite cardImage;
        public int cardValue;
    }

    [System.Serializable]
    public class Product
    {
        public ProductLevel productLevel;
        public string productName;
        [TextArea(2,2)]
        public string productDescription;
        public Sprite productImage;
    }

    public enum CardType
    {
        Production, Design, Art, Code
    }

    public enum LevelType
    {
        PapersPlease,
        SuperHot,
        SuperMario
    }

    public enum ProductLevel
    {
        None, Raw, Medium, Welldone
    }
}

