using UnityEngine;

public class ActorController : MonoBehaviour, IActorController
{
    public Stats Stats { get; }
    public ActorStateInfo stateInfo;
    public void Heal(float hl)
    {
        Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject org)
    {
        Stats.HP -= dmg;
    }
}