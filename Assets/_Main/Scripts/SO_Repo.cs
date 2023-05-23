using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Repo", menuName = "IGDF/New Repo")]
    public class SO_Repo : ScriptableObject
    {
        public Sprite[] cardTypeIcons;
        public Color[] chaColors;
        public Sprite[] stamps;
        public Color[] stampColors;
        public Sprite[] cardBGImages;
        public Color[] cardElementsColor;
        public Sprite[] hoverTipBGs;
        public Sprite[] gameNameImages;
        public Color orangeColor;
        public CharacterInfo[] characterInfos;
        public HoverTipInfo[] hoverTipInfos;
        public SO_Skill[] defaultSkills;
        public Sprite[] valueChanges;
        public SO_Skill[] skillList;
        public Sprite defaultWebImage;
        public TutorialText[] tutorialTexts;
        public SO_SkillParent[] skillParents;

        [Header("Audio")]
        public SoundAudioClip[] bgMusics;
        public SoundAudioClip[] studioClips;
        public SoundAudioClip[] openPageClips;
        public SoundAudioClip[] uiClips;
    }

    [System.Serializable]
    public class CharacterInfo
    {
        public CharacterType type;
        public string nameEng;
        [TextArea(3,3)]
        public string desEng;
        public string nameChi;
        [TextArea(3, 3)]
        public string desChi;
    }

    [System.Serializable]
    public class HoverTipInfo
    {
        public HoverTipType type;
        public string nameEng;
        [TextArea(3, 3)]
        public string desEng;
        public string nameChi;
        [TextArea(3, 3)]
        public string desChi;
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundType soundType;
        public AudioClip audioClip;
        [Range(0f,1f)]
        public float volume;
    }

    public enum SoundType {
        PulleySlide,
        PulleyStop,
        TrainMove,
        SkillRobotEyeOpen,
        CardIndraft,
        SpringShrink,
        SpringStretch,
        ScoreIndicator,
        MainMachineRegular ,
        BGMusic1,
        BGMusic2,
        BGMusic3,
        BGMusic4,
        MainMachineLose,
        MainMachineSuccess,
        ProducedRare,
        ProducedMedium,
        ProducedWelldone,
        Wind,
        ShutterDoor
    }

    [System.Serializable]
    public class TutorialText
    {
        public TutorialType type;
        [TextArea (3,3)]
        public string textChi;
        [TextArea(3, 3)]
        public string textEng;
        public UnityEngine.Video.VideoClip video;
    }
}
