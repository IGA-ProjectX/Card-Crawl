using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.Video;

namespace IGDF
{
    public enum TutorialType { Task,Exp,Skill,Residue,DDL,TaskToDDL,LeftClick,Ability,Begin}

    public class M_Tutorial : MonoBehaviour
    {
        public SpriteRenderer bg_Dark;
        public Transform hl_Card;
        public Transform hl_Character;
        public Transform hl_Skill;
        public Transform hl_Residue;
        public Transform hl_DDL;
        public Transform hl_Exp;
        public Transform textArea;
        public Transform hl_AllCard;
        public Transform hl_AllCha;
        public Transform hl_Video;
        public Transform b_Close;
        public Transform b_Play;

        public static M_Tutorial instance;
        private int turnCounter = 0;

        private bool isTaskIntroduced = false;
        private bool isExpIntroduced = false;
        private bool isSkillIntroduced = false;
        private bool isAbilityIntroduced = false;
        private bool isDDLIntroduced = false;
        private bool isResidueIntroduced = false;
        private bool isTaskToDDLIntroduced = false;
        private bool isBeginIntroduced = false;

        private List<Transform> toShrinkElements = new List<Transform>();
        private bool isTutorialState = false;

        private Transform videoParent;
        private VideoPlayer videoPlayer;
        private bool isPlayed = false;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            videoParent = GameObject.Find("Canvas").transform.Find("Video");
            videoPlayer = videoParent.GetComponentInChildren<VideoPlayer>();

        }

        private void Update()
        {
            M_HoverTip.instance.EnterState(HoverState.CardDragging);
        }

        public void TurnCounterIncrease()
        {
            EnterTurnManager();
            turnCounter++;
        }

        public void EnterTurnManager()
        {
            if (turnCounter == 0)
            {
                ReInitializeDeck();
                AssignHighlightObjs();
                FindObjectOfType<M_Main>().GameProduced += DeleteTutorial;
            }
        }

        private void ReInitializeDeck()
        {
            SO_Level tutorial = M_Global.instance.levels[0];
            List<Card> tempList = new List<Card>();
            foreach (Card card in tutorial.cards_Design) tempList.Add(card);
            foreach (Card card in tutorial.cards_Art) tempList.Add(card);
            foreach (Card card in tutorial.cards_Code) tempList.Add(card);
            foreach (Card card in tutorial.cards_Production) tempList.Add(card);
            M_Main.instance.m_Card.inGameDeck = tempList;
        }

        public bool GetTutorialState()
        {
            return isTutorialState;
        }

        private void AssignHighlightObjs()
        {
            bg_Dark.color = new Color(0, 0, 0, 0);
            hl_Card.localScale = Vector3.zero;
            hl_Character.localScale = Vector3.zero;
            hl_Skill.localScale = Vector3.zero;
            hl_Residue.localScale = Vector3.zero;
            hl_DDL.localScale = Vector3.zero;
            hl_Exp.localScale = Vector3.zero;
            hl_AllCard.localScale = Vector3.zero;
            hl_AllCha.localScale = Vector3.zero;
            hl_Video.localScale = Vector3.zero;
            textArea.localScale = Vector3.zero;
            b_Close.gameObject.SetActive(false);
            b_Play.gameObject.SetActive(false);
        }

