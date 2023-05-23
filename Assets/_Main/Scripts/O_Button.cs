using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

namespace IGDF
{ 
    public class O_Button : MonoBehaviour
    {
        public enum ButtonType 
        { 
            StartGame, ExitGame, Credits, 
            CabinBetweenStudioSkill, CabinBetweenStudioWebsite ,BackToOverview,
            EnterVivarium,EnterWebsite,OpenSettingPanel,ExitRoom,
            CallOutLevelList,
            CloseTutorial, PlayVideo
        }
        public ButtonType buttonType;
        private M_SceneTransition m_SceneTransition;
        [HideInInspector]public bool isClickable;

        public Action StartGameFunc;

        void Start()
        {
            m_SceneTransition = FindObjectOfType<M_SceneTransition>();
            StartGameFunc += FindObjectOfType<M_Setting>().ClickClose;
            StartGameFunc += FindObjectOfType<M_Setting>().ReturnCredits;
        }

        private void OnMouseDown()
        {
            if (isClickable)
            {
                switch (buttonType)
                {
                    case ButtonType.StartGame:
                        StartGame();
                        StartGameFunc();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.ExitGame:
                        ExitGame();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.Credits:
                        FindObjectOfType<M_Setting>().CallOutCredits();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.CabinBetweenStudioSkill:
                        CabinBetweenStudioSkill();
                        break;
                    case ButtonType.CabinBetweenStudioWebsite:
                        CabinBetweenStudioWebsite();
                        break;
                    case ButtonType.EnterVivarium:
                        EnterVivarium();
                        break;
                    case ButtonType.EnterWebsite:
                        EnterWebsite();
                        break;
                    case ButtonType.OpenSettingPanel:
                        OpenSettingPanel();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.ExitRoom:
                        m_SceneTransition.ExitCurrentCabin();
                        break;
                    case ButtonType.CallOutLevelList:
                        FindObjectOfType<M_Level>().OpenLevelSelectionPanel();
                        break;
                    default:
                        break;
                }
            }
            TutorialButtonClick();
        }

        private void OnMouseEnter()
        {
            if (isClickable)
                if (buttonType == ButtonType.StartGame || buttonType == ButtonType.ExitGame|| buttonType == ButtonType.Credits|| buttonType == ButtonType.OpenSettingPanel)
                transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(10, 15)), 0.4f);
            //TutorialButtonEnter();
        }

        private void OnMouseExit()
        {
            if (isClickable)
                if (buttonType == ButtonType.StartGame || buttonType == ButtonType.ExitGame || buttonType == ButtonType.Credits || buttonType == ButtonType.OpenSettingPanel) 
                    transform.DORotate(new Vector3(0, 0, 0), 0.4f);
            //TutorialButtonExit();
        }

        void RoadBaseButtonColorChangeTo(Color targetColor)
        {
            if (buttonType == ButtonType.StartGame || buttonType == ButtonType.ExitGame || buttonType == ButtonType.Credits || buttonType == ButtonType.OpenSettingPanel)
                DOTween.To(() => GetComponent<TMPro.TMP_Text>().color, x => GetComponent<TMPro.TMP_Text>().color = x, targetColor, 0.3f);
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

        public void EnterVivarium()
        {
            if (m_SceneTransition.currentView!= M_SceneTransition.CabinView.InSkill)
            {
                m_SceneTransition.currentView = M_SceneTransition.CabinView.InSkill;
                SceneManager.LoadScene(2, LoadSceneMode.Additive);

                Sequence s = DOTween.Sequence();
                s.AppendInterval(0.2f);
                s.AppendCallback(() => ResetRoomPosition());
                s.AppendInterval(0.1f);
                s.AppendCallback(() => M_Vivarium.instance.InitializeVivarium());
                s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());
            }


            void ResetRoomPosition()
            {
                FindObjectOfType<M_Vivarium>().transform.position = new Vector3(-32, 0, 0);
            }
        }

        public void EnterWebsite()
        {
            if (m_SceneTransition.currentView != M_SceneTransition.CabinView.InWebsite)
            {
                m_SceneTransition.currentView = M_SceneTransition.CabinView.InWebsite;
                SceneManager.LoadScene(3, LoadSceneMode.Additive);

                Sequence s = DOTween.Sequence();
                s.AppendInterval(0.2f);
                s.AppendCallback(() => ResetRoomPosition());
                s.AppendCallback(() => M_WebsiteRoom.instance.InitializeWebsiteRoom());
                s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());
            }
            void ResetRoomPosition()
            {
                FindObjectOfType<M_WebsiteRoom>().transform.position = new Vector3(32, 0, 0);
            }
        }

        void OpenSettingPanel()
        {
            FindObjectOfType<M_Setting>().OpenSettingPanel();
        }

        private void TutorialButtonEnter()
        {
            if (M_Tutorial.instance != null)
                if (buttonType == ButtonType.CloseTutorial || buttonType == ButtonType.PlayVideo)
                {
                    transform.DOMoveX(5, 0.5f);
                }
        }

        private void TutorialButtonClick()
        {
            if (M_Tutorial.instance != null)
                if (buttonType == ButtonType.CloseTutorial) M_Tutorial.instance.ExitTutorialState();
                else if (buttonType == ButtonType.PlayVideo) M_Tutorial.instance.PlayTutorialVideo();
        }

        private void TutorialButtonExit()
        {
            if (M_Tutorial.instance != null)
                if (buttonType == ButtonType.CloseTutorial || buttonType == ButtonType.PlayVideo)
                {
                    transform.DOMoveX(4.9f, 0.5f);
                }
        }
    }
}