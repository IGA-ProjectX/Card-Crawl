using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        public void ShuffleDeck(SO_Deck deckData)
        {
            List<Card> tempList = new List<Card>();
            foreach (Card card in deckData.cards_Production) tempList.Add(card);
            foreach (Card card in deckData.cards_Design) tempList.Add(card);
            foreach (Card card in deckData.cards_Art) tempList.Add(card);
            foreach (Card card in deckData.cards_Code) tempList.Add(card);
            
            int rand;
            Card tempValue;
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                rand = Random.Range(0, i + 1);
                tempValue = tempList[rand];
                tempList[rand] = tempList[i];
                tempList[i] = tempValue;
            }
            inGameDeck = tempList;
        }

        public void DrawCard()
        {
            for (int i = 0; i < 4; i++)
            {
                if (cardsInTurn[i] == null && inGameDeck.Count != 0)
                {
                    Transform go = Instantiate(pre_Card, taskSlots[i].transform.position, Quaternion.identity).transform;
                    go.position = new Vector3(go.position.x, go.position.y + 1, 0);
                    cardsInTurn[i] = go;
                    go.GetComponent<O_Card>().InitializeCard(inGameDeck[0], i, go.position + new Vector3(0, -1, 0));
                    inGameDeck.RemoveAt(0);
                    go.DOMoveY(go.position.y - 1, 0.2f);
                }
            }
            StopCoroutine(TurnEnd());
        }

        public void ShowMovableSlot(Card cardData)
        {
            Transform[] staffSlots = M_Main.instance.m_Staff.staffSlots;
            SpriteRenderer staffGridSprite = staffSlots[(int)cardData.cardType].GetComponent<SpriteRenderer>();
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
                int ddlValue = M_Main.instance.m_Staff.GetDDLValue();
                if (ddlValue+cardValue>0 && cardValue<=0)
                {
                    DOTween.To(() => produGridSprite.color, x => produGridSprite.color = x, Color.green, 0.3f);
                    isUsable = true;
                    isToPro = true;
                }
                else
                {
                    DOTween.To(() => produGridSprite.color, x => produGridSprite.color = x, Color.red, 0.3f);
                    isUsable = false;
                    isToPro = false;
                }
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
                    s.AppendCallback(() => cardTrans.DOScale(0, 0.3f));
                    s.AppendInterval(0.2f);
                    s.AppendCallback(() => M_Main.instance.m_Staff.ChangeDeadLineValue(cardValue));
                    s.AppendCallback(() => cardsInTurn[cardTrans.GetComponent<O_Card>().inSlotIndex] = null);
                    s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCard());
                    s.AppendCallback(() => CheckInTurnCardNumber());
                }
                else
                {
                    Sequence s = DOTween.Sequence();
                    s.Append(cardTrans.DOMove(staffSlots[(int)cardType].position, 0.2f));
                    s.AppendCallback(() => cardTrans.DOScale(0, 0.3f));
                    s.AppendInterval(0.2f);
                    s.AppendCallback(() => M_Main.instance.m_Staff.ChangeStaffValue((int)cardType, cardValue));
                    s.AppendCallback(() => cardsInTurn[cardTrans.GetComponent<O_Card>().inSlotIndex] = null);
                    s.AppendCallback(() => cardTrans.GetComponent<O_Card>().DestroyCard());
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
                if (cardsInTurn[i] != null && cardsInTurn[i].GetComponent<O_Card>().cardCurrentValue < 0)
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
            yield return new WaitForSeconds(0.2f);
            DrawCard();
        }
    }
}