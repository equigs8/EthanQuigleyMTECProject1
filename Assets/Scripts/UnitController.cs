using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UnitController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control movement speed

    private Rigidbody2D rb;

    public bool isControlling = false;

    public GameObject unitUI;

    private GameManager gameManager;

    public Transform castleTarget;

    public List<Transform> castleTargetList = new List<Transform>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("gameManager is null in unitController attached to object " + gameObject.name);
        }
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on this GameObject!");
        }

        GetCastlesForList();

        
    }

    private void GetCastlesForList()
    {
        Debug.Log("Inside GetCastleForList()");

        GameObject[] gameObjectHolder = GameObject.FindGameObjectsWithTag("Castle");

        Debug.Log(gameObjectHolder);

        foreach (GameObject castle in gameObjectHolder)
        {
            castleTargetList.Add(castle.GetComponent<Transform>());
        }

    }

    void Update()
    {
        if (isControlling)
        {
            unitUI.SetActive(true);

            // Get input from WASD keys
            float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 for Left, 0 for None, 1 for Right
            float verticalInput = Input.GetAxisRaw("Vertical");   // -1 for Down, 0 for None, 1 for Up

            // Calculate movement direction
            Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized; // .normalized makes diagonal movement not faster

            // Apply movement using Rigidbody2D for physics-based movement
            rb.linearVelocity = moveDirection * moveSpeed;


            //  Alternative using transform.position (less realistic physics, but can be useful)
            //  Vector2 moveAmount = moveDirection * moveSpeed * Time.deltaTime; // Time.deltaTime makes movement frame-rate independent
            //  transform.position += new Vector3(moveAmount.x, moveAmount.y, 0f); // Assuming 2D, z is 0
        }
        else
        {
            unitUI.SetActive(false);
            RunAI();
        }
    }

    private void RunAI()
    {
        FindClosestCastle();
        
        if (castleTarget == null)
        {
            Debug.LogWarning("castleTarget is Null");
        }
        Vector3 direction = castleTarget.position - transform.position;
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime; 
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(gameObject.name + " Collided with " + collision.gameObject.name);
    }

    public void FindClosestCastle()
    {
        Debug.Log("castleTargetList Count = " + castleTargetList.Count);

        if (castleTarget == null && castleTargetList.Count == 0)
        {
            Debug.Log("No Castles in Targeting List");
            return;
        }

        Transform closestCastle = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform castle in castleTargetList)
        {
            
            float distance = Vector3.Distance(transform.position, castle.position);
            //Debug.Log("Distance from " + gameObject.name + " to " + castle.name + " is " + distance);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCastle = castle;
            }
        }

        castleTarget = closestCastle;

        if (castleTarget != null)
        {
            Debug.Log("Closest target found: " + castleTarget.name);
        }
        else
        {
            Debug.Log("No targets found.");
        }
    }

    private void OnMouseDown()
    {
        if (!gameManager.CheckIfControlling())
        {
            isControlling = true;
            gameManager.ControllingUnit(true);
        }
        
    }

    public void Leave()
    {
        isControlling = false;
        gameManager.ControllingUnit(false);
    }
}
