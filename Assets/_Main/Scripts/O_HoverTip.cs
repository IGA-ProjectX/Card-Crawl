using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public enum HoverTipType { Card, Skill, DDLMachine, SkillMachine }
    public class O_HoverTip : MonoBehaviour
    {
        public HoverTipType tipType;
        private GameObject ui_HoverTip;
        private RectTransform ui_HoverContent;
        private Vector2 boundaryPos = new Vector2(5, -2);

        private float hoveringTime = 0.3f;
        private float timer;
        private bool isOpen;

        private void Start()
        {
            ui_HoverTip = GameObject.Find("Canvas").transform.Find("Hover Tip").gameObject;
            ui_HoverContent = ui_HoverTip.transform.GetChild(0).GetComponent<RectTransform>();
            ui_HoverTip.SetActive(false);
        }

        private void OnMouseEnter()
        {
            timer = hoveringTime;
        }

        private void OnMouseExit()
        {
            isOpen = false;
            ui_HoverTip.SetActive(false);
        }

        private void OnMouseOver()
        {
            timer -= Time.deltaTime;
            if (timer < 0 && !isOpen)
            {
                isOpen = true;
                ActiveHoverTip();
            }
            SetPosDependsOnMouse();
        }

        private void SetPosDependsOnMouse()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ui_HoverTip.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);

            float pivotX = 1;
            float pivotY = 1;
            if (mouseWorldPos.x < boundaryPos.x) pivotX = 0;
            if (mouseWorldPos.y < boundaryPos.y) pivotY = 0;
            ui_HoverContent.pivot = new Vector2(pivotX, pivotY);
     
            float offsetPosX = -50;
            float offsetPosY = -50;
            if (pivotX == 0) offsetPosX = 50;
            if (pivotY == 0) offsetPosY = 50;
            ui_HoverContent.anchoredPosition = new Vector2(offsetPosX, offsetPosY);
        }

        private void ActiveHoverTip()
        {
            ui_HoverTip.SetActive(true);
            SetPosDependsOnMouse();
            switch (tipType)
            {
                case HoverTipType.Card:
                    break;
                case HoverTipType.Skill:
                    break;
                case HoverTipType.DDLMachine:
                    break;
                case HoverTipType.SkillMachine:
                    break;
                default:
                    break;
            }
        }
    }
}