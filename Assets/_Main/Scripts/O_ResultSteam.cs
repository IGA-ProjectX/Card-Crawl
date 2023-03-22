using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

namespace IGDF
{
    public class O_ResultSteam : MonoBehaviour
    {
        Image i_Game;
        Image i_Review;
        TMP_Text t_Name;
        TMP_Text t_Summary;
        TMP_Text t_Date;
        TMP_Text t_ReviewLevel;
        TMP_Text t_ReviewNumber;
        List<TMP_Text> userTags = new List<TMP_Text>();
        List<Slider> sliders = new List<Slider>();

        void Start()
        {
            i_Game = transform.Find("I_Game").GetComponent<Image>();
            i_Review = transform.Find("I_Review").GetComponent<Image>();
            t_Name = transform.Find("T_Name").GetComponent<TMP_Text>();
            t_Summary = transform.Find("T_Summary").GetComponent<TMP_Text>();
            t_Date = transform.Find("T_Release Date").GetComponent<TMP_Text>();
            t_ReviewLevel = transform.Find("I_Review").Find("Layout Control").Find("T_Review Level").GetComponent<TMP_Text>();
            t_ReviewNumber = transform.Find("I_Review").Find("Layout Control").Find("T_Review Number").GetComponent<TMP_Text>();
            foreach (ContentSizeFitter userTagChild in transform.Find("User Tags").GetComponentsInChildren<ContentSizeFitter>())
                userTags.Add(userTagChild.GetComponentInChildren<TMP_Text>());
            sliders.Add(transform.Find("S_Name").GetComponent<Slider>());
            sliders.Add(transform.Find("S_Sum").GetComponent<Slider>());
            sliders.Add(transform.Find("S_Release Date").GetComponent<Slider>());
        }

        private void Update()
        {
            
        }

        public void GameProduced()
        {
            LevelType currentLevelType = M_Global.instance.levels[M_Global.instance.targetLevel].levelType;
            int maxDDL = M_Global.instance.levels[M_Global.instance.targetLevel].staffValue[4];

            if (M_Main.instance.m_Staff.GetDDLValue() == maxDDL)
            {
                ProductStateUpdate(GetProductShowcase(currentLevelType), ProductLevel.Welldone);
            }
            else if (M_Main.instance.m_Staff.GetDDLValue() < maxDDL && M_Main.instance.m_Staff.GetDDLValue() >= 0.66 * maxDDL)
            {
                ProductStateUpdate(GetProductShowcase(currentLevelType), ProductLevel.Medium);
            }
            else if (M_Main.instance.m_Staff.GetDDLValue() < 0.66 * maxDDL)
            {
                ProductStateUpdate(GetProductShowcase(currentLevelType), ProductLevel.Raw);
            }
        }

        ProductShowcase GetProductShowcase(LevelType currentLevelType)
        {
            ProductShowcase onShowProduct = null;
            foreach (ProductShowcase product in M_Global.instance.mainData.productShowcases)
                if (product.levelType == currentLevelType)
                    onShowProduct = product;
            return onShowProduct;
        }

        void ProductStateUpdate(ProductShowcase toChangeProduct, ProductLevel toChangeLevel)
        {
            ProductLevel currentLevel = toChangeProduct.productLevel;
            switch (toChangeLevel)
            {
                case ProductLevel.None:
                    break;
                case ProductLevel.Raw:
                    if (currentLevel == ProductLevel.None)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Raw);
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel),toChangeProduct);
                    }
                    M_Audio.PlaySound(SoundType.ProducedRare);
                    break;
                case ProductLevel.Medium:
                    if (currentLevel != ProductLevel.Welldone && currentLevel != ProductLevel.Medium)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Medium);
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel),toChangeProduct);
                    }
                    M_Audio.PlaySound(SoundType.ProducedMedium);
                    break;
                case ProductLevel.Welldone:
                    if (currentLevel != ProductLevel.Welldone)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Welldone);
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel),toChangeProduct);
                    }
                    M_Audio.PlaySound(SoundType.ProducedWelldone);
                    break;
            }
            ObjPopOut(transform, 1);
        }

        void ProductUpgrade(ProductShowcase toChangeProduct, ProductLevel targetLevel)
        {
            toChangeProduct.productLevel = targetLevel;
            switch (targetLevel)
            {
                case ProductLevel.Raw:
                    toChangeProduct.userReviewLevel = "Mixed";
                    toChangeProduct.userReviewNumber = "(" + Random.Range(5, 500) + " Reviews" + ")";
                    break;
                case ProductLevel.Medium:
                    toChangeProduct.userReviewLevel = "Very Positive";
                    toChangeProduct.userReviewNumber = "(" + Random.Range(1, 9) + "," + Random.Range(100, 900) + " Reviews" + ")";
                    break;
                case ProductLevel.Welldone:
                    toChangeProduct.userReviewLevel = "Overwhelmingly Positive";
                    toChangeProduct.userReviewNumber = "(" + Random.Range(1, 5) + "," + Random.Range(100, 900) + "," + Random.Range(100, 900) + " Reviews" + ")";
                    break;
            }
            toChangeProduct.producedDate = GetCurrentDate();
            ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, toChangeProduct.productLevel),toChangeProduct);
        }

        public void ProductInfoSync(Product toUpdateInfo, ProductShowcase targetProduct)
        {
            switch (toUpdateInfo.productLevel)
            {
                case ProductLevel.Raw:
                    t_ReviewLevel.color = M_Global.instance.repository.stampColors[0];
                    break;
                case ProductLevel.Medium:
                    t_ReviewLevel.color = M_Global.instance.repository.stampColors[1];
                    break;
                case ProductLevel.Welldone:
                    t_ReviewLevel.color = M_Global.instance.repository.stampColors[2];
                    break;
            }
            t_ReviewLevel.text = targetProduct.userReviewLevel;
            t_ReviewNumber.text = targetProduct.userReviewNumber;
            t_Date.text = targetProduct.producedDate;
            i_Game.sprite = toUpdateInfo.productImage;
            t_Name.text = toUpdateInfo.productName;
            t_Summary.text = toUpdateInfo.productDescription;

            for (int i = 0; i < toUpdateInfo.productUserTags.Length; i++)
            {
                userTags[i].text = toUpdateInfo.productUserTags[i];
                
                if (i == 0)
                {
                    userTags[i].GetComponentInParent<RectTransform>().anchoredPosition = new Vector3(userTags[i].GetComponentInParent<RectTransform>().rect.width / 2, 0, 0);
                }
                else
                {
                    userTags[i].GetComponentInParent<RectTransform>().anchoredPosition
                        = new Vector3(userTags[i].GetComponentInParent<RectTransform>().rect.width
                        + userTags[i - 1].GetComponentInParent<RectTransform>().anchoredPosition.x + 10, 0, 0);
                }
            }
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

        void ObjPopOut(Transform transToPop, float timeInTotal)
        {
            Sequence s = DOTween.Sequence();
            s.Append(transToPop.DOScale(1.2f, 0.6f * timeInTotal));
            s.Append(transToPop.DOScale(0.9f, 0.3f * timeInTotal));
            s.Append(transToPop.DOScale(1f, 0.1f * timeInTotal));
        }

        string GetCurrentDate()
        {
            string timeTest = System.DateTime.UtcNow.ToLocalTime().ToString("yyyy.MM.dd");
            return timeTest;
        }
    }
}