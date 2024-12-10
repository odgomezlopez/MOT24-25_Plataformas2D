using UnityEngine;

public class EnemyController : ActorController
{
    [Header("Datos")]
    //Información del enemigo
    [SerializeField] private EnemyStats stats;
    public new EnemyStats Stats { get { return stats; } }

    //Estados del enemigo
    [Header("Estado actual")]
    [SerializeField] string currentStateName = "";

    IState currentState;

    [Header("Estados")]
    [SerializeField] public EnemyPatrolRayCast2D enemyPatrolRayCast;
    [SerializeField] public SleepState sleepState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Inicializo los estados
        enemyPatrolRayCast = new(gameObject);
        sleepState = new(gameObject);

        //Defino el estado inicial
        ChangeState(enemyPatrolRayCast);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.UpdateState();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdateState();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
        currentStateName = currentState.ToString();
    }

}
