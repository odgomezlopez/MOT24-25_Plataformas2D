using UnityEngine;

public class EnemyController : ActorController
{

    [SerializeField] public Stats stats;
    public override IStats Stats => stats; // Return the concrete stats as IStats.


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
