using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IGDF
{
    public enum HoverTipType { Card, Skill, DDLMachine, SkillMachine ,SkillParent,Character}
    public class O_HoverTip : MonoBehaviour
    {
        public HoverTipType tipType;
        private Vector2 boundaryPos = new Vector2(5, -2);
        private float hoveringTime = 0.3f;
        private float timer;
        private bool isOpen;
        public static bool isAllowOpen = true;
        private GameObject selectionBox;

        private void Start()
        {
            selectionBox = FindObjectOfType<M_Main>().transform.Find("Selection Box").gameObject;
            selectionBox.SetActive(false);
        }

        private void OnMouseEnter()
        {
            timer = hoveringTime;
            if (isAllowOpen)
            {
                var boxCollider = GetComponent<BoxCollider2D>();
                var size = boxCollider.size;
                var offset = boxCollider.offset;

                var topLeftLocal = offset + new Vector2(-size.x * 0.5f, size.y * 0.5f);
                var topLeftWorld = transform.TransformPoint(topLeftLocal);
                var topRightLocal = offset + new Vector2(size.x * 0.5f, size.y * 0.5f);
                var topRightWorld = transform.TransformPoint(topRightLocal);
                var bottomLeftLocal = offset + new Vector2(-size.x * 0.5f, -size.y * 0.5f);
                var bottomLeftWorld = transform.TransformPoint(bottomLeftLocal);
                var bottomRightLocal = offset + new Vector2(size.x * 0.5f, -size.y * 0.5f);
                var bottomRightWorld = transform.TransformPoint(bottomRightLocal);

                selectionBox.SetActive(true);

                selectionBox.transform.Find("TopLeft").transform.position = topLeftWorld;
                selectionBox.transform.Find("TopRight").transform.position = topRightWorld;
                selectionBox.transform.Find("BottomLeft").transform.position = bottomLeftWorld;
                selectionBox.transform.Find("BottomRight").transform.position = bottomRightWorld;
            }
        }

        private void OnMouseExit()
        {
            isOpen = false;
            M_Global.instance.ui_HoverTip.SetActive(false);
            selectionBox.SetActive(false);
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
     
            float offsetPosX = -40;
            float offsetPosY = -40;
            if (pivotX == 0) offsetPosX = 40;
            if (pivotY == 0) offsetPosY = 40;
            M_Global.instance.ui_HoverContent.anchoredPosition = new Vector2(offsetPosX, offsetPosY);
        }

        private void ActiveHoverTip()
        {
            M_Global.instance.ui_HoverTip.SetActive(true);
            SetPosDependsOnMouse();
            if (M_Global.instance.GetLanguage() == SystemLanguage.English)
                switch (tipType)
                {
                    case HoverTipType.Card:
                        SetTipName(GetComponent<O_Card>().GetCardData().cardNameEng);
                        SetTipDescription(GetComponent<O_Card>().GetCardData().cardSummaryEng);
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
                    case HoverTipType.Character:
                        break;
                }
            else if (M_Global.instance.GetLanguage() == SystemLanguage.Chinese)
                switch (tipType)
                {
                    case HoverTipType.Card:
                        SetTipName(GetComponent<O_Card>().GetCardData().cardNameChi);
                        SetTipDescription(GetComponent<O_Card>().GetCardData().cardSummaryChi);
                        break;
                    case HoverTipType.Skill:
                        SetTipName(GetComponent<O_Skill>().skillData.skillNameChi);
                        SetTipDescription(GetComponent<O_Skill>().skillData.skillDescriptionChi);
                        break;
                    case HoverTipType.DDLMachine:
                        break;
                    case HoverTipType.SkillMachine:
                        break;
                    case HoverTipType.SkillParent:
                        SetTipName(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillNameChi);
                        SetTipDescription(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillDescriptionChi);
                        break;
                    case HoverTipType.Character:
                        break;
                }
        }

        private void SetTipName(string tipName)
        {
            M_Global.instance.ui_HoverContent.Find("Name").GetComponent<TMP_Text>().text = tipName;
        }

        private void SetTipDescription(string tipDescription)
        {
            M_Global.instance.ui_HoverContent.Find("Description").GetComponent<TMP_Text>().text = tipDescription;
        }
    }
}