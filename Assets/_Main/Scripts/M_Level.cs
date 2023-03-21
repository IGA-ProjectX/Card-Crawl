using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Psychoflow.SSWaterReflection2D;

namespace IGDF
{
    public class M_Level : MonoBehaviour
    {
        //public Transform[] ui_Levels;

        //private void Start()
        //{
        //    InitializeLevels();
        //    OnLevelChange();
        //}

        //private void Update()
        //{
        //    if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //    {
        //        OnLevelChange();
        //    }
        //    if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //    {
        //        OnLevelChange();
        //    }
        //}

        //public void InitializeLevels()
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        ui_Levels[i].Find("Level Name").GetComponent<TMP_Text>().text = M_Global.instance.levels[i].levelName;
        //    }
        //}

        //public void OnLevelChange()
        //{
        //    M_Global.instance.targetLevel++;
        //    if (M_Global.instance.targetLevel >= M_Global.instance.levels.Length)
        //    {
        //        M_Global.instance.targetLevel = 0;
        //    }
        //    HighlightSelectedLevel();

        //    void HighlightSelectedLevel()
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            if (i == M_Global.instance.targetLevel)
        //                ui_Levels[i].Find("Level BG").GetComponent<Image>().color = Color.cyan;
        //            else
        //                ui_Levels[i].Find("Level BG").GetComponent<Image>().color = Color.white;
        //        }
        //    }
        //}

        public void LoadLevel1PaperPlease()
        {
            EnterLevel(0);
        }

        public void LoadLevel2SuperMario()
        {
            EnterLevel(1);
        }

        public void LoadLevel3SuperHot()
        {
            EnterLevel(2);
        }

        void EnterLevel(int levelIndex)
        {
            M_Global.instance.targetLevel = levelIndex;
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => LoadStudio());
            s.Append(GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f));
            s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());

            void LoadStudio()
            {
                SceneManager.LoadScene(1, LoadSceneMode.Additive);
            }
        }

        public void CloseLevelSelectionPanel()
        {
            GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f);
        }

        public void RemoveExistingStudioScene()
        {
            SceneManager.UnloadSceneAsync(1);
        }
    }
}