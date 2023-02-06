using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Obj_Card : MonoBehaviour,ICommand
{
    [HideInInspector] public CardType cardType;
    [HideInInspector] public CardState cardState;
    private BaseCard cardData;
    [HideInInspector]public int cardValue;
    private Vector3 lastMousePosition = Vector3.zero;
    [HideInInspector]public int inSlotIndex;
    [HideInInspector] public bool isForDestroy = false;

    public void InitializeCard(BaseCard card,int index)
    {
        cardData = card;
        transform.Find("Card Image").GetComponent<SpriteRenderer>().sprite = card.cardImage;
        transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardName;
        inSlotIndex = index;
        cardState = CardState.InPool;
        if (card is ValueCard)
        {
            ValueCard valueCard = (ValueCard)card;
            if (card is HealthCard) cardType = CardType.Health;
            else if (card is GoldCard) cardType = CardType.Gold;
            else if (card is WeaponCard) cardType = CardType.Weapon;
            else if (card is ShieldCard) cardType = CardType.Shield;
            else if (card is MonsterCard) cardType = CardType.Monster;
            else if (card is SpecialCard) cardType = CardType.Special;

            if (valueCard.cardValue == 0)
            {
                transform.Find("Card Value").gameObject.SetActive(false);
                transform.GetComponent<SpriteRenderer>().sprite = GameBootstrap.instance.cardLayouts[0];
            }
            else ValueChange(valueCard.cardValue);
        }
        else return;
    }

    public void ValueChange(int valueToAdd)
    {
        cardValue += valueToAdd;
        transform.Find("Card Value").GetComponent<TMP_Text>().text = cardValue.ToString();
        if (cardValue <= 0) DestroyCard();
    }

    #region - Interaction -
    public void OnMouseDown()
    {
        transform.DOMoveZ(-0.2f, 0.1f);
    }

    public void OnMouseDrag()
    {
        if (lastMousePosition != Vector3.zero)
        {
            Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
            transform.position += offset;
        }
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FindObjectOfType<ActionResolving>().DetectNearestSlot(inSlotIndex,this);
    }

    public void OnMouseUp()
    {
        lastMousePosition = Vector3.zero;
        int nearestSlotIndex = FindObjectOfType<ActionResolving>().GetNearestSlotIndex(inSlotIndex, this);
        if (nearestSlotIndex == inSlotIndex) CardMoveBack();
        else
        {
            TurnResolving tr = FindObjectOfType<TurnResolving>();
            ActionResolving ar = FindObjectOfType<ActionResolving>();
            if (tr.cardsInTurn[nearestSlotIndex] == null)
            {
                tr.cardsInTurn[inSlotIndex] = null;
                tr.cardsInTurn[nearestSlotIndex] = transform;
                inSlotIndex = nearestSlotIndex;
                ar.CardToSlot(this);
                CardMoveBack();
            }
            else
            {
                if (tr.cardsInTurn[nearestSlotIndex].GetComponent<Obj_Card>() != null)
                    ar.CardToCard(this, tr.cardsInTurn[nearestSlotIndex].GetComponent<Obj_Card>());
                else
                    ar.CardToCharacter(this, tr.cardsInTurn[nearestSlotIndex].GetComponent<Obj_Character>());
            }
            tr.NewTurnChecker();
        }

    }
    #endregion

    void CardMoveBack()
    {
        TurnResolving tr = FindObjectOfType<TurnResolving>();
        Vector3 targetPos = new Vector3(tr.cardSlots[inSlotIndex].position.x, tr.cardSlots[inSlotIndex].position.y, 0);
        transform.DOMove(targetPos, 0.3f);
    }

    public void DestroyCard()
    {
        TurnResolving tr = FindObjectOfType<TurnResolving>();
        tr.cardsInTurn[inSlotIndex] = null;
        Destroy(gameObject, 0.4f);
    }

    public void WaitingForDestroy()
    {
        TurnResolving tr = FindObjectOfType<TurnResolving>();
        isForDestroy = true;
        tr.cardsToDestroy.Add(transform);
        GetComponent<SpriteRenderer>().color = Color.cyan;
        GetComponent<BoxCollider2D>().enabled = false;
        FindObjectOfType<TurnResolving>().AddCommand(this);
    }

    public void Execute()
    {
        DestroyCard();
    }
}
