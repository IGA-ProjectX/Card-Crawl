using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class M_Main : MonoBehaviour
    {
        public static M_Main instance;
        public SO_Repo repository;

        public SO_Deck deck_Tester_PapersPlease;
        private int[] staffValue_Tester_PapersPlease = { 0, 10, 10, 10 ,15};
        public SO_Skill[] skill_Tester;

        [HideInInspector] public M_Card m_Card;
        [HideInInspector] public M_Staff m_Staff;
        [HideInInspector] public M_Skill m_Skill;
        [HideInInspector] public M_SkillResolve m_SkillResolve;

        void Start()
        {
            instance = this;
            m_Card = GetComponent<M_Card>();
            m_Staff = GetComponent<M_Staff>();
            m_Skill = GetComponent<M_Skill>();
            m_SkillResolve = GetComponent<M_SkillResolve>();
            m_Card.InitializeDeck(deck_Tester_PapersPlease);
            m_Staff.InitializeStaffValues(staffValue_Tester_PapersPlease);
            m_Skill.InitializeSkills(skill_Tester);
        }
    }
}