using UnityEngine;

//[CreateAssetMenu(fileName = "new PlayerStats", menuName = "Actor/PlayerStats")]
[System.Serializable]
public class PlayerStats : IStats
{
    [SerializeField] protected RangedSmartFloat hp;

    public float HP { get => hp.CurrentValue; set => hp.CurrentValue = value; }

    public float speed = 5;
    public float jumpForce = 8f;
}
