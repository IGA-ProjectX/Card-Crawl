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
        public Sprite[] hoverTipBGs;
        public Sprite[] gameNameImages;

        [Header("Audio")]
        public SoundAudioClip[] bgMusics;
        public SoundAudioClip[] studioClips;
        public SoundAudioClip[] openPageClips;
        public SoundAudioClip[] uiClips;
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
}
