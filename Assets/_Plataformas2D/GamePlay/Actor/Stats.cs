using UnityEngine;

[System.Serializable]
public class Stats : IStats

{
    [SerializeField] protected float hp; // Fuente de datos, modificable
    public float HP { get => hp; set => hp = value; }
}
