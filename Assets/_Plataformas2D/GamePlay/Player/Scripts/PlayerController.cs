using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    //Redefino la variable Stats del tipo PlayerStats con new
    [SerializeField] public new PlayerStats Stats;

    //Referencias a componentes
    PlayerInput playerInput;

    private void Start()
    {
        //Si está en el mismo GameObject
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogWarning("PlayerInput not found on GameObject.");
        }

        //Si está en otro GameObject
        //playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {

    }
}
