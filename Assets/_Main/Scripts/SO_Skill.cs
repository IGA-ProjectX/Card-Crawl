using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "IGDF/New Skill")]
    public class SO_Skill : ScriptableObject
    {
        public int skillIndex;

        public string skillNameChi;
        [TextArea(3,3)]
        public string skillDescriptionChi;

        public string skillNameEng;
        [TextArea(3, 3)]
        public string skillDescriptionEng;

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
        SelectOneNoneExpCardMultitarget,
        xzxzxzx,
        zxzxzxz,
        BasicSyntax,
        BasicGameEngine,
        ScriptableObject,
        FiniteStateMachine,
        GamePhysics,
        DesignPattern,
        Shader,
    }

    public enum SkillUseType
    {
        None,
        ClickUse,
        TargetTask,
        TargetBoost,
        TargetExp,
        TargetNoExp,
        TargetPro,
        TargetArt,
        TargetDesign
    }
}
