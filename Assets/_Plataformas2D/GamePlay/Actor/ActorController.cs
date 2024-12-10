using UnityEngine;

public class ActorController : MonoBehaviour, IActorController
{
    public virtual IStats Stats
    {
        get => throw new
            System.NotImplementedException();
    }

    public ActorStateInfo stateInfo;


    //public Stats Stats { get; }
    public void Heal(float hl)
    {
        Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject org)
    {
        Stats.HP -= dmg;
    }


}
