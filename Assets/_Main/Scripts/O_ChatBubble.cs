using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class O_ChatBubble : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 lastMousePosition = Vector3.zero;


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
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(1.2f, 0.1f));
            s.Append(transform.DOScale(0, 0.3f));
            Destroy(gameObject, 0.5f);
        }

        public void PopUpChatBubble(CharacterType chaType, string sentence)
        {
            GetComponentInChildren<Text>().color = M_Main.instance.repository.chaColors[(int)chaType];
            GetComponentInChildren<Text>().text = sentence;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(1.2f, 0.3f));
            s.Append(transform.DOScale(0.9f, 0.1f));
            s.Append(transform.DOScale(1f, 0.05f));
        }

        public void InitializeChatBubble(CharacterType chaType,string sentence)
        {
            GetComponentInChildren<Text>().color = M_Main.instance.repository.chaColors[(int)chaType];
            StartCoroutine(Type(sentence));
        }

        IEnumerator Type(string sentence)
        {
            string typeText = "";
            foreach (char letter in sentence)
            {
                typeText += letter;
                transform.Find("Text").GetComponent<Text>().text = typeText;
                yield return new WaitForSeconds(0.04f);
            }

        }
    }
}