using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float xLimit = 7f;
    public float zLimit = 7f;

    private bool collected = false;
    private ScoreCounter scoreCounter;

    void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag("Snake")) return;

        collected = true;

        SnakeMovement snake = other.GetComponent<SnakeMovement>();
        if (snake != null)
        {
            snake.Grow();
        }

        if (scoreCounter != null)
        {
            scoreCounter.AddScore(1);
        }

        Respawn();
    }

    void Respawn()
    {
        transform.position = new Vector3(
            Random.Range(-xLimit, xLimit),
            0.35f,
            Random.Range(-zLimit, zLimit)
        );

        collected = false;
    }
}