        public void GetCardInfoAndDetermineTutorial(O_Card card)
        {
            if (!isTaskIntroduced && card.cardCurrentValue <= 0)
            {
                hl_Card.position = card.transform.position;
                switch (card.cardCurrentType)
                {
                    case CardType.Design:
                        hl_Character.position = FindCharacterPosition(CardType.Design);
                        break;
                    case CardType.Art:
                        hl_Character.position = FindCharacterPosition(CardType.Art);
                        break;
                    case CardType.Code:
                        hl_Character.position = FindCharacterPosition(CardType.Code);
                        break;
                }
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Card));
                StartCoroutine(HighlightElement(hl_Character));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Task)));
                isTaskIntroduced = true;
            }

            if (!isTaskToDDLIntroduced && isResidueIntroduced && card.cardCurrentValue<=0)
            {
                hl_Card.position = card.transform.position;
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_DDL));
                StartCoroutine(HighlightElement(hl_Card));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.TaskToDDL)));
                isTaskToDDLIntroduced = true;
            }

            if (!isExpIntroduced && card.cardCurrentType == CardType.Production)
            {
                hl_Card.position = card.transform.position;
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Card));
                StartCoroutine(HighlightElement(hl_Exp));
                StartCoroutine(HighlightElement(hl_DDL));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Exp)));
                isExpIntroduced = true;
            }

            if (!isAbilityIntroduced && card.cardCurrentValue > 0)
            {
                hl_Card.position = card.transform.position;
                switch (card.cardCurrentType)
                {
                    case CardType.Design:
                        hl_Character.position = FindCharacterPosition(CardType.Design);
                        break;
                    case CardType.Art:
                        hl_Character.position = FindCharacterPosition(CardType.Art);
                        break;
                    case CardType.Code:
                        hl_Character.position = FindCharacterPosition(CardType.Code);
                        break;
                }
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Card));
                StartCoroutine(HighlightElement(hl_Character));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Ability)));
                isAbilityIntroduced = true;
            }

            Vector3 FindCharacterPosition(CardType cardType)
            {
                O_Character[] chas = FindObjectsOfType<O_Character>();

                switch (cardType)
                {
                    case CardType.Design:
                        foreach (O_Character character in chas)
                            if (character.thisCharacter == CharacterType.Designer)
                                return (Vector2)character.transform.position + character.GetComponent<BoxCollider2D>().offset;
                        break;
                    case CardType.Art:
                        foreach (O_Character character in chas)
                            if (character.thisCharacter == CharacterType.Artist)
                                return (Vector2)character.transform.position + character.GetComponent<BoxCollider2D>().offset;
                        break;
                    case CardType.Code:
                        foreach (O_Character character in chas)
                            if (character.thisCharacter == CharacterType.Programmer)
                                return (Vector2)character.transform.position + character.GetComponent<BoxCollider2D>().offset;
                        break;
                }
                return Vector3.zero;
            }
        }

        public void IntroBegin()
        {
            if (!isBeginIntroduced)
            {
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_AllCard));
                StartCoroutine(HighlightElement(hl_AllCha));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Begin)));
                isBeginIntroduced = true;
            }
        }

        public void IntroResidue(O_Card card)
        {
            if (!isResidueIntroduced && card.cardCurrentValue <= 0)
            {
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Residue));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Residue)));
                isResidueIntroduced = true;
            }
        }

        public void IntroSkill()
        {
            if (!isSkillIntroduced)
            {
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Skill));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.Skill)));
                isSkillIntroduced = true;
            }
        }

        public void IntroDDL(O_Card card)
        {
            if (!isDDLIntroduced)
            {
                hl_Card.position = card.transform.position;
                EnterTutorialState();
                StartCoroutine(HighlightElement(hl_Card));
                StartCoroutine(HighlightElement(hl_DDL));
                StartCoroutine(EnableText(GetTutorialText(TutorialType.DDL)));
                isDDLIntroduced = true;
            }
        }

        private void EnterTutorialState()
        {
            StopAllCoroutines();
            AssignHighlightObjs();
            toShrinkElements.Clear();
            foreach (Transform card in M_Main.instance.m_Card.cardsInTurn)
                if (card!=null) card.GetComponent<O_Card>().SetDraggableState(false);
            Time.timeScale = 0;
            StartCoroutine(BackGroundFadeTo(0.6f));
            StartCoroutine(ButtonMoveIn());
            isTutorialState = true;
            isPlayed = false;
        }

        public void ExitTutorialState()
        {
            if (isTutorialState)
            {
                Time.timeScale = 1;
                foreach (Transform card in M_Main.instance.m_Card.cardsInTurn)
                    if (card != null) card.GetComponent<O_Card>().SetDraggableState(true);
                StartCoroutine(BackGroundFadeTo(0f));
                foreach (Transform trans in toShrinkElements)
                    trans.DOScale(Vector3.zero, 0.6f);
                b_Play.DOMoveX(10.5f, 0.6f);
                b_Close.DOMoveX(10.5f, 0.6f);
                isTutorialState = false;
            }
        }

        public void PlayTutorialVideo()
        {
            if (!isPlayed)
            {
                foreach (Transform trans in toShrinkElements)
                {
                    if (trans.gameObject.name != "Description")
                    {
                        StopCoroutine(HighlightElement(trans));
                        StartCoroutine(MinimizeElement(trans));
                    }

                }
                StartCoroutine(PlayVideo());
                StartCoroutine(HighlightElement(hl_Video));
                isPlayed = true;
            }

        }

        private IEnumerator BackGroundFadeTo(float targetTransparency)
        {
            if (targetTransparency == 0)
            {
                float transparency = 0.6f;
                while (transparency > targetTransparency)
                {
                    transparency -= Time.unscaledDeltaTime;
                    bg_Dark.color = new Color(0, 0, 0, transparency);
                    yield return null;
                }
            }
            else
            {
                M_Main.instance.m_HoverTip.EnterState(HoverState.AllDisactive);
                float transparency = 0f;
                while (transparency <= targetTransparency)
                {
                    transparency += Time.unscaledDeltaTime;
                    bg_Dark.color = new Color(0, 0, 0, transparency);
                    yield return null;
                }
            }
        }

        private IEnumerator HighlightElement(Transform trans)
        {
            toShrinkElements.Add(trans);
            TextTitleSync(trans);

            float scale = 0f;
            while (scale < 1)
            {
                scale += Time.unscaledDeltaTime * 2;
                trans.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }

        private IEnumerator MinimizeElement(Transform trans)
        {
            float scale = 1f;
            while (scale > 0)
            {
                scale -= Time.unscaledDeltaTime * 2;
                trans.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }

        private IEnumerator EnableText(string content)
        {
            toShrinkElements.Add(textArea);
            float scaleX = 0f;
            while (scaleX < 1 )
            {
                scaleX += Time.unscaledDeltaTime;
                textArea.localScale = new Vector3(scaleX, scaleX, scaleX);
            }

            TMPro.TMP_Text desText = textArea.GetComponentInChildren<TMPro.TMP_Text>();
            desText.text = content;
            desText.maxVisibleCharacters = 0;
            foreach (char letter in desText.text)
            {
                desText.maxVisibleCharacters++;
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }

        private string GetTutorialText(TutorialType targetType)
        {
            foreach (TutorialText targetTT in M_Global.instance.repository.tutorialTexts)
            {
                if (targetTT.type == targetType)
                {
                    if (targetTT.video != null) videoPlayer.clip = targetTT.video;
                    if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) return targetTT.textChi;
                    else return targetTT.textEng;
                }
            }
            return "";
        }

        public void DeleteTutorial()
        {
            Destroy(gameObject, 0.5f);
        }

        private IEnumerator PlayVideo()
        {
            toShrinkElements.Add(videoParent);
            videoParent.gameObject.SetActive(true);
            float scale = 0f;
            while (scale < 1)
            {
                scale += Time.unscaledDeltaTime*2;
                videoParent.localScale = new Vector3(scale, scale, scale);
                yield return null;
            }
        }

        private IEnumerator ButtonMoveIn()
        {
            b_Close.gameObject.SetActive(true);
            b_Play.gameObject.SetActive(true);
            float xPos = 11;
            while (xPos > 8.6f)
            {
                xPos -= Time.unscaledDeltaTime * 4;
                b_Close.position = new Vector3(xPos, b_Close.position.y, b_Close.position.z);
                b_Play.position = new Vector3(xPos, b_Play.position.y, b_Play.position.z);
                yield return null;
            }
        }

        private void TextTitleSync(Transform trans)
        {
            bool isChinese = M_Global.instance.GetLanguage() == SystemLanguage.Chinese ? true : false;
            TMPro.TMP_Text textArea = trans.GetComponentInChildren<TMPro.TMP_Text>() != null ? trans.GetComponentInChildren<TMPro.TMP_Text>() : null;
            if (trans == hl_AllCard) textArea.text = isChinese ? "技能与任务卡区域" : "Card Area";
            else if (trans == hl_AllCha) textArea.text = isChinese ? "开发者角色区域" : "Developer Area";
            else if (trans == hl_Card) textArea.text = isChinese ? "目标卡牌" : "Card";
            else if (trans == hl_Character) textArea.text = isChinese ? "对应角色" : "Developer";
            else if (trans == hl_DDL) textArea.text = isChinese ? "死线机器" : "Deadline Machine";
            else if (trans == hl_Exp) textArea.text = isChinese ? "已获得经验" : "Obtained Exp";
            else if (trans == hl_Residue) textArea.text = isChinese ? "剩余任务指示器" : "Residue Task Number";
            else if (trans == hl_Skill) textArea.text = isChinese ? "眼球技能机器人" : "Skill Eye Robot";
        }
    }
}