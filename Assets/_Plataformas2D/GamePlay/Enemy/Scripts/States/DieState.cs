using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DieState : IState
{
    //Referencias a componentes
    ActorController actorController;
    GameObject gameObject;
    Rigidbody2D rb;
    Animator animator;


    //Variables
    [SerializeField] string animationTrigger = "";
    bool hasAnimationEnded = false;
    int enterAnimationHash;

    [SerializeField] UnityEvent onDie;

    public DieState(GameObject g)
    {
        Init(g);
    }

    public void Init(GameObject g)
    {
        gameObject = g;
        actorController = gameObject.GetComponent<ActorController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    public void OnEnter()
    {
        //Init. The enter animationHash avoid ending when the previous animation ends.
        hasAnimationEnded = false;
        enterAnimationHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;

        //Trigger Die Animation
        if (animationTrigger != "" && animator) animator.SetTrigger(animationTrigger);
        onDie.Invoke();

        //Disable RigidBody
        rb.linearVelocity = Vector3.zero;
        rb.Sleep();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Disable all Collider2D components
        Collider2D[] colliders2D = gameObject.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders2D)
        {
            col.enabled = false;
        }

        // Disable all 3D Collider components
        //Collider[] colliders3D = gameObject.GetComponentsInChildren<Collider>();
        //foreach (Collider col in colliders3D)
        //{
        //    col.enabled = false;
        //}

        // Disable all Canvas components
        Canvas[] canvases = gameObject.GetComponentsInChildren<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.enabled = false;
        }
    }

    public void OnExit()
    {
    }

    public void UpdateState()
    {
        if(hasAnimationEnded) return;
        if(animator == null) {
            OnDieAnimationEnds();
            return;
        }

        // When normalizedTime >= 1.0, the animation has played fully
        if (IsAnimationEnded())
        {
            OnDieAnimationEnds();
        }
    }

    private bool IsAnimationEnded()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (enterAnimationHash == animator.GetCurrentAnimatorStateInfo(0).fullPathHash) return false;
        // Optionally, ensure you’re in the correct animation state:
        // if (!stateInfo.IsName("DieAnimation")) return;
        return (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
    }

    private void OnDieAnimationEnds()
    {
        hasAnimationEnded = true;

        //Disable rigidBody



        // Now either destroy or disable the GameObject:
        // Option A: Permanently remove the object from the scene
        //Object.Destroy(gameObject);

        // Option B: Simply disable the GameObject (it will no longer be active in the scene)
        //gameObject.SetActive(false);

        gameObject.Release();
    }

    public void FixedUpdateState()
    {
    }
}
