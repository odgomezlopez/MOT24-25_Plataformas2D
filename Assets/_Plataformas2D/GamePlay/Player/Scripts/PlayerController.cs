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


    //Referencias a componentes
    PlayerInput playerInput;

    protected override void Start()
    {

        base.Start();

        //Si está en el mismo GameObject
        playerInput = FindFirstObjectByType<PlayerInput>();
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
        Time.timeScale = 1f;
    }

    private void OnDie(float f)
    {
        if(f <= 0)
        {
            //Se reinicie el nivel
            onDie.Invoke();
            Time.timeScale = 0.5f;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponentInChildren<Animator>().Play("hurt");
            playerInput.DeactivateInput();

            //GameObject.FindAnyObjectByType<GameManager>().GameOver();
            GameManager.Instance.GameOver();
        }
    }
}
