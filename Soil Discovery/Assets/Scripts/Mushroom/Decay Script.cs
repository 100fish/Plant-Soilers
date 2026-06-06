using UnityEngine;
using System.Collections;

public class DecayScript : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MushroomSpawner.Instance != null)
        {
            MushroomSpawner.Instance.spawnedMushrooms.Add(gameObject);
            StartCoroutine(Decay());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(5f);
        if (MushroomSpawner.Instance.spawnedMushrooms.Count > 2)
        {
            MushroomSpawner.Instance.spawnedMushrooms.Remove(gameObject);
            Destroy(gameObject);
        }
        else
        {
                       StartCoroutine(Decay());
        }
    }
}
