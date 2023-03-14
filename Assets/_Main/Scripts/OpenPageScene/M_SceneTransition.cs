using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace IGDF
{
    public class M_SceneTransition : MonoBehaviour
    {
        public Transform carDoor;
        private bool isMoved = false;

        // Start is called before the first frame update
        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            //SceneManager.LoadScene("Studio", LoadSceneMode.Additive);
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    SceneManager.LoadScene(5);
            //}
            if (Input.GetKeyDown(KeyCode.V)) CameraMoveIn();
        }

        void CameraMoveIn()
        {
            if (isMoved)
            {
                DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 5.1f, 2f);
                carDoor.DOMoveY(carDoor.position.y + 12, 2f);
                Sequence s = DOTween.Sequence();
                s.AppendInterval(2);
                s.AppendCallback(() => M_Main.instance.GameStart());
                //s.app
            }
            else
            {
                DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 11f, 2f);
                isMoved = true;
            }
        }
    }
}
