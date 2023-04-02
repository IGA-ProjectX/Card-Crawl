using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF {

    public enum HoverState { AllDisactive, AllActive, CardDragging, SkillTargeting, CardDrawing }
    public class M_HoverTip : MonoBehaviour
    {
        private List<O_HoverTip> hoverTips = new List<O_HoverTip>();
        private HoverState currentState;

        public void EnterState(HoverState targetState)
        {
            currentState = targetState;
            switch (targetState)
            {
                case HoverState.AllDisactive:
                    foreach (O_HoverTip tip in hoverTips) tip.ChangeAllowOpenState(false, false);
                    break;
                case HoverState.AllActive:
                    foreach (O_HoverTip tip in hoverTips) tip.ChangeAllowOpenState(true, true);
                    break;
                case HoverState.CardDragging:
                    foreach (O_HoverTip tip in hoverTips)
                    {
                        if (tip.tipType == HoverTipType.Character) tip.ChangeAllowOpenState(false, false);
                        else tip.ChangeAllowOpenState(false, false);
                    }
                    break;
                case HoverState.SkillTargeting:
                    foreach (O_HoverTip tip in hoverTips)
                    {
                        if (tip.tipType == HoverTipType.Card) tip.ChangeAllowOpenState(false, true);
                        else tip.ChangeAllowOpenState(false, false);
                    }
                    break;
                case HoverState.CardDrawing:
                    foreach (O_HoverTip tip in hoverTips)
                    {
                        if (tip.tipType == HoverTipType.Card) tip.ChangeAllowOpenState(false, false);
                        else tip.ChangeAllowOpenState(true, true);
                    }
                    break;
            }
        }

        public void HoverTipListAddOrRemove(O_HoverTip targetObj,bool isAdd)
        {
            if (isAdd) hoverTips.Add(targetObj);
            else hoverTips.Remove(targetObj);
        }

        public HoverState GetCurrentState()
        {
            return currentState;
        }
    }
}