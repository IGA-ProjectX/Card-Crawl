using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Psychoflow.SSWaterReflection2D;
using System;

namespace IGDF
{
    public class M_SceneTransition : MonoBehaviour
    {
        public static M_SceneTransition instance;
        public enum CabinView { Overview, Studio, Skill, Website, InStudio, InSkill, InWebsite }
        [HideInInspector]public CabinView currentView = CabinView.Overview;
        public Transform[] cars;
        private float transitionTime = 2;
        public Transform windContainer;
        public GameObject p_ResultFail;
        public GameObject p_ResultSteam;
        public GameObject p_Pause;

        public Action ToOverView;
        public Action ToCabinView;
        public Action<CabinView> ToRoom;

        private void Awake()
        {
            instance = this;
        }

        public void EnterCabinView(CabinView targetCabin)
        {
            FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.Train);
      
            switch (targetCabin)
            {
                case CabinView.Overview:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 50f, transitionTime);
                    WindDepthChange("Overview");
                    FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.Overview);
                    ToOverView();
                    break;
                case CabinView.Studio:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(0, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    currentView = CabinView.Studio;
                    Debug.Log(currentView);
                    break;
                case CabinView.Skill:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(-32f, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Skill;
                    Debug.Log(currentView);
                    break;
                case CabinView.Website:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(32f, transitionTime);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Website;
                    break;
            }
            if (targetCabin != CabinView.Overview) ToCabinView();
        }

