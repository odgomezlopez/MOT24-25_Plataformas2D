using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RayCastChecker2D))]
public class EnemyController : ActorController
{
    [Header("Datos")]
    //Información del enemigo
    [SerializeField] private EnemyStats stats;

    //public new EnemyStats Stats { get { return stats; } }
    public override IStats Stats => stats;


    [Header("Maquina de estados")]
    [SerializeField] public StateMachine stateMachine;

    [SerializeField] public EnemyPatrolRayCast2D enemyPatrolRayCast;
    [SerializeField] public SleepState sleepState;
    [SerializeField] public DieState dieState;

    //Referencias
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //Referencias
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        //Inicializo componentes
        //rb.bodyType = RigidbodyType2D.Dynamic;

        //Inicializo los estados
        enemyPatrolRayCast.Init(gameObject);
        sleepState.Init(gameObject);
        dieState.Init(gameObject);


        //Defino el estado inicial
        stateMachine.ChangeState(sleepState);
        
        //Eventos
        stats.hp.OnValueUpdate.AddListener(OnDie);
    }

    private void OnDisable()
    {
        stats.hp.OnValueUpdate.RemoveListener(OnDie);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState?.UpdateState();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState?.FixedUpdateState();
    }

    private void OnDie(float f)
    {
        if (f <= 0)
        {
            stateMachine.ChangeState(dieState);
        }
    }
}
