using System.Collections;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] pathsToFollow;

    private Vector2 updatedPosition;

    private int pathCurrent;

    private float timeFloat;

    [SerializeField]
    private float speedFloat;

    private bool canBeginPath;

    private void Start()
    {
            pathCurrent = 0;
            timeFloat = 0f;
            canBeginPath = true;
    }

    private void Update()
    {
        if (canBeginPath)
        {
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        canBeginPath = false;

        Vector2 p0 = pathsToFollow[pathCurrent].GetChild(0).position;
        Vector2 p1 = pathsToFollow[pathCurrent].GetChild(1).position;
        Vector2 p2 = pathsToFollow[pathCurrent].GetChild(2).position;
        Vector2 p3 = pathsToFollow[pathCurrent].GetChild(3).position;

        while (timeFloat < 1f)
        {
            timeFloat += Time.deltaTime * speedFloat;
            //This is following the mathematical formula for a cubic bezier curve, which is what we are using to follow the path.
            updatedPosition = Mathf.Pow(1 - timeFloat, 3) * p0 + 3 * Mathf.Pow(1 - timeFloat, 2) * timeFloat * p1 + 3 * (1 - timeFloat) * Mathf.Pow(timeFloat, 2) * p2 + Mathf.Pow(timeFloat, 3) * p3;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, updatedPosition - (Vector2)transform.position);
            transform.position = updatedPosition;

            yield return new WaitForEndOfFrame();
        }

        timeFloat = 0f;

        pathCurrent += 1;
        if (pathCurrent >= pathsToFollow.Length)
        {
            pathCurrent = 0;
        }

        canBeginPath = true;
    }
}
