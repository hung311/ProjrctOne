using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextScore : MonoBehaviour
{
    public TMP_Text tmpText;
    public float score = 0f;
    public float highScore = 0f;
    public float lowScore = 0f;

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        score = PlayerPrefs.GetFloat("Score", 0);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        tmpText.text = "Score: " + score;
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }
        UpdateScoreText();

    }

    public void LowScore(int points1) 
    {
        lowScore += points1;
        PlayerPrefs.SetFloat("lowScore",lowScore); 
        PlayerPrefs.Save();
    }
}
