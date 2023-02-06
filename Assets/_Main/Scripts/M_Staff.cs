using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_Staff : MonoBehaviour
{
    public Transform[] staffSlots;
    private int[] inTurnValues = { 0, 0, 0, 0 };

    public void InitializeStaffValues(int[] valueArray)
    {
        for (int i = 0; i < valueArray.Length; i++)
        {
           ChangeStaffValue(i, valueArray[i]);
        }
    }

    public void ChangeStaffValue(int index,int value)
    {
        inTurnValues[index] += value;
        staffSlots[index].Find("Text Value").GetComponent<TMP_Text>().text = inTurnValues[index].ToString();
    }

    public int GetStaffValue(int index)
    {
        return inTurnValues[index];
    }
}
