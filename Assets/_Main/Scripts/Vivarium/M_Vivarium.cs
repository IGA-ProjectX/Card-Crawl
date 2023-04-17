using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF {
    public class M_Vivarium : MonoBehaviour
    {
        public static M_Vivarium instance;
        public GameObject[] skillRobots;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            M_HoverTip.instance.EnterState(HoverState.InVivarium);
        }

        void Update()
        {

        }

        public void InitializeVivarium()
        {
            for (int i = 0; i < skillRobots.Length; i++)
            {
                skillRobots[i].GetComponent<O_V_SkillRobot>().InitializeSkillRobot(M_Global.instance.skillList[i],i);
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