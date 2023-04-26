using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace IGDF
{ 
    public class O_Button : MonoBehaviour
    {
        public enum ButtonType 
        { 
            StartGame, ExitGame, Credits, 
            CabinBetweenStudioSkill, CabinBetweenStudioWebsite ,BackToOverview,
            EnterVivarium,EnterWebsite,OpenSettingPanel,ExitRoom,
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
            if (isClickable)
            {
                switch (buttonType)
                {
                    case ButtonType.StartGame:
                        StartGame();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.ExitGame:
                        ExitGame();
                        transform.DORotate(new Vector3(0, 0, 0), 0.4f);
                        break;
                    case ButtonType.Credits:
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
                //RoadBaseButtonColorChangeTo(Color.white);
            }
        }

        private void OnMouseEnter()
        {
            if (isClickable)
                if (buttonType == ButtonType.StartGame || buttonType == ButtonType.ExitGame|| buttonType == ButtonType.Credits|| buttonType == ButtonType.OpenSettingPanel)
                transform.DORotate(new Vector3(0, 0, Random.Range(10, 15)), 0.4f);
        }

        private void OnMouseExit()
        {
            if (isClickable)
                if (buttonType == ButtonType.StartGame || buttonType == ButtonType.ExitGame || buttonType == ButtonType.Credits || buttonType == ButtonType.OpenSettingPanel) 
                    transform.DORotate(new Vector3(0, 0, 0), 0.4f);
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
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
         
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.2f);
            s.AppendCallback(() => ResetRoomPosition());
            s.AppendInterval(0.1f);
            s.AppendCallback(() => M_Vivarium.instance.InitializeVivarium());
            s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());

            void ResetRoomPosition()
            {
                FindObjectOfType<M_Vivarium>().transform.position = new Vector3(-32, 0, 0);
            }
        }

        public void EnterWebsite()
        {
            SceneManager.LoadScene(3, LoadSceneMode.Additive);

            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.2f);
            s.AppendCallback(() => ResetRoomPosition());
            s.AppendCallback(() => M_WebsiteRoom.instance.InitializeWebsiteRoom());
            s.AppendCallback(() => FindObjectOfType<M_SceneTransition>().EnterCurrentCabin());

            void ResetRoomPosition()
            {
                FindObjectOfType<M_WebsiteRoom>().transform.position = new Vector3(32, 0, 0);
            }
        }

        void OpenSettingPanel()
        {
            FindObjectOfType<M_Setting>().OpenSettingPanel();
        }
    }
}