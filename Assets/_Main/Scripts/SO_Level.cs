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
        public int[] staffValue;
        public Sprite levelButtonImage;
        [Header("Deck")]
        public Card[] cards_Production;
        public Card[] cards_Design;
        public Card[] cards_Art;
        public Card[] cards_Code;
        [Header("Product Level")]
        public Product[] productLevels;
        [Header("Narrative")]
        public TalkContent[] talkList;
    }

    [System.Serializable]
    public class Card
    {
        public CardType cardType;
        public string cardOutNE;
        public string cardOutNC;
        public string cardNameEng;
        public string cardNameChi;
        [TextArea(4,10)]
        public string cardSummaryEng;
        [TextArea(4, 10)]
        public string cardSummaryChi;
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
        public string[] productUserTags;
    }

    public class ChatContent
    {
        public CharacterType instigator;
        public string content;

    }

    public enum CardType { Production, Design, Art, Code }

    public enum LevelType
    {
        PapersPlease,
        SuperHot,
        SuperMario
    }

    public enum ProductLevel { None, Raw, Medium, Welldone }

    public enum CharacterType { Producer, Designer, Artist, Programmer }

    [System.Serializable]
    public class TalkContent
    {
        public TalkConditionType conditionType;
        public CharacterType talkCharacter;
        [TextArea(3,3)]
        public string talkContentChi;
        [TextArea(3,3)]
        public string talkContentEng;
    }
}

