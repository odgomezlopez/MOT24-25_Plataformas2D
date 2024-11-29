using UnityEngine;

//[CreateAssetMenu(fileName = "new PlayerStats", menuName = "Actor/PlayerStats")]
[System.Serializable]
public class PlayerStats : Stats
{
    //Stats propios solo del jugadorç
    [Header("Movimiento")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float decceleration = 15f;

    //Modificadores
    [SerializeField, Range(1, 3), Tooltip("Multiply the base speed by its value")] private float runSpeedModifier = 1.5f;
    [SerializeField, Range(0, 1)] private float airMomentumModifier = 0.5f;

    
    [Header("Salto")]
    public float jumpForce = 8f;
    [SerializeField, Range(0, 2)] public float gravityScaleDefault = 1f;
    [SerializeField, Range(0, 2)] public float gravityScaleFalling = 1.5f;

    /*[Header("Dash")]
    public float dashForce = 8f;
    public float dashCooldown = 0.5f;*/

    public float GetComputedSpeed(bool isRunning)
    {
        return isRunning ? speed * runSpeedModifier : speed;
    }

    public float GetComputedAccelerationSeconds(bool isGrounded)
    {
        // Base acceleration rate depends on whether the player is grounded
        float computedAcceleration = isGrounded ? acceleration : acceleration * airMomentumModifier;
        // Return the computed acceleration
        return computedAcceleration;
    }

    public float GetComputedDeccelerationSeconds()
    {
        return decceleration;
    }
}
