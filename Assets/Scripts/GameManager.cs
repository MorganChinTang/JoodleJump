using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    public GameObject platformPrefab;
    
    public int platformCount;

    public List<GameObject> activePlatforms = new List<GameObject>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 spawnPosition = new Vector3();

        for (int i = activePlatforms.Count; i <= platformCount; i++)
        {
            Debug.Log("Created");
            spawnPosition.y += Random.Range(8.0f, 12.0f);
            spawnPosition.x = Random.Range(-30.0f, 30.0f);
            Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            activePlatforms.Add(platformPrefab);
        }
    }
    void gameOver()
    {
        Debug.Log("Game Over");
    }

    void ClearPlatforms()
    {
        foreach (var platform in activePlatforms)
        {
            Destroy(platform);
        }
        activePlatforms.Clear();
    }

}

