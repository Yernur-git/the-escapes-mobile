using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] BulletStats stats;
    [SerializeField] LayerMask obstacleMask;

    bool spent;

    public void Launch(Vector2 direction)
    {
        GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * stats.speed;
        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        Destroy(gameObject, stats.lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (spent) return;
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            spent = true;
            player.TakeDamage(stats.damage);
            Destroy(gameObject);
        }
        else if ((obstacleMask.value & (1 << other.gameObject.layer)) != 0)
        {
            spent = true;
            Destroy(gameObject);
        }
    }
}
