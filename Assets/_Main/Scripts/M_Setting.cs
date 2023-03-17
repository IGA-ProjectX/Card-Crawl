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
                    t_ResetProgress.text = "重置进度";
                    t_LanguageTitle.text = "语言：";
                    t_LanguageCurrent.text = "Splified Chinese";
                    break;
                case SystemLanguage.English:
                    M_Global.instance.SetLanguage(SystemLanguage.English);
                    t_ResetProgress.text = "Reset Progress";
                    t_LanguageTitle.text = "Language:";
                    t_LanguageCurrent.text = "英文";
                    break;
            }
        }

        public void ClickClose()
        {
            transform.DOScale(Vector3.zero, 1);
        }

        public void ClickResetProgress()
        {

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
    }
}