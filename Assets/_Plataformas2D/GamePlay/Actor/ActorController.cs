using UnityEngine;

public abstract class ActorController : MonoBehaviour, IActorController
{
    public abstract IStats Stats { 
        get; 
    }

    public void Heal(float hl)
    {
        Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject org)
    {
        Stats.HP -= dmg;
    }
}
