using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace IGDF
{
    public class M_Card : MonoBehaviour
    {
        public GameObject pre_Card;
        public Transform[] taskSlots;
        public List<Card> inGameDeck = new List<Card>();
        public List<Transform> cardsInTurn = new List<Transform>();
        [HideInInspector] public bool isUsable = false;
        [HideInInspector] public bool isToPro = false;
        public GameObject pre_CardSlider;
        public Transform outScreenJoint;

        public void InitializeDeck(SO_Level deckData)
        {
            List<Card> tempList = new List<Card>();
            foreach (Card card in deckData.cards_Production) tempList.Add(card);
            foreach (Card card in deckData.cards_Design) tempList.Add(card);
            foreach (Card card in deckData.cards_Art) tempList.Add(card);
            foreach (Card card in deckData.cards_Code) tempList.Add(card);
            ShuffleDeck(tempList);
        }

        public void ShuffleDeck(List<Card> deckToShuffle)
        {
            int rand;
            Card tempValue;
            for (int i = deckToShuffle.Count - 1; i >= 0; i--)
            {
                rand = Random.Range(0, i + 1);
                tempValue = deckToShuffle[rand];
                deckToShuffle[rand] = deckToShuffle[i];
                deckToShuffle[i] = tempValue;
            }
            inGameDeck = deckToShuffle;
        }

        public void DrawCard()
        {
            for (int i = 0; i < 4; i++)
            {
                if (cardsInTurn[i] == null && inGameDeck.Count != 0)
                {
                    InstantiateCardSlider(i);
                    Transform go = Instantiate(pre_Card, taskSlots[i].transform.position, Quaternion.identity,taskSlots[i].parent).transform;
                    cardsInTurn[i] = go;
                    go.GetComponent<O_Card>().InitializeCard(inGameDeck[0], i);
                    inGameDeck.RemoveAt(0);
                    ClipperLeftUpperToMiddleDown(go.parent);
                    go.parent.GetComponentInChildren<O_ClipperLine>().cardTrans = go;
                    go.parent.GetComponentInChildren<O_ClipperLine>().SetLineState("Manuel");
                }
            }

            StopCoroutine(TurnEnd());
        }

        public void DrawCard(List<int> toDrawSlots)
        {
            foreach (int slotIndex in toDrawSlots)
            {
                InstantiateCardSlider(slotIndex);
                Transform go = Instantiate(pre_Card, taskSlots[slotIndex].transform.position, Quaternion.identity, taskSlots[slotIndex].parent).transform;
                cardsInTurn[slotIndex] = go;
                go.GetComponent<O_Card>().InitializeCard(inGameDeck[0], slotIndex);
                inGameDeck.RemoveAt(0);
                ClipperLeftUpperToMiddleDown(go.parent);
            }
        }
        #region Card Drag & Use & Half Cat Anim
        public void ShowMovableState(Transform cardTrans, List<CardType> targetableTypes, int cardValue)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            foreach (CardType targetType in targetableTypes)
            {
                if (targetType == CardType.Production)
                {
                    staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Approved);
                }
                else
                {
                    int staffValue = M_Main.instance.m_Staff.GetStaffValue((int)targetType);
                    if (staffValue + cardValue >= 0)
                    {
                        staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Approved);
                    }
                    else
                    {
                        staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Disapproved);
                    }
                }
            }
        }

        public void CardTargetingDetection(Transform cardTrans, List<CardType> targetableTypes, int cardValue)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            foreach (CardType targetType in targetableTypes)
            {
                float distanceFromSlot = Vector2.Distance(cardTrans.position, staffSlots[(int)targetType].position);
                if (distanceFromSlot <= 0.6f)
                {
                    staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatSlotChangeTo(SlotCondition.Expanded);
                }
                else
                {
                    staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatSlotChangeTo(SlotCondition.Shrinked);
                }
            }
        }

        public void CardUseOrMoveBack(Transform cardTrans, List<CardType> targetableTypes, int cardValue)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            bool isUsed = false;
            foreach (CardType targetType in targetableTypes)
            {
                float distanceFromSlot = Vector2.Distance(cardTrans.position, staffSlots[(int)targetType].position);
                if (distanceFromSlot <= 0.6f)
                {
                    if (cardTrans.GetComponent<O_Card>().cardCurrentType!= CardType.Production && cardValue<0 && targetType == CardType.Production)
                    {
                        Sequence s = DOTween.Sequence();
                        s.Append(cardTrans.DOMove(staffSlots[0].position, 0.2f));
                        s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCardInScreen());
                        s.AppendCallback(() => M_Main.instance.m_Staff.ChangeDeadLineValue(cardValue));
                        s.AppendCallback(() => CheckInTurnCardNumber());
                        s.AppendInterval(0.8f);
                        s.AppendCallback(() => staffSlots[0].GetComponent<O_HalfCat>().CatSlotChangeTo(SlotCondition.Shrinked));
                

                        cardTrans.GetComponent<O_Card>().SetLineStateAuto();
                        staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Inactivated);
                        isUsed = true;
                    }
                    else
                    {
                        if (M_Main.instance.m_Staff.GetStaffValue((int)targetType) >= -cardValue)
                        {
                            Sequence s = DOTween.Sequence();
                            s.Append(cardTrans.DOMove(staffSlots[(int)targetType].position, 0.2f));
                            s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCardInScreen());
                            s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue((int)targetType, cardValue));
                            s.AppendCallback(() => CheckInTurnCardNumber());
                            s.AppendInterval(0.8f);
                            s.AppendCallback(() => staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatSlotChangeTo(SlotCondition.Shrinked));
                

                            cardTrans.GetComponent<O_Card>().SetLineStateAuto();
                            staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Inactivated);
                            isUsed = true;
                        }
                    }
           
                }
            }
            if (!isUsed) cardTrans.DOMove(cardTrans.parent.Find("Card Pivot").position + new Vector3(0, 0, 0.2f), 0.3f);
            foreach (CardType targetType in targetableTypes)
                staffSlots[(int)targetType].GetComponent<O_HalfCat>().CatIconChangeTo(IconCondition.Inactivated);
        }

