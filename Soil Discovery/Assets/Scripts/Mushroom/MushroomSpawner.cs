using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushroomSpawner : MonoBehaviour
{
    public List<GameObject> mushroomPrefabs;
    private bool canSpawn = true;
    private bool isSpawning = false;
    private List<GameObject> spawnedMushrooms = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            //Debug.Log("Spawning mushroom...");
            StartCoroutine(WaitThenSpawn());
            isSpawning = true;
        }

        if(spawnedMushrooms.Count == 0 && isSpawning == false)
        {
            canSpawn = true;
        }

    }

    void SpawnMushroom()
    {
        float distance = 15f; // Distance from the camera
        Vector3 screenPos = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), distance);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        int mushroomIndex = Random.Range(0, mushroomPrefabs.Count - 1);
        //Debug.Log("Spawning mushroom at: " + worldPos);
        GameObject mushroom = Instantiate(mushroomPrefabs[mushroomIndex], worldPos, Quaternion.identity);
        spawnedMushrooms.Add(mushroom);
        isSpawning = false;
        //Debug.Log("Spawned mushroom at: " + worldPos);
    }

    private IEnumerator WaitThenSpawn()
    {
        canSpawn = false;
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before spawning the next mushroom
        SpawnMushroom();
    }
}
