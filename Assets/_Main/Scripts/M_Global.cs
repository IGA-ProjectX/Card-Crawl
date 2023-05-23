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
        public GameObject debugPanel;

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

        }

        private void OnApplicationQuit()
        {
            SaveTheGameToJson();
        }

        public void LoadSkillIntoInUse(int skillSequance, SO_Skill toLoad)
        {
            mainData.inUseSkills[skillSequance] = toLoad.skillIndex;
        }

        public SO_Skill[] GetSkillListInUse()
        {
            SO_Skill[] currentList = new SO_Skill[4];
            for (int i = 0; i < currentList.Length; i++)
            {
                currentList[i] = GetSingleSkillInUse(mainData.inUseSkills[i]);
            }
            return currentList;
        }

        public SO_Skill GetSingleSkillInUse(int targetIndex)
        {
            SO_Skill skillToGet = null;
            foreach (SO_Skill skill in repository.skillList)
            {
                if (skill.skillIndex == targetIndex)
                {
                    skillToGet = skill;
                }
            }
            return skillToGet;
        }

        public void OpenDebugPanel(string content)
        {
            //debugPanel.SetActive(true);
            //debugPanel.GetComponentInChildren<TMPro.TMP_Text>().text += content;
        }
    }
}

