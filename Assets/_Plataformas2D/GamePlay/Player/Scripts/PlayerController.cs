using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ActorController
{
    //Defino los stats a usar
    PlayerInput playerInput;

    [SerializeField] public PlayerStats stats;
    public override IStats Stats => stats; // Return the concrete stats as IStats.



    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {

    }
}
