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
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
            s.AppendCallback(() => M_Main.instance.m_Card.ShuffleDeck(M_Main.instance.m_Card.inGameDeck));
            s.AppendCallback(() => M_Main.instance.m_Card.DrawCard(toDrawIndexes));
        }

        private void Skill_WithdrawOneTask(O_Card targetCard)
        {
            targetCard.CardBackToDeck();
     
            Sequence s = DOTween.Sequence();
            s.AppendInterval(M_Main.instance.m_Card.horiTime + M_Main.instance.m_Card.verTime + 0.3f);
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
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
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
        }

        private void Skill_GainOneExpDoubleItsValue(O_Card targetCard)
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => targetCard.SetLineStateAuto());
            s.Append(targetCard.transform.DOMove(M_Main.instance.m_Staff.staffSlots[0].position, 0.2f));
            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue(0, targetCard.cardCurrentValue * 2));
            s.AppendCallback(() => targetCard.DestroyCardInScreen());
            s.AppendInterval(0.1f);
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
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
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());
        }
    }
}

