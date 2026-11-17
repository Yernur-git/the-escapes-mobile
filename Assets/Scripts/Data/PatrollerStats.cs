using UnityEngine;

[CreateAssetMenu(fileName = "PatrollerStats", menuName = "Game/Patroller Stats")]
public class PatrollerStats : ScriptableObject
{
    public float patrolSpeed, chaseSpeed;
    public float viewRange, fov;
    public float sightInterval, loseTime;
    public float attackRange, windUp;
    public float recovery, cooldown;
    public float waypointDistance;
    public int damage;
}
