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
        public GameObject pre_FlowerBud;
        public GameObject pre_FruitSkill;

        void Start()
        {
            instance = this;
            InitializeSkillTrees();
        }

        public void InitializeSkillTrees()
        {
            for (int i = 0; i < skillParents.Length; i++)
            {
                for (int j = 0; j < skillParents[i].nodeList.Length; j++)
                {
                    //NodeInfo nodeInfo = skillParents[i].nodeList[j];
                    //Transform newBud = Instantiate(pre_FlowerBud, skillTrees[i].transform.Find("FlowerPivots").GetChild(j)).transform;
                    //newBud.GetComponent<O_FlowerBud>().InitializeBud(nodeInfo, skillParents[i].characterType);
                    //newBud.name = skillParents[i].characterType + " " + (j + 1);
                    InstantiateNewBud(i, j);
                }
            }
        }

        public void InstantiateNewBud(int treeIndex,int flowerIndex)
        {
            NodeInfo nodeInfo = skillParents[treeIndex].nodeList[flowerIndex];
            Transform newBud = Instantiate(pre_FlowerBud, skillTrees[treeIndex].transform.Find("FlowerPivots").GetChild(flowerIndex)).transform;
            newBud.GetComponent<O_FlowerBud>().InitializeBud(nodeInfo, skillParents[treeIndex].characterType);
            newBud.name = skillParents[treeIndex].characterType + " " + (flowerIndex + 1);
        }

    }
}