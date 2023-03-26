using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IGDF
{
    public class M_Button : MonoBehaviour
    {
        public enum ButtonState { Overview, Train, InCar }
        private List<O_Button> buttons = new List<O_Button>();

        private void Start()
        {
            foreach (O_Button button in FindObjectsOfType<O_Button>()) buttons.Add(button);
            EnterButtonState(ButtonState.Overview);
        }

        public void EnterButtonState(ButtonState targetState)
        {
            switch (targetState)
            {
                case ButtonState.Overview:
                    foreach (O_Button button in buttons)
                    {
                        switch (button.buttonType)
                        {
                            case O_Button.ButtonType.StartGame:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.ExitGame:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.Credits:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioSkill:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioWebsite:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.BackToOverview:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.EnterRoom:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.OpenSettingPanel:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.ExitRoom:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CallOutLevelList:
                                button.isClickable = false;
                                break;
                        }
                    }
                    break;
                case ButtonState.Train:
                    foreach (O_Button button in buttons)
                    {
                        switch (button.buttonType)
                        {
                            case O_Button.ButtonType.StartGame:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.ExitGame:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.Credits:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioSkill:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioWebsite:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.BackToOverview:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.EnterRoom:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.OpenSettingPanel:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.ExitRoom:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CallOutLevelList:
                                button.isClickable = true;
                                break;
                        }
                    }
                    break;
                case ButtonState.InCar:
                    foreach (O_Button button in buttons)
                    {
                        switch (button.buttonType)
                        {
                            case O_Button.ButtonType.StartGame:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.ExitGame:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.Credits:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioSkill:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.CabinBetweenStudioWebsite:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.BackToOverview:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.EnterRoom:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.OpenSettingPanel:
                                button.isClickable = false;
                                break;
                            case O_Button.ButtonType.ExitRoom:
                                button.isClickable = true;
                                break;
                            case O_Button.ButtonType.CallOutLevelList:
                                button.isClickable = false;
                                break;
                        }
                    }
                    break;
            }
        }

        public void ButtonListAddOrRemove(O_Button targetObj, bool isAdd)
        {
            if (isAdd) buttons.Add(targetObj);
            else buttons.Remove(targetObj);
        }
    }
}
