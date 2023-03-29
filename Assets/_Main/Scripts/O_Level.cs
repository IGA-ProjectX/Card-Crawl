using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace IGDF
{
    public class O_Level : MonoBehaviour
    {
        SO_Level thisLevel;
        int levelIndex;
        TMP_Text levelName;
        SpriteRenderer levelBG;

        public void InitializeLevelObj(SO_Level targetLevel,int targetIndex)
        {
            levelName = transform.Find("Level Name").GetComponent<TMP_Text>();
            if (targetLevel != null)
            {
                thisLevel = targetLevel;
                transform.Find("Level BG").GetComponent<SpriteRenderer>().sprite = thisLevel.levelButtonImage;
                levelName.text = thisLevel.levelName;
            }
            else
            {
                levelName.text = "Unlocked";
            }
        }

        private void OnMouseDown()
        {
                EnterLevel(levelIndex);
        }

        void EnterLevel(int levelIndex)
        {
            M_Global.instance.targetLevel = levelIndex;
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => LoadStudio());
            s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());
            //s.Append(GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f));


            void LoadStudio()
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
            }
        }

        public void ChangeTransparency(float targetValue, float time)
        {
            levelBG.DOFade(targetValue, time);
            DOTween.To(() => levelName.alpha, x => levelName.alpha = x, targetValue, time);
            //DOTween.To(() => levelBG.co, x => levelName.alpha = x, 0, 0.2f);
        }
    }
}