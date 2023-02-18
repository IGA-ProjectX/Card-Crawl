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

        private Vector3 lastMousePosition = Vector3.zero;
        [HideInInspector] public Vector3 inSlotPos;
        [HideInInspector] public int inSlotIndex;

        [HideInInspector] public bool isCardReadyForSkill = false;

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
            inSlotIndex = index;
        }

        #region - Interaction -
        public void OnMouseDown()
        {
            switch (M_Main.instance.m_Skill.skillUseState)
            {
                case SkillUseState.WaitForUse:
                    transform.DOMoveZ(-0.2f, 0.1f);
                    m_Card.ShowMovableSlot(cardCurrentType);
                    break;
                case SkillUseState.Targeting:
                    M_Main.instance.m_SkillResolve.EffectResolve(M_Main.instance.m_Skill.activatedSkill, this);
                    break;
            }
        }

        public void OnMouseDrag()
        {
            if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse)
            {
                if (lastMousePosition != Vector3.zero)
                {
                    Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                    transform.position += offset;
                }
                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_Card.ShowMovableState(transform, cardCurrentType, cardCurrentValue);
            }
        }

        public void OnMouseUp()
        {
            if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse)
            {
                lastMousePosition = Vector3.zero;
                m_Card.CardUseOrMoveBack(transform, cardCurrentType, cardCurrentValue);
            }
        }
        //public void OnDrag(PointerEventData eventData)
        //{
        //    if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse)
        //    {
        //        if (lastMousePosition != Vector3.zero)
        //        {
        //            Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
        //            transform.position += offset;
        //        }
        //        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        m_Card.ShowMovableState(transform, cardCurrentType, cardCurrentValue);
        //    }
        //}

        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    switch (M_Main.instance.m_Skill.skillUseState)
        //    {
        //        case SkillUseState.WaitForUse:
        //            transform.DOMoveZ(-0.2f, 0.1f);
        //            m_Card.ShowMovableSlot(cardCurrentType);
        //            break;
        //        case SkillUseState.Targeting:
        //            M_Main.instance.m_SkillResolve.EffectResolve(M_Main.instance.m_Skill.activatedSkill, this);
        //            break;
        //    }
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse)
        //    {
        //        lastMousePosition = Vector3.zero;
        //        m_Card.CardUseOrMoveBack(transform, cardCurrentType, cardCurrentValue);
        //    }
        //}

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    //switch (M_Main.instance.m_Skill.skillUseState)
        //    //{
        //    //    case SkillUseState.WaitForUse:
        //    //        transform.DOMoveZ(-0.2f, 0.1f);
        //    //        m_Card.ShowMovableSlot(cardCurrentType);
        //    //        break;
        //    //    case SkillUseState.Targeting:
        //    //        M_Main.instance.m_SkillResolve.EffectResolve(M_Main.instance.m_Skill.activatedSkill, this);
        //    //        break;
        //    //}
        //}
        #endregion

        public void DestroyCardOutScene()
        {
            transform.SetParent(null);
            M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null;
            M_Main.instance.CheckDevCircumstance();
            Destroy(gameObject, 0.1f);
        }

        public void DestroyCardInScreen()
        {
            transform.SetParent(null);
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null);
            s.Append(transform.DOScale(0,0.3f));
            s.AppendCallback(() => M_Main.instance.CheckDevCircumstance());
            Destroy(gameObject, 0.4f);
        }

        public void CardBackToDeck()
        {
            M_Main.instance.m_Card.inGameDeck.Add(cardData);
            M_Main.instance.m_Card.ClipperMiddleDownToLeftUpper(transform.parent);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.02f);
            s.AppendCallback(() => DestroyCardOutScene());
        }
    }
}