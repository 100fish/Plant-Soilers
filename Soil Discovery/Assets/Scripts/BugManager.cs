using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class BugManager : MonoBehaviour
{

    public List<Vector2> BugPositionList = new List<Vector2>();
    public static BugManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
