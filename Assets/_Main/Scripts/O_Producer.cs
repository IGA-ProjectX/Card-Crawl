using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace IGDF
{
    public class O_Producer : O_Character
    {
        public enum ProducerState { OutScreen,InScreenIdle,Walking}
        private ProducerState currentState = ProducerState.OutScreen;
        public float outScreenTime;
        public float inScreenTime;
        private float outScreenTimer;
        private float inScreenTimer;
        private bool isLeftWalkIn = false;

        // Start is called before the first frame update
        void Start()
        {
            EnterOutScreenIdle();
        }

        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case ProducerState.OutScreen:
                    outScreenTimer -= Time.deltaTime;
                    break;
                case ProducerState.InScreenIdle:
                    inScreenTimer -= Time.deltaTime;
                    break;
            }

            if (outScreenTimer <= 0 && currentState == ProducerState.OutScreen) EnterWalkIn();

            if (inScreenTimer <= 0 && currentState == ProducerState.InScreenIdle) EnterLeave();
        }

        private float GetRestTime()
        {
            float restTime = Random.Range(outScreenTime - 1.5f, outScreenTime + 1.5f);
            return restTime;
        }

        private float GetObserveTime()
        {
            float restTime = Random.Range(inScreenTime - 1.5f, inScreenTime + 1.5f);
            return restTime;
        }

        void EnterWalkIn()
        {
            currentState = ProducerState.Walking;
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0) isLeftWalkIn = true;
            else isLeftWalkIn = false;

            if (isLeftWalkIn) WalkIn(-11, -5, 1);
            else WalkIn(11, 5, -1);

            void WalkIn(float start,float end,int flipper)
            {
                transform.localScale = new Vector3(flipper, 1, 1);
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() => transform.position = new Vector3(start, transform.position.y, transform.position.z));
                s.Append(transform.DOMoveX(end, 5f));
                s.AppendCallback(() => EnterInScreenIdle());
            }
        }

        void EnterInScreenIdle()
        {
            inScreenTimer = GetObserveTime();
            currentState = ProducerState.InScreenIdle;
        }

        void EnterLeave()
        {
            currentState = ProducerState.Walking;
            if (isLeftWalkIn) Leave(-11, -1);
            else Leave(11, 1);

            void Leave( float end, int flipper)
            {
                transform.localScale = new Vector3(flipper, 1, 1);
                Sequence s = DOTween.Sequence();
                s.Append(transform.DOMoveX(end, 5f));
                s.AppendCallback(() => EnterOutScreenIdle());
            }
        }

        void EnterOutScreenIdle()
        {
            outScreenTimer = GetRestTime();
            currentState = ProducerState.OutScreen;
        }
    }
}

