using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Patroller : MonoBehaviour
{
    enum State { Patrol, Chase, Attack, Recovery }

    [SerializeField] Transform player;
    [SerializeField] Transform[] points;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float patrolSpeed = 1.2f, chaseSpeed = 2.6f;
    [SerializeField] float viewRange = 4.5f, fov = 90f;
    [SerializeField] float sightInterval = .15f, loseTime = 2f;
    [SerializeField] float attackRange = 1f, windUp = .6f;
    [SerializeField] float recovery = .6f, cooldown = 1.5f;
    [SerializeField] float waypointDistance = .08f;
    [SerializeField] int damage = 1;
    [SerializeField] SpriteRenderer warning;
    [SerializeField] TextMesh stateLabel;
    [SerializeField] Transform visual;

    State state;
    Rigidbody2D rb;
    PlayerHealth playerHealth;
    Camera viewCamera;
    int point;
    float timer, unseenTime, sightTimer, attackTimer;
    bool seesPlayer;
    Vector2 lastSeen;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
        viewCamera = Camera.main;
        stateLabel.gameObject.SetActive(Debug.isDebugBuild);
        SetState(State.Patrol);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.zero;
        if (!playerHealth.IsAlive) return;
        float dt = Time.fixedDeltaTime;
        timer += dt;
        attackTimer -= dt;
        sightTimer -= dt;
        if (sightTimer <= 0f)
        {
            seesPlayer = CanSeePlayer();
            sightTimer = sightInterval;
            if (seesPlayer) lastSeen = player.position;
        }

        switch (state)
        {
            case State.Patrol:
                Patrol();
                if (seesPlayer) SetState(State.Chase);
                break;
            case State.Chase:
                unseenTime = seesPlayer ? 0f : unseenTime + dt;
                MoveTo(lastSeen, chaseSpeed);
                if (seesPlayer && DistanceToPlayer() <= attackRange &&
                    attackTimer <= 0f && OnScreen())
                    SetState(State.Attack);
                else if (unseenTime >= loseTime)
                    SetState(State.Patrol);
                break;
            case State.Attack:
                if (!OnScreen()) SetState(State.Recovery);
                else if (timer >= windUp)
                {
                    if (DistanceToPlayer() <= attackRange && ClearLine())
                        playerHealth.TakeDamage(damage);
                    attackTimer = cooldown;
                    SetState(State.Recovery);
                }
                break;
            case State.Recovery:
                if (timer >= recovery)
                    SetState(seesPlayer ? State.Chase : State.Patrol);
                break;
        }
    }

    void Patrol()
    {
        if (points.Length == 0) return;
        Vector2 target = points[point].position;
        MoveTo(target, patrolSpeed);
        if (Vector2.Distance(rb.position, target) < waypointDistance)
            point = (point + 1) % points.Length;
    }

    void MoveTo(Vector2 target, float speed)
    {
        Vector2 direction = target - rb.position;
        if (direction.sqrMagnitude > waypointDistance * waypointDistance)
            rb.SetRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
    }

    bool CanSeePlayer()
    {
        Vector2 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > viewRange) return false;
        if (Vector2.Angle(transform.right, toPlayer) > fov / 2f) return false;
        return ClearLine();
    }

    bool ClearLine()
    {
        Vector2 toPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rb.position, toPlayer.normalized,
            toPlayer.magnitude, obstacleMask);
        return hit.collider == null;
    }

    bool OnScreen()
    {
        Vector3 p = viewCamera.WorldToViewportPoint(transform.position);
        return p.z > 0f && p.x > 0f && p.x < 1f && p.y > 0f && p.y < 1f;
    }

    float DistanceToPlayer()
    {
        return Vector2.Distance(rb.position, player.position);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        unseenTime = 0f;
        warning.enabled = next == State.Attack;
        stateLabel.text = next.ToString().ToLower();
    }

    void LateUpdate()
    {
        visual.rotation = Quaternion.identity;
        stateLabel.transform.rotation = Quaternion.identity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position,
            Quaternion.Euler(0f, 0f, -fov / 2f) * transform.right * viewRange);
        Gizmos.DrawRay(transform.position,
            Quaternion.Euler(0f, 0f, fov / 2f) * transform.right * viewRange);
    }
}
