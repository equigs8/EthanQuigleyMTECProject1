using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCard : MonoBehaviour
{

    public float manaCost;
    public Transform enemyCardPositionTransform;
    public float enemyCardMoveSpeed;
    public bool cardPlayed;
    public GameObject enemyUnitPrefab;
    [SerializeField] private float _dissolveTime = 0.75f;

    public SpriteRenderer _spriteRenderer;
    private Material _material;
    public GameManager gameManager;
    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    //public Rigidbody2D rb;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _material = _spriteRenderer.material;
        cardPlayed = false;
    }

    public float GetManaCost()
    {
        return manaCost;
    }
    private void Update()
    {
        if (!cardPlayed)
        {

            if (enemyCardPositionTransform.position.x == transform.position.x && enemyCardPositionTransform.position.y == transform.position.y)
            {
                StartCoroutine(Vanish());
            }
            Vector3 direction = enemyCardPositionTransform.position - transform.position;
            direction.Normalize();
            transform.position += direction * enemyCardMoveSpeed * Time.deltaTime;
            
        }

    }

    internal void SetDestination(Transform transform)
    {
        enemyCardPositionTransform = transform;
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
        GameObject enemyUnit = Instantiate(enemyUnitPrefab, transform.position,Quaternion.identity) as GameObject;
        cardPlayed = true;
        Destroy(gameObject);
    }
}
