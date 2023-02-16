using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "IGDF/New Skill")]
    public class SO_Skill : ScriptableObject
    {
        public int skillIndex;
        [TextArea(2, 2)]
        public string skillName;
        [TextArea(3,3)]
        public string skillDescription;
        public Sprite skillImage;
        public SkillType skillType;
        public SkillUseType skillUseType;
    }

    public enum SkillType
    {
        None,
        WithdrawOneTask,
        RedrawAllCard,
        ChangeOneCardProfessionRandomly,
        RemoveOneTaskGainNoExp,
        RemoveOneTaskGainHalfExp,
        SelectOneTaskNoDDL,
        ChangeOneTaskToHalf,
        RemoveAllTaskChangeDDLTo1,
        GainOneExpDoubleItsValue,
        EvenDistributeProfessionValue,
        RemoveOneTaskDecreaseEqualExp,
    }

    public enum SkillUseType
    {
        None,
        ClickUse,
        TargetTask,
        TargetBoost,
        TargetExp,
        TargetNoExp
    }
}
