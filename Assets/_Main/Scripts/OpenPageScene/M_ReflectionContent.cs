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
        public float cloudSpawnSpeed;
        private float cloudSpawnTimer;

        private List<Transform> carWheels = new List<Transform>();
        public float wheelSpeed;

        void Start()
        {
            for (int i = 0; i < roadSignSpawnTimers.Length; i++)
                roadSignSpawnTimers[i] = Random.Range(roadSignAttribute[i].spawnSpeed * 0.7f, roadSignAttribute[i].spawnSpeed * 1.3f);
            cloudSpawnTimer = Random.Range(cloudSpawnSpeed * 0.7f, cloudSpawnSpeed * 1.3f);
            for (int i = 1; i < 6; i++)
            {
                carWheels.Add(GameObject.Find("Car").transform.GetChild(i).transform);
            }
        }

        void Update()
        {
            for (int i = 0; i < roadSignSpawnTimers.Length; i++)
            {
                roadSignSpawnTimers[i] -= Time.deltaTime;
                if (roadSignSpawnTimers[i] < 0) InstantiateRoadSignInRow(i);
            }
            cloudSpawnTimer -= Time.deltaTime;
            if (cloudSpawnTimer < 0) InstantiateCloud();
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

        void InstantiateCloud()
        {
            cloudSpawnTimer = Random.Range(cloudSpawnSpeed * 0.7f, cloudSpawnSpeed * 1.3f);
            int randomSpriteIndex = Random.Range(0, clouds.Length);
            Transform cloud = Instantiate(pre_Cloud, roadSignAttribute[0].pivot.position, Quaternion.identity, objContainer).transform;
            cloud.GetComponent<SpriteRenderer>().sprite = clouds[randomSpriteIndex];
            cloud.GetComponent<SpriteRenderer>().sortingOrder = -10;

            float randomScale = Random.Range(7f, 10f);
            cloud.localScale = new Vector2(randomScale, randomScale);
            cloud.DOMoveX(endPosX, cloudMoveSpeed);
            Destroy(cloud.gameObject, cloudMoveSpeed + 2);
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