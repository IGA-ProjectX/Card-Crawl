using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace IGDF
{
    public class O_Result : MonoBehaviour
    {
        Image stamp;
        Image gameContent;
        TMP_Text nameText;
        Text descriptionText;
        Text dateText;

        void Start()
        {
             stamp = transform.Find("BG").Find("Stamp").GetComponent<Image>();
             gameContent = transform.Find("Mask").Find("Game Content").GetComponent<Image>();
             nameText = transform.Find("Name").Find("Name Text").GetComponent<TMP_Text>();
             descriptionText = transform.Find("Description").GetComponent<Text>();
             dateText = transform.Find("Date").GetComponent<Text>();
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
                        dateText.text = "Released: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        dateText.text = "Released: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    break;
                case ProductLevel.Medium:
                    if (currentLevel != ProductLevel.Welldone && currentLevel != ProductLevel.Medium)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Medium);
                        dateText.text = "Released: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        dateText.text = "Released: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    break;
                case ProductLevel.Welldone:
                    if (currentLevel != ProductLevel.Welldone)
                    {
                        ProductUpgrade(toChangeProduct, ProductLevel.Welldone);
                        dateText.text = "Released: " + GetCurrentDate();
                    }
                    else
                    {
                        ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, currentLevel));
                        dateText.text = "Released: " + GetProductShowcase(M_Global.instance.levels[M_Global.instance.targetLevel].levelType).producedDate;
                    }
                    break;
            }
            ObjPopOut(transform, 1);
        }

        void ProductUpgrade(ProductShowcase toChangeProduct, ProductLevel targetLevel)
        {
            toChangeProduct.productLevel = targetLevel;
            toChangeProduct.producedDate= GetCurrentDate();
            ProductInfoSync(GetProductInfo(M_Global.instance.levels[M_Global.instance.targetLevel].levelType, toChangeProduct.productLevel));
        }

        Product GetProductInfo(LevelType targetGameType ,ProductLevel targetProductLevel)
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
                    stamp.sprite = M_Global.instance. repository.stamps[0];
                    stamp.color = M_Global.instance.repository.stampColors[0];
                    break;
                case ProductLevel.Medium:
                    stamp.sprite = M_Global.instance.repository.stamps[1];
                    stamp.color = M_Global.instance.repository.stampColors[1];
                    break;
                case ProductLevel.Welldone:
                    stamp.sprite = M_Global.instance.repository.stamps[2];
                    stamp.color = M_Global.instance.repository.stampColors[2];
                    break;
            }
            gameContent.sprite = toUpdateInfo.productImage;
            nameText.text = toUpdateInfo.productName;
            descriptionText.text = toUpdateInfo.productDescription;
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