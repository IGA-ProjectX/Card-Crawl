using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public enum IconCondition { Inactivated, Approved, Disapproved }
    public class M_Staff : MonoBehaviour
    {
        public Transform[] staffSlots;
        private int[] inTurnValues = { 0, 0, 0, 0};
        private int deadLine;

        public void InitializeStaffValues(int[] valueArray)
        {
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (i < 4) ChangeStaffValue(i, valueArray[i]);
                else ChangeDeadLineValue(valueArray[i]);
            }
        }

        public void ChangeStaffValue(int index, int value)
        {
            if (value < 0) 
            {
                inTurnValues[0] -= value;
                staffSlots[0].GetChild(2).Find("Number").GetComponent<TMP_Text>().text = inTurnValues[0].ToString();
            }
            inTurnValues[index] += value;

            if (index == 0) staffSlots[0].GetChild(2).Find("Number").GetComponent<TMP_Text>().text = inTurnValues[0].ToString();
            else staffSlots[index].GetChild(0).Find("Number").GetComponent<TMP_Text>().text = inTurnValues[index].ToString();

            //staffSlots[index].Find("Cat Behind").Find("Text Value").GetComponent<TMP_Text>().text = inTurnValues[index].ToString();

            switch (index)
            {
                case 0:
                    M_Main.instance.m_ChatBubble.TryTriggerTalkStaffValueChange(CharacterType.Producer);
                    break;
                case 1:
                    M_Main.instance.m_ChatBubble.TryTriggerTalkStaffValueChange(CharacterType.Designer);
                    break;
                case 2:
                    M_Main.instance.m_ChatBubble.TryTriggerTalkStaffValueChange(CharacterType.Artist);
                    break;
                case 3:
                    M_Main.instance.m_ChatBubble.TryTriggerTalkStaffValueChange(CharacterType.Programmer);
                    break;
            }
        }

        public void StaffIconChangeTo(int targetStaff,IconCondition targetCondition)
        {
            if (targetStaff == 0)
            {
                SpriteRenderer valueText = staffSlots[0].GetChild(2).Find("Icon").GetComponent<SpriteRenderer>();
                switch (targetCondition)
                {
                    case IconCondition.Inactivated:
                        DOTween.To(() => valueText.color, x => valueText.color = x, Color.black, 0.3f);
                        break;
                    case IconCondition.Approved:
                        DOTween.To(() => valueText.color, x => valueText.color = x, Color.cyan, 0.3f);
                        break;
                    case IconCondition.Disapproved:
                        DOTween.To(() => valueText.color, x => valueText.color = x, Color.red, 0.3f);
                        break;
                }
            }
            else
            {
                SpriteRenderer icon = staffSlots[targetStaff].GetChild(0).Find("Icon").GetComponent<SpriteRenderer>();
                switch (targetCondition)
                {
                    case IconCondition.Inactivated:
                        DOTween.To(() => icon.color, x => icon.color = x, Color.black, 0.3f);
                        break;
                    case IconCondition.Approved:
                        DOTween.To(() => icon.color, x => icon.color = x, Color.cyan, 0.3f);
                        break;
                    case IconCondition.Disapproved:
                        DOTween.To(() => icon.color, x => icon.color = x, Color.red, 0.3f);
                        break;
                }
            }
        }

        public void GainExpDirectly(int value)
        {
            inTurnValues[0] += value;
            staffSlots[0].GetChild(2).Find("Number").GetComponent<TMP_Text>().text = inTurnValues[0].ToString();
        }

        public int GetStaffValue(int index)
        {
            return inTurnValues[index];
        }

        public void ChangeDeadLineValue(int value)
        {
            M_Audio.PlaySound(SoundType.ScoreIndicator);
            deadLine += value;
            if (deadLine < 0) deadLine = 0;
            M_Main.instance.m_DDL.GetValueChangeDot(deadLine);
        }

        public int GetDDLValue()
        {
            return deadLine;
        }
    }
}

