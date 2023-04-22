using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IGDF
{
    public class M_Global : MonoBehaviour
    {
        public SO_Data mainData;
        public SO_Level[] levels;
        public int targetLevel = 1;
        public SO_Skill[] skillList;
        public SO_Repo repository;
        private SystemLanguage currentLanguage = SystemLanguage.Chinese;
        public static M_Global instance;
        [HideInInspector] public GameObject ui_HoverTip;
        [HideInInspector] public RectTransform ui_HoverContent;
        [HideInInspector] public Transform chatBubbleParent;

        private void Awake()
        {
            if (instance != null) Destroy(this);
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            M_Setting.globalVolumeOffset = 1;
            M_Audio.PlaySceneMusic(M_SceneTransition.CabinView.Overview);
        }

        private void Start()
        {
            ui_HoverTip = GameObject.Find("Canvas").transform.Find("Hover Tip").gameObject;
            ui_HoverContent = ui_HoverTip.transform.GetChild(0).GetComponent<RectTransform>();
            ui_HoverTip.SetActive(false);
            chatBubbleParent = GameObject.Find("Canvas").transform.Find("ChatBubbles");

            SaveTheGameFromJson();
        }

        public void PlayerExpUp(int newExp) 
        {
            mainData.playExp += newExp;
            FindObjectOfType<O_UpperUIBar>().ChangeExp();
            O_UpperUIBar.instance.UIBarSlideDownwardsHandlerUpwards();
        }

        public SystemLanguage GetLanguage()
        {
            return currentLanguage;
        }

        public void SetLanguage(SystemLanguage targetLanguage)
        {
            currentLanguage = targetLanguage;
        }

        public void SaveTheGameFromJson()
        {
            //string json = JsonUtility.ToJson(mainData);
            //Debug.Log(json);
            //SO_Data newdata = JsonUtility.FromJson<SO_Data>(json);
            //Debug.Log(mainData.playExp);
            //Debug.Log(newdata.playExp);
        }

        public void LoadTheGameFromJson()
        {
            
        }

        private void OnApplicationQuit()
        {
            Debug.Log("dsadasda");
        }
    }
}

