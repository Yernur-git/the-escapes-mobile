using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerStats stats;
    Vector2 move;
    Rigidbody2D rb;

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
        rb.linearVelocity = Vector2.ClampMagnitude(move, 1f) * stats.speed;
    }
}
