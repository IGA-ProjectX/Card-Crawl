using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace IGDF
{
    public class O_Skill : MonoBehaviour
    {
        [HideInInspector]public SO_Skill skillData;
        private bool isUsed = false;

        public void InitializeSkill(SO_Skill receivedData)
        {
            skillData = receivedData;
            transform.Find("Img_Skill").GetComponent<SpriteRenderer>().sprite = skillData.skillImage;
            transform.Find("T_Name").GetComponent<TMP_Text>().text = skillData.skillName;
        }

        private void OnMouseDown()
        {
            if (M_Main.instance.m_Skill.skillUseState == SkillUseState.WaitForUse && !isUsed)
                M_Main.instance.m_Skill.UseSkill(this);
        }

        private void OnMouseEnter()
        {
            if (!isUsed)
            {
                SpriteRenderer skillBG = transform.Find("BG_Skill").GetComponent<SpriteRenderer>();
                DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.cyan, 0.3f);
            }
        }

        private void OnMouseExit()
        {
            if (!isUsed)
            {
                SpriteRenderer skillBG = transform.Find("BG_Skill").GetComponent<SpriteRenderer>();
                DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.white, 0.3f);
            } 
        }

        private void OnMouseOver()
        {
            
        }

        public void SetSkillUninteractable()
        {
            isUsed = true;
            SpriteRenderer skillBG = transform.Find("BG_Skill").GetComponent<SpriteRenderer>();
            DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.grey, 0.3f);
        }
    }
}