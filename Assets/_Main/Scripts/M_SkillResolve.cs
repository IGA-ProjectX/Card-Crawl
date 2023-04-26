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
                    Skill_EvenDistributeProfessionValue();
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
                    Skill_SelectOneTaskNoDDL(targetCard);
                    break;
                case SkillType.ChangeOneTaskToHalf:
                    Skill_ChangeOneTaskToHalf(targetCard);
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

        #region Producer Skills
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
            M_Card.cardUsedNumInTurn++;
            targetCard.CardBackToDeck();
            targetCard.SetDraggableState(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
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
            M_Card.cardUsedNumInTurn++;
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => M_Main.instance.m_DDL.DDLSlotChangeTo(SlotCondition.Expanded));
            s.AppendCallback(() => targetCard.SetLineStateAuto());
            s.AppendCallback(() => targetCard.SetDraggableState(false));
            s.Append(targetCard.transform.DOMove(M_Main.instance.m_Staff.staffSlots[0].position, 0.6f));
            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue(0, targetCard.cardCurrentValue * 2));
            s.AppendCallback(() => targetCard.DestroyCardInScreen());
            s.AppendInterval(1f);
            s.AppendCallback(() => M_Main.instance.m_DDL.DDLSlotChangeTo(SlotCondition.Shrinked));
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
        }

        private void Skill_ChangeOneTaskToHalf(O_Card targetCard)
        {
            int changedValue = (targetCard.cardCurrentValue + targetCard.cardCurrentValue % 2) / 2;
            targetCard.ChangeCardValue(changedValue);
        }

        private void Skill_EvenDistributeProfessionValue()
        {
            M_Staff ms = M_Main.instance.m_Staff;
            int totalValue = ms.GetStaffValue(1) + ms.GetStaffValue(2) + ms.GetStaffValue(3);
            int evenValue = totalValue / 3;

            int valueChange_Des = evenValue - ms.GetStaffValue(1);
            int valueChange_Art = evenValue - ms.GetStaffValue(2);
            int valueChange_Cod = totalValue - evenValue * 2 - ms.GetStaffValue(3);

            ms.ChangeStaffValue(1, valueChange_Des);
            ms.ChangeStaffValue(2, valueChange_Art);
            ms.ChangeStaffValue(3, valueChange_Cod);
        }

        private void Skill_SelectOneTaskNoDDL(O_Card targetCard)
        {
            targetCard.ChangeDDLAffected(false);
            TMPro.TMP_Text targetName =  targetCard.transform.Find("Card Name").GetComponent<TMPro.TMP_Text>();
            DOTween.To(() => targetName.color, x => targetName.color = x, Color.cyan, 0.7f);
        }
        #endregion

        #region Programmer Skills
        private void Skill_Pro_BasicSyntax(O_Card targetCard)
        {
            M_Card.cardUsedNumInTurn++;
            //˫����ֵ��ȡһ�ų�����������
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => targetCard.SetLineStateAuto());
            s.AppendCallback(() => targetCard.SetDraggableState(false));
            s.Append(targetCard.transform.DOMove(M_Main.instance.m_Staff.staffSlots[3].position, 0.4f));
            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue(3, targetCard.cardCurrentValue * 2));
            s.AppendCallback(() => targetCard.DestroyCardInScreen());
            s.AppendInterval(0.1f);
            //s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_BasicGameEngine(O_Card targetCard)
        {
            M_Card.cardUsedNumInTurn++;
            //�Ƴ�һ���������󣬲���ö�Ӧ��Ŀ����
            SpriteRenderer targetBG = targetCard.transform.Find("Card BG").GetComponent<SpriteRenderer>();
            DOTween.To(() => targetBG.color, x => targetBG.color = x, Color.white, 0.2f);
            M_Main.instance.m_Staff.GainExpDirectly(-targetCard.cardCurrentValue);
            targetCard.CardMoveOutOfScreenRightWards();
            targetCard.SetDraggableState(false);
            //Sequence s = DOTween.Sequence();
            //s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            //s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_ScriptableObject()
        {
            //���ʦ�ļ��ܵ������ڳ���Ա�ļ��ܵ��� - ��ʵ��Bug�����ǲ����
            int designerValue = M_Main.instance.m_Staff.GetStaffValue(1);
            int programmerValue = M_Main.instance.m_Staff.GetStaffValue(3);
            int offset;
            if (designerValue > programmerValue) offset = designerValue - programmerValue;
            else offset = programmerValue - designerValue;
            M_Main.instance.m_Staff.ChangeStaffValue(1, offset);
        }
        private void Skill_Pro_FiniteStateMachine()
        {
            //��ǰ����Ա���ܵ�������
            M_Main.instance.m_Staff.ChangeStaffValue(3, M_Main.instance.m_Staff.GetStaffValue(3));
        }
        private void Skill_Pro_GamePhysics(O_Card targetCard)
        {
            M_Card.cardUsedNumInTurn++;
            //�Ƴ�һ�ų��������޾��飬�����Ӷ�Ӧ�ľ���ֵ�ļ��ܵ���
            M_Main.instance.m_Staff.ChangeStaffValue(3, -targetCard.cardCurrentValue);
            targetCard.CardMoveOutOfScreenRightWards();
            targetCard.SetDraggableState(false);
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            //s.AppendCallback(() => M_Main.instance.m_Card.CheckInTurnCardNumber());
        }
        private void Skill_Pro_DesignPattern()
        {
            int codeTaskCount = 0;
            //����Ľ�����������г�������
            for (int i = 0; i < M_Main.instance.m_Card.cardsInTurn.Count; i++)
            {
                O_Card cardEntity;
                if (M_Main.instance.m_Card.cardsInTurn[i] != null)
                {
                    cardEntity = M_Main.instance.m_Card.cardsInTurn[i].GetComponent<O_Card>();
                    if (cardEntity.cardCurrentType == CardType.Code && cardEntity.cardCurrentValue < 0)
                    {
                        codeTaskCount++;
                        cardEntity.CardMoveOutOfScreenRightWards();
                        cardEntity.SetDraggableState(false);
                    }
                }
            }
            M_Card.cardUsedNumInTurn += codeTaskCount;
        }
        private void Skill_Pro_Shader()
        {
            //����Ա�ļ��ܵ��� += �����ļ��ܵ���
            int artValue = M_Main.instance.m_Staff.GetStaffValue(2);
            M_Main.instance.m_Staff.ChangeStaffValue(3, artValue);
        }
        #endregion
    }
}

