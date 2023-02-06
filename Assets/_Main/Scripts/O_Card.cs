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
        private Vector3 lastMousePosition = Vector3.zero;
        [HideInInspector] public bool isForDestroy = false;
        [HideInInspector] public Vector3 inSlotPos;
        [HideInInspector] public int inSlotIndex;

        private M_Card m_Card;

        private void Start()
        {
            m_Card = FindObjectOfType<M_Card>();
        }

        public void InitializeCard(Card card, int index,Vector3 pos)
        {
            cardData = card;
            cardCurrentValue = cardData.cardValue;
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
            transform.DOMoveZ(-0.2f, 0.1f);
            m_Card.ShowMovableSlot(cardData);
        }

        public void OnMouseDrag()
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_Card.ShowMovableState(transform,cardData.cardType,cardCurrentValue);
        }

        public void OnMouseUp()
        {
            lastMousePosition = Vector3.zero;
            m_Card.CardUseOrMoveBack(transform, cardData.cardType, cardCurrentValue);
        }
        #endregion

        public void DestroyCard()
        {
            Destroy(gameObject, 0.5f);
        }

        //public void WaitingForDestroy()
        //{
        //    TurnResolving tr = FindObjectOfType<TurnResolving>();
        //    isForDestroy = true;
        //    tr.cardsToDestroy.Add(transform);
        //    GetComponent<SpriteRenderer>().color = Color.cyan;
        //    GetComponent<BoxCollider2D>().enabled = false;
        //    FindObjectOfType<TurnResolving>().AddCommand(this);
        //}

        //public void Execute()
        //{
        //    DestroyCard();
        //}
    }
}