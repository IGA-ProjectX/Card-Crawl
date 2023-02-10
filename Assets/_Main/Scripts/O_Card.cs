using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

        public void InitializeCard(Card card, int index,Vector3 pos)
        {
            cardData = card;
            cardCurrentValue = cardData.cardValue;
            cardCurrentType = cardData.cardType;
            if(card.cardImage!=null)
            transform.Find("Card Image Content").GetComponent<SpriteRenderer>().sprite = card.cardImage;
            transform.Find("Card Name").GetComponent<TMP_Text>().text = card.cardName;
            transform.Find("Card Value").GetComponent<TMP_Text>().text = card.cardValue.ToString();
            transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardTypeIcons[(int)card.cardType];
            inSlotPos = pos;
            inSlotIndex = index;
        }

        #region - Interaction -
        public void OnMouseDown()
        {
            switch (M_Main.instance.m_Skill.skillUseState)
            {
                case SkillUseState.WaitForUse:
                    transform.DOMoveZ(-0.2f, 0.1f);
                    m_Card.ShowMovableSlot(cardData);
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
                m_Card.ShowMovableState(transform, cardData.cardType, cardCurrentValue);
            }
        }

        public void OnMouseUp()
        {
            if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse)
            {
                lastMousePosition = Vector3.zero;
                m_Card.CardUseOrMoveBack(transform, cardData.cardType, cardCurrentValue);
            }
        }
        #endregion

        public void DestroyCard()
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => M_Main.instance.m_Card.cardsInTurn[inSlotIndex] = null);
            s.AppendCallback(() => transform.DOScale(0, 0.3f));
            Destroy(gameObject, 0.5f);
        }

        public void CardBackToDeck()
        {
            M_Main.instance.m_Card.inGameDeck.Add(cardData);
            DestroyCard();
        }
    }
}