using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class Card : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private bool isDragging = false;
    public Transform cardPosition;
    public Transform cardSlotTransform;

    public Transform dropZone;
    public List<Transform> dropZones = new List<Transform>();
    public GameObject[] dropZonesGameObject;

    public Transform currentDropZoneTransform;
    private Vector3 offset;
    private Camera mainCamera;

    [SerializeField] private bool isOverDropZone = false; // Flag to track if over drop zone


    [SerializeField] private float _dissolveTime = 0.75f;

    public SpriteRenderer _spriteRenderer;
    private Material _material;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    [SerializeField] private int slot;
    public int manaCost;

    public UnitType unitType;

    public string name;
    public GameObject manaCostUI;
    public TMP_Text manaCostUIText;
    
    
        
    private void OnEnable()
    {
        name = unitType.name;
        manaCost = unitType.manaCost;

        gameObject.GetComponent<SpriteRenderer>().sprite = unitType.cardSprite;
    }
    void Start()
    {
        mainCamera = Camera.main;
        _material = _spriteRenderer.material;

        cardSlotTransform = this.GetComponent<Transform>();

        dropZonesGameObject = GameObject.FindGameObjectsWithTag("TargetArea");

        foreach (GameObject dropZone in dropZonesGameObject)
        {
            dropZones.Add(dropZone.GetComponent<Transform>());
        }
        
    }

    private void Update()
    {
        CardColorChange();
    }

    public int GetManaCost()
    {
        return manaCost;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset.z = 0;
        
    }

    private void CardColorChange()
    {
        if (!gameManager.EnoughMana(manaCost))
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            Debug.Log("Not Enough Mana to play card");
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // TODO: Display mana cost


            foreach (GameObject dropZone in dropZonesGameObject)
            {
                Debug.Log(dropZone.name + " is about to be reviled");
                SpriteRenderer sp = dropZone.GetComponent<SpriteRenderer>();
                sp.color = new Color(1f, 1f, 1f, .5f);
            }

            manaCostUI.SetActive(true);
            manaCostUIText.text = "Cost: " + manaCost.ToString();

            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;
            transform.position = newPosition;

            // Check for overlap while dragging
            CheckForOverlap();
        }
    }

    public int GetSlot()
    {
        return slot;
    }

    void OnMouseUp()
    {
        if (isOverDropZone)
        {
            Debug.Log("OnMouseUp and isOverDropZone");
            
            if (gameManager.EnoughMana(manaCost))
            {
                SetCardPostion(currentDropZoneTransform, slot);
                StartCoroutine(Vanish());
            }else
            { 
                Debug.Log("Not Enough Mana to play card");
            }
            
        }
        manaCostUI.SetActive(false);
        isDragging = false;
        isOverDropZone = false; // Reset the flag
        transform.position = cardPosition.position;
        Debug.Log("Mouse Up");
        foreach (GameObject dropZone in dropZonesGameObject)
        {
            SpriteRenderer sp = dropZone.GetComponent<SpriteRenderer>();
            sp.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    internal void SetUnitType(UnitType unit)
    {
        unitType = unit;
    }

    void CheckForOverlap()
    {
        foreach (Transform dropZoneTransform in dropZones)
        {
        
        //Debug.Log("dropZoneTransform " + dropZoneTransform);
        
        Bounds bounds = dropZoneTransform.GetComponent<BoxCollider2D>().bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;

            if (transform.position.x >= center.x - size.x / 2 && transform.position.x <= center.x + size.x / 2 &&
                transform.position.y >= center.y - size.y / 2 && transform.position.y <= center.y + size.y / 2)
            {
              
                    //Debug.Log(gameObject.name + " entered the drop zone.");
                    isOverDropZone = true; // Set the flag
                    currentDropZoneTransform = dropZoneTransform;

            }
            
            
            if (!isOverDropZone)
            {
                //Debug.Log(gameObject.name + " Not in the drop zone.");
                isOverDropZone = false; // Reset the flag
                currentDropZoneTransform = cardSlotTransform;
                
                    
            }
            
            
        }
    }
    public void SetCardPostion(Transform transform, int cardSlot)
    {
        cardPosition = transform;
        slot = cardSlot;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }

    private IEnumerator Vanish()
    {
        Debug.Log("Corutine runnning");
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1f, (elapsedTime / _dissolveTime));

            _material.SetFloat(_dissolveAmount, lerpedDissolve);

            yield return null;
        }
        gameManager.PlayCard(slot,transform,manaCost,unitType);
        Destroy(gameObject);
    }
}
