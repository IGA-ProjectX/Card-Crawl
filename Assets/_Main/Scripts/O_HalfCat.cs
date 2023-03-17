using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public enum SlotCondition { Expanded, Shrinked }
    public enum IconCondition { Inactivated, Approved, Disapproved }
    public class O_HalfCat : MonoBehaviour
    {
        private IconCondition currentIconCondition = IconCondition.Inactivated;
        private Transform formerBody;
        private Transform behindBody;
        private float horiMoveDistance = 0.6f;
        private float expandTime = 0.3f;
        private float shrinkTime = 0.1f;
        private SpriteRenderer iconRight;
        private SpriteRenderer iconLeft;
        private float expandedFormerX;
        private float shrinkedFormerX;
        private float expandedBehindX;
        private float shrinkedBehindX;

        private SpriteMask catFormerMask;
        private SpriteMask catBehindMask;

        private void Start()
        {
            formerBody = transform.Find("Cat Former");
            behindBody = transform.Find("Cat Behind");
            iconRight = formerBody.Find("Icon Right").GetComponent<SpriteRenderer>();
            iconLeft = behindBody.Find("Icon Left").GetComponent<SpriteRenderer>();
            expandedFormerX = formerBody.position.x + horiMoveDistance;
            shrinkedFormerX = formerBody.position.x;
            expandedBehindX = behindBody.position.x - horiMoveDistance;
            shrinkedBehindX = behindBody.position.x;
            catFormerMask = transform.Find("Cat Former").GetComponentInChildren<SpriteMask>();
            catBehindMask = transform.Find("Cat Behind").GetComponentInChildren<SpriteMask>();
            //UpdateMaskState(false);
        }

        public void CatSlotChangeTo(SlotCondition targetState)
        {
            switch (targetState)
            {
                case SlotCondition.Expanded:
                    if (currentIconCondition == IconCondition.Approved)
                    {
                        formerBody.DOMoveX(expandedFormerX, expandTime);
                        behindBody.DOMoveX(expandedBehindX, expandTime);
                    }
                    break;
                case SlotCondition.Shrinked:
                    formerBody.DOMoveX(shrinkedFormerX, shrinkTime);
                    behindBody.DOMoveX(shrinkedBehindX, shrinkTime);
                    break;
            }
        }

        public void CatIconChangeTo(IconCondition targetState)
        {
            switch (targetState)
            {
                case IconCondition.Inactivated:
                    DOTween.To(() => iconRight.color, x => iconRight.color = x, Color.black, 0.3f);
                    DOTween.To(() => iconLeft.color, x => iconLeft.color = x, Color.black, 0.3f);
                    currentIconCondition = IconCondition.Inactivated;
                    break;
                case IconCondition.Approved:
                    DOTween.To(() => iconRight.color, x => iconRight.color = x, Color.cyan, 0.3f);
                    DOTween.To(() => iconLeft.color, x => iconLeft.color = x, Color.cyan, 0.3f);
                    currentIconCondition = IconCondition.Approved;
                    break;
                case IconCondition.Disapproved:
                    DOTween.To(() => iconRight.color, x => iconRight.color = x, Color.red, 0.3f);
                    DOTween.To(() => iconLeft.color, x => iconLeft.color = x, Color.red, 0.3f);
                    currentIconCondition = IconCondition.Disapproved;
                    break;
            }
        }

        public void UpdateMaskState(bool state)
        {
            catBehindMask.enabled = state;
            catFormerMask.enabled = state;
        }
    }
}

