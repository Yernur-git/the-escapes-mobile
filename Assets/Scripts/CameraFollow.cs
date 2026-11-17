using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;
    Vector2 worldSize;
    Camera view;

    public void Configure(Transform player, Vector2 size)
    {
        target = player;
        worldSize = size;
        view = GetComponent<Camera>();
        Follow();
    }

    void LateUpdate()
    {
        if (target != null) Follow();
    }

    void Follow()
    {
        float halfHeight = view.orthographicSize;
        float halfWidth = halfHeight * view.aspect;
        float maxX = Mathf.Max(0f, worldSize.x / 2f - halfWidth);
        float maxY = Mathf.Max(0f, worldSize.y / 2f - halfHeight);
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, -maxX, maxX),
            Mathf.Clamp(target.position.y, -maxY, maxY), transform.position.z);
    }
}
