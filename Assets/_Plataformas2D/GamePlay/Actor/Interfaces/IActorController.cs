using UnityEngine;

public interface IActorController
{
    public void TakeDamage(float dmg, GameObject org);
    public void Heal(float hl);

}
