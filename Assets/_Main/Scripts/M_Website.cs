using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class M_Website : MonoBehaviour
    {
        [SerializeField] private CanvasGroup ui_ShowcaseGroup;
        [SerializeField] private Transform obj_Logo;
        [SerializeField] private Transform ui_ShowcaseParent;
        private List<Transform> products = new List<Transform>();

        private void Start()
        {
            for (int i = 0; i < ui_ShowcaseParent.childCount; i++)
            {
                products.Add(ui_ShowcaseParent.GetChild(i));
            }
        }

        public void OpenWeb()
        {
            for (int i = 0; i < products.Count; i++)
            {
                TMP_Text t_Name = products[i].Find("T_Name").GetComponent<TMP_Text>();
                TMP_Text t_UserReview = products[i].Find("I_Game").Find("BG_Review").Find("T_User Review").GetComponent<TMP_Text>();
                TMP_Text t_ReleaseDate = products[i].Find("T_Release Date").GetComponent<TMP_Text>();
                Image i_Game = products[i].Find("I_Game").GetComponent<Image>();


                if (i < M_Global.instance.mainData.productShowcases.Length && M_Global.instance.mainData.productShowcases[i].productLevel != ProductLevel.None)
                {
                    Product currentProduct = GetProductInfo(M_Global.instance.mainData.productShowcases[i].levelType, M_Global.instance.mainData.productShowcases[i].productLevel);
                    t_Name.text = currentProduct.productName;
                    t_UserReview.text = M_Global.instance.mainData.productShowcases[i].useReviews;
                    t_ReleaseDate.text = "Release Date: " + M_Global.instance.mainData.productShowcases[i].producedDate;
                    i_Game.sprite = currentProduct.productImage;
                }
                else
                {
                    t_Name.text = "Unproduced";
                    t_UserReview.text = "None Review";
                    t_ReleaseDate.text = "Release Date: ----.--.--";
                    i_Game.sprite = null;
                }
            }
            DOTween.To(() => ui_ShowcaseGroup.alpha, x => ui_ShowcaseGroup.alpha = x, 1, 1f);
        }

        Product GetProductInfo(LevelType targetGameType, ProductLevel targetProductLevel)
        {
            SO_Level targetGame = null;
            foreach (SO_Level level in M_Global.instance.levels)
                if (level.levelType == targetGameType) targetGame = level;

            Product targetProductInfo = null;
            foreach (Product product in targetGame.productLevels)
                if (product.productLevel == targetProductLevel)
                    targetProductInfo = product;
            return targetProductInfo;
        }
    }
}
