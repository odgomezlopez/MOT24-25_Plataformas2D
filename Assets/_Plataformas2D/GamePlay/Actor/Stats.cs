using UnityEngine;

[System.Serializable]
public class Stats : IStats

{
    [SerializeField] public RangedSmartFloat hp;
    public float HP { get => hp.CurrentValue; set => hp.CurrentValue = value; }
}
