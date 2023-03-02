using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace IGDF
{
    public class M_SkillResolve : MonoBehaviour
    {
        public void EffectResolve(O_Skill activatedSkill)
        {
            switch (activatedSkill.skillData.skillType)
            {
                case SkillType.RedrawAllCard:
                    Skill_RedrawAllCard();
                    break;
                case SkillType.ChangeOneCardProfessionRandomly:
                    break;
                case SkillType.RemoveAllTaskChangeDDLTo1:
                    break;
                case SkillType.EvenDistributeProfessionValue:
                    break;
                case SkillType.Shader:
                    Skill_Pro_Shader();
                    break;
                case SkillType.ScriptableObject:
                    Skill_Pro_ScriptableObject();
                    break;
                case SkillType.FiniteStateMachine:
                    Skill_Pro_FiniteStateMachine();
                    break;
                case SkillType.DesignPattern:
                    Skill_Pro_DesignPattern();
                    break;
            }
            activatedSkill.SetSkillUninteractable();
        }

        public void EffectResolve(O_Skill activatedSkill, O_Card targetCard)
        {
            switch (activatedSkill.skillData.skillType)
            {
                case SkillType.WithdrawOneTask:
                    Skill_WithdrawOneTask(targetCard);
                    break;
                case SkillType.ChangeOneCardProfessionRandomly:
                    Skill_ChangeOneCardProfessionRandomly(targetCard);
                    break;
                case SkillType.RemoveOneTaskGainNoExp:
                    break;
                case SkillType.RemoveOneTaskGainHalfExp:
                    break;
                case SkillType.SelectOneTaskNoDDL:
                    break;
                case SkillType.ChangeOneTaskToHalf:
                    break;
                case SkillType.GainOneExpDoubleItsValue:
                    Skill_GainOneExpDoubleItsValue(targetCard);
                    break;
                case SkillType.RemoveOneTaskDecreaseEqualExp:
                    break;
                case SkillType.SelectOneNoneExpCardMultitarget:
                    Skill_SelectOneNoneExpCardMultitarget(targetCard);
                    break;
                case SkillType.BasicSyntax:
                    Skill_Pro_BasicSyntax(targetCard);
                    break;
                case SkillType.BasicGameEngine:
                    Skill_Pro_BasicGameEngine(targetCard);
                    break;
                case SkillType.GamePhysics:
                    Skill_Pro_GamePhysics(targetCard);
                    break;
            }
            activatedSkill.SetSkillUninteractable();
        }

        private void Skill_RedrawAllCard()
        {
            List<int> toDrawIndexes = new List<int>();
            for (int i = 0; i < M_Main.instance.m_Card.cardsInTurn.Count; i++)
            {
                if (M_Main.instance.m_Card.cardsInTurn[i] != null)
                {
                    toDrawIndexes.Add(M_Main.instance.m_Card.cardsInTurn[i].GetComponent<O_Card>().inSlotIndex);
                    M_Main.instance.m_Card.cardsInTurn[i].GetComponent<O_Card>().CardBackToDeck();
                }
            }
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            //s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
            s.AppendCallback(() => M_Main.instance.m_Card.ShuffleDeck(M_Main.instance.m_Card.inGameDeck));
            s.AppendCallback(() => M_Main.instance.m_Card.DrawCard(toDrawIndexes));
        }

        private void Skill_WithdrawOneTask(O_Card targetCard)
        {
            targetCard.CardBackToDeck();
            targetCard.SetDraggableState(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            //s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
            s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }

        private void Skill_ChangeOneCardProfessionRandomly(O_Card targetCard)
        {
            int randomType = 0;
            do { randomType = Random.Range(1, 4); }
            while (randomType == (int)targetCard.cardCurrentType);
            switch (randomType)
            {
                case 1:
                    targetCard.cardCurrentType = CardType.Design;
                    break;
                case 2:
                    targetCard.cardCurrentType = CardType.Art;
                    break;
                case 3:
                    targetCard.cardCurrentType = CardType.Code;
                    break;
            }
            SpriteRenderer cardBG = targetCard.transform.Find("Card BG").GetComponent<SpriteRenderer>();
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.yellow, 0.4f));
            s.AppendInterval(0.3f);
            s.AppendCallback(() => targetCard.transform.Find("Card Image Type").GetComponent<SpriteRenderer>().sprite = M_Main.instance.repository.cardTypeIcons[randomType]);
            s.AppendInterval(0.1f);
            s.AppendCallback(() => DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.white, 0.4f));
            //s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
        }

        private void Skill_GainOneExpDoubleItsValue(O_Card targetCard)
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => targetCard.SetLineStateAuto());
            s.AppendCallback(() => targetCard.SetDraggableState(false));
            s.Append(targetCard.transform.DOMove(M_Main.instance.m_Staff.staffSlots[0].position, 0.4f));
            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue(0, targetCard.cardCurrentValue * 2));
            s.AppendCallback(() => targetCard.DestroyCardInScreen());
            s.AppendInterval(0.1f);
            //s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
            s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }

        private void Skill_SelectOneNoneExpCardMultitarget(O_Card targetCard)
        {
            switch (targetCard.cardCurrentType)
            {
                case CardType.Design:
                    targetCard.targetableType.Add(CardType.Art);
                    targetCard.targetableType.Add(CardType.Code);
                    break;
                case CardType.Art:
                    targetCard.targetableType.Add(CardType.Design);
                    targetCard.targetableType.Add(CardType.Code);
                    break;
                case CardType.Code:
                    targetCard.targetableType.Add(CardType.Design);
                    targetCard.targetableType.Add(CardType.Art);
                    break;
            }
            SpriteRenderer cardBG = targetCard.transform.Find("Card BG").GetComponent<SpriteRenderer>();
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.yellow, 0.2f));
            s.AppendInterval(0.2f);
            s.AppendCallback(() => DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.white, 0.2f));
            s.AppendInterval(0.2f);
            s.AppendCallback(() => DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.yellow, 0.2f));
            s.AppendInterval(0.2f);
            //s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
        }

        #region Programmer Skill
        private void Skill_Pro_BasicSyntax(O_Card targetCard)
        {
            //双倍价值获取一张程序技能提升卡
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => targetCard.SetLineStateAuto());
            s.AppendCallback(() => targetCard.SetDraggableState(false));
            s.Append(targetCard.transform.DOMove(M_Main.instance.m_Staff.staffSlots[3].position, 0.4f));
            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue(3, targetCard.cardCurrentValue * 2));
            s.AppendCallback(() => targetCard.DestroyCardInScreen());
            s.AppendInterval(0.1f);
            s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_BasicGameEngine(O_Card targetCard)
        {
            //移除一个程序需求，并获得对应项目经验
            M_Main.instance.m_Staff.GainExpDirectly(-targetCard.cardCurrentValue);
            targetCard.CardMoveOutOfScreenRightWards();
            targetCard.SetDraggableState(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_ScriptableObject()
        {
            //设计师的技能点数等于程序员的技能点数 - 其实有Bug，但是不想改
            int designerValue = M_Main.instance.m_Staff.GetStaffValue(1);
            int programmerValue = M_Main.instance.m_Staff.GetStaffValue(3);
            int offset;
            if (designerValue > programmerValue) offset = designerValue - programmerValue;
            else offset = programmerValue - designerValue;
            M_Main.instance.m_Staff.ChangeStaffValue(1, offset);
        }
        private void Skill_Pro_FiniteStateMachine()
        {
            //当前程序员技能点数翻倍
            M_Main.instance.m_Staff.ChangeStaffValue(3, M_Main.instance.m_Staff.GetStaffValue(3));
        }
        private void Skill_Pro_GamePhysics(O_Card targetCard)
        {
            //移除一张程序需求，无经验，并增加对应的绝对值的技能点数
            M_Main.instance.m_Staff.ChangeStaffValue(3, -targetCard.cardCurrentValue);
            targetCard.CardMoveOutOfScreenRightWards();
            targetCard.SetDraggableState(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_DesignPattern()
        {
            //无损耗解决该轮内所有程序需求
            for (int i = 0; i < M_Main.instance.m_Card.cardsInTurn.Count; i++)
            {
                O_Card cardEntity;
                if (M_Main.instance.m_Card.cardsInTurn[i] != null)
                {
                    cardEntity = M_Main.instance.m_Card.cardsInTurn[i].GetComponent<O_Card>();
                    if (cardEntity.cardCurrentType == CardType.Code && cardEntity.cardCurrentValue < 0)
                    {
                        cardEntity.CardMoveOutOfScreenRightWards();
                        cardEntity.SetDraggableState(false);
                    }
                }
                //Sequence s = DOTween.Sequence();
                //s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
                //s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
                M_Main.instance.m_Card.CheckInTurnCardNumber();
            }
        }
        private void Skill_Pro_Shader()
        {
            //程序员的技能点数 += 美术的技能点数
            int artValue = M_Main.instance.m_Staff.GetStaffValue(2);
            M_Main.instance.m_Staff.ChangeStaffValue(3, artValue);
        }
        #endregion
    }
}

