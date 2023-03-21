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
        Text t_Name;
        Text t_Summary;
        Text t_Date;
        Text t_Review;
        List<GameObject> userTags = new List<GameObject>();

        void Start()
        {
            i_Game = transform.Find("I_Game").GetComponent<Image>();
            t_Name = transform.Find("T_Name").GetComponent<Text>();
            t_Summary = transform.Find("T_Summary").GetComponent<Text>();
            t_Date = transform.Find("T_Release Date").GetComponent<Text>();
            t_Review = transform.Find("I_Review").Find("T_Review Content").GetComponent<Text>();
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
                        t_Date.text = "Release Date: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        t_Date.text = "Release Date: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    M_Audio.PlaySound(SoundType.ProducedRare);
                    break;
                case ProductLevel.Medium:
                    if (currentLevel != ProductLevel.Welldone && currentLevel != ProductLevel.Medium)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Medium);
                        t_Date.text = "Release Date: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        t_Date.text = "Release Date: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    M_Audio.PlaySound(SoundType.ProducedMedium);
                    break;
                case ProductLevel.Welldone:
                    if (currentLevel != ProductLevel.Welldone)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Welldone);
                        t_Date.text = "Release Date: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        t_Date.text = "Release Date: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    M_Audio.PlaySound(SoundType.ProducedWelldone);
                    break;
            }
            ObjPopOut(transform, 1);
        }

        void ProductUpgrade(ProductShowcase toChangeProduct, ProductLevel targetLevel)
        {
            toChangeProduct.productLevel = targetLevel;
            toChangeProduct.producedDate = GetCurrentDate();
            ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, toChangeProduct.productLevel));
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

        public void ProductInfoSync(Product toUpdateInfo)
        {
            switch (toUpdateInfo.productLevel)
            {
                case ProductLevel.None:
                    break;
                case ProductLevel.Raw:
                    t_Review.text = "Mixed";
                    t_Review.color = M_Global.instance.repository.stampColors[0];
                    break;
                case ProductLevel.Medium:
                    t_Review.text = "Very Positive";
                    t_Review.color = M_Global.instance.repository.stampColors[1];
                    break;
                case ProductLevel.Welldone:
                    t_Review.text = "Overwhelmingly Positive";
                    t_Review.color = M_Global.instance.repository.stampColors[2];
                    break;
            }
            i_Game.sprite = toUpdateInfo.productImage;
            t_Name.text = toUpdateInfo.productName;
            t_Summary.text = toUpdateInfo.productDescription;
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