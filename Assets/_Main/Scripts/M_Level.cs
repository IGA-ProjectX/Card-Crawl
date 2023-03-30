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
        //public float scrollSpeed;
        //public float scrollValue;
        //public int level_Top;

        private void Start()
        {
            GenerateLevelList();
            //UpdateAllLevelListState(0);
        }

        private void Update()
        {
            //if (Input.GetAxis("Mouse ScrollWheel") !=0)
            //{
            //    transform.Find("Level List").Translate(new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed), 0);
            //    scrollValue += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
            //    if (scrollValue < 0) scrollValue = 0;
            //    else if (scrollValue > 15) scrollValue = 15;
            //    level_Top = Mathf.CeilToInt(scrollValue);
            //    //foreach (Transform levelTrans in levelList) levelTrans.GetComponent<O_Level>().UpdateInScreenState();
            //    //if (level_Top < 0) level_Top = 0;
            //    //else if (level_Top > 15) level_Top = 15;
            //    UpdateAllLevelListState(0.3f);
            //}
        }

        private void GenerateLevelList()
        {
            //for (int i = 0; i < M_Global.instance.levels.Length; i++)
            //{
            //    Transform newLevel = Instantiate(pre_Level, transform.Find("Level List")).transform;
            //    levelList.Add(newLevel);
            //    newLevel.GetComponent<O_Level>().InitializeLevelObj(M_Global.instance.levels[i], i);
            //    newLevel.localPosition = new Vector3(0, transform.position.y + i * -2, 0);
            //}

            for (int i = 0; i < M_Global.instance.levels.Length; i++)
            {
                Transform newLevel = Instantiate(pre_Level, transform.Find("View Mask").Find("Levels")).transform;
                levelList.Add(newLevel);
                newLevel.GetComponent<O_Level>().InitializeLevelObj(M_Global.instance.levels[i], i);
                //newLevel.GetComponent<Button>().onClick.AddListener()
                //newLevel.GetComponent<O_Level>().InitializeLevelObj(M_Global.instance.levels[i], i);
                //newLevel.localPosition = new Vector3(0, transform.position.y + i * -2, 0);
            }
        }

        //void UpdateAllLevelListState(float transitionTime)
        //{
        //    int level_Bottom = level_Top + 3;
        //    for (int i = 0; i < levelList.Count; i++)
        //    {
        //        if (i >= level_Top && i <= level_Bottom)
        //        {
        //            levelList[i].DOScale(1, transitionTime);
        //            int offset = i - level_Top;
        //            levelList[i].DOMoveY(3 - offset * 2, transitionTime);
        //            levelList[i].GetComponent<O_Level>().ChangeTransparency(1, transitionTime);
        //        }
        //        else 
        //        {
        //            levelList[i].DOScale(0, 0.3f);
        //            if (i < level_Top)
        //            {
        //                int offset = level_Top - i;
        //                levelList[i].DOMoveY(3 + offset * 0.5f, transitionTime);
        //                levelList[i].GetComponent<O_Level>().ChangeTransparency(0, transitionTime);
        //            }
        //            else if (i > level_Bottom)
        //            {
        //                int offset = i - level_Bottom;
        //                levelList[i].DOMoveY(-3- offset * 0.5f, transitionTime);
        //                levelList[i].GetComponent<O_Level>().ChangeTransparency(0, transitionTime);
        //            }
        //        }
        //    }
        //}


        //public void LoadLevel1PaperPlease()
        //{
        //    EnterLevel(0);
        //}

        //public void LoadLevel2SuperMario()
        //{
        //    EnterLevel(1);
        //}

        //public void LoadLevel3SuperHot()
        //{
        //    EnterLevel(2);
        //}

        //void EnterLevel(int levelIndex)
        //{
        //    M_Global.instance.targetLevel = levelIndex;
        //    Sequence s = DOTween.Sequence();
        //    s.AppendCallback(() => LoadStudio());
        //    s.Append(GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(0, 0.4f));
        //    s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());

        //    void LoadStudio()
        //    {
        //        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        //    }
        //}

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