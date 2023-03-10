using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Skill Parent", menuName = "IGDF/New Skill Parent")]
    public class SO_SkillParent : ScriptableObject
    {
        public CharacterType characterType;
        public NodeInfo[] nodeList;
    }

    [System.Serializable]
    public class NodeInfo
    {
        public NodeIndex thisNodeIndex;
        public NodeIndex[] parentNodeIndexes;
        public SO_Skill[] childSkills;
    }

    public enum NodeIndex { None, C1, C2, C3, B1, B2, B3, A1, A2, A3 }
}
