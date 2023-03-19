using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public static class M_Audio
    {
        private static Transform currentSceneAudio;
        private static float sceneAudioTransitionTime = 1;

        public static void PlaySceneMusic(M_SceneTransition.CabinView cabinView)
        {
            if (currentSceneAudio != null)
            {
                AudioSource[] exsitingAudios = currentSceneAudio.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource audio in exsitingAudios)
                    DOTween.To(() => audio.volume, x => audio.volume = x, 0, sceneAudioTransitionTime);
                Object.Destroy(currentSceneAudio.gameObject, sceneAudioTransitionTime+1);
                currentSceneAudio = null;
            }
            Transform sceneAudioParent = new GameObject("Sound " + cabinView).transform;
            switch (cabinView)
            {
                case M_SceneTransition.CabinView.Overview:
                    PlayLoopSoundFadeIn(SoundType.BGMusic1);
                    PlayLoopSoundFadeIn(SoundType.TrainMove);
                    break;
                case M_SceneTransition.CabinView.Studio:
                    break;
                case M_SceneTransition.CabinView.Skill:
                    break;
                case M_SceneTransition.CabinView.Website:
                    break;
                case M_SceneTransition.CabinView.InStudio:
                    PlayLoopSoundFadeIn(SoundType.BGMusic4);
                    PlayLoopSoundFadeIn(SoundType.MainMachineRegular);
                    break;
                case M_SceneTransition.CabinView.InSkill:
                    break;
                case M_SceneTransition.CabinView.InWebsite:
                    PlayLoopSoundFadeIn(SoundType.BGMusic3);
                    break;
                default:
                    break;
            }

            void PlayLoopSoundFadeIn(SoundType toPlaySoundType)
            {
                GameObject soundGameObject = new GameObject("Sound " + toPlaySoundType);
                soundGameObject.transform.SetParent(sceneAudioParent);
                AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
                audioSource.clip = GetAudioClip(toPlaySoundType).audioClip;
                audioSource.loop = true;
                audioSource.volume = 0;
                audioSource.Play();
                DOTween.To(() => audioSource.volume, x => audioSource.volume = x, GetAudioClip(toPlaySoundType).volume, sceneAudioTransitionTime);
                currentSceneAudio = sceneAudioParent;

            }
        }

        public static void PlaySound(SoundType toPlaySoundType)
        {
            GameObject soundGameObject = new GameObject("Sound " + toPlaySoundType);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(toPlaySoundType).audioClip;
            audioSource.volume = GetAudioClip(toPlaySoundType).volume;
            audioSource.Play();
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }

        public static void PlayDelaySound(SoundType toPlaySoundType, float time)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(time);
            s.AppendCallback(() => PlaySound(toPlaySoundType));
        }

        private static SoundAudioClip GetAudioClip(SoundType toPlaySoundType)
        {
            foreach (SoundAudioClip soundAudioClip in M_Global.instance.repository.bgMusics)
            {
                if (soundAudioClip.soundType == toPlaySoundType)
                {
                    return soundAudioClip;
                }
            }
            foreach (SoundAudioClip soundAudioClip in M_Global.instance.repository.studioClips)
            {
                if (soundAudioClip.soundType == toPlaySoundType)
                {
                    return soundAudioClip;
                }
            }
            foreach (SoundAudioClip soundAudioClip in M_Global.instance.repository.openPageClips)
            {
                if (soundAudioClip.soundType == toPlaySoundType)
                {
                    return soundAudioClip;
                }
            }
            foreach (SoundAudioClip soundAudioClip in M_Global.instance.repository.uiClips)
            {
                if (soundAudioClip.soundType == toPlaySoundType)
                {
                    return soundAudioClip;
                }
            }
            Debug.LogError("Sound " + toPlaySoundType + " not Found");
            return null;
        }
    }
}