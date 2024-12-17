using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ActorController : MonoBehaviour, IActorController
{
    public virtual IStats Stats
    {
        get => throw new
            System.NotImplementedException();
    }

    public ActorStateInfo stateInfo;
    [Expandable] public ActorData actorData;

    protected virtual void Start()
    {
        Stats.Reset();
    }


    //public Stats Stats { get; }
    public void Heal(float hl)
    {
        Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject org)
    {
        //if(org != gameObject)
        Stats.HP -= dmg;
    }

    #if UNITY_EDITOR
    public virtual void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += _OnValidate;
    }
    #endif

    private void _OnValidate()
    {
        if (this == null) return;
        actorData?.ApplyGraphics2D(gameObject);
    }


}
