using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip backGroundMenu;
    private float musicVolume;
    private float musicGameVolume;
    private float SFX;
    private float highScore;

    private void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        musicSource.clip = backGroundMenu;
        musicSource.Play();
    }

    private void Update()
    {
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("SampleScene");
        musicVolume = PlayerPrefs.GetFloat("musicVolume",0);
        musicGameVolume = PlayerPrefs.GetFloat("musicGameVolume", 0);
        SFX = PlayerPrefs.GetFloat("SFX", 0);
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("musicGameVolume", musicGameVolume);
        PlayerPrefs.SetFloat("SFX",SFX);
        PlayerPrefs.SetFloat("HighScore", highScore);
        PlayerPrefs.Save();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
