using UnityEngine;
using UnityEngine.Events;

public class EnemyController : ActorController
{
    [Header("Datos")]
    //Información del enemigo
    [SerializeField] private EnemyStats stats;

    //public new EnemyStats Stats { get { return stats; } }
    public override IStats Stats => stats;
    [SerializeField] UnityEvent onDie;

    //Estados del enemigo
    [Header("Estado actual")]
    [SerializeField] string currentStateName = "";

    IState currentState;

    [Header("Estados")]
    [SerializeField] public EnemyPatrolRayCast2D enemyPatrolRayCast;
    [SerializeField] public SleepState sleepState;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

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
            
            //Destroy(gameObject, 1f);
            gameObject.SetActive(false);//TODO desactivar en corutina si quiero lanzar antes algún FX.
        }
    }
}
