using UnityEngine;

public interface IActorController
{
    public void TakeDamage(float dmg, GameObject org, ActorController actorOrigin);
    public void Heal(float hl);

}
