using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameBootstrap : MonoBehaviour
{
    public static GameBootstrap instance;
    public Info_CardData cardData;
    public List<BaseCard> baseDeck = new List<BaseCard>();
    public CharacterCard playerCharacter;
    public Sprite[] cardLayouts;
    public GameObject pre_Character;

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerCharacter = cardData.characterCards[1];
            InitializeDeck();
            CharacterInstantiate();
            GetComponent<TurnResolving>().Shuffle(baseDeck);
            GetComponent<TurnResolving>().DrawCardWhenTurnBegin();
        }
    }

    void InitializeDeck()
    {
        foreach (var card in cardData.healthCards)
            baseDeck.Add(card);
        foreach (var card in cardData.goldCards)
            baseDeck.Add(card);
        foreach (var card in cardData.weaponCards)
            baseDeck.Add(card);
        foreach (var card in cardData.shieldCards)
            baseDeck.Add(card);
        for (int i = 0; i < cardData.monsterCards.Length; i++)
        {
            if (i< cardData.monsterCards.Length-1)
            {
                baseDeck.Add(cardData.monsterCards[i]);
                baseDeck.Add(cardData.monsterCards[i]);
            }
            else
            {
                baseDeck.Add(cardData.monsterCards[i]);
                baseDeck.Add(cardData.monsterCards[i]);
                baseDeck.Add(cardData.monsterCards[i]);
            }
        }
    }

    void CharacterInstantiate()
    {
        Transform go = Instantiate(pre_Character, GetComponent<TurnResolving>().cardSlots[5].transform.position, Quaternion.identity).transform;
        go.position = new Vector3(go.position.x, go.position.y - 5, 0);
        GetComponent<TurnResolving>().cardsInTurn[5] = go;
        go.GetComponent<Obj_Character>().InitializeCharacter(playerCharacter);
        go.DOMoveY(go.position.y + 5, 0.7f);
    }
}
