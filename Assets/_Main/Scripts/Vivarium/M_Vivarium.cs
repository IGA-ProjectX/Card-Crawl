using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF {
    public class M_Vivarium : MonoBehaviour
    {
        public static M_Vivarium instance;
        public GameObject[] skillRobots;
        public Transform[] waterSupplys;
        private TMPro.TMP_Text[] text_WaterSupplys = new TMPro.TMP_Text[4];
        private GameObject[] water_Animators = new GameObject[4];
        public AnimationClip waterAnim;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            for (int i = 0; i < waterSupplys.Length; i++)
            {
                text_WaterSupplys[i] = waterSupplys[i].Find("Text").GetComponent<TMPro.TMP_Text>();
                water_Animators[i] = waterSupplys[i].Find("Water Anim").gameObject;
            }

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

        public GameObject GetCharacterWaterAnim(CharacterType targetType)
        {
            switch (targetType)
            {
                case CharacterType.Producer:
                    return water_Animators[0];
                case CharacterType.Designer:
                    return water_Animators[1];
                case CharacterType.Artist:
                    return water_Animators[2];
                case CharacterType.Programmer:
                    return water_Animators[3];
                default:     
                    return water_Animators[0];
            }
        }
    }
}