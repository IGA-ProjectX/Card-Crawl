using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace IGDF
{
    public class M_LevelUI : MonoBehaviour
    {
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