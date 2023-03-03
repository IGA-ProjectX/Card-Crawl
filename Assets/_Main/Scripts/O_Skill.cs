using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace IGDF
{
    public class O_Skill : MonoBehaviour
    {
        public enum EyeState { Focus, LookAround, Close, FollowMouse}
        [HideInInspector]public SO_Skill skillData;
        private bool isUsed = false;
        private EyeState eyeState = EyeState.Close;

        private Vector2 upperLidOpenPos;
        private Vector2 bottomLidOpenPos;
        private Vector2 upperLidClosePos = new Vector2(0, 0.23f);
        private Vector2 bottomLidClosePos = new Vector2(0, -0.3f);
        private Vector2 eyeballMiddlePos;

        private Transform eyelidUpper;
        private Transform eyelidBottom;
        private Transform eyeball;

        private float timer;

        private void Start()
        {
            timer = Random.Range(3.4f, 7.4f);
        }

        private void Update()
        {
            switch (eyeState)
            {
                case EyeState.Focus:
                    break;
                case EyeState.LookAround:
                    timer -= Time.deltaTime;
                    if (timer < 0)
                    {
                        timer = Random.Range(7.8f, 9.6f);
                        Vector2 lookOffset = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                        Sequence s = DOTween.Sequence();
                        s.Append(eyeball.DOMove((Vector2)eyeball.position + lookOffset, 0.3f));
                        s.AppendInterval(Random.Range(2f, 4.5f));
                        s.Append(eyeball.DOMove(eyeballMiddlePos, 0.3f));
                    }
                    break;
                case EyeState.Close:
                    break;
                case EyeState.FollowMouse:
                    break;
            }
        }

        public void InitializeSkill(SO_Skill receivedData)
        {
            skillData = receivedData;
            Debug.Log(skillData.skillNameEng);
            //transform.Find("Skill Image").GetComponent<SpriteRenderer>().sprite = skillData.skillImage;
            //transform.Find("Skill Name").GetComponent<TMP_Text>().text = skillData.skillNameEng;
            eyelidUpper = transform.Find("Eye White").Find("Eyelid Upper");
            eyelidBottom = transform.Find("Eye White").Find("Eyelid Bottom");
            eyeball = transform.Find("Eye White").Find("Eye Black");
            upperLidOpenPos = eyelidUpper.position;
            bottomLidOpenPos = eyelidBottom.position;
            eyelidUpper.localPosition = upperLidClosePos;
            eyelidBottom.localPosition = bottomLidClosePos;
            upperLidClosePos = eyelidUpper.position;
            bottomLidClosePos = eyelidBottom.position;
            eyeballMiddlePos = eyeball.position;
        }

        public void OpenEye()
        {
            float speed = Random.Range(0.45f, 1.2f);
            eyelidUpper.DOMoveY(upperLidOpenPos.y, speed);
            eyelidBottom.DOMoveY(bottomLidOpenPos.y, speed);
            eyeState = EyeState.LookAround;
        }

        public void CloseEye()
        {
            float speed = Random.Range(0.45f, 1.2f);
            eyelidUpper.DOMoveY(upperLidClosePos.y, speed);
            eyelidBottom.DOMoveY(bottomLidClosePos.y, speed);
            eyeState = EyeState.Close;
        }

        private void OnMouseDown()
        {
            if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse && !isUsed)
                M_Main.instance.m_Skill.UseSkill(this);
        }

        private void OnMouseEnter()
        {
            if (!isUsed)
            {
                //SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
                //DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.cyan, 0.3f);
                eyeState = EyeState.Focus;
                eyeball.DOMove(eyeballMiddlePos, 0.3f);
                eyeball.DOScale(0.9f, 0.3f);
            }
        }

        private void OnMouseExit()
        {
            if (!isUsed)
            {
                //SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
                //DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.white, 0.3f);
                eyeState = EyeState.LookAround;
                //eyeball.DOMove(eyeballMiddlePos, 0.3f);
                eyeball.DOScale(1f, 0.3f);
            } 
        }

        public void SetSkillUninteractable()
        {
            isUsed = true;
            CloseEye();
            //SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
            //DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.grey, 0.3f);
        }


        //public void InitializeSkill(SO_Skill receivedData)
        //{
        //    skillData = receivedData;
        //    transform.Find("Skill Image").GetComponent<SpriteRenderer>().sprite = skillData.skillImage;
        //    transform.Find("Skill Name").GetComponent<TMP_Text>().text = skillData.skillNameEng;
        //}

        //private void OnMouseDown()
        //{
        //    if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse && !isUsed)
        //        M_Main.instance.m_Skill.UseSkill(this);
        //}

        //private void OnMouseEnter()
        //{
        //    if (!isUsed)
        //    {
        //        SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
        //        DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.cyan, 0.3f);
        //    }
        //}

        //private void OnMouseExit()
        //{
        //    if (!isUsed)
        //    {
        //        SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
        //        DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.white, 0.3f);
        //    }
        //}

        //public void SetSkillUninteractable()
        //{
        //    isUsed = true;
        //    SpriteRenderer skillBG = transform.Find("Skill BG").GetComponent<SpriteRenderer>();
        //    DOTween.To(() => skillBG.color, x => skillBG.color = x, Color.grey, 0.3f);
        //}
    }
}