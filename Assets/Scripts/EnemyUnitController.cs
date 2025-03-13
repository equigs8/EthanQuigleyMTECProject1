using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnitController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control movement speed

    private Rigidbody2D rb;

    private GameManager gameManager;

    public Transform castleTarget;

    public List<Transform> castleTargetList = new List<Transform>();

    public bool touchingPlayer;
    public float timeToAttack = 10f;

    public bool isAttacking;

    public int attackingStrength;

    public HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;
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
        touchingPlayer = false;
        isAttacking = false;
        //Debug.Log("Before GetCastleForList()");
        GetCastlesForList();
        //Debug.Log("After GetCastleForList()");

    }

    private void GetCastlesForList()
    {
        castleTargetList = new List<Transform>();
        //Debug.Log("Inside GetCastleForList()");

        GameObject[] gameObjectHolder = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] gameObjectHolderUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Debug.Log(gameObjectHolder);

        foreach (GameObject castle in gameObjectHolder)
        {
            castleTargetList.Add(castle.GetComponent<Transform>());
        }
        foreach (GameObject unit in gameObjectHolderUnits)
        {
            castleTargetList.Add(unit.GetComponent<Transform>());
        }

    }

    void Update()
    {
        castleTargetList = new List<Transform>();
        GetCastlesForList();
        FindClosestCastle();

        if (castleTarget == null)
        {
            Debug.LogWarning("castleTarget is Null");
            return;
        }

        if (touchingPlayer && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (!touchingPlayer && castleTarget != null)
        {
            Vector3 direction = castleTarget.position - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
        }



    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("border"))
        {
            Debug.Log(gameObject.name + " Collided with " + collision.gameObject.name);
        }
        
        //touchingPlayer = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("border"))
        {
            Debug.Log(gameObject.name + " Is no longer Colliding with " + collision.gameObject.name);
            touchingPlayer = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("border"))
        {
            touchingPlayer = true;
        }
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
            Debug.Log("Closest target found: " + castleTarget.name);
        }
        else
        {
            Debug.Log("No targets found.");
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

    private IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Enemy Attack Coroutine started");
        yield return new WaitForSeconds(timeToAttack);
        if (gameObject != null)
        {
            Debug.Log("Enemy gameObject not Null");
            UnitController target = castleTarget.GetComponent<UnitController>();
            target.TakeDamage(attackingStrength);
        }

        Debug.Log("Enemy Attack Over");
        isAttacking = false;
    }
}
