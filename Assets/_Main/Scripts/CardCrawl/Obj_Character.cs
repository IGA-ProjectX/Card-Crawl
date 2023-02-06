using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Obj_Character : MonoBehaviour
{
    private CardType cardType;
    private BaseCard cardData;
    private int cardCurrentHealth;
    private int cardMaxHealth;
    private int cardGold;

    public void InitializeCharacter(BaseCard card)
    {
        if (card is CharacterCard)
        {
            CharacterCard characterCard = (CharacterCard)card;
            cardType = CardType.Character;
            cardMaxHealth = characterCard.maxHealth;
            cardCurrentHealth = cardMaxHealth;
            cardGold = 0;
            cardData = card;
        }
        else return;
        transform.Find("Card Image").GetComponent<SpriteRenderer>().sprite = card.cardImage;
        transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardName;
        transform.Find("Card Health").GetComponent<TMP_Text>().text = cardCurrentHealth + "/" + cardMaxHealth;
        transform.Find("Card Gold").GetComponent<TMP_Text>().text = cardGold.ToString();
    }

    public void HealthChange(int healthToChange)
    {
        cardCurrentHealth += healthToChange;
        if (cardCurrentHealth > cardMaxHealth) cardCurrentHealth = cardMaxHealth;
        if (cardCurrentHealth > 0)
            transform.Find("Card Health").GetComponent<TMP_Text>().text = cardCurrentHealth + "/" + cardMaxHealth;
        else
        {
            cardCurrentHealth = 0;
            transform.Find("Card Health").GetComponent<TMP_Text>().text = "0/" + cardMaxHealth;
            PlayerDefeated();
        }
    }

    public void GoldChange(int goldToChange)
    {
        cardGold += goldToChange;
        if (cardGold < 0) cardGold = 0;
        transform.Find("Card Gold").GetComponent<TMP_Text>().text = cardGold.ToString();
    }

    void PlayerDefeated()
    {

    }
}
