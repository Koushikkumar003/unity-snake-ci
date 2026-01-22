using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SnakeMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float turnSpeed = 160f;

    [Header("Dash (NEW ACTION)")]
    public float dashSpeed = 5f;        // burst speed
    public float dashDuration = 0.15f;  // short & controlled
    public float dashCooldown = 1.5f;   // no spamming

    [Header("Body")]
    public GameObject bodyPrefab;

    private Rigidbody rb;
    private bool canMove = true;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

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

        HandleDashTimers();

        // Forward movement
        float currentSpeed = isDashing ? dashSpeed : moveSpeed;
        Vector3 move = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // 🔥 Sharp turning
        float turn = Input.GetAxisRaw("Horizontal");
        if (turn != 0)
        {
            Quaternion rot = Quaternion.Euler(0f, turn * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * rot);
        }

        // Save head position
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

    void Update()
    {
        // 🚀 DASH INPUT (NEW)
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;
    }

    void HandleDashTimers()
    {
        if (isDashing)
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
                isDashing = false;
        }

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.fixedDeltaTime;
    }

    // 🧱 Stop on wall hit
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
