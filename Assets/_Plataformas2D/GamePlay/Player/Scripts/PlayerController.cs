using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    //Redefino la variable Stats del tipo PlayerStats con new
    //[SerializeField] public new PlayerStats Stats {  get; }
    [SerializeField] private PlayerStats stats;
    public override IStats Stats => stats;

    [SerializeField] UnityEvent onDie;

    //Referencias a componentes
    PlayerInput playerInput;



    protected override void Start()
    {
        base.Start();

        //Si está en el mismo GameObject
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogWarning("PlayerInput not found on GameObject.");
        }
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
        if(f <= 0)
        {
            //Se reinicie el nivel
            onDie.Invoke();
            GameObject.FindAnyObjectByType<GameManager>().GameOver();
        }
    }
}
