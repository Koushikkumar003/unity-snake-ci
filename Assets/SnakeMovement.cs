using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SnakeMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float turnSpeed = 50f;

    [Header("Body")]
    public GameObject bodyPrefab;

    private Rigidbody rb;
    private bool canMove = true;

    private List<Transform> bodyParts = new List<Transform>();
    private List<Vector3> positionHistory = new List<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints =
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // Forward movement
        Vector3 move = transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotation
        float turn = Input.GetAxis("Horizontal");
        Quaternion rot = Quaternion.Euler(0f, turn * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rot);

        // Save position history
        positionHistory.Insert(0, transform.position);
        if (positionHistory.Count > 500)
            positionHistory.RemoveAt(positionHistory.Count - 1);

        // Move body parts
        for (int i = 0; i < bodyParts.Count; i++)
        {
            int index = Mathf.Min((i + 1) * 15, positionHistory.Count - 1);
            bodyParts[i].position = positionHistory[index];
        }
    }

    // 🔴 STOP MOVEMENT WHEN WALL IS HIT
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            canMove = false;
        }
    }

    // Grow snake
    public void Grow()
    {
        GameObject body = Instantiate(bodyPrefab);

        Vector3 spawnPos = bodyParts.Count == 0
            ? transform.position
            : bodyParts[bodyParts.Count - 1].position;

        body.transform.position = spawnPos;
        bodyParts.Add(body.transform);

        for (int i = 0; i < 20; i++)
            positionHistory.Add(spawnPos);
    }
}
