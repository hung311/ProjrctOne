using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Collider2D spawnArea;
    public float spawnInterval = 1f;

    private GameObject spawnedObject;
    public int maxObjectsToSpawn = 5;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    void Start()
    {
        LoadSlimeStates();
        StartCoroutine(SpawnObjectAtIntervals());
    }   
    private IEnumerator SpawnObjectAtIntervals()
    {
        while (true)
        {
            int activeObjects = spawnedObjects.FindAll(obj => obj != null).Count;

            if (activeObjects < maxObjectsToSpawn)
            {
                SpawnRandomObject();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnRandomObject()
    {
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        spawnedObject.SetActive(true);
        spawnedObjects.Add(spawnedObject);
    }

    private void LoadSlimeStates()
    {
        int slimeCount = PlayerPrefs.GetInt("SlimeCount", 0);

        for (int i = 0; i < slimeCount; i++)
        {
            float x = PlayerPrefs.GetFloat($"Slime_{i}_X", 0);
            float y = PlayerPrefs.GetFloat($"Slime_{i}_Y", 0);
            float z = PlayerPrefs.GetFloat($"Slime_{i}_Z", 0);
            Vector3 slimePosition = new Vector3(x, y, z);

            float hp = PlayerPrefs.GetFloat($"Slime_{i}_HP");
            GameObject spawnedSlime = Instantiate(objectToSpawn, slimePosition, Quaternion.identity);
            spawnedSlime.SetActive(true);

            SlimeConTroller slimeController = spawnedSlime.GetComponent<SlimeConTroller>();
            if (slimeController != null)
            {
                slimeController.HpSlime = hp;
                slimeController.UpdateHealthUI();
            }

            spawnedObjects.Add(spawnedSlime);

        }
    }

}

