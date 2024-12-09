using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConvertScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitScene()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            Vector3 playerPosition = playerController.transform.position;

            PlayerPrefs.SetFloat("PlayerX", playerPosition.x);
            PlayerPrefs.SetFloat("PlayerY", playerPosition.y);
            PlayerPrefs.SetFloat("PlayerZ", playerPosition.z);

            PlayerPrefs.SetFloat("HpPlayer", playerController.HpPlayer);

            SaveSlimeStates();
            SaveScore();
            PlayerPrefs.Save();
        }
        SceneManager.LoadScene("MainMenu");
    }
    
    private void SaveScore()
    {
        TextScore textScore = FindObjectOfType<TextScore>();
        PlayerPrefs.SetFloat("Score", textScore.lowScore);
        PlayerPrefs.Save();
    }

    private void SaveSlimeStates()
    {
        // xóa mấy con cũ 
        if (PlayerPrefs.HasKey("SlimeCount"))
        {
            int oldSlimeCount = PlayerPrefs.GetInt("SlimeCount");
            for (int i = 0; i < oldSlimeCount; i++)
            {
                PlayerPrefs.DeleteKey($"Slime_{i}_X");
                PlayerPrefs.DeleteKey($"Slime_{i}_Y");
                PlayerPrefs.DeleteKey($"Slime_{i}_Z");
                PlayerPrefs.DeleteKey($"Slime_{i}_HP");
            }
        }

        // lưu mấy con mới 
        SlimeConTroller[] slimes = FindObjectsOfType<SlimeConTroller>();
        PlayerPrefs.SetInt("SlimeCount", slimes.Length);

        for (int i = 0; i < slimes.Length; i++)
        {
            if (slimes[i] != null)
            {
                Vector3 slimePosition = slimes[i].transform.position;
                PlayerPrefs.SetFloat($"Slime_{i}_X", slimePosition.x);
                PlayerPrefs.SetFloat($"Slime_{i}_Y", slimePosition.y);
                PlayerPrefs.SetFloat($"Slime_{i}_Z", slimePosition.z);

                PlayerPrefs.SetFloat($"Slime_{i}_HP", slimes[i].HpSlime);
            }
        }
        PlayerPrefs.Save();

    }
}
