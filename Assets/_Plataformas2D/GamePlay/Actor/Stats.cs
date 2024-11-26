using UnityEngine;

[System.Serializable]
public class Stats : IStats

{
    [Header("Basic Data")]
    public string actorName = "Default name";


    [Header("HP data")]
    [SerializeField] protected RangedSmartFloat hp;
    public float HP { get => hp.CurrentValue; set => hp.CurrentValue = value; }


    //Metodos

    public void Reset() { 
        hp.Reset(); 
    }

    public void Update(Stats newStats)
    {
        hp.Update(newStats.hp);
    }
}
