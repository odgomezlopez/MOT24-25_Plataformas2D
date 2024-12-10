using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class EnemyPatrolRayCast2D : IState
{
    //Referencias a componentes
    EnemyController enemyController;
    EnemyStats enemyStats => (EnemyStats) enemyController.Stats;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    GameObject gameObject;

    [SerializeField] bool currentDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public EnemyPatrolRayCast2D(GameObject g)
    {
        gameObject = g;
        enemyController = g.GetComponent<EnemyController>();
        rb = g.GetComponent<Rigidbody2D>();
        spriteRenderer = g.GetComponentInChildren<SpriteRenderer>();
    }

    public void OnEnter()
    {
        enemyController.stateInfo.hasWallFound.OnValueUpdate.AddListener(ChangeDirection);
        enemyController.stateInfo.hasFallFound.OnValueUpdate.AddListener(ChangeDirection);
    }

    public void UpdateState()
    {
        //transform.position = Vector2.MoveTowards(transform.position, currentTarget,enemyStats.speed * Time.deltaTime);
        //rb.MovePosition(Vector2.MoveTowards(transform.position, currentTarget, enemyStats.speed * Time.deltaTime));
        rb.linearVelocityX = enemyStats.speed * (currentDirection ? 1 : -1);
     }

    public void FixedUpdateState()
    {
    }

    public void OnExit()
    {
        enemyController.stateInfo.hasWallFound.OnValueUpdate.RemoveListener(ChangeDirection);
        enemyController.stateInfo.hasFallFound.OnValueUpdate.RemoveListener(ChangeDirection);
    }

    void ChangeDirection(bool state)
    {
        if (state) {  
            currentDirection = !currentDirection;
            Flip(currentDirection ^ !enemyController.stateInfo.isSpriteFlippedByDefault);

            enemyController.ChangeState(enemyController.sleepState);
        }
    }

    void Flip(bool state)
    {
        spriteRenderer.flipX = state;
    }

 
}
