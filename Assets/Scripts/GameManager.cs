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
    public GameObject enemyUnitPrefab;

    public float maxMana = 1f;
    public float currentMana;
    public float manaGain = 0.001f;
    public TMP_Text manaText;

    public float enemyCurrentMana;
    
    public GameObject[] playerCastles;
    public GameObject[] enemyCastles;
    public bool isWinner;
    public string winnerName;


    public void checkIfWinner()
    {
        foreach (GameObject gameObjectCastle in playerCastles)
        {
            if(gameObjectCastle.GetComponent<Castle>().GetIsAlive())
            {
                return;
            }else
            {
                isWinner = true;
                winnerName = "Enemy";
            }
        }
        foreach (GameObject gameObjectEnemyCastle in enemyCastles)
        {
            if(gameObjectEnemyCastle.GetComponent<Castle>().GetIsAlive())
            {
                return;
            }else
            {
                isWinner = true;
                winnerName = "Player";
            }
        }
    }

    public float checkCastleHealth(Castle castle)
    {
        return castle.GetCurrentHealth();
    }

    public void Start()
    {
        isWinner = false;
        currentMana = maxMana;
        enemyCurrentMana = maxMana;
        manaText.text = currentMana.ToString();
    }
    
    void Update()
    {
        if (!isWinner)
        {
            if (currentMana < maxMana)
            {
                currentMana += manaGain;
                manaText.text = currentMana.ToString();
            }
            if (enemyCurrentMana < maxMana)
            {
                enemyCurrentMana += manaGain;
            } 
        }
        Winner();
    }

    void Winner()
    {
        Debug.Log("The Winner is " + winnerName);
    }
    public float GetEnemyCurrentMana()
    {
        return enemyCurrentMana;
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

    public void PlayEnemyCard(Transform transform, float manaCost)
    {
        GameObject enemyUnit = Instantiate(enemyUnitPrefab) as GameObject;
        enemyUnit.transform.position = transform.position;
        enemyCurrentMana -= manaCost;
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