using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{ 
    public class O_Button : MonoBehaviour
    {
        public enum ButtonType 
        { 
            StartGame, ExitGame, Credits, 
            CabinBetweenStudioSkill, CabinBetweenStudioWebsite ,BackToOverview,
            EnterRoom,OpenSettingPanel,ExitRoom,
            CallOutLevelList
        }
        public ButtonType buttonType;
        private M_SceneTransition m_SceneTransition;
        [HideInInspector]public bool isClickable;

        void Start()
        {
            m_SceneTransition = FindObjectOfType<M_SceneTransition>();
        }

        private void OnMouseDown()
        {
            if(isClickable)
            switch (buttonType)
            {
                case ButtonType.StartGame:
                    StartGame();
                    break;
                case ButtonType.ExitGame:
                    ExitGame();
                    break;
                case ButtonType.Credits:
                    break;
                case ButtonType.CabinBetweenStudioSkill:
                    CabinBetweenStudioSkill();
                    break;
                case ButtonType.CabinBetweenStudioWebsite:
                    CabinBetweenStudioWebsite();
                    break;
                case ButtonType.BackToOverview:
                    BackToOverview();
                    break;
                case ButtonType.EnterRoom:
                    EnterRoom();
                    break;
                case ButtonType.OpenSettingPanel:
                    OpenSettingPanel();
                    break;
                case ButtonType.ExitRoom:
                    m_SceneTransition.ExitCurrentCabin();
                    break;
                case ButtonType.CallOutLevelList:
                    GameObject.Find("Canvas").transform.Find("Level Selection").DOScale(1, 0.4f);
                    break;
                default:
                    break;
            }
        }

        void StartGame()
        {
            m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Studio);
        }

        void ExitGame()
        {
            Application.Quit();
        }

        void CabinBetweenStudioSkill()
        {
            if (m_SceneTransition.currentView == M_SceneTransition.CabinView.Studio)
            {
                m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Skill);
            }
            else
            {
                m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Studio);
            }
          
        }

        void CabinBetweenStudioWebsite()
        {
            if (m_SceneTransition.currentView == M_SceneTransition.CabinView.Website)
            {
                m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Studio);
            }
            else
            {
                m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Website);
            }
        }

        void BackToOverview()
        {
            m_SceneTransition.EnterCabinView(M_SceneTransition.CabinView.Overview);
        }

        public void EnterRoom()
        {
            m_SceneTransition.EnterCurrentCabin();
        }

        void OpenSettingPanel()
        {
            GameObject.Find("Canvas").transform.Find("Setting Panel").DOScale(Vector3.one, 1);
            FindObjectOfType<M_Setting>().UpdateCurrentExp();
        }
    }
}