using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IGDF
{
    public enum HoverTipType { Card, Skill, DDLMachine, SkillMachine ,SkillParent,Character,ResidueTask,SkillInVivarium,BudInVivarium, SkillFruit }
    public class O_HoverTip : MonoBehaviour
    {
        public HoverTipType tipType;
        private Vector2 boundaryPos = new Vector2(5, -2);
        private float hoveringTime = 0.3f;
        private float timer;
        private bool isOpen;
        private bool isAllowOpenTip;
        private bool isAllowOpenBox;
        private GameObject selectionBox;

        private void OnDestroy()
        {
           M_HoverTip.instance.HoverTipListAddOrRemove(this, false);
        }

        private void Start()
        {
            if (tipType == HoverTipType.BudInVivarium)
            {
                if (M_SkillTree.instance.GetTreeState(GetComponent<O_FlowerBud>().GetBudParentTreeType()))
                    M_HoverTip.instance.HoverTipListAddOrRemove(this, true);
            }
            else M_HoverTip.instance.HoverTipListAddOrRemove(this, true);

            if (FindObjectOfType<M_Main>()!=null)
            {
                selectionBox = FindObjectOfType<M_Main>().transform.Find("Selection Box").gameObject;
                selectionBox.SetActive(false);
            }
        }

        private void OnMouseEnter()
        {
            timer = hoveringTime;
            if (isAllowOpenBox)
            {
                OpenSelectionBoxInCertainPos();
            }

            void OpenSelectionBoxInCertainPos()
            {
                var boxCollider = GetComponent<BoxCollider2D>();
                var size = boxCollider.size;
                var offset = boxCollider.offset;
                float sizeOffset = 0.15f;

                var topLeftLocal = offset + new Vector2(-size.x * 0.5f, size.y * 0.5f) + new Vector2(sizeOffset, -sizeOffset);
                var topLeftWorld = transform.TransformPoint(topLeftLocal);
                var topRightLocal = offset + new Vector2(size.x * 0.5f, size.y * 0.5f) + new Vector2(-sizeOffset, -sizeOffset);
                var topRightWorld = transform.TransformPoint(topRightLocal);
                var bottomLeftLocal = offset + new Vector2(-size.x * 0.5f, -size.y * 0.5f) + new Vector2(sizeOffset, sizeOffset);
                var bottomLeftWorld = transform.TransformPoint(bottomLeftLocal);
                var bottomRightLocal = offset + new Vector2(size.x * 0.5f, -size.y * 0.5f) + new Vector2(-sizeOffset, sizeOffset);
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
            if (FindObjectOfType<M_Main>()!=null) selectionBox.SetActive(false);
        }

        private void OnMouseOver()
        {
            if (isAllowOpenTip)
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

            float xOffset = boundaryPos.x;
            if (M_SceneTransition.instance.currentView == M_SceneTransition.CabinView.InSkill) xOffset -= 32;
            else if (M_SceneTransition.instance.currentView == M_SceneTransition.CabinView.InWebsite) xOffset += 32;

            float pivotX = 1;
            float pivotY = 1;
            if (mouseWorldPos.x < xOffset) pivotX = 0;
            if (mouseWorldPos.y < boundaryPos.y) pivotY = 0;
            M_Global.instance.ui_HoverContent.pivot = new Vector2(pivotX, pivotY);

            float offsetAmount = -10;

            float offsetPosX = -offsetAmount;
            float offsetPosY = -offsetAmount;
            if (pivotX == 0) offsetPosX = offsetAmount;
            if (pivotY == 0) offsetPosY = offsetAmount;
            M_Global.instance.ui_HoverContent.anchoredPosition = new Vector2(offsetPosX, offsetPosY);

            if (offsetPosX == -offsetAmount)
                if (offsetPosY == -offsetAmount) M_Global.instance.ui_HoverTip.transform.GetChild(0).GetComponent<Image>().sprite = M_Global.instance.repository.hoverTipBGs[1];
                else M_Global.instance.ui_HoverTip.transform.GetChild(0).GetComponent<Image>().sprite = M_Global.instance.repository.hoverTipBGs[3];
            if (offsetPosX == offsetAmount)
                if (offsetPosY == -offsetAmount) M_Global.instance.ui_HoverTip.transform.GetChild(0).GetComponent<Image>().sprite = M_Global.instance.repository.hoverTipBGs[0];
                else M_Global.instance.ui_HoverTip.transform.GetChild(0).GetComponent<Image>().sprite = M_Global.instance.repository.hoverTipBGs[2];
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
                        SetTipName(GetHoverTipInfo(HoverTipType.DDLMachine).nameEng);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.DDLMachine).desEng);
                        break;
                    case HoverTipType.SkillMachine:
                        SetTipName(GetHoverTipInfo(HoverTipType.SkillMachine).nameEng);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.SkillMachine).desEng);
                        break;
                    case HoverTipType.SkillParent:
                        SetTipName(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillNameEng);
                        SetTipDescription(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillDescriptionEng);
                        break;
                    case HoverTipType.Character:
                        SetTipName(GetComponent<O_Character>().GetCharacterInfo().nameEng);
                        SetTipDescription(GetComponent<O_Character>().GetCharacterInfo().desEng);
                        break;
                    case HoverTipType.ResidueTask:
                        SetTipName(GetHoverTipInfo(HoverTipType.ResidueTask).nameEng);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.ResidueTask).desEng);
                        break;
                    case HoverTipType.BudInVivarium:
                        SetTipName(GetComponent<O_FlowerBud>().GetBudSkillInfo().skillNameEng + " " + GetComponent<O_FlowerBud>().GetBudPriceToUnlock());
                        SetTipDescription(GetComponent<O_FlowerBud>().GetBudSkillInfo().skillDescriptionEng);
                        break;
                    case HoverTipType.SkillInVivarium:
                        SetTipName(GetComponent<O_V_SkillRobot>().CurrentSkillInfo().skillNameEng);
                        SetTipDescription(GetComponent<O_V_SkillRobot>().CurrentSkillInfo().skillDescriptionEng);
                        break;
                    case HoverTipType.SkillFruit:
                        SetTipName(GetComponent<O_V_SkillFruit>().CurrentSkillInfo().skillNameEng);
                        SetTipDescription(GetComponent<O_V_SkillFruit>().CurrentSkillInfo().skillDescriptionEng);
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
                        SetTipName(GetHoverTipInfo(HoverTipType.DDLMachine).nameChi);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.DDLMachine).desChi);
                        break;
                    case HoverTipType.SkillMachine:
                        SetTipName(GetHoverTipInfo(HoverTipType.SkillMachine).nameChi);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.SkillMachine).desChi);
                        break;
                    case HoverTipType.SkillParent:
                        SetTipName(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillNameChi);
                        SetTipDescription(GetComponent<O_SkillParent>().nodeInfo.childSkills[0].skillDescriptionChi);
                        break;
                    case HoverTipType.Character:
                        SetTipName(GetComponent<O_Character>().GetCharacterInfo().nameChi);
                        SetTipDescription(GetComponent<O_Character>().GetCharacterInfo().desChi);
                        break;
                    case HoverTipType.ResidueTask:
                        SetTipName(GetHoverTipInfo(HoverTipType.ResidueTask).nameChi);
                        SetTipDescription(GetHoverTipInfo(HoverTipType.ResidueTask).desChi);
                        break;
                    case HoverTipType.BudInVivarium:
                        SetTipName(GetComponent<O_FlowerBud>().GetBudSkillInfo().skillNameChi + " " + GetComponent<O_FlowerBud>().GetBudPriceToUnlock());
                        SetTipDescription(GetComponent<O_FlowerBud>().GetBudSkillInfo().skillDescriptionChi);
                        break;
                    case HoverTipType.SkillInVivarium:
                        SetTipName(GetComponent<O_V_SkillRobot>().CurrentSkillInfo().skillNameChi);
                        SetTipDescription(GetComponent<O_V_SkillRobot>().CurrentSkillInfo().skillDescriptionChi);
                        break;
                    case HoverTipType.SkillFruit:
                        SetTipName(GetComponent<O_V_SkillFruit>().CurrentSkillInfo().skillNameChi);
                        SetTipDescription(GetComponent<O_V_SkillFruit>().CurrentSkillInfo().skillDescriptionChi);
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

        public void ChangeAllowOpenState(bool tipState,bool boxState)
        {
            isAllowOpenTip = tipState;
            isAllowOpenBox = boxState;
        }

        public void SetSelectionBoxState(bool targetState)
        {
            selectionBox.SetActive(targetState);
        }

        public HoverTipInfo GetHoverTipInfo(HoverTipType targetTipType)
        {
            foreach (HoverTipInfo hoverTipInfo in M_Global.instance.repository.hoverTipInfos)
            {
                if (hoverTipInfo.type == targetTipType)
                {
                    return hoverTipInfo;
                }
            }
            Debug.LogError("No Tip Info");
            return null;
        }
    }
}