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
            transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardName;
            transform.Find("Card Value").GetComponent<TMP_Text>().text = card.cardValue.ToString();
            transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardTypeIcons[(int)card.cardType];
            targetableType.Add(card.cardType);
            if (card.cardType!=0 && card.cardValue<0) targetableType.Add(CardType.Production);
            inSlotIndex = index;
        }

        public void SetDraggableState(bool isForDrag)
        {
            isDraggable = isForDrag;
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
                M_Main.instance.m_Skill.EnterWaitForUseState();
            }
            else
            {
                m_Card.ShowMovableState(transform, targetableType, cardCurrentValue);
                M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Grabbed);
            }
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
            }
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
            //Sequence s = DOTween.Sequence();
            //s.AppendCallback(() => M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null);
            //s.Append(transform.DOScale(0,0.3f));
            //s.AppendCallback(() => M_Main.instance.CheckDevCircumstance());
            Destroy(gameObject, 0.9f);
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
    }
}