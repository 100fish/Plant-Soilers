using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Camera mainCamera;

    [SerializeField]
    private Transform[] pathsToFollow;

    public float startOffsetSeconds;

    private Vector2 updatedPosition;

    private int pathCurrent;

    private float timeFloat;

    [SerializeField]
    private float speedFloat;

    private bool canBeginPath;
    private bool caught = false;
    private GameObject instance;

    private void Start()
    {
            instance = this.gameObject; //??
            pathCurrent = 0;
            timeFloat = 0f;
            //canBeginPath = true;
            StartCoroutine(INITIALRandomWait());
    }

    private void Update()
    {
        if (canBeginPath)
        {
            StartCoroutine(RandomWait());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("TOUCH") && !caught)
        {
            caught = true;
            timeFloat = 0f;
            canBeginPath = false;
            StopCoroutine(FollowPath());
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(CaughtBug());
        }
    }

    private IEnumerator INITIALRandomWait()
    {
        canBeginPath = false;
        yield return new WaitForSeconds(startOffsetSeconds);
        canBeginPath = true;
    }

    private IEnumerator RandomWait()
    {
             canBeginPath = false;
             yield return new WaitForSeconds(Random.Range(2f, 5f));
             StartCoroutine(FollowPath());
    }
    private IEnumerator FollowPath()
    {
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
            Vector3 updatedPositionZFix = new Vector3(updatedPosition.x, updatedPosition.y, 8); //arbitrary z value to keep the object visible
            transform.position = updatedPositionZFix;

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

    private IEnumerator CaughtBug()
    {
        transform.GetChild(1).GetComponent<Canvas>().enabled = true;
        RectTransform panelRect = instance.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        Vector3 screenPos = mainCamera.WorldToScreenPoint(instance.transform.position);
        panelRect.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
        yield return new WaitForSeconds(2f);
        instance.GetComponent<SpriteRenderer>().enabled = false;
        instance.transform.GetChild(1).GetComponent<Canvas>().enabled = false;
    }

}
