using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shooter : MonoBehaviour
{
    enum State { Idle, KeepDistance, Aim, Cooldown }

    [SerializeField] Transform player;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] EnemyBullet bullet;
    [SerializeField] LineRenderer aimLine;
    [SerializeField] Transform visual;
    [SerializeField] TextMesh stateLabel;
    [SerializeField] float viewRange = 7f, preferredDistance = 5f;
    [SerializeField] float distanceMargin = .5f, moveSpeed = 2.2f;
    [SerializeField] float homeRange = 5f, returnSpeed = 1.5f;
    [SerializeField] float homeTolerance = .2f, turnSpeed = 340f;
    [SerializeField] float sightInterval = .15f, loseTime = 1f;
    [SerializeField] float repositionTime = .25f, windUp = .6f, cooldown = 1.5f;

    State state;
    Rigidbody2D rb;
    PlayerHealth playerHealth;
    Camera viewCamera;
    Vector2 home;
    float timer, sightTimer, unseenTime;
    bool seesPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
        viewCamera = Camera.main;
        home = rb.position;
        stateLabel.gameObject.SetActive(Debug.isDebugBuild);
        SetState(State.Idle);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = Vector2.zero;
        if (!playerHealth.IsAlive) return;
        float dt = Time.fixedDeltaTime;
        timer += dt;
        sightTimer -= dt;
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
                    MoveTo(home, returnSpeed);
                }
                if (seesPlayer) SetState(State.KeepDistance);
                break;
            case State.KeepDistance:
                if (seesPlayer) Face(player.position);
                KeepDistance();
                unseenTime = seesPlayer ? 0f : unseenTime + dt;
                if (unseenTime >= loseTime) SetState(State.Idle);
                else if (seesPlayer && timer >= repositionTime && OnScreen())
                    SetState(State.Aim);
                break;
            case State.Aim:
                if (!OnScreen()) SetState(State.Cooldown);
                else
                {
                    if (seesPlayer) Face(player.position);
                    DrawAim();
                    if (timer >= windUp)
                    {
                        if (CanSeePlayer()) Fire();
                        SetState(State.Cooldown);
                    }
                }
                break;
            case State.Cooldown:
                if (seesPlayer) Face(player.position);
                KeepDistance();
                if (timer >= cooldown)
                    SetState(seesPlayer ? State.KeepDistance : State.Idle);
                break;
        }
    }

    void KeepDistance()
    {
        if (!seesPlayer) return;
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;
        if (distance < preferredDistance - distanceMargin)
            MoveTo(rb.position - toPlayer.normalized, moveSpeed);
        else if (distance > preferredDistance + distanceMargin &&
            Vector2.Distance(rb.position, home) < homeRange)
            MoveTo(player.position, moveSpeed);
    }

    void MoveTo(Vector2 target, float speed)
    {
        target = home + Vector2.ClampMagnitude(target - home, homeRange);
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
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
        if (toPlayer.magnitude > viewRange) return false;
        return Physics2D.Raycast(rb.position, toPlayer.normalized,
            toPlayer.magnitude, obstacleMask).collider == null;
    }

    bool OnScreen()
    {
        Vector3 p = viewCamera.WorldToViewportPoint(rb.position);
        return p.z > 0f && p.x > 0f && p.x < 1f && p.y > 0f && p.y < 1f;
    }

    Vector2 AimDirection()
    {
        return Quaternion.Euler(0, 0, rb.rotation) * Vector2.right;
    }

    void DrawAim()
    {
        Vector2 direction = AimDirection();
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, viewRange, obstacleMask);
        Vector2 end = hit.collider != null ? hit.point : rb.position + direction * viewRange;
        aimLine.SetPosition(0, rb.position);
        aimLine.SetPosition(1, end);
    }

    void Fire()
    {
        EnemyBullet shot = Instantiate(bullet, rb.position, Quaternion.identity);
        Physics2D.IgnoreCollision(shot.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        shot.Launch(AimDirection());
    }

    void SetState(State next)
    {
        state = next;
        timer = 0f;
        unseenTime = 0f;
        aimLine.enabled = next == State.Aim;
        stateLabel.text = next == State.KeepDistance ? "keep distance" : next.ToString().ToLower();
        if (next == State.Aim) DrawAim();
    }

    void LateUpdate()
    {
        visual.rotation = Quaternion.identity;
    }
}
