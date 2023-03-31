using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Psychoflow.SSWaterReflection2D;

namespace IGDF
{
    public class M_SceneTransition : MonoBehaviour
    {
        public enum CabinView { Overview, Studio, Skill, Website, InStudio, InSkill, InWebsite }
        [HideInInspector]public CabinView currentView = CabinView.Overview;
        public Transform[] cars;
        private float transitionTime = 2;
        public Transform windContainer;
        public GameObject p_ResultFail;
        public GameObject p_ResultSteam;

        public void EnterCabinView(CabinView targetCabin)
        {
            FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.Train);
      
            switch (targetCabin)
            {
                case CabinView.Overview:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 50f, transitionTime);
                    WindDepthChange("Overview");
                    FindObjectOfType<M_Button>().EnterButtonState(M_Button.ButtonState.Overview);
                    break;
                case CabinView.Studio:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(0, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    currentView = CabinView.Studio;
                    break;
                case CabinView.Skill:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(-32f, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.Website:
                    BackToCabinView();
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(32f, transitionTime);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Website;
                    break;
            }
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
                case CabinView.Studio:
                    cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y + 12, transitionTime);
                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(transitionTime);
                    //s.AppendCallback(() => TransitToRoomState());
                    s.AppendCallback(() => M_Main.instance.GameStart());
                    s.AppendCallback(() => p_ResultSteam.SetActive(true));
                    s.AppendCallback(() => p_ResultFail.SetActive(true));
                    currentView = CabinView.InStudio;
                    M_Audio.PlaySceneMusic(CabinView.InStudio);
                    break;
                case CabinView.Skill:
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y + 12, transitionTime);
                    currentView = CabinView.InSkill;
                    break;
                case CabinView.Website:
                    cars[2].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y + 12, transitionTime);
                    currentView = CabinView.InWebsite;
                    Sequence ws = DOTween.Sequence();
                    ws.AppendInterval(transitionTime);
                    ws.AppendCallback(() => FindObjectOfType<M_Website>().OpenWeb());
                    M_Audio.PlaySceneMusic(CabinView.InWebsite);

                    break;
            }
            //void TransitToRoomState()
            //{
            //    cars[0].Find("Door Mask").GetComponent<SpriteMask>().enabled = false;
            //    //Camera.main.cullingMask = mask_WaterScene;
            //}
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
                    Transform panelToScaleDown;
                    if (M_Main.instance.isGameFinished) panelToScaleDown = GameObject.Find("Canvas").transform.Find("Result Steam");
                    else panelToScaleDown = GameObject.Find("Canvas").transform.Find("Result Fail");
                    Sequence ss = DOTween.Sequence();
                    ss.Append(panelToScaleDown.DOScale(0, 0.4f));
                    ss.AppendCallback(() => p_ResultSteam.SetActive(false));
                    ss.AppendCallback(() => p_ResultFail.SetActive(false));
                    ss.Append(cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y - 12, transitionTime));
                    ss.AppendCallback(() => SceneManager.UnloadSceneAsync(1));
                    currentView = CabinView.Studio;
                    break;
                case CabinView.InSkill:
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y - 12, transitionTime);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.InWebsite:
                    FindObjectOfType<M_Website>().CloseWeb();
                    cars[2].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y - 12, transitionTime);

                    currentView = CabinView.Website;
                    break;
            }
            M_Audio.PlaySceneMusic(CabinView.Overview);
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
            GameObject.Find("Canvas").transform.Find("B_ReturnToOverview").DOScale(targetScale, 0.4f);
        }
    }
}
