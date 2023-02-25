using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class O_ChatBubble : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler,IBeginDragHandler
    {
        private Vector3 lastMousePosition = Vector3.zero;
        private bool isDrag = false;
        private bool isProcessDestroy = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            lastMousePosition = Vector3.zero;
            isDrag = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isDrag && !isProcessDestroy)
            {
                isProcessDestroy = true;
                Sequence s = DOTween.Sequence();
                s.Append(transform.DOScale(1.2f, 0.1f));
                s.Append(transform.DOScale(0, 0.3f));
                Destroy(gameObject, 0.5f);
            }
        }

        public void PopUpChatBubble(CharacterType chaType, string sentence)
        {
            foreach (O_Character characterObj in GameObject.Find("Environment").transform.Find("Characters").GetComponentsInChildren<O_Character>())
                if (characterObj.thisCharacter == chaType)
                    transform.position = characterObj.transform.position;
            Text chatText = transform.Find("Chat Bubble").GetComponentInChildren<Text>();
            chatText.color = M_Main.instance.repository.chaColors[(int)chaType];
            chatText.text = sentence;
            //transform.Find("Close Button").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(1.2f, 0.3f));
            s.Append(transform.DOScale(0.9f, 0.1f));
            s.Append(transform.DOScale(1f, 0.05f));
        }
    }
}