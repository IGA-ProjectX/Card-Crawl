using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class M_SkillTree : MonoBehaviour
    {
        public static M_SkillTree instance;
        public GameObject[] skillTrees;
        public SO_SkillParent[] skillParents;
        public GameObject pre_SkillToSet;

        void Start()
        {
            instance = this;
            InitializeSkillTrees();
            OpenSkillTreePanel(3);
        }

        public void OpenSkillTreePanel(int toOpenIndex)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == toOpenIndex) skillTrees[i].SetActive(true);
                else skillTrees[i].SetActive(false);
            }
        }

        public void InitializeSkillTrees()
        {
            for (int i = 0; i < skillParents.Length; i++)
            {
                for (int j = 0; j < skillParents[i].nodeList.Length; j++)
                {
                    NodeInfo nodeInfo = skillParents[i].nodeList[j];
                    string childName = "Null";
                    switch (nodeInfo.thisNodeIndex)
                    {
                        case NodeIndex.C1: childName = "C1"; break;
                        case NodeIndex.C2: childName = "C2"; break;
                        case NodeIndex.C3: childName = "C3"; break;
                        case NodeIndex.B1: childName = "B1"; break;
                        case NodeIndex.B2: childName = "B2"; break;
                        case NodeIndex.B3: childName = "B3"; break;
                        case NodeIndex.A1: childName = "A1"; break;
                        case NodeIndex.A2: childName = "A2"; break;
                        case NodeIndex.A3: childName = "A3"; break;
                    }
                    GameObject nodeTrans = skillTrees[i].transform.Find(childName).gameObject;
                    nodeTrans.GetComponent<O_SkillParent>().InitializeSkillParent(nodeInfo);
                }
            }
        }


    }
}