using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushroomSpawner : MonoBehaviour
{
    public List<GameObject> mushroomPrefabs;
    private bool canSpawn = true;
    private bool isSpawning = false;
    public List<GameObject> spawnedMushrooms = new List<GameObject>();
    public static MushroomSpawner Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject dummyShroom = new GameObject("DummyShroom");

        Instance = this;
        spawnedMushrooms.Add(dummyShroom);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            Debug.Log("waitingthenspawning");
            StartCoroutine(WaitThenSpawn());
            isSpawning = true;
            return;
        }

        if(spawnedMushrooms.Count == 0 && isSpawning == false)
        {
            canSpawn = true;
        }

    }

    void SpawnMushroom()
    {
        Debug.Log("SPAWNING");
        float distance = 15f; // Distance from the camera
        Vector3 screenPos = new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), distance);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3 worldPosZFix = new Vector3(worldPos.x, worldPos.y, 7.9f); //Arbitrary z value to keep the object visible
        int mushroomIndex = Random.Range(0, mushroomPrefabs.Count - 1);
        //Debug.Log("Spawning mushroom at: " + worldPos);
        GameObject mushroom = Instantiate(mushroomPrefabs[mushroomIndex], worldPosZFix, Quaternion.identity);
        //spawnedMushrooms.Add(mushroom);
        isSpawning = false;
        //Debug.Log("Spawned mushroom at: " + worldPos);
    }

    private IEnumerator WaitThenSpawn()
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before spawning the next mushroom
        Debug.Log("Done waiting, spawning mushroom.");
        SpawnMushroom();
    }
}
