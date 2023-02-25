using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IGDF
{
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
                staffSlots[0].Find("Text Value").GetComponent<TMP_Text>().text = inTurnValues[0].ToString();
            }
            inTurnValues[index] += value;
            staffSlots[index].Find("Text Value").GetComponent<TMP_Text>().text = inTurnValues[index].ToString();

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

        public int GetStaffValue(int index)
        {
            return inTurnValues[index];
        }

        public void ChangeDeadLineValue(int value)
        {
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

