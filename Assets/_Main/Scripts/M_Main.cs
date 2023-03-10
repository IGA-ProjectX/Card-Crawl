using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace IGDF
{
    public class M_Main : MonoBehaviour
    {
        public static M_Main instance;
        public SO_Repo repository;

        public Transform resultPanel;

        [HideInInspector] public M_Card m_Card;
        [HideInInspector] public M_Staff m_Staff;
        [HideInInspector] public M_Skill m_Skill;
        [HideInInspector] public M_SkillResolve m_SkillResolve;
        [HideInInspector] public M_DDL m_DDL;
        [HideInInspector] public M_ChatBubble m_ChatBubble;

        bool isGameFinished = false;

        void Start()
        {
            instance = this;
            m_Card = GetComponent<M_Card>();
            m_Staff = GetComponent<M_Staff>();
            m_Skill = GetComponent<M_Skill>();
            m_SkillResolve = GetComponent<M_SkillResolve>();
            m_DDL = GetComponent<M_DDL>();
            m_ChatBubble = GetComponent<M_ChatBubble>();

            m_ChatBubble.PrepareTalkList(M_Global.instance.levels[M_Global.instance.targetLevel].talkList);
            m_Card.InitializeDeck(M_Global.instance.levels[M_Global.instance.targetLevel]);
            m_Card.InitializeMoveValue();
            m_DDL.CreateDots();
            m_DDL.InitializeNumberList();
            m_Staff.InitializeStaffValues(M_Global.instance.levels[M_Global.instance.targetLevel].staffValue);
            m_Skill.InitializeSkills(M_Global.instance.skillList);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
       
            //}
            if(!isGameFinished)
            CheckDevCircumstance();
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
                isGameFinished = true;
            }

            void GameDevSucceed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.WinGame);
                M_Global.instance.PlayerExpUp(m_Staff.GetStaffValue(0));
                FindObjectOfType<O_Result>().GameProduced();
            }

            void GameDevFailed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.LoseGame);
                FindObjectOfType<O_Result>().GameProduced();
            }
        }

        public void GameStart()
        {
            m_Card.DrawCard();
            foreach (Transform skillTrans in m_Skill.skillObjects)
            {
                skillTrans.GetComponent<O_Skill>().OpenEye();
            }
        }
    }
}