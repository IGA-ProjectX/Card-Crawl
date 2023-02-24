using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Achievement", menuName = "IGDF/New Achievement")]
    public class SO_Achievement : ScriptableObject
    {
        public List<AchievementContent> achievements;
    }

    [System.Serializable]
    public class AchievementContent
    {
        public string enumType;
        public string title;
        public string description;
    }
}