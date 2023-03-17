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
        public LayerMask mask_WaterScene;
        public LayerMask mask_RoomScene;
        private float transitionTime = 2;
        private float carDoorDefaultY;

        private void Start()
        {
            carDoorDefaultY = cars[0].Find("Car Door").position.y;
        }

        public void EnterCabinView(CabinView targetCabin)
        {
            switch (targetCabin)
            {
                case CabinView.Overview:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 50f, transitionTime);
                    break;
                case CabinView.Studio:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    Camera.main.transform.DOMoveX(0, transitionTime);
                    currentView = CabinView.Studio;
                    break;
                case CabinView.Skill:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    Camera.main.transform.DOMoveX(-33, transitionTime);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.Website:
                    DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, transitionTime);
                    Camera.main.transform.DOMoveX(33, transitionTime);
                    currentView = CabinView.Website;
                    break;
            }
        }

        public void EnterCurrentCabin()
        {
            DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 5.1f, transitionTime);
            switch (currentView)
            {
                case CabinView.Studio:
                    cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y + 12, transitionTime);
                    Sequence s = DOTween.Sequence();
                    s.AppendInterval(transitionTime);
                    //s.AppendCallback(() => TransitToRoomState());
                    s.AppendCallback(() => M_Main.instance.GameStart());
                    currentView = CabinView.InStudio;
                    break;
                case CabinView.Skill:
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y + 12, transitionTime);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.Website:
                    cars[1].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y + 12, transitionTime);
                    currentView = CabinView.Website;
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
            switch (currentView)
            {
                case CabinView.InStudio:
                    cars[0].Find("Car Door").DOMoveY(cars[0].Find("Car Door").position.y - 12, transitionTime);
                    currentView = CabinView.Studio;
                    break;
                case CabinView.InSkill:
                    cars[1].Find("Car Door").DOMoveY(cars[1].Find("Car Door").position.y - 12, transitionTime);
                    currentView = CabinView.Skill;
                    break;
                case CabinView.InWebsite:
                    cars[2].Find("Car Door").DOMoveY(cars[2].Find("Car Door").position.y - 12, transitionTime);
                    currentView = CabinView.Website;
                    break;
            }
        }
    }
}
