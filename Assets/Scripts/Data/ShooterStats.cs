using UnityEngine;

[CreateAssetMenu(fileName = "ShooterStats", menuName = "Game/Shooter Stats")]
public class ShooterStats : ScriptableObject
{
    public float viewRange, preferredDistance;
    public float distanceMargin, moveSpeed;
    public float homeRange, returnSpeed;
    public float homeTolerance, turnSpeed;
    public float sightInterval, loseTime;
    public float repositionTime, windUp, cooldown;
}
