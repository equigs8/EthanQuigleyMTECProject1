using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{

    public List<GameObject> cardsInHand = new List<GameObject>();
    //public EnemyDeck enemyDeck;
    //public Transform dropArea1;
    //public Transform dropArea2;
    private float waitToPlay;
    private Transform whatFileToPlayIn;
    public float mana;
    public GameObject cardToPlay;
    private bool enoughManaToPlayCard;
    public bool pickedCardToPlay;
    public Transform[] dropAreas;
    public GameManager gameManager;
    public bool playingCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < dropAreas.Length; i++)
        {
            if (dropAreas[i] == null)
            {
                Debug.LogError(dropAreas[i] + " is Null. Which is Index, " + i);
            }
        }
        
        while (cardsInHand.Count < 5)
        {
            Debug.Log("DrawCards");
            //enemyDeck.drawCard(cardsInHand)
        }
        pickedCardToPlay = false;
        playingCard = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentMana();
        PickWhatFileToPlayIn();

        if (!pickedCardToPlay)
        {
            PickCardToPlay();
            Debug.Log("Picked Card");
        }

        Debug.Log("Before Mana < cardToPlay");
        if (mana >= cardToPlay.GetComponent<EnemyCard>().GetManaCost() && !playingCard)
        {
            Debug.Log("Inside Mana < cardToPlay");
            StartCoroutine(PlayCard());
        }

        
    }

    private void GetCurrentMana()
    {
        mana = gameManager.GetEnemyCurrentMana();
    }

    private void PickCardToPlay()
    {
        int randomHandIndex = Random.Range(1, cardsInHand.Count);

        cardToPlay = cardsInHand[randomHandIndex];
        pickedCardToPlay = true;
    }

    private void PickWhatFileToPlayIn()
    {
        whatFileToPlayIn = dropAreas[Random.Range(0, 1)];
    }

    private IEnumerator PlayCard()
    {
        playingCard = true;
        waitToPlay = Random.Range(1f, 20f);

        Debug.Log("Enemy Start PlayCard()");
        yield return new WaitForSeconds(waitToPlay);

        gameManager.PlayEnemyCard(whatFileToPlayIn, cardToPlay.GetComponent<EnemyCard>().GetManaCost());
        pickedCardToPlay = false;
        playingCard = false;
    }
}
