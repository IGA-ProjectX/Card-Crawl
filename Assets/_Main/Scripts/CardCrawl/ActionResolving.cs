using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResolving : MonoBehaviour
{
    public GameObject cardHighlight;

    public void DetectNearestSlot(int originIndex, Obj_Card hoverCard)
    {
        int nearestIndex = 0;
        float longestDistance = 10;
        List<float> distances = new List<float>();
        foreach (Transform slotTrans in FindObjectOfType<TurnResolving>().cardSlots)
            distances.Add(Vector3.Distance(hoverCard.transform.position, slotTrans.position));

        List<int> moveableIndexList = GetMoveableSlot(hoverCard);
        for (int i = 0; i < moveableIndexList.Count; i++)
            if (distances[moveableIndexList[i]] < longestDistance)
            {
                longestDistance = distances[moveableIndexList[i]];
                nearestIndex = moveableIndexList[i];
            }
        if (nearestIndex == originIndex)
            cardHighlight.SetActive(false);
        else
        {
            cardHighlight.SetActive(true);
            cardHighlight.transform.position = FindObjectOfType<TurnResolving>().cardSlots[nearestIndex].position;
        }
    }

    public int GetNearestSlotIndex(int originIndex, Obj_Card hoverCard)
    {
        int nearestIndex = 0;
        float longestDistance = 10;
        List<float> distances = new List<float>();
        foreach (Transform slotTrans in FindObjectOfType<TurnResolving>().cardSlots)
            distances.Add(Vector3.Distance(hoverCard.transform.position, slotTrans.position));

        List<int> moveableIndexList = GetMoveableSlot(hoverCard);
        for (int i = 0; i < moveableIndexList.Count; i++)
            if (distances[moveableIndexList[i]] < longestDistance)
            {
                longestDistance = distances[moveableIndexList[i]];
                nearestIndex = moveableIndexList[i];
            }
        cardHighlight.SetActive(false);
        return nearestIndex;
    }

    public List<int> GetMoveableSlot(Obj_Card hoverCard)
    {
        TurnResolving tr = FindObjectOfType<TurnResolving>();
        List<int> tempSlots = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        if (hoverCard.inSlotIndex < 4) //Upper
        {
            for (int i = 0; i < 4; i++) //去除所有上层卡池序列（除自身卡池外）
                if (i != hoverCard.inSlotIndex)
                    tempSlots.Remove(i);

            if (hoverCard.cardType == CardType.Monster)//当Hover Card为Monster Card, 去除所有下层卡池序列（除Shield卡和Character卡池外）
            {
                for (int i = 0; i < 4; i++)
                    if (tr.cardsInTurn[i + 4] == null) tempSlots.Remove(i + 4);
                    else if (tr.cardsInTurn[i + 4].GetComponent<Obj_Card>() != null)
                        if (tr.cardsInTurn[i + 4].GetComponent<Obj_Card>().cardType != CardType.Shield)
                            tempSlots.Remove(i + 4);
            }
            else //当Hover Card不为Monster Card, 去除所有下层已有卡牌卡池序列及Character Card卡池
            {
                for (int j = 0; j < 4; j++)
                    if (tr.cardsInTurn[j + 4] != null) tempSlots.Remove(j + 4);
                if (tempSlots.Contains(5)) tempSlots.Remove(5);
            }
        }
        else //Bottom
        {
            if (hoverCard.inSlotIndex != 7) //当卡不在背包池时，去除所有下层卡池序列（除自身卡池外）
            {
                for (int i = 0; i < 4; i++)
                    if ((i + 4) != hoverCard.inSlotIndex)
                        tempSlots.Remove(i + 4);
                if (hoverCard.cardType == CardType.Weapon)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tr.cardsInTurn[j] == null) tempSlots.Remove(j);
                        else

                            if (tr.cardsInTurn[j].GetComponent<Obj_Card>().cardType != CardType.Monster)
                            tempSlots.Remove(j);

                    }
                }
                tempSlots.Remove(7);
            }
            else //当卡牌在背包栏时，去除所有上层序列，玩家卡序列，和已被占用卡池序列
            {
                for (int i = 0; i < 4; i++) tempSlots.Remove(i);
                for (int i = 0; i < 4; i++)
                {
                    if (tr.cardsInTurn[i + 4] != null && i != 3)
                    {
                        tempSlots.Remove(i + 4);
                    }
                }
            }
        }
        return tempSlots;
    }

    public void CardToCard(Obj_Card instigator,Obj_Card target)
    {
        switch (instigator.cardType)
        {
            case CardType.Weapon:
                target.ValueChange(-instigator.cardValue);
                instigator.DestroyCard();
                break;
            case CardType.Monster:
                if (target.cardType == CardType.Shield)
                {
                    int difference = instigator.cardValue - target.cardValue;
                    target.ValueChange(-instigator.cardValue);
                    if (difference > 0) FindObjectOfType<Obj_Character>().HealthChange(-difference);
                    instigator.DestroyCard();
                }
                break;
            case CardType.Special:
                break;
        }
    }

    public void CardToCharacter(Obj_Card instigator, Obj_Character target)
    {
        if (instigator.cardType == CardType.Monster)
        {
            target.HealthChange(-instigator.cardValue);
            instigator.DestroyCard();
        }
    }

    public void CardToSlot(Obj_Card instigator)
    {
        switch (instigator.cardType)
        {
            case CardType.Health:
                if (instigator.inSlotIndex != 7)
                {
                    FindObjectOfType<Obj_Character>().HealthChange(instigator.cardValue);
                    instigator.WaitingForDestroy();
                }
                break;
            case CardType.Gold:
                FindObjectOfType<Obj_Character>().GoldChange(instigator.cardValue);
                instigator.WaitingForDestroy();
                break;
            case CardType.Shield:
                if(instigator.inSlotIndex ==4 || instigator.inSlotIndex ==6)
                instigator.GetComponent<BoxCollider2D>().enabled = false;
                break;
        }
    }
}
