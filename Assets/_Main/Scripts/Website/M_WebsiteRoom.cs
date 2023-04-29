using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF {
    public class M_WebsiteRoom : MonoBehaviour
    {
        public static M_WebsiteRoom instance;
        public Vector3 intialPos;
        public Vector3 finialPos;
        private float openSpeed = 0.4f;

        private Transform webBG;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            webBG = transform.Find("Web BG");
        }

        public void InitializeWebsiteRoom()
        {
            webBG.transform.localPosition = intialPos;
            webBG.transform.localScale = Vector3.zero;
        }

        public void WebsiteScaleUp()
        {
            webBG.DOScale(Vector3.one, openSpeed);
            DOTween.To(() => webBG.localPosition, x => webBG.localPosition = x, finialPos, openSpeed);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(openSpeed);
            s.AppendCallback(() => M_Website.instance.OpenWeb());
        }

        public void WebsiteScaleDown()
        {
            webBG.DOScale(Vector3.zero, openSpeed);
            DOTween.To(() => webBG.localPosition, x => webBG.localPosition = x, intialPos, openSpeed);
            Sequence s = DOTween.Sequence();
        }
    }
}