        public void EnterCurrentCabin()
        {
            FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.InCar);
            ReturnOverviewScaleChangeTo(0);
            DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 5.1f, transitionTime);
            WindDepthChange("InRoom");
            M_Audio.PlayFixedTimeSound(SoundType.ShutterDoor, transitionTime);
            switch (currentView)
            {
                case CabinView.InStudio:
                    cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y + 12, transitionTime);
                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(transitionTime);
                    s.AppendCallback(() => M_Main.instance.GameStart());
                    s.AppendCallback(() => SetInStudioPanelState(true));
                    s.AppendCallback(() => MaxmizeOpenSettingInStudio());
                    //currentView = CabinView.InStudio;
                    M_Audio.PlaySceneMusic(CabinView.InStudio);
                    break;
                case CabinView.InSkill:
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y + 12, transitionTime);
                    Sequence ss = DOTween.Sequence();
                    ss.AppendInterval(transitionTime);
                    ss.AppendCallback(() =>M_Vivarium.instance.AliveTheScene());
                    ss.AppendCallback(() => MaxmizeInRoomExitButton());
                    //currentView = CabinView.InSkill;
                    Debug.Log(currentView);
                    break;
                case CabinView.InWebsite:
                    cars[2].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y + 12, transitionTime);
                    //currentView = CabinView.InWebsite;
                    Sequence ws = DOTween.Sequence();
                    ws.AppendInterval(transitionTime);
                    ws.AppendCallback(() => FindObjectOfType<M_WebsiteRoom>().WebsiteScaleUp());
                    ws.AppendCallback(() => WebsiteSceneCullingMaskChangeTo(currentView));
                    ws.AppendInterval(1);
                    ws.AppendCallback(() => MaxmizeInRoomExitButton());
                    M_Audio.PlaySceneMusic(CabinView.InWebsite);
                    break;
            }
            ToRoom(currentView);
        }

        public void ExitCurrentCabin()
        {
            FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.Train);
            BackToCabinView();
            WindDepthChange("Car");
            M_Audio.PlayFixedTimeSound(SoundType.ShutterDoor, transitionTime);
            switch (currentView)
            {
                case CabinView.InStudio:
                    FindObjectOfType<M_HoverTip>().EnterState(HoverState.AllDisactive);
                    MinimizeOpenSettingInStudio();
                    Transform panelToScaleDown;
                    if (M_Main.instance.isGameFinished) panelToScaleDown = GameObject.Find("Canvas").transform.Find("Result Steam");
                    else panelToScaleDown = GameObject.Find("Canvas").transform.Find("Result Fail");
                    Sequence ss = DOTween.Sequence();
                    ss.Append(panelToScaleDown.DOScale(0, 0.4f));
                    ss.AppendCallback(() => SetInStudioPanelState(false));
                    ss.Append(cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y - 12, transitionTime));
                    ss.AppendCallback(() => M_Main.instance.DestroyAllChatbubble());
                    ss.AppendCallback(() => SceneManager.UnloadSceneAsync(1));
                    ss.AppendCallback(()=> currentView = CabinView.Studio);
                    break;
                case CabinView.InSkill:
                    Sequence s = DOTween.Sequence();
                    s.AppendCallback(() =>
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y - 12, transitionTime));
                    s.AppendInterval(transitionTime);
                    s.AppendCallback(() => SceneManager.UnloadSceneAsync(2));
                    s.AppendCallback(() => currentView = CabinView.Skill);
                    break;
                case CabinView.InWebsite:
                    Sequence sw = DOTween.Sequence();
                    sw.AppendCallback(() => FindObjectOfType<M_Website>().CloseWeb());
                    sw.AppendInterval(0.5f);
                    sw.Append(cars[2].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y - 12, transitionTime));
                    sw.AppendInterval(transitionTime);
                    sw.AppendCallback(() => SceneManager.UnloadSceneAsync(3));
                    sw.AppendCallback(() => currentView = CabinView.Website);
                    sw.AppendCallback(() => WebsiteSceneCullingMaskChangeTo(currentView));
                    break;
            }
            M_Audio.PlaySceneMusic(CabinView.Overview);
            ToCabinView();
        }

        public void WindDepthChange(string view)
        {
            float windScale = 0;
            float windYpos = 0;
            if (view == "Car")
            {
                windScale = 0.55f;
                windYpos = -0.5f;
            }
            else if(view == "InRoom")
            {
                windScale = 0.15f;
                windYpos = -0.42f;
            }
            else if(view == "Overview")
            {
                windScale = 1;
                windYpos = -0.7f;
            }
            windContainer.DOMoveY(windYpos, transitionTime);
            windContainer.DOScale(windScale, transitionTime);
        }

        public void BackToOverview()
        {
            EnterCabinView(CabinView.Overview);
            ReturnOverviewScaleChangeTo(0);
            FindObjectOfType<M_Level>().CloseLevelSelectionPanel();
            Camera.main.transform.DOMoveX(0, transitionTime);
        }

        public void BackToCabinView()
        {
            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime));
            s.AppendInterval(transitionTime);
            s.AppendCallback(() => ReturnOverviewScaleChangeTo(1));
        }

        void ReturnOverviewScaleChangeTo(float targetScale)
        {
            if (targetScale == 1)
            {
                if (currentView == CabinView.InStudio || currentView == CabinView.InSkill || currentView == CabinView.InWebsite)
                {

                }
                else GameObject.Find("Canvas").transform.Find("B_ReturnToOverview").DOScale(targetScale, 0.4f);
            }
            else GameObject.Find("Canvas").transform.Find("B_ReturnToOverview").DOScale(targetScale, 0.4f);
        }

        void SetInStudioPanelState(bool targetState)
        {
            p_ResultSteam.SetActive(targetState);
            p_ResultFail.SetActive(targetState);
            p_Pause.SetActive(targetState);
        }

        void WebsiteSceneCullingMaskChangeTo(CabinView cabinView)
        {
            if (cabinView == CabinView.InWebsite) Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("AboveSign"));
            else Camera.main.cullingMask = LayerMask.NameToLayer("Everything");
        }

        public void MinimizeInRoomExitButton()
        {
            GameObject.Find("Canvas").transform.Find("B_ReturnToCabin").DOScale(0, 0.4f);
            ExitCurrentCabin();
        }

        void MaxmizeInRoomExitButton()
        {
            GameObject.Find("Canvas").transform.Find("B_ReturnToCabin").DOScale(1, 0.4f);
        }

        public void MinimizeOpenSettingInStudio()
        {
            GameObject.Find("Canvas").transform.Find("B_OpenSettingInStudio").DOScale(0, 0.4f);
        }

        public void MaxmizeOpenSettingInStudio()
        {
            GameObject.Find("Canvas").transform.Find("B_OpenSettingInStudio").DOScale(1, 0.4f);
        }
    }
}
