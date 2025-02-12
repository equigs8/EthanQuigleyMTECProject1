using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public bool[] avilableCardSlots;

    public void DrawCard()
    {
        Card randomCard = deck[Random.Range(0, deck.Count)];

        for (int i = 0; i < avilableCardSlots.Length; i++)
        {
            Debug.Log(i + " " + avilableCardSlots[i]);
            if (avilableCardSlots[i] == true)
            {
                randomCard.gameObject.SetActive(true);
                randomCard.transform.position = cardSlots[i].position;
                randomCard.SetCardPostion(cardSlots[i].transform);
                avilableCardSlots[i] = false;
                deck.Remove(randomCard);
                return;
            }
        }
    }

    public void Shuffle()
    {
       
    }

    public void PlayCard()
    {
        Debug.Log("Played Card");
    }
}