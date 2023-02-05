using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card Data",menuName ="Scriptable Objs/Card Data")]
public class Info_CardData : ScriptableObject
{
    public HealthCard[] healthCards;
    public GoldCard[] goldCards;
    public WeaponCard[] weaponCards;
    public ShieldCard[] shieldCards;
    public MonsterCard[] monsterCards;
    public SpecialCard[] specialCards;
    public CharacterCard[] characterCards;
    public CardUseLogic[] cardUseLogics;
}

[System.Serializable] public class BaseCard { public string cardName; public Sprite cardImage; }
[System.Serializable] public class ValueCard : BaseCard { public int cardValue; }
[System.Serializable] public class HealthCard : ValueCard { }
[System.Serializable] public class GoldCard : ValueCard { }
[System.Serializable] public class WeaponCard : ValueCard { }
[System.Serializable] public class ShieldCard : ValueCard { }
[System.Serializable] public class MonsterCard : ValueCard { }
[System.Serializable] public class SpecialCard : ValueCard { public CardEffect cardEffect; [TextArea(2,2)] public string effectDescription; public bool isLocked; public int purchaseValue; }
[System.Serializable] public class CharacterCard : BaseCard { public int maxHealth; }
[System.Serializable] public class CardUseLogic { 
    public CardType cardType;
    public CardPosTarget[] targets_Pool;
    public CardPosTarget[] targets_Hand;
    public CardPosTarget[] targets_Backpack;
    public CardPosTarget[] targets_Use;
}

public enum CardEffect { None, Equalize, Swap, Trade, Steal, Leech, Lash }
public enum CardType { Health, Gold, Weapon, Shield, Monster, Special, Character }
public enum CardState { InPool, InHand, InBackpack}
public enum CardPosTarget { None, EmptyBackpack, EmptyHand, EmptyPool, Monster, Character, Health, Gold, Weapon, Shield, ValueSpecial ,ShopBox}
