using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


[CreateAssetMenu(fileName = "new AttackPrefab", menuName = "Actions/Attack/Prefab", order = 1)]
public class AttackPrefab : Attack
{
    public enum AttackPrefabType { Melee, Distace }

    [Header("Prefab")]
    public AttackPrefabType type = AttackPrefabType.Melee;

    public float prefabSpeed = 1f;
    public GameObject basePrefab;
    public Color attackColor = Color.white;

    [Header("Hitbox Config")]
    public HitBoxTriggerAction hitBoxTriggerAction = HitBoxTriggerAction.Release;
    public string obstacleTag = "Floor";
    public bool triggerOnObstacles = true;
    public float extraTime = 0f;
    public float hitboxLifetime = 10f; // How long the hitbox stays before disappearing



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

        attackG.SetActive(false);

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
            hitBox2D.SetTimeLimit(hitboxLifetime);

            // Apply the HitBox configuration
            hitBox2D.triggerAction = hitBoxTriggerAction;
            hitBox2D.obstacleTag = obstacleTag;
            hitBox2D.triggerOnObstacles = triggerOnObstacles;
            hitBox2D.extraWaitTime = extraTime;
        }
        else
        {
            Debug.LogError("The attack prefab should have an HitBox2D component");
        }

        //Si es ataque Melee, podemos modificar la velocidad del animator
        if(type == AttackPrefabType.Melee)
        {
            var animator = attackG.GetComponentInChildren<Animator>();
            if (animator) animator.speed = prefabSpeed;
        }


        //Si es ataque a distancia modificamos los valores del componente MoveFowards2D
        if(type == AttackPrefabType.Distace)
        {
            var moveFowards2D = attackG.GetComponent<MoveFowards2D>();
            if (moveFowards2D)
            {
                moveFowards2D.Speed = prefabSpeed;
                moveFowards2D.MoveRight = (flipSprite2D) ? flipSprite2D.IsFacingRight : true;
                //moveFowards2D.MoveRight = spriteRenderer.transform.localScale.x > 0; //flipSprite2D.IsFacingRight;//; //NOTA. Debe tener en cuenta como rotamos el sprite del jugador
            }

            //Rotamos para que mire en la dirección en la que se lanza
            Vector3 scale = attackG.transform.localScale;
            scale.x = (flipSprite2D.IsFacingRight ? 1 : -1) * Mathf.Abs(scale.x);
            attackG.transform.localScale = scale;
        }

        //Activamos el ataque
        attackG.SetActive(true);
    }
}
