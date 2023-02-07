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
        public TMP_Text deadlineText;
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
        }

        public int GetStaffValue(int index)
        {
            return inTurnValues[index];
        }

        public void ChangeDeadLineValue(int value)
        {
            deadLine += value;
            deadlineText.text = deadLine.ToString();
        }

        public int GetDDLValue()
        {
            return deadLine;
        }
    }
}

