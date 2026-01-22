using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // SnakeHead
    public float height = 15f;         // Camera height
    public float smoothSpeed = 5f;     // Smoothness (lower = slower)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x,
            height,
            target.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Always look straight down
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
