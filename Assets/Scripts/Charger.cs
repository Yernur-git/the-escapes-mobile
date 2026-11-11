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
    [SerializeField] float triggerRange = 6f, returnSpeed = 1.8f;
    [SerializeField] float homeTolerance = .3f, turnSpeed = 460f;
    [SerializeField] float sightInterval = .15f, idleDelay = .3f;
    [SerializeField] float windUp = .7f, dashSpeed = 11f, dashLength = 6f;
    [SerializeField] float stunTime = 1f, cooldown = 1.5f;
    [SerializeField] float collisionSkin = .02f;
    [SerializeField] int damage = 1;

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
            sightTimer = sightInterval;
        }

        switch (state)
        {
            case State.Idle:
                if (Vector2.Distance(rb.position, home) > homeTolerance)
                {
                    Face(home);
                    rb.MovePosition(Vector2.MoveTowards(rb.position, home,
                        returnSpeed * Time.fixedDeltaTime));
                }
                else if (timer >= idleDelay && attackTimer <= 0f && seesPlayer && OnScreen())
                    SetState(State.WindUp);
                break;
            case State.WindUp:
                if (!OnScreen()) SetState(State.Stunned);
                else
                {
                    Face(player.position);
                    DrawWarning();
                    if (timer >= windUp)
                    {
                        if (CanSeePlayer()) SetState(State.Dash);
                        else SetState(State.Stunned);
                    }
                }
                break;
            case State.Dash:
                float distance = Mathf.Min(dashSpeed * Time.fixedDeltaTime, dashLength - travelled);
                if (!OnScreen() || distance <= 0f ||
                    hitbox.Cast(dashDirection, walls, hits, distance + collisionSkin) > 0)
                    SetState(State.Stunned);
                else
                {
                    rb.MovePosition(rb.position + dashDirection * distance);
                    travelled += distance;
                }
                break;
            case State.Stunned:
                if (timer >= stunTime) SetState(State.Idle);
                break;
        }
    }

    void Face(Vector2 target)
    {
        Vector2 direction = target - rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.SetRotation(Mathf.MoveTowardsAngle(rb.rotation, angle, turnSpeed * Time.fixedDeltaTime));
    }

    bool CanSeePlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        if (toPlayer.magnitude > triggerRange) return false;
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
        RaycastHit2D wall = Physics2D.Raycast(rb.position, direction, dashLength, obstacleMask);
        warningLane.SetPosition(0, rb.position);
        warningLane.SetPosition(1, wall.collider != null ? wall.point : rb.position + direction * dashLength);
    }

    void OnCollisionEnter2D(Collision2D collision) { Hit(collision); }
    void OnCollisionStay2D(Collision2D collision) { Hit(collision); }

    void Hit(Collision2D collision)
    {
        if (state != State.Dash || !OnScreen()) return;
        PlayerHealth target = collision.collider.GetComponent<PlayerHealth>();
        if (target != null) target.TakeDamage(damage);
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
        if (next == State.Stunned) attackTimer = cooldown;
    }

    void LateUpdate()
    {
        visual.rotation = Quaternion.identity;
    }
}
