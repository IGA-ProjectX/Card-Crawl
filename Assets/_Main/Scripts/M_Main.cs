using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Psychoflow.SSWaterReflection2D;

namespace IGDF
{
    public class M_Main : MonoBehaviour
    {
        public static M_Main instance;
        public SO_Repo repository;
        [HideInInspector] public M_Card m_Card;
        [HideInInspector] public M_Staff m_Staff;
        [HideInInspector] public M_Skill m_Skill;
        [HideInInspector] public M_SkillResolve m_SkillResolve;
        [HideInInspector] public M_DDL m_DDL;
        [HideInInspector] public M_ChatBubble m_ChatBubble;
        [HideInInspector] public M_HoverTip m_HoverTip;

        [HideInInspector]public bool isGameFinished = false;

        private void Awake()
        {
            instance = this;
            m_Card = GetComponent<M_Card>();
            m_Staff = GetComponent<M_Staff>();
            m_Skill = GetComponent<M_Skill>();
            m_SkillResolve = GetComponent<M_SkillResolve>();
            m_DDL = GetComponent<M_DDL>();
            m_ChatBubble = GetComponent<M_ChatBubble>();
            m_HoverTip = GetComponent<M_HoverTip>();
        }

        void Start()
        {
            SpriteSelfWaterReflection studio = transform.parent.GetComponentInChildren<SpriteSelfWaterReflection>();
            studio.ReflectionProvider = FindObjectOfType<SSWaterReflectionProvider>();
            studio.ClearAndInitReflectionRenderers();
            m_HoverTip.EnterState(HoverState.AllDisactive);
            InitializeLevel();
        }

        public void CheckDevCircumstance()
        {
            int nullCount = 0;
            foreach (Transform transform in m_Card.cardsInTurn)
            {
                if (transform == null) nullCount++;
            }

            if (m_Card.inGameDeck.Count == 0 && nullCount == 4)
            {
                GameDevSucceed();
                isGameFinished = true;
            }
            if (m_Staff.GetDDLValue() <= 0)
            {
                GameDevFailed();
                isGameFinished = false;
            }

            void GameDevSucceed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.WinGame);
                M_Global.instance.PlayerExpUp(m_Staff.GetStaffValue(0));
                FindObjectOfType<O_ResultSteam>().GameProduced();
                M_Audio.PlaySound(SoundType.MainMachineSuccess);
            }

            void GameDevFailed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.LoseGame);
                Transform resultFailPanel = FindObjectOfType<O_ResultSteam>().transform.parent.Find("Result Fail");
                if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
                    resultFailPanel.Find("T_Result").GetComponent<TMP_Text>().text = "¿ª·¢Ê§°Ü¡£¡£¡£";
                else resultFailPanel.Find("T_Result").GetComponent<TMP_Text>().text = "Project Unfinished...";
                resultFailPanel.DOScale(1, 0.4f);
                M_Audio.PlaySound(SoundType.MainMachineLose);
            }
        }

        public void GameStart()
        {
          
            m_Card.DrawCard();
            foreach (Transform skillTrans in m_Skill.skillObjects) skillTrans.GetComponent<O_Skill>().OpenEye();
            //{
            //    
            //}
            //foreach (Transform  staffTrans in m_Staff.staffSlots)
            //{
            //    staffTrans.GetComponent<O_HalfCat>().UpdateMaskState(true);
            //}
        }

        public void InitializeLevel()
        {
            m_ChatBubble.PrepareTalkList(M_Global.instance.levels[M_Global.instance.targetLevel].talkList);
            m_Card.InitializeDeck(M_Global.instance.levels[M_Global.instance.targetLevel]);
            m_Card.InitializeMoveValue();
            m_DDL.CreateDots();
            m_DDL.InitializeNumberList();
            m_Staff.InitializeStaffValues(M_Global.instance.levels[M_Global.instance.targetLevel].staffValue);
            m_Skill.InitializeSkills(M_Global.instance.skillList);

            m_DDL.UpdateName();
        }
    }
}