using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace IGDF
{
    public class O_SkillSlot : MonoBehaviour
    {
        public int slotIndex;

        private void OnMouseEnter()
        {
            if (O_SkillParent.skillToSet != null)
            {
                SpriteRenderer slotBG = transform.GetComponent<SpriteRenderer>();
                DOTween.To(() => slotBG.color, x => slotBG.color = x, Color.cyan, 0.2f);
                O_SkillParent.selectedSlot = this;
            }
        }

        private void OnMouseExit()
        {
            if (O_SkillParent.skillToSet != null)
            {
                SpriteRenderer slotBG = transform.GetComponent<SpriteRenderer>();
                DOTween.To(() => slotBG.color, x => slotBG.color = x, Color.white, 0.2f);
                O_SkillParent.selectedSlot = null;
            }
        }

        //private void OnMouseUp()
        //{
        //    if (skillToSet != null)
        //    {
        //        Debug.Log(skillToSet.skillNameEng);
        //        SpriteRenderer slotBG = transform.GetComponent<SpriteRenderer>();
        //        DOTween.To(() => slotBG.color, x => slotBG.color = x, Color.white, 0.2f);
        //        transform.Find("Text").GetComponent<TMP_Text>().text = skillToSet.skillNameEng;
        //        M_Global.instance.skillList[slotIndex] = skillToSet;
        //        skillToSet = null;
        //    }
        //    Debug.Log(skillToSet.skillNameEng);
        //}

        public void UpdateSkillToList(SO_Skill skillToSet)
        {
            SpriteRenderer slotBG = transform.GetComponent<SpriteRenderer>();
            DOTween.To(() => slotBG.color, x => slotBG.color = x, Color.white, 0.2f);
            transform.Find("Text").GetComponent<TMP_Text>().text = skillToSet.skillNameEng;
            M_Global.instance.skillList[slotIndex] = skillToSet;
        }
    }
}