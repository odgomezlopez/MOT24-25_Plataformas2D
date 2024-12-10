using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyPatrol : MonoBehaviour
{
    //Parámetros
    [SerializeField] float patrolDistance = 5f;

    //[SerializeField] List<Transform> patrulla;

    Vector2 posInicial, posDestino;
    Vector2 currentTarget;

    //Referencias a componentes
    EnemyController enemyController;
    EnemyStats enemyStats => (EnemyStats) enemyController.Stats;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        posInicial = transform.position;
        posDestino = new Vector2(posInicial.x - patrolDistance, posInicial.y);

        currentTarget = posDestino;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, currentTarget,enemyStats.speed * Time.deltaTime);
        rb.MovePosition(Vector2.MoveTowards(transform.position, currentTarget, enemyStats.speed * Time.deltaTime));
        //rb.linearVelocityX = enemyStats.speed * -Mathf.Sign(transform.position.x-currentTarget.x);
       
        Flip(Mathf.Sign(transform.position.x - currentTarget.x));

        if (Vector2.Distance(transform.position,currentTarget) < 0.1)
        {
            if(currentTarget == posDestino) currentTarget = posInicial;
            else currentTarget = posDestino;
        }
    }

    void Flip(float dir)
    {
        if (dir != 0)
            spriteRenderer.flipX = dir < 0;
    }
}
