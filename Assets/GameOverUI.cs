using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public Text gameOverText;
    public Button restartButton;

    void Start()
    {
        // Safety checks
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        else
            Debug.LogError("GameOverText is NOT assigned in GameOverUI");

        if (restartButton != null)
            restartButton.gameObject.SetActive(false);
        else
            Debug.LogError("RestartButton is NOT assigned in GameOverUI");
    }

    public void ShowGameOver()
    {
        if (gameOverText == null || restartButton == null)
        {
            Debug.LogError("GameOverUI references missing!");
            return;
        }

        StopAllCoroutines(); // prevent duplicate animations
        StartCoroutine(GameOverAnimation());
    }

    IEnumerator GameOverAnimation()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // Reset animation state
        gameOverText.transform.localScale = Vector3.zero;

        Color c = gameOverText.color;
        c.a = 0f;
        gameOverText.color = c;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;

            gameOverText.transform.localScale =
                Vector3.Lerp(Vector3.zero, Vector3.one, t);

            c.a = Mathf.Clamp01(t);
            gameOverText.color = c;

            yield return null;
        }

        // Ensure final values
        gameOverText.transform.localScale = Vector3.one;
        c.a = 1f;
        gameOverText.color = c;
    }
}
