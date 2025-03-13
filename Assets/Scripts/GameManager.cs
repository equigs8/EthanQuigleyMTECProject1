using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public List<Card> emptyCards = new List<Card>();
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
    
    public List<GameObject> playerCastles = new List<GameObject>();
    public List<GameObject> enemyCastles = new List<GameObject>();
    public bool isWinner;
    public string winnerName;


    public UnitType[] Units;
    public List<Card> playerHand = new List<Card>();

    public GameObject pauseMenu;
    public bool gamePaused;

    public void checkIfWinner()
    {
        if (enemyCastles.Count == 0)
        {
            
            winnerName = "Player";
            isWinner = true;
        }
        else if (playerCastles.Count == 0)
        {

            winnerName = "Enemy";
            isWinner = true;
        }
    }

    public string GetWinnerName()
    {
        return winnerName;
    }
    public bool ThereIsAWinner()
    {
        return isWinner;
    }
    internal void removeCastle(string owner, GameObject castle)
    {
        if (owner == "player")
        {
            playerCastles.Remove(castle);
        }
        else
        {
            enemyCastles.Remove(castle);
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
        manaText.text = Mathf.Round(currentMana).ToString();
        
        InitializeDeck(); 
    }

    private void InitializeDeck()
    {
        int counter = 0;
        foreach (UnitType unit in Units)
        {
            Debug.Log(unit);

            emptyCards[counter].SetUnitType(unit);

            counter += 1;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseGame();
            
        }
        if (!isWinner && !gamePaused)
        {
            if (currentMana < maxMana)
            {
                currentMana += manaGain;
                manaText.text = Mathf.Round(currentMana).ToString();
            }
            if (enemyCurrentMana < maxMana)
            {
                enemyCurrentMana += manaGain;
            }
            
            if (playerHand.Count < 5)
            {
                DrawCard();
            }

            checkIfWinner();
        }
        else if (isWinner)
        {
            Winner();
        }
        
    }

    public void PauseGame()
    {
        if (!gamePaused)
        {
            pauseMenu.SetActive(true);
            gamePaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1f;
        }
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
        Card randomCard = emptyCards[Random.Range(0, emptyCards.Count)];

        for (int i = 0; i < avilableCardSlots.Length; i++)
        {
            Debug.Log(i + " " + avilableCardSlots[i]);
            if (avilableCardSlots[i] == true)
            {
                playerHand.Insert(i, randomCard);
                emptyCards.Remove(randomCard);
                randomCard.gameObject.SetActive(true);
                randomCard.transform.position = cardSlots[i].position;
                randomCard.SetCardPostion(cardSlots[i].transform, i);
                avilableCardSlots[i] = false;
                //deck.Remove(randomCard);
                return;
            }
        }
    }

    public void Shuffle()
    {
       
    }

    public void PlayCard(int slotNum, Transform transform, int manaCost, UnitType unitType)
    {
        Debug.Log("Played Card");
        Debug.Log(avilableCardSlots[slotNum]);
        avilableCardSlots[slotNum] = true;

        playerHand.RemoveAt(slotNum);

        //List<Card> tempHand = new List<Card>();
        //foreach (Card card in playerHand)
        //{
        //    tempHand.Add(card);
        //    Debug.Log("added to Temp Hand");
        //}
        //playerHand = tempHand;


        
        GameObject unit = Instantiate(unitPrefab) as GameObject;
        unit.GetComponent<UnitController>().SetUnitType(unitType);
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