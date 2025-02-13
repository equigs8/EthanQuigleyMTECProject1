using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private bool isDragging = false;
    public Transform cardPosition;
    public Transform cardSlot;

    public Transform dropZone;
    private Vector3 offset;
    private Camera mainCamera;

    private bool isOverDropZone = false; // Flag to track if over drop zone


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
                SetCardPostion(dropZone, slot);
                StartCoroutine(Vanish());
            }else
            {
                transform.position = cardPosition.position;
            }
            
        }
        isDragging = false;
        isOverDropZone = false; // Reset the flag
        transform.position = cardPosition.position;
    }

    void CheckForOverlap()
    {
        Bounds bounds = dropZone.GetComponent<BoxCollider2D>().bounds;
        Vector3 center = bounds.center;
        Vector3 size = bounds.size;

        if (transform.position.x >= center.x - size.x / 2 && transform.position.x <= center.x + size.x / 2 &&
            transform.position.y >= center.y - size.y / 2 && transform.position.y <= center.y + size.y / 2)
        {
            if (!isOverDropZone)
            {
                Debug.Log(gameObject.name + " entered the drop zone.");
                isOverDropZone = true; // Set the flag
                
                // Perform actions when object *enters* the drop zone (e.g., change color)
                // dropZone.GetComponent<SpriteRenderer>().color = Color.green; // Example
            }
            // Object is over the drop zone, allow free movement
        }
        else
        {
            if (isOverDropZone)
            {
                Debug.Log(gameObject.name + " exited the drop zone.");
                isOverDropZone = false; // Reset the flag
                // Perform actions when object *exits* the drop zone (e.g., reset color)
                // dropZone.GetComponent<SpriteRenderer>().color = Color.white; // Example
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
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0, 1f, (elapsedTime / _dissolveTime));

            _material.SetFloat(_dissolveAmount, lerpedDissolve);

            yield return null;
        }
        gameManager.PlayCard(slot,dropZone,manaCost);
        this.gameObject.SetActive(false);
    }
}
