using UnityEngine;

[System.Serializable]
public class SleepState : IState
{
    //Referencias a componentes
    EnemyController enemyController;
    GameObject gameObject;
    Rigidbody2D rb;

    //Variables
    [SerializeField] float currentWait = 0f;

    [SerializeField] float waitTime = 2f;

    public SleepState(GameObject g)
    {
        Init(g);
    }

    public void Init(GameObject g)
    {
        gameObject = g;
        enemyController = gameObject.GetComponent<EnemyController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        currentWait = 0f;
        rb.linearVelocityX = 0f;
    }

    public void OnExit()
    {
    }

    public void UpdateState()
    {
        currentWait += Time.deltaTime;

        if (currentWait > waitTime)
            enemyController.stateMachine.ChangeState(enemyController.enemyPatrolRayCast);
    }

    public void FixedUpdateState()
    {
    }
}
