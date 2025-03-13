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

    public bool touchingEnemy;
    public float timeToAttack = 10f;

    public bool isAttacking;

    public int attackingStrength;

    public HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;

    public bool isEnemy;

    public UnitType unitType;
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
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        touchingEnemy = false;
        isAttacking = false;
        Debug.Log("Before GetCastleForList()");
        //GetCastlesForList();
        Debug.Log("After GetCastleForList()");

    }

    private void GetCastlesForList()
    {
        Debug.Log("Inside GetCastleForList()");




        if (isEnemy)
        {
            GameObject[] gameObjectHolder = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("Unit");
            AddToCastleTargetList(gameObjectHolder, enemyUnits);
        }
        else
        {
            GameObject[] gameObjectHolder = GameObject.FindGameObjectsWithTag("Castle");
            GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
            AddToCastleTargetList(gameObjectHolder, enemyUnits);
        }

        

    }

    private void AddToCastleTargetList(GameObject[] list1, GameObject[] list2)
    {
        foreach (GameObject castle in list1)
        {
            castleTargetList.Add(castle.GetComponent<Transform>());
        }
        foreach (GameObject enemyUnit in list2)
        {
            castleTargetList.Add(enemyUnit.GetComponent<Transform>());
        }
    }

    void Update()
    {
        castleTargetList = new List<Transform>();
        GetCastlesForList();

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
            if (!isEnemy)
            {
                unitUI.SetActive(false);
            }
            
            ComputerControllsUnit();
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealth();
    }
    void UpdateHealth()
    {
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ComputerControllsUnit()
    {
        
        FindClosestCastle();

        if (castleTarget == null)
        {
            Debug.LogWarning("castleTarget is Null");
            return;
        }
        
        if (touchingEnemy && !isAttacking)
        {
            StartCoroutine(Attack(castleTarget));
        }else if(!touchingEnemy && castleTarget != null)
        {
            Vector3 direction = castleTarget.position - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;   
        }

        
        
    }

    internal void SetUnitType(UnitType type)
    {
        if (type != null)
        {
            unitType = type;
            name = type.name;
            gameObject.GetComponent<SpriteRenderer>().sprite = type.unitSprite;
            attackingStrength = type.attack;
            maxHealth = type.health;
            currentHealth = maxHealth;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Border"))
        { 
            Debug.Log(gameObject.name + " Collided with " + collision.gameObject.name);
            //touchingEnemy = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Border"))
        {
            touchingEnemy = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Border"))
        {
            touchingEnemy = true;
        }
    }

    public void FindClosestCastle()
    {
        //Debug.Log("castleTargetList Count = " + castleTargetList.Count);

        if (castleTarget == null && castleTargetList.Count == 0)
        {
            Debug.Log("No Castles in Targeting List");
            return;
        }

        Transform closestCastle = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform castle in castleTargetList)
        {
            if (castle != null)
            {
                float distance = Vector3.Distance(transform.position, castle.position);
                //Debug.Log("Distance from " + gameObject.name + " to " + castle.name + " is " + distance);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCastle = castle;
                }
            }
        }

        castleTarget = closestCastle;

        if (castleTarget != null)
        {
            //Debug.Log("Closest target found: " + castleTarget.name);
        }
        else
        {
            Debug.Log("No targets found.");
        }
    }

    private void OnMouseDown()
    {
        if (!gameManager.CheckIfControlling() && isEnemy)
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

    private IEnumerator Attack(Transform castleTarget)
    {
        isAttacking = true;
        Debug.Log(gameObject.name + " Attack Coroutine started");
        yield return new WaitForSeconds(timeToAttack);
        if (gameObject != null && castleTarget != null)
        {
            Debug.Log(gameObject.name + " Is Attacking " + castleTarget.name);
            if (castleTarget.CompareTag("Castle") || castleTarget.CompareTag("Player"))
            {
                Castle target = castleTarget.GetComponent<Castle>();
                target.TakeDamage(attackingStrength);

            }
            else
            {
                UnitController target = castleTarget.GetComponent<UnitController>();
                target.TakeDamage(attackingStrength);
            }
        }
        Debug.Log(gameObject.name + " Attack Over");
        isAttacking = false;
    }
}
