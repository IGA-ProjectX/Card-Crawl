using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Data", menuName = "IGDF/New Data")]
    public class SO_Data : ScriptableObject
    {
        public int playExp;
        public ProductShowcase[] productShowcases;
    }

    [System.Serializable]
    public class ProductShowcase
    {
        public LevelType levelType;
        public ProductLevel productLevel;
        public string producedDate;
        public string userReviewLevel;
        public string userReviewNumber;
    }

}
