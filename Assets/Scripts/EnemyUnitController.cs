using System.Collections;
using System.Collections.Generic;
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
        //Debug.Log("Inside GetCastleForList()");

        GameObject[] gameObjectHolder = GameObject.FindGameObjectsWithTag("Player");

        //Debug.Log(gameObjectHolder);

        foreach (GameObject castle in gameObjectHolder)
        {
            castleTargetList.Add(castle.GetComponent<Transform>());
        }

    }

    void Update()
    {
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
        Debug.Log(gameObject.name + " Collided with " + collision.gameObject.name);
        touchingPlayer = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        touchingPlayer = false;
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


    private IEnumerator Attack()
    {
        isAttacking = true;
        Debug.Log("Attack Coroutine started");
        yield return new WaitForSeconds(timeToAttack);

        Castle target = castleTarget.GetComponent<Castle>();
        target.TakeDamage(attackingStrength);

        Debug.Log("Attack Over");
        isAttacking = false;
    }
}
