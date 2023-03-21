using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IGDF
{
    public enum HoverTipType { Card, Skill, DDLMachine, SkillMachine ,SkillParent}
    public class O_HoverTip : MonoBehaviour
    {
        public HoverTipType tipType;
        private Vector2 boundaryPos = new Vector2(5, -2);
        private float hoveringTime = 0.3f;
        private float timer;
        private bool isOpen;
        public static bool isAllowOpen = true;

        private void OnMouseEnter()
        {
            timer = hoveringTime;
        }

        private void OnMouseExit()
        {
            isOpen = false;
            M_Global.instance.ui_HoverTip.SetActive(false);
        }

        private void OnMouseOver()
        {
            if (isAllowOpen)
            {
                timer -= Time.deltaTime;
                if (timer < 0 && !isOpen)
                {
                    isOpen = true;
                    ActiveHoverTip();
                }
                SetPosDependsOnMouse();
            }
            else
            {
                M_Global.instance.ui_HoverTip.SetActive(false);
            }
        }

        private void SetPosDependsOnMouse()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            M_Global.instance.ui_HoverTip.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);

            float pivotX = 1;
            float pivotY = 1;
            if (mouseWorldPos.x < boundaryPos.x) pivotX = 0;
            if (mouseWorldPos.y < boundaryPos.y) pivotY = 0;
            M_Global.instance.ui_HoverContent.pivot = new Vector2(pivotX, pivotY);
     
            float offsetPosX = -50;
            float offsetPosY = -50;
            if (pivotX == 0) offsetPosX = 50;
            if (pivotY == 0) offsetPosY = 50;
            M_Global.instance.ui_HoverContent.anchoredPosition = new Vector2(offsetPosX, offsetPosY);
        }

        private void ActiveHoverTip()
        {
            M_Global.instance.ui_HoverTip.SetActive(true);
            SetPosDependsOnMouse();
            switch (tipType)
            {
                case HoverTipType.Card:
                    break;
                case HoverTipType.Skill:
                    SetTipName(GetComponent<O_Skill>().skillData.skillNameEng);
                    SetTipDescription(GetComponent<O_Skill>().skillData.skillDescriptionEng);
                    break;
                case HoverTipType.DDLMachine:
                    break;
                case HoverTipType.SkillMachine:
                    break;
                case HoverTipType.SkillParent:
                    SetTipName(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillNameEng);
                    SetTipDescription(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillDescriptionEng);
                    break;
            }
        }

        private void SetTipName(string tipName)
        {
            M_Global.instance.ui_HoverContent.Find("Name").GetComponent<Text>().text = tipName;
        }

        private void SetTipDescription(string tipDescription)
        {
            M_Global.instance.ui_HoverContent.Find("Description").GetComponent<Text>().text = tipDescription;
        }
    }
}