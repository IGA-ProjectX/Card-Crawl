using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

namespace IGDF
{
    public enum IconCondition { Inactivated, Approved, Disapproved }
    public class M_Staff : MonoBehaviour
    {
        public Transform[] staffSlots;
        private int[] inTurnValues = { 0, 0, 0, 0};
        private int deadLine;
        public GameObject pre_TargetBox;
        public Transform parent_TargetBoxes;
        public GameObject pre_ValueUp;

        public Action<int, bool> EffectChange;

        public void InitializeStaffValues(int[] valueArray)
        {
            for (int i = 0; i < valueArray.Length; i++)
            {
                if (i < 4) ChangeStaffValue(i, valueArray[i]);
                else ChangeDeadLineValue(valueArray[i]);
            }
            EffectChange += ValueChangePopAndFade;
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
            else 
            {
                if (EffectChange != null) EffectChange(index, (value > 0) ? true : false);

                staffSlots[index].GetChild(0).Find("Number").GetComponent<TMP_Text>().text = inTurnValues[index].ToString();
                if (inTurnValues[index] > 5)
                    staffSlots[index].GetChild(0).Find("Alert").GetComponent<SpriteRenderer>().enabled = false;
                else
                    staffSlots[index].GetChild(0).Find("Alert").GetComponent<SpriteRenderer>().enabled = true;
            }

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

        public void ValueChangePopAndFade(int targetStaff,bool isValueUp)
        {
            var boxCollider = staffSlots[targetStaff].GetComponent<BoxCollider2D>();
            var size = boxCollider.size;
            var offset = boxCollider.offset;

            var topMidLocal = offset + new Vector2(0, size.y * (isValueUp ? 0.5f : 0.7f));
            var topMidWorld = staffSlots[targetStaff].TransformPoint(topMidLocal);

            Transform newValueUp = Instantiate(pre_ValueUp, topMidWorld, Quaternion.identity).transform;
            newValueUp.localScale = Vector3.zero;
            SpriteRenderer valueSprite = newValueUp.GetComponent<SpriteRenderer>();
            valueSprite.sprite = isValueUp ? M_Global.instance.repository.valueChanges[0] : M_Global.instance.repository.valueChanges[1];

            Sequence s = DOTween.Sequence();
            s.Append(newValueUp.DOScale(1, 0.4f));
            s.Append(newValueUp.DOMoveY(newValueUp.position.y + (isValueUp ? 0.2f : -0.2f), 0.4f));
            s.AppendCallback(() => DOTween.To(()=>valueSprite.color, x => valueSprite.color = x, new Color(0, 0, 0, 0), 0.4f));
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

        public void OpenTargetBoxWithState(int targetStaff, IconCondition targetCondition)
        {
            GameObject targetBox = Instantiate(pre_TargetBox, parent_TargetBoxes).gameObject;
            var boxCollider = staffSlots[targetStaff].GetComponent<BoxCollider2D>();
            var size = boxCollider.size;
            var offset = boxCollider.offset;
            float sizeOffset = 0.15f;

            var topLeftLocal = offset + new Vector2(-size.x * 0.5f, size.y * 0.5f) + new Vector2(sizeOffset, -sizeOffset);
            var topLeftWorld = boxCollider.transform.TransformPoint(topLeftLocal);
            var topRightLocal = offset + new Vector2(size.x * 0.5f, size.y * 0.5f) + new Vector2(-sizeOffset, -sizeOffset);
            var topRightWorld = boxCollider.transform.TransformPoint(topRightLocal);
            var bottomLeftLocal = offset + new Vector2(-size.x * 0.5f, -size.y * 0.5f) + new Vector2(sizeOffset, sizeOffset);
            var bottomLeftWorld = boxCollider.transform.TransformPoint(bottomLeftLocal);
            var bottomRightLocal = offset + new Vector2(size.x * 0.5f, -size.y * 0.5f) + new Vector2(-sizeOffset, sizeOffset);
            var bottomRightWorld = boxCollider.transform.TransformPoint(bottomRightLocal);


            switch (targetCondition)
            {
                case IconCondition.Approved:
                    ChangeColor(Color.green);
                    break;
                case IconCondition.Disapproved:
                    ChangeColor(Color.red);
                    break;
            }

            targetBox.transform.Find("TopLeft").transform.position = topLeftWorld;
            targetBox.transform.Find("TopRight").transform.position = topRightWorld;
            targetBox.transform.Find("BottomLeft").transform.position = bottomLeftWorld;
            targetBox.transform.Find("BottomRight").transform.position = bottomRightWorld;

            void ChangeColor(Color targetColor)
            {
                targetBox.transform.Find("TopLeft").GetChild(0).GetComponent<SpriteRenderer>().color = targetColor;
                targetBox.transform.Find("TopRight").GetChild(0).GetComponent<SpriteRenderer>().color = targetColor;
                targetBox.transform.Find("BottomLeft").GetChild(0).GetComponent<SpriteRenderer>().color = targetColor;
                targetBox.transform.Find("BottomRight").GetChild(0).GetComponent<SpriteRenderer>().color = targetColor;
            }
        }

        public void DeleteAllTargetBoxes()
        {
            List<GameObject> childBoxes = new List<GameObject>();
            for (int i = 0; i < parent_TargetBoxes.childCount; i++)
            {
                childBoxes.Add(parent_TargetBoxes.GetChild(i).gameObject);
            }
            foreach (var item in childBoxes)
            {
                Destroy(item, 0.1f);
                item.SetActive(false);
            }
        }
    }
}

