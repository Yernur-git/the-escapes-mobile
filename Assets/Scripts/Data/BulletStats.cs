using UnityEngine;

[CreateAssetMenu(fileName = "BulletStats", menuName = "Game/Bullet Stats")]
public class BulletStats : ScriptableObject
{
    public float speed, lifetime;
    public int damage;
}
