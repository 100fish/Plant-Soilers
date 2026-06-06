using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class BugManager : MonoBehaviour
{
    //public List<Vector2> BugPositionList = new List<Vector2>();

    [SerializeField] private int initialBugs;
    [SerializeField] private GameObject bugPrefab;

    private GameObject[] bugs;
    private PathFollow[] bugPathFollows ;

    private Vector2[] bugPositionArray = new Vector2[6];

    public static BugManager Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bugs = new GameObject[initialBugs];
        bugPathFollows = new PathFollow[initialBugs];


        for (int i = 0; i < initialBugs; i++)
        {
            bugs[i] = Instantiate(bugPrefab, new Vector3(0, 0, 8), Quaternion.identity);


            bugPathFollows[i] = bugs[i].GetComponent<PathFollow>();
        }

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
            if (Physics.Raycast(new Vector3(bugs[i].transform.position.x, bugs[i].transform.position.y, bugs[i].transform.position.z - 10), transform.forward, out hit))
            {
                bugPositionArray[i] = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            }
        }

        return bugPositionArray;
    }

    private void CheckToSwapBugs()
    {
        foreach (var item in bugPathFollows)
        {
            if (item.canBeginPath == true)
            {

            }
        }
    }
}
