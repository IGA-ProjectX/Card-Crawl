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

        private void Awake()
        {
            if (instance != null) Destroy(this);
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void PlayerExpUp(int newExp) 
        {
            mainData.playExp += newExp;
        }

        public SystemLanguage GetLanguage()
        {
            return currentLanguage;
        }

        public void SetLanguage(SystemLanguage targetLanguage)
        {
            currentLanguage = targetLanguage;
        }
    }
}

