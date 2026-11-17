using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Charger : MonoBehaviour
{
    enum State { Idle, WindUp, Dash, Stunned }

    [SerializeField] Transform player;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LineRenderer warningLane;
    [SerializeField] Transform visual;
    [SerializeField] TextMesh stateLabel;
    [SerializeField] SpriteRenderer uniform;
    [SerializeField] ChargerStats stats;

    State state;
    Rigidbody2D rb;
    Collider2D hitbox;
    PlayerHealth playerHealth;
    Camera viewCamera;
    ContactFilter2D walls;
    readonly RaycastHit2D[] hits = new RaycastHit2D[1];
    Vector2 home, dashDirection;
    float timer, sightTimer, attackTimer, travelled;
    bool seesPlayer;
    Color normalColor;

    public void Configure(Transform target)
    {
        player = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
        viewCamera = Camera.main;
        home = rb.position;
        normalColor = uniform.color;
        walls.SetLayerMask(obstacleMask);
        walls.useTriggers = false;
        stateLabel.gameObject.SetActive(Debug.isDebugBuild);
        SetState(State.Idle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.zero;
        if (!playerHealth.IsAlive) return;
        timer += Time.fixedDeltaTime;
        attackTimer -= Time.fixedDeltaTime;
        sightTimer -= Time.fixedDeltaTime;
        if (sightTimer <= 0f)
        {
            seesPlayer = CanSeePlayer();
            sightTimer = stats.sightInterval;
        }

        switch (state)
        {
            case State.Idle:
                if (Vector2.Distance(rb.position, home) > stats.homeTolerance)
                {
                    Face(home);
                    rb.MovePosition(Vector2.MoveTowards(rb.position, home,
                        stats.returnSpeed * Time.fixedDeltaTime));
                }
                else if (timer >= stats.idleDelay && attackTimer <= 0f && seesPlayer && OnScreen())
                    SetState(State.WindUp);
                break;
            case State.WindUp:
                if (!OnScreen()) SetState(State.Stunned);
                else
                {
                    Face(player.position);
                    DrawWarning();
                    if (timer >= stats.windUp)
                    {
                        if (CanSeePlayer()) SetState(State.Dash);
                        else SetState(State.Stunned);
                    }
                }
                break;
            case State.Dash:
                float distance = Mathf.Min(stats.dashSpeed * Time.fixedDeltaTime, stats.dashLength - travelled);
                if (!OnScreen() || distance <= 0f ||
                    hitbox.Cast(dashDirection, walls, hits, distance + stats.collisionSkin) > 0)
                    SetState(State.Stunned);
                else
                {
                    rb.MovePosition(rb.position + dashDirection * distance);
                    travelled += distance;
                }
                break;
            case State.Stunned:
                if (timer >= stats.stunTime) SetState(State.Idle);
                break;
        }
    }

    void Face(Vector2 target)
    {
        Vector2 direction = target - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.SetRotation(Mathf.MoveTowardsAngle(rb.rotation, angle, stats.turnSpeed * Time.fixedDeltaTime));
    }

    bool CanSeePlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        if (toPlayer.magnitude > stats.triggerRange) return false;
        return Physics2D.Raycast(rb.position, toPlayer.normalized,
            toPlayer.magnitude, obstacleMask).collider == null;
    }

    bool OnScreen()
    {
        Vector3 p = viewCamera.WorldToViewportPoint(rb.position);
        return p.z > 0f && p.x > 0f && p.x < 1f && p.y > 0f && p.y < 1f;
    }

    Vector2 Facing()
    {
        return Quaternion.Euler(0, 0, rb.rotation) * Vector2.right;
    }

    void DrawWarning()
    {
        Vector2 direction = Facing();
        RaycastHit2D wall = Physics2D.Raycast(rb.position, direction, stats.dashLength, obstacleMask);
        warningLane.SetPosition(0, rb.position);
        warningLane.SetPosition(1, wall.collider != null ? wall.point : rb.position + direction * stats.dashLength);
    }

    void OnCollisionEnter2D(Collision2D collision) { Hit(collision); }
    void OnCollisionStay2D(Collision2D collision) { Hit(collision); }

    void Hit(Collision2D collision)
    {
        if (state != State.Dash || !OnScreen()) return;
        PlayerHealth target = collision.collider.GetComponent<PlayerHealth>();
        if (target != null) target.TakeDamage(stats.damage);
        SetState(State.Stunned);
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        warningLane.enabled = next == State.WindUp;
        stateLabel.text = next == State.WindUp ? "wind-up" : next.ToString().ToLower();
        uniform.color = next == State.Stunned ? normalColor * Color.gray : normalColor;
        if (next == State.WindUp) DrawWarning();
        if (next == State.Dash)
        {
            dashDirection = Facing();
            travelled = 0f;
        }
        if (next == State.Stunned) attackTimer = stats.cooldown;
    }

    void LateUpdate()
    {
        visual.rotation = Quaternion.identity;
    }
}
