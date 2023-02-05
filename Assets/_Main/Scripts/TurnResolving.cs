using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurnResolving : MonoBehaviour
{
    public Transform[] cardSlots;
    public GameObject pre_Card;
    public List<Transform> cardsInTurn = new List<Transform>();
    public List<BaseCard> inGameDeck = new List<BaseCard>();
    public List<Transform> cardsToDestroy = new List<Transform>();
    Stack<ICommand> iCommandList = new Stack<ICommand>();

    public void Shuffle(List<BaseCard> _list)
    {
        List<BaseCard> tempList = new List<BaseCard>(_list);
        int rand;
        BaseCard tempValue;
        for (int i = tempList.Count - 1; i >= 0; i--)
        {
            rand = Random.Range(0, i + 1);
            tempValue = tempList[rand];
            tempList[rand] = tempList[i];
            tempList[i] = tempValue;
        }
        inGameDeck = tempList;
    }

    public void DrawCardWhenTurnBegin()
    {
        for (int i = 0; i < 4; i++)
        {
            if (cardsInTurn[i] == null)
            {
                Transform go = Instantiate(pre_Card, cardSlots[i].transform.position, Quaternion.identity).transform;
                go.position = new Vector3(go.position.x, go.position.y +2, 0);
                go.localScale = Vector3.one;
                GetComponent<TurnResolving>().cardsInTurn[i] = go;
                go.GetComponent<Obj_Card>().InitializeCard(inGameDeck[0], i);
                inGameDeck.RemoveAt(0);
                go.DOScale(1, 1.2f);
                go.DOMoveY(go.position.y - 2, 0.7f);
            }
        }
    }

    public void NewTurnChecker()
    {
        int counter = 4;
        for (int i = 0; i < 4; i++)
            if (cardsInTurn[i] == null) counter--;
        if (counter == 1)
        {
            foreach (var cmd in iCommandList) cmd.Execute();
            iCommandList.Clear();
            DrawCardWhenTurnBegin();
        }
    }

    public void AddCommand(ICommand newCommand)
    {
        iCommandList.Push(newCommand);
    } 
}
