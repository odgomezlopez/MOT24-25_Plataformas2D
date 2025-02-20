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
    [SerializeField] UnityEvent onDie;


    [Header("Maquina de estados")]
    [SerializeField] public StateMachine stateMachine;

    [SerializeField] public EnemyPatrolRayCast2D enemyPatrolRayCast;
    [SerializeField] public SleepState sleepState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        //Inicializo los estados
        //enemyPatrolRayCast = new(gameObject);
        enemyPatrolRayCast.Init(gameObject);

        //sleepState = new(gameObject);
        sleepState.Init(gameObject);

        //Defino el estado inicial
        stateMachine.ChangeState(sleepState);
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




    private void OnEnable()
    {
        stats.hp.OnValueUpdate.AddListener(OnDie);
    }

    private void OnDisable()
    {
        stats.hp.OnValueUpdate.RemoveListener(OnDie);
    }

    private void OnDie(float f)
    {
        if (f <= 0)
        {
            //Se reinicie el nivel
            onDie.Invoke();
            
            Destroy(gameObject);//, 0.5f
            //gameObject.SetActive(false);//TODO desactivar en corutina si quiero lanzar antes algún FX.
        }
    }
}
