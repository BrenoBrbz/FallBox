using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public float timeToIncrease = 1.5f;
    private float timer = 0f;
    public bool isAlive = true;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();
    }

    private void Update()
    {
        if (isAlive)
        {
            timer += Time.deltaTime;

            if (timer >= timeToIncrease)
            {
                score += 1;
                timer = 0f;
                UpdateScoreUI();
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log("High Score salvo: " + highScore);
        }
    }
}
