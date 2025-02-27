using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ActorController : MonoBehaviour, IActorController
{
    public virtual IStats Stats
    {
        get => throw new
            System.NotImplementedException();
    }

    [Expandable] public ActorData actorData;
    private ActorAnimator actorAnimator;
    public FlipSprite2D flipSprite2D;

    protected virtual void Awake()
    {
        actorAnimator = GetComponent<ActorAnimator>();
        actorData?.ApplyGraphics2D(gameObject);
        Stats.Reset();
    }
    protected virtual void Start()
    {


    }


    //public Stats Stats { get; }
    public void Heal(float hl)
    {
        Stats.HP += hl;
    }

    public void TakeDamage(float dmg, GameObject org, ActorController attackOrigin = null)
    {
        if (this == attackOrigin) return;
        Stats.HP -= dmg;
        if(Stats.HP > 0)actorAnimator.SetTrigger("OnDamage");
    }

    public void Die()
    {
        actorAnimator.SetTrigger("OnDie");
        Stats.HP = 0f;
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
