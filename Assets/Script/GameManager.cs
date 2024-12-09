using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button uiButton;
    public Button uiButton1;
    public bool isPaused = false;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider musicSliderSFX;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicGameVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicGameVolume();
            SetSFXVolume();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            uiButton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            uiButton1.onClick.Invoke(); 
        }
    }

    public void SetMusicGameVolume()
    {
        float volumeMusicGameVolume = musicSlider.value;
        myMixer.SetFloat("musicGame", Mathf.Log10(volumeMusicGameVolume) * 20);
        PlayerPrefs.SetFloat("musicGameVolume", volumeMusicGameVolume);
        PlayerPrefs.Save();
    }
    public void SetSFXVolume()
    {
        float volumeSFX = musicSliderSFX.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volumeSFX) * 20);
        PlayerPrefs.SetFloat("SFX", volumeSFX);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicGameVolume");
        musicSliderSFX.value = PlayerPrefs.GetFloat("SFX");

        SetMusicGameVolume();
        SetSFXVolume();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}
