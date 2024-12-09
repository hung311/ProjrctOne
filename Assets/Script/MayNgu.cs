using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MayNgu : MonoBehaviour
{
    public TMP_Text tmpText;
    public TMP_Text tmpText1;
    private float musicVolume;
    private float musicGameVolume;
    private float SFX;
    private float highScore;
    private float score;

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        score = PlayerPrefs.GetFloat("lowScore", 0);
        UpdateScoreText();
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("SampleScene");
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0);
        musicGameVolume = PlayerPrefs.GetFloat("musicGameVolume", 0);
        SFX = PlayerPrefs.GetFloat("SFX", 0);
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("musicGameVolume", musicGameVolume);
        PlayerPrefs.SetFloat("SFX", SFX);
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ExitScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void UpdateScoreText()
    {
        tmpText.text = "HighScore: " + highScore;
        tmpText1.text = "Score: " + score;
    }
}
