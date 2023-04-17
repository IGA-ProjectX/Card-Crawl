using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace IGDF
{
    public class O_SkillParent : MonoBehaviour
    {
        [HideInInspector]public NodeInfo nodeInfo;
        public static SO_Skill skillToSet;
        public static Transform skillToSetObj;
        public static O_SkillSlot selectedSlot;

        public void InitializeSkillParent(NodeInfo nodeInfoToSet)
        {
            nodeInfo = nodeInfoToSet;
        }

        public void OnMouseDown()
        {
            //skillToSetObj = Instantiate(M_SkillTree.instance.pre_SkillToSet, 
            //    Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            //    Quaternion.identity,GameObject.Find("Canvas").transform).transform;
            //skillToSet = nodeInfo.childSkills[0];
            //skillToSetObj.Find("Text").GetComponent<Text>().text = skillToSet.skillNameEng;
            //skillToSetObj.localScale = Vector3.zero;
            //skillToSetObj.DOScale(1, 0.4f);
        }

        public void OnMouseDrag()
        {
            if (skillToSetObj != null)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                skillToSetObj.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
            }
        }

        public void OnMouseUp()
        {
            if (skillToSetObj != null)
            {
                skillToSetObj.DOScale(0, 0.4f);
                Destroy(skillToSetObj.gameObject, 1f);
                if (selectedSlot!=null)
                {
                    selectedSlot.UpdateSkillToList(skillToSet);
                }
            }
            skillToSetObj = null;
            skillToSet = null;
        }

        public void SetSkillToList()
        {
          
        }
    }
}