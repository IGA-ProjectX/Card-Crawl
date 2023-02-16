using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace IGDF
{
    public enum SkillUseState { WaitForUse, Targeting, EffectApply }
    public class M_Skill : MonoBehaviour
    {
        public Transform[] skillObjects;
        [HideInInspector]public SkillUseState skillUseState = SkillUseState.WaitForUse;
        [HideInInspector]public O_Skill activatedSkill;

        private void Update()
        {
            if (skillUseState == SkillUseState.Targeting && Input.GetMouseButtonDown(1))
            {
                EnterWaitForUseState();
            }
        }

        public void InitializeSkills(SO_Skill[] skillArray)
        {
            for (int i = 0; i < skillArray.Length; i++)
                skillObjects[i].GetComponent<O_Skill>().InitializeSkill(skillArray[i]);
        }

        public void UseSkill(O_Skill receivedSkill)
        {
            activatedSkill = receivedSkill;
            skillUseState = SkillUseState.Targeting;
            switch (activatedSkill.skillData.skillUseType)
            {
                case SkillUseType.None:
                    Debug.LogError("Skill " + activatedSkill.skillData.skillIndex + " Has No SkillUseType");
                    break;
                case SkillUseType.ClickUse:
                    M_Main.instance.m_SkillResolve.EffectResolve(activatedSkill);
                    break;
                case SkillUseType.TargetTask:
                    EnterTargetingState("Task");
                    break;
                case SkillUseType.TargetBoost:
                    EnterTargetingState("Boost");
                    break;
                case SkillUseType.TargetExp:
                    EnterTargetingState("Exp");
                    break;
                case SkillUseType.TargetNoExp:
                    EnterTargetingState("NoExp");
                    break;
            }
        }

        public void EnterTargetingState(string targetType)
        {
            List<O_Card> cardObjs = new List<O_Card>();
            foreach (Transform cardTrans in M_Main.instance.m_Card.cardsInTurn)
            {
                if (cardTrans!=null) cardObjs.Add(cardTrans.GetComponent<O_Card>());
            }

            if (targetType == "Task")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentValue < 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "Boost")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentValue >= 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "Exp")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Production) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "NoExp")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType != CardType.Production) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
        }

        public void SetCardForSkillState(O_Card targetCard,bool targetState)
        {
            targetCard.isCardReadyForSkill = targetState;
            Image targetCardBG = targetCard.GetComponent<Image>();
            if (targetState)
                DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.green, 0.3f);
            else
                DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.red, 0.3f);
        }

        public void EnterWaitForUseState()
        {
            skillUseState = SkillUseState.WaitForUse;
            activatedSkill = null;
            foreach (Transform cardTrans in M_Main.instance.m_Card.cardsInTurn)
            {
                if (cardTrans!=null)
                {
                    cardTrans.GetComponent<O_Card>().isCardReadyForSkill = false;
                    Image targetCardBG = cardTrans.GetComponent<Image>();
                    DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.white, 0.3f);
                }
            }
        }
    }
}
