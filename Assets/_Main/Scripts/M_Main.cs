using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class M_Main : MonoBehaviour
    {
        public static M_Main instance;
        public SO_Repo repository;

        public SO_Level deck_Tester_PapersPlease;
        private int[] staffValue_Tester_PapersPlease = { 0, 10, 10, 10 ,15};
        public SO_Skill[] skill_Tester;

        public GameObject obj_ReturnButton;

        [HideInInspector] public M_Card m_Card;
        [HideInInspector] public M_Staff m_Staff;
        [HideInInspector] public M_Skill m_Skill;
        [HideInInspector] public M_SkillResolve m_SkillResolve;
        [HideInInspector] public M_DDL m_DDL;

        public SpringJoint2D springJ;

        void Start()
        {
            instance = this;
            m_Card = GetComponent<M_Card>();
            m_Staff = GetComponent<M_Staff>();
            m_Skill = GetComponent<M_Skill>();
            m_SkillResolve = GetComponent<M_SkillResolve>();
            m_DDL = GetComponent<M_DDL>();

            m_Card.InitializeDeck(M_Global.instance.levels[M_Global.instance.targetLevel]);
            m_Card.InitializeMoveValue();
            m_DDL.CreateDots();
            m_DDL.InitializeNumberList();
            m_Staff.InitializeStaffValues(staffValue_Tester_PapersPlease);
            m_Skill.InitializeSkills(skill_Tester);
        }

        public void CheckDevCircumstance()
        {
            int nullCount = 0;
            foreach (Transform transform in m_Card.cardsInTurn)
            {
                if (transform == null) nullCount++;
            }
            if (m_Card.inGameDeck.Count == 0 && nullCount == 4) GameDevSucceed();
            if (m_Staff.GetDDLValue() <= 0) GameDevFailed();

            void GameDevSucceed()
            {
                M_Global.instance.PlayerExpUp(m_Staff.GetStaffValue(0));
                obj_ReturnButton.SetActive(true);
            }

            void GameDevFailed()
            {
                obj_ReturnButton.SetActive(true);
            }
        }
    }
}