using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BugManager : MonoBehaviour
{

    public List<Vector2> BugPositionList = new List<Vector2>();

    [SerializeField]private Transform[] bugs;
    private Vector2[] bugPositionArray = new Vector2[6];

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

    public Vector2[] GetBugPositionArray()
    {
        for (int i = 0; i < bugPositionArray.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(bugs[i].position.x, bugs[i].position.y, bugs[i].position.z - 1), transform.forward, out hit))
            {
                bugPositionArray[i] = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            }
        }

        return bugPositionArray;
    }
}
