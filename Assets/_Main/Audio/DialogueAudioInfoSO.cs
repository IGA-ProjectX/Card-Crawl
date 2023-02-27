using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "DialogueAudioInfo", menuName = "IGDF/DialogueAudioInfoSO")]
    public class DialogueAudioInfoSO : ScriptableObject
    {
        public string id;
        public AudioClip[] dialogueTypingSoundClips;
        [Range(1, 5)]
        public int frequencyLevel = 2;
        [Range(-3, 3)]
        public float minPitch = 0.5f;
        [Range(-3, 3)]
        public float maxPitch = 3f;
        public bool stopAudioSource;
    }
}