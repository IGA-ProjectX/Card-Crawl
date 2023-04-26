using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace IGDF
{
    public class M_Global : MonoBehaviour
    {
        public SO_Data soData;
        public SO_Level[] levels;
        public int targetLevel = 1;
        //public SO_Skill[] skillList;
        public SO_Repo repository;
        private SystemLanguage currentLanguage = SystemLanguage.Chinese;
        public static M_Global instance;
        [HideInInspector] public GameObject ui_HoverTip;
        [HideInInspector] public RectTransform ui_HoverContent;
        [HideInInspector] public Transform chatBubbleParent;
        public JsonData mainData;

        private void Awake()
        {
            if (instance != null) Destroy(this);
            else
            {
                LoadTheGameFromJson();
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

        public void SaveTheGameToJson()
        {
            string json = JsonUtility.ToJson(mainData);
            string filePath = Application.persistentDataPath + "/DataSave.json";
            File.WriteAllText(filePath, json);
        }

        public void LoadTheGameFromJson()
        {
            string filePath = Application.persistentDataPath + "/DataSave.json";
            mainData = JsonUtility.FromJson<JsonData>(File.ReadAllText(filePath));

            //mainData = JsonUtility.FromJson<JsonData>(File.ReadAllText(Application.dataPath + "/SaveFile/Save.json"));
            //Debug.Log(mainData.unlockedSkillNodes[2].thisNodeIndex);

        }

        private void OnApplicationQuit()
        {
            SaveTheGameToJson();
            //FindObjectOfType<M_DataSave>().WriteFile(mainData);
        }
    }
}

