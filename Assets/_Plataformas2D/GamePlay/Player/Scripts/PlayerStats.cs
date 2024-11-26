using UnityEngine;

//[CreateAssetMenu(fileName = "new PlayerStats", menuName = "Actor/PlayerStats")]
[System.Serializable]
public class PlayerStats : Stats
{
    //Stats propios solo del jugador
    public float speed = 5;
    public float jumpForce = 8f;

    public float runSpeedModifier = 1.5f;
    public float airMomentumModifier = 0.5f;
}
