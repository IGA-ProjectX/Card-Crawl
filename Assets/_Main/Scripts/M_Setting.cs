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
            SpriteRenderer i_GameName = GameObject.Find("Game Name").GetComponent<SpriteRenderer>();

            switch (targetLanguage)
            {
                case SystemLanguage.Chinese:
                    M_Global.instance.SetLanguage(SystemLanguage.Chinese);
                    t_ResetProgress.text = "重置进度";
                    t_LanguageTitle.text = "语言：";
                    t_LanguageCurrent.text = "简体中文";
                    i_GameName.sprite = M_Global.instance.repository.gameNameImages[1];
                    break;
                case SystemLanguage.English:
                    M_Global.instance.SetLanguage(SystemLanguage.English);
                    t_ResetProgress.text = "Reset Progress";
                    t_LanguageTitle.text = "Language:";
                    t_LanguageCurrent.text = "English";
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