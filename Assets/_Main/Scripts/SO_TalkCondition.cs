using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Talk Condition", menuName = "IGDF/New Talk Condition")]
    public class SO_TalkCondition : ScriptableObject
    {
        public List<TalkCondition> talkConditions;
    }

    [System.Serializable]
    public class TalkCondition
    {
        [TextArea(2,2)]
        public string enumType;
        public CharacterType talkingCha;
    }
}
