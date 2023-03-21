using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace IGDF
{
    public enum SystemLanguage { Chinese, English }
    public class M_Setting : MonoBehaviour
    {
        void LanguageUpdate(SystemLanguage targetLanguage)
        {
            Text t_ResetProgress = transform.Find("B_ResetProgress").Find("Text").GetComponent<Text>();
            Text t_LanguageTitle = transform.Find("Language").Find("Title").GetComponent<Text>();
            Text t_LanguageCurrent = transform.Find("Language").Find("Current Language").GetComponent<Text>();

            switch (targetLanguage)
            {
                case SystemLanguage.Chinese:
                    M_Global.instance.SetLanguage(SystemLanguage.Chinese);
                    t_ResetProgress.text = "���ý���";
                    t_LanguageTitle.text = "���ԣ�";
                    t_LanguageCurrent.text = "Splified Chinese";
                    break;
                case SystemLanguage.English:
                    M_Global.instance.SetLanguage(SystemLanguage.English);
                    t_ResetProgress.text = "Reset Progress";
                    t_LanguageTitle.text = "Language:";
                    t_LanguageCurrent.text = "Ӣ��";
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
                productRecord.useReviews = "";
            }
            UpdateCurrentExp();
        }

        public void ClickLanguageChange()
        {
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
            {
                LanguageUpdate(SystemLanguage.English);
            }
            else
            {
                LanguageUpdate(SystemLanguage.Chinese);
            }
        }

        public void UpdateCurrentExp()
        {
            transform.Find("T_Exp").GetComponent<TMPro.TMP_Text>().text = "Current Exp: " + M_Global.instance.mainData.playExp;
        }
    }
}