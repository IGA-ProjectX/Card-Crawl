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
        private List<Transform> levelList = new List<Transform>();
        public GameObject pre_Level;

        private void Start()
        {
            GenerateLevelList();
        }

        private void GenerateLevelList()
        {
            for (int i = 0; i < M_Global.instance.levels.Length; i++)
            {
                Transform newLevel = Instantiate(pre_Level, transform.Find("View Mask").Find("Levels")).transform;
                levelList.Add(newLevel);
                newLevel.GetComponent<O_Level>().InitializeLevelObj(M_Global.instance.levels[i], i);
            }
        }

        public void CloseLevelSelectionPanel()
        {
            GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f);
        }

        public void OpenLevelSelectionPanel()
        {
            foreach (Transform level in levelList)
                level.GetComponent<O_Level>().UpdateNameLanguage();
            if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) transform.Find("Button Return").GetComponentInChildren<TMP_Text>().text = "·µ»Ø";
            else transform.Find("Button Return").GetComponentInChildren<TMP_Text>().text = "Return";

            GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(1, 0.4f);
        }

        //public void RemoveExistingStudioScene()
        //{
        //    SceneManager.UnloadSceneAsync(1);
        //}
    }
}