using UnityEngine;

public class PrisonExit : MonoBehaviour
{
    [SerializeField] LevelResult result;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && player.IsAlive) result.Finish(true);
    }
}
