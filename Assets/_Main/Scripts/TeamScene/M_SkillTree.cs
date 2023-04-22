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
        private Dictionary<CharacterType, bool> isActivedTree = new Dictionary<CharacterType, bool>();

        void Start()
        {
            instance = this;
            SetSkillTreeState(true, false, false, true);
            InitializeSkillTrees();
        }

        public void InitializeSkillTrees()
        {
            for (int i = 0; i < skillParents.Length; i++)
                skillTrees[i].GetComponent<O_SkillTree>().UpdateGlassState(isActivedTree[skillParents[i].characterType]);

            for (int i = 0; i < skillParents.Length; i++)
                for (int j = 0; j < skillParents[i].nodeList.Length; j++)
                    InstantiateNewBud(i, j);
        }

        public void InstantiateNewBud(int treeIndex,int flowerIndex)
        {
            NodeInfo nodeInfo = skillParents[treeIndex].nodeList[flowerIndex];
            Transform newBud = Instantiate(pre_FlowerBud, skillTrees[treeIndex].transform.Find("FlowerPivots").GetChild(flowerIndex)).transform;
            newBud.GetComponent<O_FlowerBud>().InitializeBud(nodeInfo, skillParents[treeIndex].characterType);
            newBud.name = skillParents[treeIndex].characterType + " " + (flowerIndex + 1);
        }

        void SetSkillTreeState(bool proState, bool desState, bool artState, bool codState)
        {
            isActivedTree.Add(CharacterType.Producer, proState);
            isActivedTree.Add(CharacterType.Designer, desState);
            isActivedTree.Add(CharacterType.Artist, artState);
            isActivedTree.Add(CharacterType.Programmer, codState);
        }

        public bool GetTreeState(CharacterType treeType)
        {
            return isActivedTree[treeType];
        }
    }
}