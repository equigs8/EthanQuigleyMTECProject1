using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public bool[] avilableCardSlots;

    public bool controllingUnit = false;

    public GameObject unitPrefab;

    public float maxMana = 1f;
    public float currentMana;
    public float manaGain = 0.001f;
    public TMP_Text manaText;

    public void Start()
    {
        currentMana = maxMana;
        manaText.text = currentMana.ToString();
    }
    
    void Update()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaGain;
            manaText.text = currentMana.ToString();
        }
    }

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
                randomCard.SetCardPostion(cardSlots[i].transform, i);
                avilableCardSlots[i] = false;
                deck.Remove(randomCard);
                return;
            }
        }
    }

    public void Shuffle()
    {
       
    }

    public void PlayCard(int slotNum, Transform transform, int manaCost)
    {
        Debug.Log("Played Card");
        Debug.Log(avilableCardSlots[slotNum]);
        avilableCardSlots[slotNum] = true;

        GameObject unit = Instantiate(unitPrefab) as GameObject;
        unit.transform.position = transform.position;
        currentMana -= manaCost;
    }

    public void ControllingUnit(bool isControllingUnit)
    {
        controllingUnit = isControllingUnit;
    }

    public bool CheckIfControlling()
    {
        return controllingUnit;
    }
    public bool EnoughMana(int manaAmount)
    {
        if (manaAmount <= currentMana)
        {
            return true;
        }
        return false;
    }
}