using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Psychoflow.SSWaterReflection2D;
using DG.Tweening;

namespace IGDF
{
    public class M_ReflectionContent : MonoBehaviour
    {
        public float endPosX;
        public Transform objContainer;
        [Header("RoadSign")]
        [SerializeField] Sprite[] roadSigns;
        [SerializeField] GameObject pre_RoadSign;
        public RoadSignAttribute[] roadSignAttribute;
        private float[] roadSignSpawnTimers = { 0, 0, 0, 0 };

        [Header("Cloud")]
        [SerializeField] Sprite[] clouds;
        [SerializeField] GameObject pre_Cloud;
        public float cloudMoveSpeed;

        private List<Transform> cloudTranses = new List<Transform>();
        private List<bool> cloudisMiddle = new List<bool>();

        private List<Transform> carWheels = new List<Transform>();
        public float wheelSpeed;

        void Start()
        {
            for (int i = 0; i < roadSignSpawnTimers.Length; i++)
                roadSignSpawnTimers[i] = Random.Range(roadSignAttribute[i].spawnSpeed * 0.7f, roadSignAttribute[i].spawnSpeed * 1.3f);
            cloudTranses.Add(transform.Find("Cloud Container").Find("Cloud"));
            cloudisMiddle.Add(false);
            for (int i = 1; i < 6; i++)
            {
                carWheels.Add(transform.Find("Train").Find("Cabin Studio").transform.GetChild(i).transform);
                carWheels.Add(transform.Find("Train").Find("Cabin Skill").transform.GetChild(i).transform);
                carWheels.Add(transform.Find("Train").Find("Cabin Website").transform.GetChild(i).transform);
            }
        }

        void Update()
        {
            for (int i = 0; i < roadSignSpawnTimers.Length; i++)
            {
                roadSignSpawnTimers[i] -= Time.deltaTime;
                if (roadSignSpawnTimers[i] < 0) InstantiateRoadSignInRow(i);
            }

            MoveClouds();

            foreach (Transform wheel in carWheels)
            {
                wheel.RotateAround(wheel.position, Vector3.forward, wheelSpeed);
            }
        }

        void InstantiateRoadSignInRow(int spawnRow)
        {
            roadSignSpawnTimers[spawnRow] = Random.Range(roadSignAttribute[spawnRow].spawnSpeed * 0.7f, roadSignAttribute[spawnRow].spawnSpeed * 1.3f);
            int randomSpriteIndex = Random.Range(0, roadSigns.Length);
            Transform roadSign = Instantiate(pre_RoadSign, roadSignAttribute[spawnRow].pivot.position, Quaternion.identity, objContainer).transform;
            roadSign.GetComponent<SpriteRenderer>().sprite = roadSigns[randomSpriteIndex];
            roadSign.GetComponent<SpriteRenderer>().sortingOrder = roadSignAttribute[spawnRow].layer;
            roadSign.localScale = new Vector2(roadSignAttribute[spawnRow].scale, roadSignAttribute[spawnRow].scale);

            if (spawnRow < 2)
            {
                roadSign.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                roadSign.GetComponent<SpriteSelfWaterReflection>().ReflectionMaskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            }
            else
            {
                roadSign.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                roadSign.GetComponent<SpriteSelfWaterReflection>().ReflectionMaskInteraction = SpriteMaskInteraction.None;
            }

            roadSign.GetComponent<SpriteSelfWaterReflection>().ClearAndInitReflectionRenderers();
            roadSign.GetComponent<SpriteSelfWaterReflection>().ReflectionProvider = FindObjectOfType<SSWaterReflectionProvider>();
            roadSign.DOMoveX(endPosX, roadSignAttribute[spawnRow].moveSpeed);
            Destroy(roadSign.gameObject, roadSignAttribute[spawnRow].moveSpeed + 2);
        }

        void MoveClouds()
        {
            for (int i = 0; i < cloudTranses.Count; i++)
            {
                if (cloudTranses[i].localPosition.x >= 0 && !cloudisMiddle[i])
                {
                    int randomSpriteIndex = Random.Range(0, clouds.Length);
                    Transform cloud = Instantiate(pre_Cloud, transform.Find("Cloud Container").Find("Spawn Point")).transform;
                    cloud.SetParent(transform.Find("Cloud Container"));
                    cloud.GetComponent<SpriteRenderer>().sprite = clouds[randomSpriteIndex];
                    cloud.GetComponent<SpriteRenderer>().sortingOrder = -23;
                    cloud.GetComponent<SpriteSelfWaterReflection>().ReflectionProvider = transform.Find("Cloud Container").GetComponentInChildren<SSWaterReflectionProvider>();
                    cloudTranses.Add(cloud);
                    cloudisMiddle.Add(false);
                    cloudisMiddle[i] = true;
                }
                if (cloudTranses[i].localPosition.x >= transform.Find("Cloud Container").Find("End Point").localPosition.x)
                {
                    Destroy(cloudTranses[i].gameObject, 1);
                    cloudTranses.RemoveAt(i);
                    cloudisMiddle.RemoveAt(i);
                }
            }
            foreach (Transform cloudTrans in cloudTranses)
            {
                cloudTrans.position += new Vector3(cloudMoveSpeed * cloudTrans.parent.localScale.x * 0.01f, 0, 0);
            }
        }
    }

    [System.Serializable]
    public class RoadSignAttribute
    {
        public Transform pivot;
        public float scale;
        public int layer;
        public float spawnSpeed;
        public float moveSpeed;
    }
}