#endregion

        public void CheckInTurnCardNumber()
        {
            int nullCount = 0;
            foreach (Transform transform in cardsInTurn)
            {
               if(transform == null) nullCount++;
            }
            if (nullCount>=3)
            {
                StartCoroutine(TurnEnd());
            }
        }

        public IEnumerator TurnEnd()
        {
            List<int> moveIndex = new List<int>();
            for (int i = 0; i < cardsInTurn.Count; i++)
            {
                if (cardsInTurn[i] != null )
                {
                    if (cardsInTurn[i].GetComponent<O_Card>().cardCurrentValue < 0) continue;
                    else moveIndex.Add(i);
                }
                else moveIndex.Add(i);
            }

            for (int i = 0; i < moveIndex.Count; i++)
            {
               if( taskSlots[moveIndex[i]]!=null)
                if (taskSlots[moveIndex[i]].parent.GetComponentInChildren<O_ClipperLine>().isClipperInScreen) {
                    ClipperMiddleDownToRightDown(taskSlots[moveIndex[i]].parent);
                }
            }
            for (int i = 0; i < cardsInTurn.Count; i++)
            {
                if (!moveIndex.Contains(i))
                {
                    SpriteRenderer residueCardSprite = cardsInTurn[i].Find("Card BG").GetComponent<SpriteRenderer>();
                    DOTween.To(() => residueCardSprite.color, x => residueCardSprite.color = x, Color.red, 0.1f);
                    yield return new WaitForSeconds(0.1f);
                    DOTween.To(() => residueCardSprite.color, x => residueCardSprite.color = x, Color.white, 0.1f);
                    yield return new WaitForSeconds(0.1f);
                    DOTween.To(() => residueCardSprite.color, x => residueCardSprite.color = x, Color.red, 0.1f);
                    yield return new WaitForSeconds(0.1f);
                    DOTween.To(() => residueCardSprite.color, x => residueCardSprite.color = x, Color.white, 0.1f);
                    yield return new WaitForSeconds(0.2f);
                    M_Main.instance.m_Staff.ChangeDeadLineValue(-1);
                }
            }
            //yield return new WaitForSeconds(horiTime + verTime + 0.03f);
            DrawCard();
        }

        private float horiDistance;
        private float verDistance;
        private Transform cardSlider;
        public float horiTime;
        public float verTime;

        public void InitializeMoveValue()
        {
             cardSlider = GameObject.Find("Environment").transform.Find("Card Slider");
             horiDistance = cardSlider.Find("InScreen Joints").position.x - cardSlider.Find("OutScreen Joints").position.x;
             verDistance = cardSlider.Find("InScreen Joints").Find("Pivot 1 Inner").position.y - cardSlider.Find("InScreen Joints").Find("Pivot 1 Outer").position.y;
        }

        public void ClipperLeftUpperToMiddleDown(Transform clipperTrans)
        {
            //卡牌及夹子 前后位置关系调整 解决覆盖Bug
            Transform cardTrans = clipperTrans.GetComponentInChildren<O_Card>().transform;
            cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y, 0.2f);
            clipperTrans.position = new Vector3(clipperTrans.position.x, clipperTrans.position.y, 0f);
            //卡牌 夹子 滑轮颜色的获取和变灰 + 绳子
            SpriteRenderer handlerSprite = clipperTrans.Find("Handler").GetComponent<SpriteRenderer>();
            SpriteRenderer clipperSprite = clipperTrans.Find("Clipper").GetComponent<SpriteRenderer>();
            SpriteRenderer cardBG = cardTrans.Find("Card BG").GetComponent<SpriteRenderer>();
            LineRenderer line = handlerSprite.GetComponent<LineRenderer>();
            clipperSprite.color = Color.gray;
            handlerSprite.color = Color.gray;
            cardBG.color = Color.gray;
            line.endColor = Color.gray;
            line.startColor = Color.gray;
            //卡牌+轮滑组的具体位移
            Sequence s = DOTween.Sequence();
            s.Append(clipperTrans.DOMoveX(clipperTrans.position.x + horiDistance, horiTime));
            s.AppendCallback(() => CardMoveDownwards());
            s.AppendInterval(verTime + 0.01f);
            s.AppendCallback(() => cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y, 0.1f));
            s.AppendCallback(() => clipperTrans.GetComponentInChildren<O_ClipperLine>().isClipperInScreen = true);
            s.AppendCallback(() => cardTrans.GetComponent<O_Card>().SetDraggableState(true));
            s.AppendCallback(() => M_Main.instance.m_Skill.EnterWaitForUseState());

            void CardMoveDownwards()
            {
                clipperTrans.DOMoveY(clipperTrans.position.y - verDistance, verTime);
                DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.white, verTime);
                DOTween.To(() => clipperSprite.color, x => clipperSprite.color = x, Color.white, verTime);
                DOTween.To(() => handlerSprite.color, x => handlerSprite.color = x, Color.white, verTime);
                DOTween.To(() => line.endColor, x => line.endColor = x, Color.white, verTime);
                DOTween.To(() => line.startColor, x => line.startColor = x, Color.white, verTime);
            }
        }

        public void ClipperMiddleDownToLeftUpper(Transform clipperTrans)
        {
            Transform cardTrans = clipperTrans.GetComponentInChildren<O_Card>().transform;
            cardTrans.position = new Vector3(cardTrans.position.x, cardTrans.position.y, 0.2f);
            clipperTrans.position = new Vector3(clipperTrans.position.x, clipperTrans.position.y, 0.1f);
            SpriteRenderer handlerSprite = clipperTrans.Find("Handler").GetComponent<SpriteRenderer>();
            SpriteRenderer clipperSprite = clipperTrans.Find("Clipper").GetComponent<SpriteRenderer>();
            SpriteRenderer cardBG = cardTrans.Find("Card BG").GetComponent<SpriteRenderer>();
            LineRenderer line = handlerSprite.GetComponent<LineRenderer>();
            clipperTrans.GetComponentInChildren<O_ClipperLine>().isClipperInScreen  = false;

            Sequence s = DOTween.Sequence();
            s.AppendCallback(() => cardTrans.GetComponent<O_Card>().SetDraggableState(false));
            s.AppendCallback(() => CardUpDownwards());
            s.AppendInterval(verTime);
            s.Append(clipperTrans.DOMoveX(clipperTrans.position.x - horiDistance, horiTime));

            void CardUpDownwards()
            {
                clipperTrans.DOMoveY(clipperTrans.position.y + verDistance, verTime);
                DOTween.To(() => cardBG.color, x => cardBG.color = x, Color.grey, verTime);
                DOTween.To(() => clipperSprite.color, x => clipperSprite.color = x, Color.grey, verTime);
                DOTween.To(() => handlerSprite.color, x => handlerSprite.color = x, Color.grey, verTime);
                DOTween.To(() => line.endColor, x => line.endColor = x, Color.grey, verTime);
                DOTween.To(() => line.startColor, x => line.startColor = x, Color.grey, verTime);
            }
        }

        public void ClipperMiddleDownToRightDown(Transform clipperTrans)
        {
            //if (clipperTrans.GetComponentInChildren<O_Card>() != null)
            //    clipperTrans.GetComponentInChildren<O_Card>().SetDraggableState(false);
            //M_Main.instance.m_Skill.EnterCanNotUseState();

            clipperTrans.GetComponentInChildren<O_ClipperLine>().isClipperInScreen = false;
            clipperTrans.DOMoveX(clipperTrans.position.x + horiDistance, horiTime);
            if (clipperTrans.GetComponentInChildren<O_Card>()!=null)
                clipperTrans.GetComponentInChildren<O_Card>().DestroyCardOutScene(horiTime + 0.1f);
            clipperTrans.GetComponentInChildren<O_ClipperLine>().DestroySlider(horiTime + 0.2f);
            Destroy(clipperTrans, horiTime + 0.2f);
        }

        public void InstantiateCardSlider(int index)
        {
            Transform slider = Instantiate(pre_CardSlider, outScreenJoint.GetChild(index).position, Quaternion.identity, outScreenJoint.GetChild(index)).transform;
            taskSlots[index] = slider.Find("Card Pivot");
            slider.GetComponentInChildren<O_ClipperLine>().InitializeClipperLine();
        }
    }
}