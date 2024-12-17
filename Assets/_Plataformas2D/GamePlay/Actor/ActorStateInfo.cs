using UnityEngine;

[System.Serializable]
public class ActorStateInfo
{
    [Tooltip("Current flipped state")]public bool isFlipped;
    //[Tooltip("Is the sprite flipped by default?")] public bool isSpriteFlippedByDefault = false;

    public  SmartVariable<bool> isGrounded;
    public SmartVariable<bool> hasWallFound;
    public SmartVariable<bool> hasFallFound;
}
