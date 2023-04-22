using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF {
    public class M_Vivarium : MonoBehaviour
    {
        public static M_Vivarium instance;
        public GameObject[] skillRobots;
        public TMPro.TMP_Text[] text_WaterSupplys;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            for (int i = 0; i < text_WaterSupplys.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        text_WaterSupplys[i].text = (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) ? "监督" : "Producer";
                        break;
                    case 1:
                        text_WaterSupplys[i].text = (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) ? "设计" : "Design";
                        break;
                    case 2:
                        text_WaterSupplys[i].text = (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) ? "美术" : "Artist";
                        break;
                    case 3:
                        text_WaterSupplys[i].text = (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) ? "程序" : "Coder";
                        break;
                }
            }
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.AppendCallback(() => M_HoverTip.instance.EnterState(HoverState.InVivarium));
        }

        void Update()
        {
        }

        public void InitializeVivarium()
        {
            for (int i = 0; i < skillRobots.Length; i++)
            {
                skillRobots[i].GetComponent<O_V_SkillRobot>().InitializeSkillRobot(M_Global.instance.mainData.inUseSkills[i], i);
            }
        }

        public void AliveTheScene()
        {
            for (int i = 0; i < skillRobots.Length; i++)
            {
                skillRobots[i].GetComponent<O_V_SkillRobot>().OpenEye();
            }
        }
    }
}