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
        public Transform reminder_Panel;

        private TMP_Text t_Start;
        private TMP_Text t_Exit;
        private TMP_Text t_Credits;
        private TMP_Text t_Setting;


        private void Start()
        {
            mask_Language = transform.Find("Language").Find("Capsule").Find("Rect Mask").GetComponent<RectMask2D>();
            t_Start = FindObjectOfType<M_SceneTransition>().transform.Find("Road").Find("SignBase").Find("B_Start Game").GetComponent<TMP_Text>();
            t_Exit = FindObjectOfType<M_SceneTransition>().transform.Find("Road").Find("SignBase").Find("B_Exit").GetComponent<TMP_Text>();
            t_Credits = FindObjectOfType<M_SceneTransition>().transform.Find("Road").Find("SignBase").Find("B_Credits").GetComponent<TMP_Text>();
            t_Setting = FindObjectOfType<M_SceneTransition>().transform.Find("Road").Find("SignBase").Find("B_Setting").GetComponent<TMP_Text>();

        }

        void LanguageUpdate(SystemLanguage targetLanguage)
        {
            TMP_Text t_ResetProgress = transform.Find("B_ResetProgress").Find("Text").GetComponent<TMP_Text>();
            SpriteRenderer i_GameName = GameObject.Find("Game Name").GetComponent<SpriteRenderer>();
            //TMP_Text t_ExpTitle = transform.Find("Exp").Find("T_ExpTitle").GetComponent<TMP_Text>();

            switch (targetLanguage)
            {
                case SystemLanguage.Chinese:
                    M_Global.instance.SetLanguage(SystemLanguage.Chinese);
                    t_ResetProgress.text = "重置进度";
                    //t_ExpTitle.text = "当前经验：";
                    t_Start.text = "开始";
                    t_Exit.text = "退出";
                    t_Credits.text = "开发者名单";
                    t_Setting.text = "设置";
                    i_GameName.sprite = M_Global.instance.repository.gameNameImages[1];
                    break;
                case SystemLanguage.English:
                    M_Global.instance.SetLanguage(SystemLanguage.English);
                    t_ResetProgress.text = "Reset Progress";
                    //t_ExpTitle.text = "Current EXP:";
                    t_Start.text = "Start";
                    t_Exit.text = "Exit";
                    t_Credits.text = "Credits";
                    t_Setting.text = "Setting";
                    i_GameName.sprite = M_Global.instance.repository.gameNameImages[0];
                    break;
            }
        }

        public void ClickClose()
        {
            transform.DOScale(Vector3.zero, 0.4f);
        }

        public void OpenSettingPanel()
        {
            //UpdateCurrentExp();
            transform.DOScale(Vector3.one, 0.4f);
        }

        public void ClickResetCancel()
        {
            Sequence s = DOTween.Sequence();
            s.Append(reminder_Panel.DOScale(0, 0.4f));
            s.AppendCallback(() => reminder_Panel.gameObject.SetActive(false));
    
        }

        public void ClickResetConfirm()
        {
            M_Global.instance.mainData.playExp = 0;
            foreach (ProductShowcase productRecord in M_Global.instance.mainData.productShowcases)
            {
                productRecord.producedDate = "";
                productRecord.productLevel = ProductLevel.None;
                productRecord.userReviewLevel = "";
                productRecord.userReviewNumber = "";
            }

            M_Global.instance.mainData.unlockedSkillNodes.Clear();
            M_Global.instance.mainData.unlockedSkillNodes.Add(new UnlockedSkillNode(CharacterType.Producer, NodeIndex.C1));
            M_Global.instance.mainData.unlockedSkillNodes.Add(new UnlockedSkillNode(CharacterType.Producer, NodeIndex.C2));
            M_Global.instance.mainData.unlockedSkillNodes.Add(new UnlockedSkillNode(CharacterType.Producer, NodeIndex.B1));
            M_Global.instance.mainData.unlockedSkillNodes.Add(new UnlockedSkillNode(CharacterType.Producer, NodeIndex.B2));

            M_Global.instance.mainData.inUseSkills = M_Global.instance.repository.defaultSkills;
            //UpdateCurrentExp();
            FindObjectOfType<O_UpperUIBar>().ChangeExp();
            Sequence s = DOTween.Sequence();
            s.Append(reminder_Panel.DOScale(0, 0.4f));
            s.AppendCallback(() => reminder_Panel.gameObject.SetActive(false));
        }

        public void ClickResetProgress()
        {
            reminder_Panel.gameObject.SetActive(true);
            reminder_Panel.DOScale(1, 0.4f);
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

        //public void UpdateCurrentExp()
        //{
        //    transform.Find("Exp").Find("T_ExpValue").GetComponent<TMP_Text>().text = M_Global.instance.mainData.playExp.ToString();
        //}

        public void GlobalVolumeChange()
        {
            globalVolumeOffset = transform.Find("Audio Volume").GetComponentInChildren<Slider>().value;
            M_Audio.GlobalVolumeChange(globalVolumeOffset);
        }
    }
}