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
    [SerializeField] ShooterStats stats;
    [SerializeField] SfxPlayer sounds;
    [SerializeField] AudioClip windUpSound, attackSound;

    State state;
    Rigidbody2D rb;
    PlayerHealth playerHealth;
    Camera viewCamera;
    Vector2 home;
    float timer, sightTimer, unseenTime;
    bool seesPlayer;

    public void Configure(Transform target)
    {
        player = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<PlayerHealth>();
        viewCamera = Camera.main;
        home = rb.position;
        stateLabel.gameObject.SetActive(PlayerPrefs.GetInt("guardLabels", 0) == 1);
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
            sightTimer = stats.sightInterval;
        }

        switch (state)
        {
            case State.Idle:
                if (Vector2.Distance(rb.position, home) > stats.homeTolerance)
                {
                    Face(home);
                    MoveTo(home, stats.returnSpeed);
                }
                if (seesPlayer) SetState(State.KeepDistance);
                break;
            case State.KeepDistance:
                if (seesPlayer) Face(player.position);
                KeepDistance();
                unseenTime = seesPlayer ? 0f : unseenTime + dt;
                if (unseenTime >= stats.loseTime) SetState(State.Idle);
                else if (seesPlayer && timer >= stats.repositionTime && OnScreen(.15f))
                    SetState(State.Aim);
                break;
            case State.Aim:
                if (!OnScreen(.15f)) SetState(State.Cooldown);
                else
                {
                    if (seesPlayer) Face(player.position);
                    DrawAim();
                    if (timer >= stats.windUp)
                    {
                        if (OnScreen() && CanSeePlayer()) Fire();
                        SetState(State.Cooldown);
                    }
                }
                break;
            case State.Cooldown:
                if (seesPlayer) Face(player.position);
                KeepDistance();
                if (timer >= stats.cooldown)
                    SetState(seesPlayer ? State.KeepDistance : State.Idle);
                break;
        }
    }

    void KeepDistance()
    {
        if (!seesPlayer) return;
        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;
        if (distance < stats.preferredDistance - stats.distanceMargin)
            MoveTo(rb.position - toPlayer.normalized, stats.moveSpeed);
        else if (distance > stats.preferredDistance + stats.distanceMargin &&
            Vector2.Distance(rb.position, home) < stats.homeRange)
            MoveTo(player.position, stats.moveSpeed);
    }

    void MoveTo(Vector2 target, float speed)
    {
        target = home + Vector2.ClampMagnitude(target - home, stats.homeRange);
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
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
        if (toPlayer.magnitude > stats.viewRange) return false;
        return Physics2D.Raycast(rb.position, toPlayer.normalized,
            toPlayer.magnitude, obstacleMask).collider == null;
    }

    bool OnScreen(float margin = 0f)
    {
        Vector3 p = viewCamera.WorldToViewportPoint(rb.position);
        return p.z > 0f && p.x > -margin && p.x < 1f + margin &&
            p.y > -margin && p.y < 1f + margin;
    }

    Vector2 AimDirection()
    {
        return Quaternion.Euler(0, 0, rb.rotation) * Vector2.right;
    }

    void DrawAim()
    {
        Vector2 direction = AimDirection();
        RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, stats.viewRange, obstacleMask);
        Vector2 end = hit.collider != null ? hit.point : rb.position + direction * stats.viewRange;
        aimLine.SetPosition(0, rb.position);
        aimLine.SetPosition(1, end);
    }

    void Fire()
    {
        sounds.Play(attackSound, .5f);
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
        if (next == State.Aim)
        {
            DrawAim();
            sounds.Play(windUpSound, .7f, false);
        }
    }

    void LateUpdate()
    {
        visual.rotation = Quaternion.identity;
    }
}
