using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace IGDF
{
    public enum SystemLanguage { Chinese, English }
    public class M_Setting : MonoBehaviour
    {
        private RectMask2D mask_Language;
        public static float globalVolumeOffset;

        private void Start()
        {
            mask_Language = transform.Find("Language").Find("Capsule").Find("Rect Mask").GetComponent<RectMask2D>();
        }

        void LanguageUpdate(SystemLanguage targetLanguage)
        {
            TMP_Text t_ResetProgress = transform.Find("B_ResetProgress").Find("Text").GetComponent<TMP_Text>();
            SpriteRenderer i_GameName = GameObject.Find("Game Name").GetComponent<SpriteRenderer>();

            switch (targetLanguage)
            {
                case SystemLanguage.Chinese:
                    M_Global.instance.SetLanguage(SystemLanguage.Chinese);
                    t_ResetProgress.text = "ÖØÖÃ½ø¶È";
                    i_GameName.sprite = M_Global.instance.repository.gameNameImages[1];
                    break;
                case SystemLanguage.English:
                    M_Global.instance.SetLanguage(SystemLanguage.English);
                    t_ResetProgress.text = "Reset Progress";
                    i_GameName.sprite = M_Global.instance.repository.gameNameImages[0];
                    break;
            }
        }

        public void ClickClose()
        {
            transform.DOScale(Vector3.zero, 1);
        }

        public void ClickResetProgress()
        {
            M_Global.instance.mainData.playExp = 0;
            foreach (ProductShowcase productRecord in M_Global.instance.mainData.productShowcases)
            {
                productRecord.producedDate = "";
                productRecord.productLevel = ProductLevel.None;
                productRecord.userReviewLevel = "";
                productRecord.userReviewNumber = "";
            }
            UpdateCurrentExp();
        }

        public void ClickLanguageChange()
        {
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
            {
                LanguageUpdate(SystemLanguage.English);
                DOTween.To(() => mask_Language.padding, x => mask_Language.padding = x, new Vector4(-255, 0, 255, 0), 0.5f);
            }
            else
            {
                LanguageUpdate(SystemLanguage.Chinese);
                DOTween.To(() => mask_Language.padding, x => mask_Language.padding = x, new Vector4(0, 0, 0, 0), 0.5f);
            }
        }

        public void UpdateCurrentExp()
        {
            transform.Find("T_Exp").GetComponent<TMPro.TMP_Text>().text = "Current Exp: " + M_Global.instance.mainData.playExp;
        }

        public void GlobalVolumeChange()
        {
            globalVolumeOffset = transform.Find("Audio Volume").GetComponentInChildren<Slider>().value;
            M_Audio.GlobalVolumeChange(globalVolumeOffset);
        }
    }
}