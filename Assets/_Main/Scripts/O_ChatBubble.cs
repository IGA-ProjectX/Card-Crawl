using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class O_ChatBubble : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler,IBeginDragHandler
    {
        private Vector3 lastMousePosition = Vector3.zero;
        private bool isDrag = false;
        private bool isProcessDestroy = false;
        [SerializeField] private float typingSpeed = 0.04f;
        [SerializeField] private bool makePredictable;
        [SerializeField] private DialogueAudioInfoSO currentAudioInfo;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            Invoke("DestroyBubble", 10f);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            lastMousePosition = Vector3.zero;
            isDrag = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isDrag && !isProcessDestroy)
            {
                DestroyBubble();
            }
        }

        public void DestroyBubble()
        {
            isProcessDestroy = true;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(1.2f, 0.1f));
            s.Append(transform.DOScale(0, 0.3f));
            Destroy(gameObject, 0.5f);
        }

        public void PopUpChatBubble(CharacterType chaType, TalkContent talkContent)
        {
            foreach (O_Character characterObj in M_Main.instance.transform.parent.Find("Studio Scene").Find("Characters").GetComponentsInChildren<O_Character>())
                if (characterObj.thisCharacter == chaType)
                    transform.position = characterObj.transform.position;
            TextMeshProUGUI chatText = transform.Find("Chat Bubble").GetComponentInChildren<TextMeshProUGUI>();
            chatText.color = M_Main.instance.repository.chaColors[(int)chaType];
            if (M_Global.instance.GetLanguage() == SystemLanguage.English) chatText.text = talkContent.talkContentEng;
            else chatText.text = talkContent.talkContentChi;

            chatText.maxVisibleCharacters = 0;

            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(1.2f, 0.3f));
            s.Append(transform.DOScale(0.9f, 0.1f));
            s.Append(transform.DOScale(1f, 0.05f));
            s.AppendCallback(() => StartCoroutine(DisplayLine(chatText)));
            s.AppendCallback(() => StartCoroutine(PlayAudio(talkContent.talkContentEng)));
            //s.AppendCallback(() => StartCoroutine(DisplayLine(chatText,talkContent.talkContentEng)));
        }

        private IEnumerator DisplayLine(TextMeshProUGUI dialogueText)
        {
            //// set the text to the full line, but set the visible characters to 0
            //dialogueText.text = line;
            //dialogueText.maxVisibleCharacters = 0;
            ////display each letter one at a time
            ///
            foreach (char letter in dialogueText.text)
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        //private IEnumerator DisplayLine(TextMeshProUGUI dialogueText, string line)
        //{
        //    //// set the text to the full line, but set the visible characters to 0
        //    //dialogueText.text = line;
        //    //dialogueText.maxVisibleCharacters = 0;
        //    ////display each letter one at a time
        //    ///
        //    foreach (char letter in dialogueText.text)
        //    {
        //        dialogueText.maxVisibleCharacters++;
        //        yield return new WaitForSeconds(typingSpeed);
        //    }
        //    foreach (char letter in line.ToCharArray())
        //    {
        //        PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
        //        dialogueText.maxVisibleCharacters++;
        //        yield return new WaitForSeconds(typingSpeed);
        //    }
        //}
        
        private IEnumerator PlayAudio(string line)
        {
            int maxVisibleCharacters = 0;
            foreach (char letter in line.ToCharArray())
            {
                PlayDialogueSound(line.ToCharArray().Length, letter);
                maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
        {
            // set variables for the below based on our config
            AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
            int frequencyLevel = currentAudioInfo.frequencyLevel;
            float minPitch = currentAudioInfo.minPitch;
            float maxPitch = currentAudioInfo.maxPitch;
            bool stopAudioSource = currentAudioInfo.stopAudioSource;

            // play the sound based on the config
            if (currentDisplayedCharacterCount % frequencyLevel == 0)
            {
                if (stopAudioSource)
                {
                    audioSource.Stop();
                }
                AudioClip soundClip = null;
                // create predictable audio from hashing
                if (makePredictable)
                {
                    int hashCode = currentCharacter.GetHashCode();
                    // sound clip
                    int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                    soundClip = dialogueTypingSoundClips[predictableIndex];
                    // pitch
                    int minPitchInt = (int)(minPitch * 100);
                    int maxPitchInt = (int)(maxPitch * 100);
                    int pitchRangeInt = maxPitchInt - minPitchInt;
                    // cannot divide by 0, so if there is no range then skip the selection
                    if (pitchRangeInt != 0)
                    {
                        int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                        float predictablePitch = predictablePitchInt / 100f;
                        audioSource.pitch = predictablePitch;
                    }
                    else
                    {
                        audioSource.pitch = minPitch;
                    }
                }
                // otherwise, randomize the audio
                else
                {
                    // sound clip
                    int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                    soundClip = dialogueTypingSoundClips[randomIndex];
                    // pitch
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                }

                // play sound
                audioSource.PlayOneShot(soundClip);
            }
        }
    }
}