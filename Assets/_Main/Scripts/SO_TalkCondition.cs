using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Talk Condition", menuName = "IGDF/New Talk Condition")]
    public class SO_TalkCondition : ScriptableObject
    {
        [TextArea(2,2)]
        public List<string> talkConditions;
    }
}
