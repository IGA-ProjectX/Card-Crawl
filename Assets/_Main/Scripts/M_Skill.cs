using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace IGDF
{
    public enum SkillUseState { WaitForUse, Targeting ,CanNotUse}
    public class M_Skill : MonoBehaviour
    {
        public Transform[] skillObjects;
        public TMPro.TMP_Text[] skillNames;
        private SkillUseState skillUseState = SkillUseState.CanNotUse;
        [HideInInspector]public O_Skill activatedSkill;

        private void Update()
        {
            if (skillUseState == SkillUseState.Targeting && Input.GetMouseButtonDown(1))
            {
                activatedSkill.ExitTargetingState();
                EnterWaitForUseState();
            }
            if (M_Main.instance.m_Card.inGameDeck.Count == 0)
            {
                skillUseState = SkillUseState.WaitForUse;
            }
        }

        public void InitializeSkills(SO_Skill[] skillArray)
        {
            for (int i = 0; i < skillArray.Length; i++)
            {
                skillObjects[i].GetComponent<O_Skill>().InitializeSkill(skillArray[i]);
                skillNames[i].text = (M_Global.instance.GetLanguage() == SystemLanguage.Chinese) ? skillArray[i].skillNameChi : skillArray[i].skillNameEng;
            }

        }

        public void UseSkill(O_Skill receivedSkill)
        {
            activatedSkill = receivedSkill;
            if (receivedSkill.skillData.skillUseType!=SkillUseType.ClickUse) skillUseState = SkillUseState.Targeting;
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
                case SkillUseType.TargetProTask:
                    EnterTargetingState("ProTask");
                    break;
                case SkillUseType.TargetDesignTask:
                    EnterTargetingState("DesignTask");
                    break;
                case SkillUseType.TargetArtTask:
                    EnterTargetingState("ArtTask");
                    break;
                case SkillUseType.TargetProBoost:
                    EnterTargetingState("ProBoost");
                    break;
                case SkillUseType.TargetDesignBoost:
                    EnterTargetingState("DesignBoost");
                    break;
                case SkillUseType.TargetArtBoost:
                    EnterTargetingState("ArtBoost");
                    break;
            }
        }

        public void EnterTargetingState(string targetType)
        {
            M_Main.instance.m_HoverTip.EnterState(HoverState.SkillTargeting);
            List<O_Card> cardObjs = new List<O_Card>();
            foreach (Transform cardTrans in M_Main.instance.m_Card.cardsInTurn)
                if (cardTrans != null) cardObjs.Add(cardTrans.GetComponent<O_Card>());
            //foreach (O_Card card in cardObjs)
            //    card.SetDraggableState(false);

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
            else if (targetType == "ProBoost")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Code && card.cardCurrentValue>=0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "DesignBoost")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Design && card.cardCurrentValue >= 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "ArtBoost")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Art && card.cardCurrentValue >= 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "ProTask")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Code && card.cardCurrentValue < 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "DesignTask")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Design && card.cardCurrentValue < 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
            else if (targetType == "ArtTask")
            {
                foreach (O_Card card in cardObjs)
                    if (card.cardCurrentType == CardType.Art && card.cardCurrentValue < 0) SetCardForSkillState(card, true);
                    else SetCardForSkillState(card, false);
            }
        }

        public void SetCardForSkillState(O_Card targetCard,bool targetState)
        {
            targetCard.isCardReadyForSkill = targetState;
            //SpriteRenderer targetCardBG = targetCard.transform.Find("Card BG").GetComponent<SpriteRenderer>();
      
            //if (targetState)
            //    DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.green, 0.3f);
            //else
            //    DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.red, 0.3f);

            SpriteRenderer targetCardCover = targetCard.transform.Find("Card Dark").GetComponent<SpriteRenderer>();

            if (targetState)
                DOTween.To(() => targetCardCover.color, x => targetCardCover.color = x, new Color(0, 0, 0, 0), 0.3f);
            else
                DOTween.To(() => targetCardCover.color, x => targetCardCover.color = x, new Color(0, 0, 0, 0.6f), 0.3f);
        }

        public void EnterWaitForUseState()
        {
            skillUseState = SkillUseState.WaitForUse;
            activatedSkill = null;
            foreach (Transform cardTrans in M_Main.instance.m_Card.cardsInTurn)
            {
                if (cardTrans!=null)
                {
                    //cardTrans.GetComponent<O_Card>().SetDraggableState(true);
                    cardTrans.GetComponent<O_Card>().isCardReadyForSkill = false;

                    SpriteRenderer targetCardCover = cardTrans.transform.Find("Card Dark").GetComponent<SpriteRenderer>();
                    DOTween.To(() => targetCardCover.color, x => targetCardCover.color = x, new Color(0, 0, 0, 0), 0.3f);

                    //SpriteRenderer targetCardBG = cardTrans.transform.Find("Card BG").GetComponent<SpriteRenderer>();
                    //DOTween.To(() => targetCardBG.color, x => targetCardBG.color = x, Color.white, 0.3f);
                }
            }
            M_Main.instance.m_HoverTip.EnterState(HoverState.AllActive);
        }

        public void EnterCanNotUseState()
        {
            skillUseState = SkillUseState.CanNotUse;
        }

        public SkillUseState GetSkillState()
        {
            return skillUseState;
        }
    }
}
