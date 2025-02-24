using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[CreateAssetMenu(fileName = "new AttackPrefab", menuName = "Actions/Attack/Prefab", order = 1)]
public class AttackPrefab : Action
{
    public enum AttackPrefabType { Melee, Distace }
    public AttackPrefabType type = AttackPrefabType.Melee;

    public float damage = 1f;
    public float speed = 1f;

    public GameObject basePrefab;
    public Color attackColor = Color.white;


    //Sobreescribimos el método use
    public override void Use(GameObject g)
    {
        base.Use(g);

        //ACTION 1
        //Instanciamos el ataque dentro del padre
        //SpriteRenderer spriteRenderer = g.GetComponent<SpriteRenderer>();
        FlipSprite2D flipSprite2D = g.GetComponent<FlipSprite2D>();
        
        GameObject attackG;
        if (type == AttackPrefabType.Melee)
        {
            //attackG = Instantiate(basePrefab, spriteRenderer.transform);
            //attackG = Instantiate(basePrefab, flipSprite2D.FlippedTransform);
            //attackG = ObjectPoolManager.Instance.GetObject(basePrefab, flipSprite2D.FlippedTransform);

            attackG = basePrefab.Spawn(flipSprite2D.FlippedTransform, g.layer);
        }
        else{
            //attackG = Instantiate(basePrefab, g.transform.position, g.transform.rotation);
            //GameObject attackG = ObjectPoolManager.Instance.Spawn(basePrefab, g.transform.position, transform.rotation);

            attackG = basePrefab.Spawn(g.transform.position, g.transform.rotation, g.layer);
        }

        //g.layer = g.layer;
        //LayerHelper.SetLayerRecursively(attackG, g.layer);

        //Cambiar el color del ataque
        SpriteRenderer attackRenderer = attackG.GetComponentInChildren<SpriteRenderer>();
        if (attackRenderer)
        {
            attackRenderer.color = attackColor;
        }


        //Actualizo la inf de la flipBox
        var hitBox2D = attackG.GetComponent<HitBox2D>();
        if (hitBox2D)
        {
            hitBox2D.Origin = g.GetComponent<ActorController>() ?? null;
            hitBox2D.Damage = damage;
            hitBox2D.SetTimeLimit(10f);
        }
        else
        {
            Debug.LogError("The attack prefab should have an HitBox2D component");
        }

        //Si es ataque Melee, podemos modificar la velocidad del animator
        if(type == AttackPrefabType.Melee)
        {
            var animator = attackG.GetComponentInChildren<Animator>();
            if (animator) animator.speed = speed;
        }


        //Si es ataque a distancia modificamos los valores del componente MoveFowards2D
        if(type == AttackPrefabType.Distace)
        {
            var moveFowards2D = attackG.GetComponent<MoveFowards2D>();
            if (moveFowards2D)
            {
                moveFowards2D.Speed = speed;
                moveFowards2D.MoveRight = flipSprite2D.IsFacingRight;
                //moveFowards2D.MoveRight = spriteRenderer.transform.localScale.x > 0; //flipSprite2D.IsFacingRight;//; //NOTA. Debe tener en cuenta como rotamos el sprite del jugador
            }
        }
    }
}
