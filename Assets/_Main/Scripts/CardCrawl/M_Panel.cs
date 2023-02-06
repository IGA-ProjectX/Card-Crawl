using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class M_Panel : MonoBehaviour
{
    public Info_Resource gameResource;
    public Info_CardData cardData;

    public Transform p_CardCollection;


    public void OnOpenCardCollection()
    {
        PI_CardCollection();
        p_CardCollection.DOMoveX(0, 0.7f);
    }

    public void OnCloseCardCollection()
    {

    }

    private void PI_CardCollection()
    {
        List<Transform> ui_CardTransList = new List<Transform>();
        for (int i = 0; i < p_CardCollection.Find("Card Layout").childCount; i++)
            ui_CardTransList.Add(p_CardCollection.Find("Card Layout").GetChild(i));

        for (int i = 0; i < cardData.specialCards.Length; i++)
        {
            Text nameT = ui_CardTransList[i].Find("Name").GetComponent<Text>();
            Text valueT = ui_CardTransList[i].Find("Value").GetComponent<Text>();
            Image CardI = ui_CardTransList[i].Find("Image").GetComponent<Image>();
            Image CardB = ui_CardTransList[i].GetComponent<Image>();
            nameT.text = cardData.specialCards[i].cardName;
            if (cardData.specialCards[i].cardValue == 0)
            {
                CardB.sprite = gameResource.cardBG_NoValue;
                valueT.gameObject.SetActive(false);
            }
            else
            {
                CardB.sprite = gameResource.cardBG_WithValue;
                valueT.text = cardData.specialCards[i].cardValue.ToString();
            }
            CardI.sprite = cardData.specialCards[i].cardImage;
            if (cardData.specialCards[i].isLocked)
            {
                CardI.color = Color.red;
            }
        }
    }
}
