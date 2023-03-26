using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IGDF
{
    public class O_Card : MonoBehaviour
    {
        private Card cardData;
        [HideInInspector] public int cardCurrentValue;
        [HideInInspector] public CardType cardCurrentType;
        [HideInInspector]public List<CardType> targetableType = new List<CardType>();
        private Vector3 lastMousePosition = Vector3.zero;
        [HideInInspector] public Vector3 inSlotPos;
        [HideInInspector] public int inSlotIndex;

        [HideInInspector] public bool isCardReadyForSkill = false;
        private bool isDraggable = false;

        private M_Card m_Card;

        private void Start()
        {
            m_Card = FindObjectOfType<M_Card>();
        }

        public void InitializeCard(Card card, int index)
        {
            cardData = card;
            cardCurrentValue = cardData.cardValue;
            cardCurrentType = cardData.cardType;
            if (card.cardImage != null)
                transform.Find("Card Image Content").GetComponent<SpriteRenderer>().sprite = card.cardImage;

            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
            {
                transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardOutNC;
                switch (cardCurrentType)
                {
                    case CardType.Production:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "经验";
                    break;
                    case CardType.Design:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "设计";
                        break;
                    case CardType.Art:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "美术";
                        break;
                    case CardType.Code:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "程序";
                        break;
                }
            }
            else 
            {
                transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardOutNE;
                switch (cardCurrentType)
                {
                    case CardType.Production:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "Exp";
                        break;
                    case CardType.Design:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "Design";
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().fontSize = 1.72f;
                        break;
                    case CardType.Art:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "Art";
                        break;
                    case CardType.Code:
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().text = "Code";
                        transform.Find("Card Text Type").GetComponent<TMP_Text>().fontSize = 2.3f;
                        break;
                }
            }

            if (card.cardValue > 0)
            {
                transform.Find("Card Value").GetComponent<TMP_Text>().text = "+" + card.cardValue.ToString();
                transform.Find("Card BG").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardBGImages[0];
            }
            else
            {
                transform.Find("Card Value").GetComponent<TMP_Text>().text = card.cardValue.ToString();
                transform.Find("Card BG").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardBGImages[1];
            }

            transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardTypeIcons[(int)card.cardType];


            targetableType.Add(card.cardType);
            if (card.cardType!=0 && card.cardValue<0) targetableType.Add(CardType.Production);
            inSlotIndex = index;
        }

        public void ChangeCardValue(int targetValue)
        {
            cardCurrentValue = targetValue;
            transform.Find("Card Value").GetComponent<TMP_Text>().text = cardCurrentValue.ToString();
        }

        public void SetDraggableState(bool isForDrag)
        {
            isDraggable = isForDrag;
        }

        public bool GetDraggableState()
        {
            return isDraggable;
        }

        #region - Interaction -
        public void OnMouseEnter()
        {
            M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Grabbing);
        }

        public void OnMouseDown()
        {
            if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.Targeting)
            {
                M_Main.instance.m_SkillResolve.EffectResolve(M_Main.instance.m_Skill.activatedSkill, this);
                M_Main.instance.m_Skill.activatedSkill.ExitTargetingState();
                M_Main.instance.m_Skill.EnterWaitForUseState();
            }
            else
            {
                m_Card.ShowMovableState(transform, targetableType, cardCurrentValue);
                M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Grabbed);
                M_Audio.PlaySound(SoundType.SpringStretch);
          
            }
            CardLayerChangeToAbove();
            M_Main.instance.m_HoverTip.EnterState(HoverState.CardDragging);
            GetComponent<O_HoverTip>().SetSelectionBoxState(false);
        }

        public void OnMouseDrag()
        {
            if (isDraggable && M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse) 
            { 
                if (lastMousePosition != Vector3.zero)
                {
                    Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                    transform.position += offset;
                }
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_Card.CardTargetingDetection(transform, targetableType, cardCurrentValue);
            }
        }

        public void OnMouseUp()
        {
            if (isDraggable && M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse)
            {
                lastMousePosition = Vector3.zero;
                m_Card.CardUseOrMoveBack(transform, targetableType, cardCurrentValue);
                M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Arrow);
                M_Audio.PlaySound(SoundType.SpringShrink);
        
            }
            CardLayerChangeToCommon();
            M_Main.instance.m_HoverTip.EnterState(HoverState.AllActive);
        }

        private void OnMouseExit()
        {
            M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Arrow);
        }
        #endregion

        public void DestroyCardOutScene(float destroyTime)
        {
            M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null;
            M_Main.instance.CheckDevCircumstance();
            Destroy(gameObject, destroyTime);
        }

        public void DestroyCardInScreen()
        {
            transform.SetParent(null);
            M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null;
            transform.DOScale(0, 0.6f);
            transform.DORotate(new Vector3(0,0,180), 0.8f);
            M_Main.instance.CheckDevCircumstance();
            Destroy(gameObject, 0.9f);
            M_Audio.PlaySound(SoundType.CardIndraft);
            GetComponent<O_HoverTip>().ChangeAllowOpenState(false, false);
        }

        public void CardBackToDeck()
        {
            M_Main.instance.m_Card.inGameDeck.Add(cardData);
            M_Main.instance.m_Card.ClipperMiddleDownToLeftUpper(transform.parent);
            transform.parent.GetComponentInChildren<O_ClipperLine>().DestroySlider(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.2f);
            DestroyCardOutScene( M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.1f);
        }

        public void CardMoveOutOfScreenRightWards()
        {
            M_Main.instance.m_Card.inGameDeck.Add(cardData);
            M_Main.instance.m_Card.ClipperMiddleDownToRightDown(transform.parent);
            transform.parent.GetComponentInChildren<O_ClipperLine>().DestroySlider(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.2f);
            DestroyCardOutScene(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.1f);
        }

        public void SetLineStateAuto()
        {
            transform.parent.GetComponentInChildren<O_ClipperLine>().SetLineState("Auto");
        }

        private int layerModifyAmount = 7;

        private void CardLayerChangeToAbove()
        {
            int cardModifyOffsetedAmount = layerModifyAmount + 2;
            transform.Find("Card BG").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Card Image Content").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Name Bg").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Card Name").GetComponent<MeshRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Card Value").GetComponent<MeshRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.Find("Card Text Type").GetComponent<MeshRenderer>().sortingOrder += cardModifyOffsetedAmount;

            transform.Find("Card Image Mask").GetComponent<SpriteMask>().frontSortingOrder += cardModifyOffsetedAmount;
            transform.parent.Find("Clipper").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.parent.Find("Handler").GetComponent<LineRenderer>().sortingOrder += cardModifyOffsetedAmount;
            transform.parent.Find("Handler").GetComponent<SpriteRenderer>().sortingOrder += cardModifyOffsetedAmount;
        }

        private void CardLayerChangeToCommon()
        {
            int cardModifyOffsetedAmount = layerModifyAmount + 2;
            transform.Find("Card BG").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Card Image Content").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Name Bg").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Card Name").GetComponent<MeshRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Card Value").GetComponent<MeshRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.Find("Card Text Type").GetComponent<MeshRenderer>().sortingOrder -= cardModifyOffsetedAmount;

            transform.Find("Card Image Mask").GetComponent<SpriteMask>().frontSortingOrder -= cardModifyOffsetedAmount;
            transform.parent.Find("Clipper").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.parent.Find("Handler").GetComponent<LineRenderer>().sortingOrder -= cardModifyOffsetedAmount;
            transform.parent.Find("Handler").GetComponent<SpriteRenderer>().sortingOrder -= cardModifyOffsetedAmount;
        }

        public Card GetCardData()
        {
            return cardData;
        }
    }
}