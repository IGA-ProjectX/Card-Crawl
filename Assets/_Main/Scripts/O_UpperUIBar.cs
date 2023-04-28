using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class O_UpperUIBar : MonoBehaviour
    {
        public TMP_Text t_Exp;
        public TMP_Text t_DevTime;
        public TMP_Text t_ReleasedGameNum;
        public TMP_Text t_Realtime;
        public RectTransform barHandler;
        public static O_UpperUIBar instance;

        private RectTransform thisRect;
        private int previousDevTime;
        private bool isOpen = false;
        private bool previousOpenState = false;

        void Start()
        {
            instance = this;
            thisRect = GetComponent<RectTransform>();
            FindObjectOfType<M_SceneTransition>().ToOverView += UIBarSlideUpwards;
            FindObjectOfType<M_SceneTransition>().ToCabinView += UIBarSlideDownwardsHandlerUpwards;
            FindObjectOfType<M_SceneTransition>().ToRoom += UIBarSlideUpwardsHandlerDropDown;
            UpdateOnBarInfo();
        }

        void Update()
        {
            ChangeRealTime();
        }

        public void UpdateOnBarInfo()
        {
            ChangeExp();
            ChangeDevTime();
            ChangeReleasedGameNum();
            ChangeRealTime();
        }

        public void ChangeExp()
        {
            int targetExp = M_Global.instance.mainData.playExp;
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) t_Exp.text = "团队经验：" + targetExp;
            else t_Exp.text = "Team Exp: " + targetExp;
        }

        public void ChangeDevTime()
        {
            //M_Global.instance.mainData.gameTimeInTotal++;
            int minutes = M_Global.instance.mainData.gameTimeInTotal % 60;
            int hour = M_Global.instance.mainData.gameTimeInTotal / 60;
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) t_DevTime.text = "开发时长：" + hour + "H ";
            else t_DevTime.text = "Dev Time: " + hour + "H ";
        }

        public void ChangeReleasedGameNum()
        {
            int releasedNum = 0;
            foreach (var product in M_Global.instance.mainData.productShowcases)
                if (product.productLevel != ProductLevel.None) releasedNum++;
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) t_ReleasedGameNum.text = "发行游戏数：" + releasedNum;
            else t_ReleasedGameNum.text = "Released Game: " + releasedNum;
        }

        public void ChangeRealTime()
        {
            string currentTime = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy.MM.dd" +" "+ "HH:mm");
            if (t_Realtime.text != currentTime) ChangeDevTime();
 
            t_Realtime.text = currentTime;
        }

        public void ExpConsumed()
        {

        }

        public void ExpObtained()
        {

        }

        #region UI Bar Movement
        public void UIBarSlideDownwardsHandlerUpwards()
        {
            DOTween.To(()=>thisRect.anchoredPosition, x => thisRect.anchoredPosition = x, new Vector2(0, -70), 1);
            DOTween.To(() => barHandler.anchoredPosition, x => barHandler.anchoredPosition = x, new Vector2(60, 292), 1);
        }

        public void UIBarSlideUpwards()
        {
            DOTween.To(()=>thisRect.anchoredPosition, x => thisRect.anchoredPosition = x, new Vector2(0, 70), 1);
        }

        public void UIBarSlideUpwardsHandlerDropDown(M_SceneTransition.CabinView targetCabin)
        {
            if (targetCabin==M_SceneTransition.CabinView.InStudio)
            {
                DOTween.To(() => thisRect.anchoredPosition, x => thisRect.anchoredPosition = x, new Vector2(0, 70), 1);
                DOTween.To(() => barHandler.anchoredPosition, x => barHandler.anchoredPosition = x, new Vector2(60, 70), 1);
            }
        }

        public void UIBarSlideDownwardsHandlerDownwards()
        {
            if (previousOpenState == isOpen)
            {
                isOpen = !isOpen;
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() => DOTween.To(() => barHandler.anchoredPosition, x => barHandler.anchoredPosition = x, new Vector2(60, -100), 0.4f));
                s.AppendInterval(0.4f);
                s.AppendCallback(() => DOTween.To(() => barHandler.anchoredPosition, x => barHandler.anchoredPosition = x, new Vector2(60, 70), 0.2f));
                s.AppendInterval(0.4f);
                s.AppendCallback(() => BarDropDown());
            }

            void BarDropDown()
            {
                if (!previousOpenState) DOTween.To(() => thisRect.anchoredPosition, x => thisRect.anchoredPosition = x, new Vector2(0, -70), 0.5f);
                else DOTween.To(() => thisRect.anchoredPosition, x => thisRect.anchoredPosition = x, new Vector2(0, 70), 0.5f);
                previousOpenState = isOpen;
            }
        }
        #endregion
    }
}