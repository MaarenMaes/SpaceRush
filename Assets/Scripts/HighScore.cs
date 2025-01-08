using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    private int highScore;
    private float currentScore;

    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    void Start()
    {
        // Retrieve the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Retrieve the current score from PlayerPrefs
        currentScore = PlayerPrefs.GetFloat("CurrentScore", 0);

        // Display the high score
        if (highScoreText != null)
        {
            highScoreText.SetText($"High Score: {highScore}");
        }
        else
        {
            Debug.Log("High Score: " + highScore);
        }

        // Display the current score
        if (currentScoreText != null)
        {
            currentScoreText.SetText($"Current Score: {currentScore}");
        }
        else
        {
            Debug.Log("Score: " + currentScore);
        }
    }
}
