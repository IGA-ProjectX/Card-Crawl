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
        public enum CabinView { Overview,Studio,Skill,Website,InStudio,InSkill,InWebsite }
        [HideInInspector]public CabinView currentView = CabinView.Overview;
        public Transform[] cars;
        private float transitionTime = 2;
        public Transform windContainer;

        public void EnterCabinView(CabinView targetCabin)
        {
            switch (targetCabin)
            {
                case CabinView.Overview:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 50f, transitionTime);
                    WindDepthChange("Overview");
                    break;
                case CabinView.Studio:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(0, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(Vector3.zero, transitionTime/2);
                    currentView = CabinView.Studio;
                    break;
                case CabinView.Skill:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(-32f, transitionTime);
                    transform.Find("Train").Find("Plat_SS").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.Website:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    WindDepthChange("Car");
                    Camera.main.transform.DOMoveX(32f, transitionTime);
                    transform.Find("Train").Find("Plat_SW").Find("Button").DORotate(new Vector3(0, 0, 180), transitionTime/2);
                    currentView = CabinView.Website;
                    break;
            }
        }

        public void EnterCurrentCabin()
        {
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
            DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
            WindDepthChange("Car");
            M_Audio.PlayFixedTimeSound(SoundType.ShutterDoor, transitionTime);
            switch (currentView)
            {
                case CabinView.InStudio:
                    Sequence s = DOTween.Sequence();
                    s.Append(GameObject.Find("Canvas").transform.Find("Result Steam").DOScale(0, 0.4f));
                    s.Append(cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y - 12, transitionTime));
                    s.AppendCallback(() => FindObjectOfType<M_Level>().RemoveExistingStudioScene());
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
    }
}
