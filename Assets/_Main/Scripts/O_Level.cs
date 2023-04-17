using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IGDF
{
    public class O_Level : MonoBehaviour
    {
        SO_Level thisLevel;
        int levelIndex;
        TMP_Text levelName;
        Image levelCover;
        Image levelLock;
        Button thisButton;
        bool isLocked = false;

        public void InitializeLevelObj(SO_Level targetLevel,int targetIndex)
        {
            levelName = transform.Find("Mask").Find("Level Name").GetComponent<TMP_Text>();
            levelCover = transform.Find("Mask").Find("Level Cover").GetComponent<Image>();
            levelLock = transform.Find("Level Lock").GetComponent<Image>();
            thisButton = transform.Find("Frame").GetComponent<Button>();
            if (targetLevel != null)
            {
                thisLevel = targetLevel;
                levelCover.sprite = thisLevel.levelButtonImage;
                if (M_Global.instance.GetLanguage() == SystemLanguage.English) levelName.text = thisLevel.levelNameEng;
                else levelName.text = thisLevel.levelNameChi;

                levelLock.CrossFadeAlpha(0, 0, true);
                isLocked = false;
            }
            else
            {
                levelName.text = "Unlocked";
                levelCover.CrossFadeAlpha(0, 0, true);
                thisButton.enabled = false;
                isLocked = true;
            }
            levelIndex = targetIndex;
        }

        public void UpdateNameLanguage()
        {
            if (isLocked)
            {
                if (M_Global.instance.GetLanguage() == SystemLanguage.English) levelName.text = "Locked";
                else levelName.text = "Î´½âËø";
            }
            else
            {
                if (M_Global.instance.GetLanguage() == SystemLanguage.English) levelName.text = thisLevel.levelNameEng;
                else levelName.text = thisLevel.levelNameChi;
            }
        }

        public void EnterLevel()
        {
            M_Global.instance.targetLevel = levelIndex;
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => SceneManager.LoadScene(1, LoadSceneMode.Additive));
            s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());
            s.Append(GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f));
        }
    }
}