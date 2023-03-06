using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace IGDF
{
    public class M_Main : MonoBehaviour
    {
        public static M_Main instance;
        public SO_Repo repository;

        public Transform resultPanel;

        [HideInInspector] public M_Card m_Card;
        [HideInInspector] public M_Staff m_Staff;
        [HideInInspector] public M_Skill m_Skill;
        [HideInInspector] public M_SkillResolve m_SkillResolve;
        [HideInInspector] public M_DDL m_DDL;
        [HideInInspector] public M_ChatBubble m_ChatBubble;

        void Start()
        {
            instance = this;
            m_Card = GetComponent<M_Card>();
            m_Staff = GetComponent<M_Staff>();
            m_Skill = GetComponent<M_Skill>();
            m_SkillResolve = GetComponent<M_SkillResolve>();
            m_DDL = GetComponent<M_DDL>();
            m_ChatBubble = GetComponent<M_ChatBubble>();

            m_ChatBubble.PrepareTalkList(M_Global.instance.levels[M_Global.instance.targetLevel].talkList);
            m_Card.InitializeDeck(M_Global.instance.levels[M_Global.instance.targetLevel]);
            m_Card.InitializeMoveValue();
            m_DDL.CreateDots();
            m_DDL.InitializeNumberList();
            m_Staff.InitializeStaffValues(M_Global.instance.levels[M_Global.instance.targetLevel].staffValue);
            m_Skill.InitializeSkills(M_Global.instance.skillList);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Card.DrawCard();
                foreach (Transform skillTrans in m_Skill.skillObjects)
                {
                    skillTrans.GetComponent<O_Skill>().OpenEye();
                }
            }
        }

        public void CheckDevCircumstance()
        {
            int nullCount = 0;
            foreach (Transform transform in m_Card.cardsInTurn)
            {
                if (transform == null) nullCount++;
            }
            if (m_Card.inGameDeck.Count == 0 && nullCount == 4) GameDevSucceed();
            if (m_Staff.GetDDLValue() <= 0) GameDevFailed();

            void GameDevSucceed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.WinGame);
                M_Global.instance.PlayerExpUp(m_Staff.GetStaffValue(0));
                GameProduced();
            }

            void GameDevFailed()
            {
                m_ChatBubble.TryTriggerTalkSpecialCondition(TalkConditionType.LoseGame);
                GameProduced();
            }
        }

        public void GameProduced()
        {
            LevelType currentLevelType = M_Global.instance.levels[M_Global.instance.targetLevel].levelType;
            int maxDDL = M_Global.instance.levels[M_Global.instance.targetLevel].staffValue[4];

            if (m_Staff.GetDDLValue() == maxDDL)
                ProductStateUpdate(CheckGameProductState(currentLevelType), ProductLevel.Welldone);
            else if (m_Staff.GetDDLValue() < maxDDL && m_Staff.GetDDLValue() >= 0.66 * maxDDL)
                ProductStateUpdate(CheckGameProductState(currentLevelType), ProductLevel.Medium);
            else if (m_Staff.GetDDLValue() < 0.66 * maxDDL)
                ProductStateUpdate(CheckGameProductState(currentLevelType), ProductLevel.Raw);

            ProductShowcase CheckGameProductState(LevelType currentLevelType)
            {
                ProductShowcase onShowProduct = null;
                foreach (ProductShowcase product in M_Global.instance.mainData.productShowcases)
                    if (product.levelType == currentLevelType)
                        onShowProduct = product;
                return onShowProduct;
            }

            void ProductStateUpdate(ProductShowcase toChangeProduct, ProductLevel toChangeLevel)
            {
                Debug.Log("Original: " + toChangeProduct.productLevel);
                ProductLevel currentLevel = toChangeProduct.productLevel;
                Image productBg = resultPanel.Find("Product").GetComponent<Image>();
                Image productContent = productBg.transform.Find("Image").GetComponent<Image>();
                Transform returnButton = resultPanel.Find("Return");

                switch (toChangeLevel)
                {
                    case ProductLevel.Raw:
                        if (currentLevel == ProductLevel.None) ProductUpgrade(ProductLevel.Raw);
                        else ProductNoChange();
                        break;
                    case ProductLevel.Medium:
                        if (currentLevel != ProductLevel.Welldone && currentLevel != ProductLevel.Medium) ProductUpgrade(ProductLevel.Medium);
                        else ProductNoChange();
                        break;
                    case ProductLevel.Welldone:
                        if (currentLevel != ProductLevel.Welldone) ProductUpgrade(ProductLevel.Welldone);
                        else ProductNoChange();
                        break;
                }

                void ProductUpgrade(ProductLevel targetLevel)
                {
                    switch (currentLevel)
                    {
                        case ProductLevel.None:
                            productBg.color = Color.white;
                            break;
                        case ProductLevel.Raw:
                            productBg.color = Color.red;
                            break;
                        case ProductLevel.Medium:
                            productBg.color = Color.green;
                            break;
                        case ProductLevel.Welldone:
                            productBg.color = Color.yellow;
                            break;
                    }
                    toChangeProduct.productLevel = targetLevel;
                    Debug.Log("Changed To: " + toChangeProduct.productLevel);
                    Sequence s = DOTween.Sequence();
                    s.AppendCallback(() => ObjPopOut(resultPanel, 1));
                    s.AppendInterval(1);
                    s.AppendCallback(() => PanelBgColorChange(targetLevel, 4f));
                    s.AppendCallback(() => ObjPopOut(returnButton, 1));
                }

                void ProductNoChange()
                {
                    switch (currentLevel)
                    {
                        case ProductLevel.None:
                            productBg.color = Color.white;
                            break;
                        case ProductLevel.Raw:
                            productBg.color = Color.red;
                            break;
                        case ProductLevel.Medium:
                            productBg.color = Color.green;
                            break;
                        case ProductLevel.Welldone:
                            productBg.color = Color.yellow;
                            break;
                    }
                    Debug.Log("No Change: " + toChangeProduct.productLevel);
                    Sequence s = DOTween.Sequence();
                    s.AppendCallback(() => ObjPopOut(resultPanel, 1));
                    s.AppendInterval(1);
                    s.AppendCallback(() => ObjPopOut(returnButton, 1));
                }

                void ObjPopOut(Transform transToPop,float timeInTotal)
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(transToPop.DOScale(1.2f, 0.6f * timeInTotal));
                    s.Append(transToPop.DOScale(0.9f, 0.3f * timeInTotal));
                    s.Append(transToPop.DOScale(1f, 0.1f * timeInTotal));
                }

                void PanelBgColorChange(ProductLevel changeToLevel, float changeTime)
                {
                    Color targetColor = Color.black;
                    switch (changeToLevel)
                    {
                        case ProductLevel.None:
                            targetColor = Color.white;
                            break;
                        case ProductLevel.Raw:
                            targetColor = Color.red;
                            break;
                        case ProductLevel.Medium:
                            targetColor = Color.green;
                            break;
                        case ProductLevel.Welldone:
                            targetColor = Color.yellow;
                            break;
                    }
                    DOTween.To(() => productBg.color, x => productBg.color = x, targetColor, changeTime);
                }
            }
        }
    }
}