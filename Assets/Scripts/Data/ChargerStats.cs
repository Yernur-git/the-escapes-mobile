using UnityEngine;

[CreateAssetMenu(fileName = "ChargerStats", menuName = "Game/Charger Stats")]
public class ChargerStats : ScriptableObject
{
    public float triggerRange, returnSpeed;
    public float homeTolerance, turnSpeed;
    public float sightInterval, idleDelay;
    public float windUp, dashSpeed, dashLength;
    public float stunTime, cooldown;
    public float collisionSkin;
    public int damage;
}
