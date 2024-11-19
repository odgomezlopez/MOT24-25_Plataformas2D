using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : ActorController
{
    [SerializeField] protected PlayerStats stats;

    public override IStats Stats { 
        get => stats;
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }
}
