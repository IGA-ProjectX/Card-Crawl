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
        private LineRenderer targetingLine;
        private GameObject targetingArrow;
        private SpriteMask eyeMask;

        private void Start()
        {
            timer = Random.Range(3.4f, 7.4f);
            targetingLine = GetComponent<LineRenderer>();
            targetingLine.enabled = false;
            targetingArrow = transform.Find("Targeting Arrow").gameObject;
            targetingArrow.SetActive(false);
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
                    EyeAndTrajectoryFollow();
                    break;
            }
        }

        public void InitializeSkill(SO_Skill receivedData)
        {
            skillData = receivedData;
            eyelidUpper = transform.Find("Eye White").Find("Eyelid Upper");
            eyelidBottom = transform.Find("Eye White").Find("Eyelid Bottom");
            eyeball = transform.Find("Eye White").Find("Eye Black");

            eyeball.Find("Eye Pupil").GetComponent<SpriteRenderer>().sprite = skillData.skillImage;

            upperLidOpenPos = eyelidUpper.position;
            bottomLidOpenPos = eyelidBottom.position;
            eyelidUpper.localPosition = upperLidClosePos;
            eyelidBottom.localPosition = bottomLidClosePos;
            upperLidClosePos = eyelidUpper.position;
            bottomLidClosePos = eyelidBottom.position;
            eyeballMiddlePos = eyeball.position;
            eyeMask = transform.Find("Eye White").GetComponent<SpriteMask>();
            //UpdateMaskState(false);
        }

        public void OpenEye()
        {
            if (M_Global.instance.targetLevel == 0)
            {
                if (skillData.skillType != SkillType.RedrawAllCard)
                {
                    SetSkillUninteractable();
                    return;
                }

            }
            

            float speed = Random.Range(0.45f, 1.2f);
            eyeball.DOMove(eyeballMiddlePos, 0.3f);
            eyelidUpper.DOMoveY(upperLidOpenPos.y, speed);
            eyelidBottom.DOMoveY(bottomLidOpenPos.y, speed);
            eyeState = EyeState.LookAround;
            M_Audio.PlaySound(SoundType.SkillRobotEyeOpen);
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
            if (eyeState != EyeState.Close && M_Tutorial.instance != null) M_Tutorial.instance.IntroSkill();
            if (eyeState != EyeState.FollowMouse)
            {
                if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse && !isUsed)
                {
                    M_Main.instance.m_Skill.UseSkill(this);
                    if (skillData.skillUseType != SkillUseType.ClickUse)
                    {
                        eyelidUpper.DOMoveY(upperLidClosePos.y + 0.49f, 0.1f);
                        eyelidBottom.DOMoveY(bottomLidClosePos.y - 0.3f, 0.1f);
                        Sequence s = DOTween.Sequence();
                        s.Append(eyeball.DOScale(0.4f, 0.1f));
                        s.Append(eyeball.DOScale(1.2f, 0.3f));
                        s.Append(eyeball.DOScale(1f, 0.1f));
                        s.AppendCallback(() => eyeState = EyeState.FollowMouse);
                        //s.AppendCallback(() => Cursor.visible = false);
                        //s.AppendCallback(() => targetingArrow.SetActive(true));
                    }
                    else 
                    {
                        M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Check);
                        Sequence s = DOTween.Sequence();
                        s.Append(eyeball.DOScale(0.4f, 0.1f));
                        s.Append(eyeball.DOScale(1.2f, 0.3f));
                        s.Append(eyeball.DOScale(1f, 0.1f));
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            if (eyeState != EyeState.FollowMouse)
            {
                if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse && !isUsed)
                {
                    eyeState = EyeState.Focus;
                    eyeball.DOMove(eyeballMiddlePos, 0.3f);
                    eyeball.DOScale(0.9f, 0.3f);
                    M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Poke);
                }
                else M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Check);
            }
        }

        private void OnMouseExit()
        {
            if (eyeState!=EyeState.FollowMouse)
            {
                if (M_Main.instance.m_Skill.GetSkillState() == SkillUseState.WaitForUse && !isUsed)
                {
                    eyeState = EyeState.LookAround;
                    eyeball.DOScale(1f, 0.3f);
                }
                M_Cursor.instance.SetActiveCursorState(M_Cursor.CursorType.Arrow);
            }

        }

        public void SetSkillUninteractable()
        {
            isUsed = true;
            ExitTargetingState();
            CloseEye();
        }

        public void EyeAndTrajectoryFollow()
        {
            if (isUsed == false)
            {
                if (targetingLine.enabled == false)
                {
                    targetingLine.enabled = true;
                }
                Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)eyeball.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                targetingArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

                eyeball.DOMove(eyeballMiddlePos + new Vector2(direction.normalized.x / 5, direction.normalized.y / 5), 0.2f);
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), eyeballMiddlePos) < 0.5f)
                {
                    targetingLine.enabled = false;
                    targetingArrow.SetActive(false);
                    Cursor.visible = true;
                }
                else
                {
                    targetingLine.enabled = true;
                    targetingArrow.SetActive(true);
                    Cursor.visible = false;
                }
                targetingArrow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetingLine.SetPosition(0, (Vector2)eyeball.position);
                targetingLine.SetPosition(1, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

        }

        public void ExitTargetingState()
        {
            if(!isUsed) OpenEye();

            eyeState = EyeState.LookAround;
            targetingLine.enabled = false;
            targetingArrow.SetActive(false);
            Cursor.visible = true;
        }

        public void UpdateMaskState(bool state)
        {
            eyeMask.enabled = state;
        }
    }
}