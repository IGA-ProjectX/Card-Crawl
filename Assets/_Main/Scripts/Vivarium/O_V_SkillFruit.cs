using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public class O_V_SkillFruit : MonoBehaviour
    {
        private SO_Skill skillInfo;
        private Vector3 lastMousePosition = Vector3.zero;
        private Vector3 initialPosition;
        private bool isPlugined = false;
        public static O_V_SkillFruit selectedSkill;

        public void InitializeSkillFruit(SO_Skill skillToSet)
        {
            skillInfo = skillToSet;
            transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = skillInfo.skillImage;
            initialPosition = transform.position;
        }

        public void OnMouseDrag()
        {
            selectedSkill = this;
            if (lastMousePosition!=Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                transform.position += new Vector3(offset.x, offset.y, 0);
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnMouseUp()
        {
            lastMousePosition = Vector3.zero;

            if (O_V_SkillRobot.selectedtSkillRobot != null && O_V_SkillRobot.selectedtSkillRobot.isReadyForLoadNewSkill)
            {
                O_V_SkillRobot.selectedtSkillRobot.LoadNewSkillIntoThis(skillInfo);
                isPlugined = true;
                transform.DOScale(0, 0.3f);
                Destroy(gameObject, 0.4f);
            }

            if (isPlugined == false) transform.DOMove(initialPosition, 0.7f);
            selectedSkill = null;
        }

        public SO_Skill CurrentSkillInfo()
        {
            return skillInfo;
        }
    }
}