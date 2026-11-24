using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    [SerializeField] SfxPlayer sounds;
    [SerializeField] AudioClip[] steps;
    Vector2 move;
    Rigidbody2D rb;
    Vector2 previousPosition;
    float stepDistance;
    bool placed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 position = rb.position;
        if (placed)
        {
            float distance = Vector2.Distance(position, previousPosition);
            if (distance < .5f) stepDistance += distance;
            if (stepDistance >= .65f)
            {
                sounds.PlayRandom(steps, .28f);
                stepDistance = 0f;
            }
        }
        previousPosition = position;
        placed = true;
        rb.linearVelocity = Vector2.ClampMagnitude(move, 1f) * stats.speed;
    }
}
