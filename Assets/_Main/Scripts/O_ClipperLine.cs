using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    public class O_ClipperLine : MonoBehaviour
    {
        private LineRenderer lineR;
        private Transform clipperTrans;
        [HideInInspector] public bool isLineAuto = true;
        private SpringJoint2D[] springs;
        private Rigidbody2D clipperRigid;
        private bool isClipperFollow = true;
        [HideInInspector] public Transform cardTrans;
        [HideInInspector]public bool isClipperInScreen = false;

        void Update()
        {
            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, clipperTrans.position);

            if (cardTrans == null && transform.parent.GetComponentInChildren<O_Card>()!=null)
            {  
                cardTrans = transform.parent.GetComponentInChildren<O_Card>().transform;
            }

            if (isClipperFollow && cardTrans != null)
            {
                //clipperTrans.position = cardTrans.position + new Vector3(0, 1.834f, 0);
                clipperTrans.position = cardTrans.position + new Vector3(0, 1.72f, 0);
            }
        }
         
        public void SetLineState(string lineState)
        {
            if (lineState == "Auto")
            {
                isClipperFollow = false;
                foreach (var spring in springs) spring.enabled = true;
                clipperRigid.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                isClipperFollow = true;
                foreach (var spring in springs) spring.enabled = false;
                clipperRigid.bodyType = RigidbodyType2D.Kinematic;
            }
        }

        public void InitializeClipperLine()
        {
            lineR = GetComponent<LineRenderer>();
            lineR.positionCount = 2;
            clipperTrans = transform.parent.Find("Clipper");
            springs = clipperTrans.GetComponents<SpringJoint2D>();
            clipperRigid = clipperTrans.GetComponent<Rigidbody2D>();
            SetLineState("Manuel");
            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, clipperTrans.position);
        }

        public void DestroySlider(float destroyTime)
        {
            Destroy(transform.parent.gameObject, destroyTime);
        }
    }
}