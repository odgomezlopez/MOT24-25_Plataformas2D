using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class EnemyPatrolRayCast2D : IState
{
    // References to components
    private EnemyController enemyController;
    private EnemyStats enemyStats => (EnemyStats)enemyController.Stats;
    private Rigidbody2D rb;
    private RayCastChecker2D rayCastInfo;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private bool currentDirectionRight; // True = Right, False = Left

    // Constructor
    public EnemyPatrolRayCast2D(GameObject g)
    {
        enemyController = g.GetComponent<EnemyController>();
        rb = g.GetComponent<Rigidbody2D>();
        spriteRenderer = g.GetComponentInChildren<SpriteRenderer>();
        rayCastInfo = g.GetComponentInChildren<RayCastChecker2D>();

        // Initialize direction based on flipX
        currentDirectionRight = true; // (g.transform.position.x < 0 ? true : false);
    }

    public void OnEnter()
    {
        rayCastInfo.hasWallFound.OnValueUpdate.AddListener(ChangeDirection);
        rayCastInfo.hasFallFound.OnValueUpdate.AddListener(ChangeDirection);
    }

    public void UpdateState()
    {
        // Move in the current direction
        float directionMultiplier = currentDirectionRight ? 1 : -1;
        rb.linearVelocity = new Vector2(enemyStats.speed * directionMultiplier, rb.linearVelocity.y);
    }

    public void FixedUpdateState()
    {
        // Optional fixed update logic
    }

    public void OnExit()
    {
        rayCastInfo.hasWallFound.OnValueUpdate.RemoveListener(ChangeDirection);
        rayCastInfo.hasFallFound.OnValueUpdate.RemoveListener(ChangeDirection);
    }

    private void ChangeDirection(bool state)
    {
        if (state)
        {
            currentDirectionRight = !currentDirectionRight;

            enemyController.stateMachine.ChangeState(enemyController.sleepState);
        }
    }


}
