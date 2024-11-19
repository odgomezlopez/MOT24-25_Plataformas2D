using UnityEngine;

public interface IActorController
{
    public IStats Stats { get;}
    public void TakeDamage(float dmg, GameObject org);
    public void Heal(float hl);

}
