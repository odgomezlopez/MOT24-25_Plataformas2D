using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;


[CreateAssetMenu(fileName = "new ActorData", menuName = "Actor/Data", order = 1)]
public class ActorData : ScriptableObject
{
    public Sprite sprite;
    public Color baseColor = Color.white;
    public bool facesRightByDefault = true;

    public RuntimeAnimatorController controller;
    public RigidbodyType2D rigidBodyType = RigidbodyType2D.Dynamic;

    public void ApplyGraphics2D(GameObject g)
    {
        SpriteRenderer renderer = g.GetComponentInChildren<SpriteRenderer>();
        Animator animator = g.GetComponentInChildren<Animator>();
        Rigidbody2D rb = g.GetComponentInChildren<Rigidbody2D>();

        //Barrera
        if (renderer)
        {
            if (sprite) renderer.sprite = sprite;   
         
            renderer.color = baseColor;
            //renderer.flipX = defaultFlip;
            renderer.transform.localScale = new Vector3(Mathf.Abs(renderer.transform.localScale.x) * (facesRightByDefault ? 1 : -1), renderer.transform.localScale.y, renderer.transform.localScale.z);
            AdjustCollider(g, renderer);
        }

        if (animator)
        {
            if(controller) animator.runtimeAnimatorController = controller;
        }

        if (rb)
        {
            rb.bodyType = rigidBodyType;
        }

    }

    private void AdjustCollider(GameObject actor, SpriteRenderer renderer)
    {
        if (renderer == null) return;

        Collider2D col = actor.GetComponentInChildren<Collider2D>();
        if (col)
        {
            if (col is BoxCollider2D boxCol)
            {
                boxCol.size = new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
                boxCol.offset = new Vector2(renderer.bounds.center.x - actor.transform.position.x, renderer.bounds.center.y - actor.transform.position.y);
            }

            if (col is CapsuleCollider2D capCol)
            {
                capCol.size = new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
                capCol.offset = new Vector2(renderer.bounds.center.x - actor.transform.position.x, renderer.bounds.center.y - actor.transform.position.y);
            }
        }
    }
}
