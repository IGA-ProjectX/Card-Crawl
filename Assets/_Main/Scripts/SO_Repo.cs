using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGDF
{
    [CreateAssetMenu(fileName = "New Repo", menuName = "IGDF/New Repo")]
    public class SO_Repo : ScriptableObject
    {
        public Sprite[] cardTypeIcons;
        public Color[] chaColors;
        public Sprite[] stamps;
        public Color[] stampColors;
        public Sprite[] cardBGImages;
    }
}
