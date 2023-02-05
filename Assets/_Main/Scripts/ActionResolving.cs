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

    //public List<int> GetMoveableSlot(Obj_Card hoverCard)
    //{
    //    TurnResolving tr = FindObjectOfType<TurnResolving>();
    //    List<int> tempSlots = new List<int>();
    //    List<CardPosTarget> moveableTargets = new List<CardPosTarget>();

    //    switch (hoverCard.cardState)
    //    {
    //        case CardState.InPool:
    //            foreach (var target in ReturnTargetList(CardState.InPool))
    //                moveableTargets.Add(target);
    //            break;
    //        case CardState.InHand:
    //            foreach (var target in ReturnTargetList(CardState.InHand))
    //                moveableTargets.Add(target);
    //            break;
    //        case CardState.InBackpack:
    //            foreach (var target in ReturnTargetList(CardState.InBackpack))
    //                moveableTargets.Add(target);
    //            break;
    //    }

    //    foreach (var target in moveableTargets)
    //    {
    //        switch (target)
    //        {
    //            case CardPosTarget.None:
    //                break;
    //            case CardPosTarget.EmptyBackpack:
    //                if (tr.cardsInTurn[7] == null) tempSlots.Add(7);
    //                break;
    //            case CardPosTarget.EmptyHand:
    //                if (tr.cardsInTurn[4] == null) tempSlots.Add(4);
    //                if (tr.cardsInTurn[6] == null) tempSlots.Add(6);
    //                break;
    //            case CardPosTarget.EmptyPool:
    //                if (tr.cardsInTurn[0] == null) tempSlots.Add(0);
    //                if (tr.cardsInTurn[1] == null) tempSlots.Add(1);
    //                if (tr.cardsInTurn[2] == null) tempSlots.Add(2);
    //                if (tr.cardsInTurn[3] == null) tempSlots.Add(3);
    //                break;
    //            case CardPosTarget.Monster:
    //                if (tr.cardsInTurn[0] != null && IsInSlotCard(0, CardType.Monster) == true) tempSlots.Add(0);
    //                if (tr.cardsInTurn[1] != null && IsInSlotCard(1, CardType.Monster) == true) tempSlots.Add(1);
    //                if (tr.cardsInTurn[2] != null && IsInSlotCard(2, CardType.Monster) == true) tempSlots.Add(2);
    //                if (tr.cardsInTurn[3] != null && IsInSlotCard(3, CardType.Monster) == true) tempSlots.Add(3);
    //                break;
    //            case CardPosTarget.Character:
    //                tempSlots.Add(5);
    //                break;
    //            case CardPosTarget.Health:
    //                for (int i = 0; i < 7; i++)
    //                    if (tr.cardsInTurn[i] != null && IsInSlotCard(i, CardType.Health) == true && !tr.cardsInTurn[i].GetComponent<Obj_Card>().isForDestroy) tempSlots.Add(i);
    //                break;
    //            case CardPosTarget.Gold:
    //                for (int i = 0; i < 7; i++)
    //                    if (tr.cardsInTurn[i] != null && IsInSlotCard(i, CardType.Gold) == true && !tr.cardsInTurn[i].GetComponent<Obj_Card>().isForDestroy) tempSlots.Add(i);
    //                break;
    //            case CardPosTarget.Weapon:
    //                for (int i = 0; i < 7; i++)
    //                    if (tr.cardsInTurn[i] != null && IsInSlotCard(i, CardType.Weapon) == true) tempSlots.Add(i);
    //                break;
    //            case CardPosTarget.Shield:
    //                for (int i = 0; i < 7; i++)
    //                    if (tr.cardsInTurn[i] != null && IsInSlotCard(i, CardType.Shield) == true) tempSlots.Add(i);
    //                break;
    //            case CardPosTarget.ValueSpecial:
    //                break;
    //            case CardPosTarget.ShopBox:
    //                break;
    //        }
    //    }
    //    tempSlots.Add(hoverCard.inSlotIndex);
    //    return tempSlots;

    //    CardPosTarget[] ReturnTargetList(CardState cardState)
    //    {
    //        for (int i = 0; i < GameBootstrap.instance.cardData.cardUseLogics.Length; i++)
    //        {
    //            if (GameBootstrap.instance.cardData.cardUseLogics[i].cardType == hoverCard.cardType)
    //            {
    //                switch (cardState)
    //                {
    //                    case CardState.InPool:
    //                        return GameBootstrap.instance.cardData.cardUseLogics[i].targets_Pool;
    //                    case CardState.InHand:
    //                        return GameBootstrap.instance.cardData.cardUseLogics[i].targets_Hand;
    //                    case CardState.InBackpack:
    //                        return GameBootstrap.instance.cardData.cardUseLogics[i].targets_Backpack;
    //                }
    //            }
    //        }
    //        return null;
    //    }

    //    bool IsInSlotCard(int index, CardType type)
    //    {
    //        bool isSame = false;
    //        Debug.Log(tr.cardsInTurn[index].GetComponent<Obj_Card>().cardType);
    //        if (tr.cardsInTurn[index].GetComponent<Obj_Card>().cardType == type) isSame = true;
    //        else isSame = false;
    //        return isSame;
    //    }
    //}

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
