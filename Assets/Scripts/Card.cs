using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

    public int getManaCost()
    {
        return manaCost;
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset.z = 0;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
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
        isDragging = false;
        isOverDropZone = false; // Reset the flag
        transform.position = cardPosition.position;
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
        gameManager.PlayCard(slot,transform,manaCost);
        Destroy(gameObject);
    }
}
