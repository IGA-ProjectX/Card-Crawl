using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Data", menuName = "IGDF/New Data")]
    public class SO_Data : ScriptableObject
    {
        public int gameTimeInTotal;
        public int playExp;
        public ProductShowcase[] productShowcases;
        public List<UnlockedSkillNode> unlockedSkillNodes;
        public SO_Skill[] inUseSkills;
    }

    [System.Serializable]
    public class ProductShowcase
    {
        public LevelType levelType;
        public ProductLevel productLevel;
        public string producedDate;
        public string userReviewLevel;
        public string userReviewNumber;
    }

    [System.Serializable]
    public class JsonData
    {
        public int gameTimeInTotal;
        public int playExp;
        public ProductShowcase[] productShowcases;
        public List<UnlockedSkillNode> unlockedSkillNodes;
        public SO_Skill[] inUseSkills;
        public int targetUnlockedLevelNum;
    }
}
