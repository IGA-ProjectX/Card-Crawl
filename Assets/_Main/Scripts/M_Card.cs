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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) DrawCard();
        }

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
                    Transform go = Instantiate(pre_Card, taskSlots[i].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform.Find("Mask")).transform;
                    cardsInTurn[i] = go;
                    go.GetComponent<O_Card>().InitializeCard(inGameDeck[0], i);
                    inGameDeck.RemoveAt(0);
                    Sequence s = DOTween.Sequence();
                    s.Append(go.DOMoveY(1.82f, 1f));
                    s.AppendCallback(()=> go.transform.SetParent(GameObject.Find("Canvas").transform));
                    s.AppendCallback(() => go.GetComponent<O_Card>().inSlotPos = go.position);
                }
            }
            StopCoroutine(TurnEnd());
        }

        public void DrawCard(List<int> toDrawSlots)
        {
            foreach (int slotIndex in toDrawSlots)
            {
                Transform go = Instantiate(pre_Card, taskSlots[slotIndex].transform.position, Quaternion.identity, GameObject.Find("Canvas").transform.Find("Mask")).transform;
                go.position = new Vector3(go.position.x, go.position.y + 1, 0);
                cardsInTurn[slotIndex] = go;
                go.GetComponent<O_Card>().InitializeCard(inGameDeck[0], slotIndex);
                inGameDeck.RemoveAt(0);
                Sequence s = DOTween.Sequence();
                s.Append(go.DOMoveY(1.82f, 1f));
                s.AppendCallback(() => go.transform.SetParent(GameObject.Find("Canvas").transform));
                s.AppendCallback(() => go.GetComponent<O_Card>().inSlotPos = go.position);
            }
        }

        public void ShowMovableSlot(CardType cardType)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            SpriteRenderer staffGridSprite = staffSlots[(int)cardType].GetComponent<SpriteRenderer>();
            DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, new Color32(255, 255, 255, 140), 0.3f);
        }

        public void ShowMovableState(Transform cardTrans,CardType cardType,int cardValue)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            float ditanceBetweenCardTypeSlot = Vector2.Distance(cardTrans.position, staffSlots[(int)cardType].position);
            float ditanceBetweenProductionSlot = Vector2.Distance(cardTrans.position, staffSlots[0].position);
            SpriteRenderer staffGridSprite = staffSlots[(int)cardType].GetComponent<SpriteRenderer>();
            SpriteRenderer produGridSprite = staffSlots[0].GetComponent<SpriteRenderer>();
            if (ditanceBetweenCardTypeSlot <= 0.6f)
            {
                if (cardValue>=0)
                {
                    DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, Color.green, 0.3f);
                    isUsable = true;
                }
                else
                {
                    int staffValue = M_Main.instance.m_Staff.GetStaffValue((int)cardType);
                    if ((staffValue+cardValue) < 0)
                    {
                        DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, Color.red, 0.3f);
                        isUsable = false;
                    }
                    else
                    {
                        DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, Color.green, 0.3f);
                        isUsable = true;
                    }
                }
            }
            else if (cardValue <= 0&& ditanceBetweenProductionSlot <= 0.6f)
            {
                DOTween.To(() => produGridSprite.color, x => produGridSprite.color = x, Color.green, 0.3f);
                isUsable = true;
                isToPro = true;
            }
            else
            {
                DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, new Color32(255, 255, 255, 140), 0.3f);
                if (cardValue<=0 && (int)cardType!=0)
                {
                    DOTween.To(() => produGridSprite.color, x => produGridSprite.color = x, new Color32(255, 255, 255, 140), 0.3f);
                }
                isUsable = false;
                isToPro = false;
            }
        }

        public void CardUseOrMoveBack(Transform cardTrans, CardType cardType, int cardValue)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            SpriteRenderer staffGridSprite = staffSlots[(int)cardType].GetComponent<SpriteRenderer>();
            SpriteRenderer produGridSprite = staffSlots[0].GetComponent<SpriteRenderer>();
            if (isUsable)
            {
                if (isToPro && (int)cardType != 0)
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(cardTrans.DOMove(staffSlots[0].position, 0.2f));
                    s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCard());
                    s.AppendCallback(() => M_Main.instance.m_Staff.ChangeDeadLineValue(cardValue));
                    s.AppendInterval(0.1f);
                    s.AppendCallback(() => CheckInTurnCardNumber());
                }
                else
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(cardTrans.DOMove(staffSlots[(int)cardType].position, 0.2f));
                    s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCard());
                    s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue((int)cardType, cardValue));
                    s.AppendInterval(0.1f);
                    s.AppendCallback(() => CheckInTurnCardNumber());
                }
            }
            else
            {
                cardTrans.DOMove(cardTrans.GetComponent<O_Card>().inSlotPos,0.3f);
            }
            DOTween.To(() => staffGridSprite.color, x => staffGridSprite.color = x, new Color32(255, 255, 255, 0), 0.3f);
            DOTween.To(() => produGridSprite.color, x => produGridSprite.color = x, new Color32(255, 255, 255, 0), 0.3f);
            isUsable = false;
            isToPro = false;
        }

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
            for (int i = 0; i < cardsInTurn.Count; i++)
            {
                if (cardsInTurn[i] != null )
                {
                    if (cardsInTurn[i].GetComponent<O_Card>().cardCurrentValue < 0)
                    {
                        Image residueCardSprite = cardsInTurn[i].GetComponent<Image>();
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
                    else
                    {
                        cardsInTurn[i].GetComponent<O_Card>().DestroyCard();
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            DrawCard();
        }
    }
}