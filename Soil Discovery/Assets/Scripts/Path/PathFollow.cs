using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    private Camera mainCamera;
    private SoilMousePass soilManager;

    [SerializeField]
    private Transform[] pathsToFollow;

    public float startOffsetSeconds;

    private Vector2 updatedPosition;

    private int pathCurrent;

    private float timeFloat;

    [SerializeField]
    private float speedFloat;

    public bool canBeginPath;
    private bool caught = false;
    private GameObject instance;

    private SpriteRenderer spriteRenderer;
    private Animation animation;
    private Animator animator;

    [SerializeField]
    private Sprite[] bugSprites;
    [SerializeField]
    private AnimationClip[] bugAnimations;
    [SerializeField]
    private AnimatorController[] bugAnimatorControllers;


    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        animation = GetComponent<Animation>();
        animator = GetComponent<Animator>();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        soilManager = GameObject.FindGameObjectWithTag("SoilManager").GetComponent<SoilMousePass>();

        instance = this.gameObject; //??
        pathCurrent = 0;
        timeFloat = 0f;
        //canBeginPath = true;

        FindPaths();
        pathCurrent = Random.Range(0, pathsToFollow.Length);
        ChooseRandomProfile();
        StartCoroutine(INITIALRandomWait());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * -1);
    }

    private void Update()
    {
        if (canBeginPath)
        {
            ChooseRandomProfile();
            StartCoroutine(RandomWait());
        }

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.forward, out hit))
        {
            Vector2 MeshHitBug = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            //BugManager.Instance.BugPositionList.Add(MeshHitBug);
        }


    }

    
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("TOUCH") && !caught /* && soilManager.someVariable == true */)
        {
            caught = true;
            timeFloat = 0f;
            canBeginPath = false;
            StopCoroutine(FollowPath());
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

        float randomZPos = Random.Range(8f, 10f);

        while (timeFloat < 1f)
        {
            timeFloat += Time.deltaTime * speedFloat;
            //This is following the mathematical formula for a cubic bezier curve, which is what we are using to follow the path.
            updatedPosition = Mathf.Pow(1 - timeFloat, 3) * p0 + 3 * Mathf.Pow(1 - timeFloat, 2) * timeFloat * p1 + 3 * (1 - timeFloat) * Mathf.Pow(timeFloat, 2) * p2 + Mathf.Pow(timeFloat, 3) * p3;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, updatedPosition - (Vector2)transform.position);
            Vector3 updatedPositionZFix = new Vector3(updatedPosition.x, updatedPosition.y, randomZPos); //random Z value between top and bottom of soil
            transform.position = updatedPositionZFix;

            yield return new WaitForEndOfFrame();
        }

        timeFloat = 0f;

        pathCurrent = Random.Range(0, pathsToFollow.Length);

        canBeginPath = true;
    }

    private IEnumerator CaughtBug()
    {
        transform.GetChild(0).GetComponent<Canvas>().enabled = true;
        RectTransform panelRect = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        Vector3 screenPos = mainCamera.WorldToScreenPoint(instance.transform.position);
        panelRect.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
        yield return new WaitForSeconds(2f);
        instance.GetComponent<SpriteRenderer>().enabled = false;
        instance.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
    }

    private void ChooseRandomProfile()
    {
        int randomBugType = Random.Range(0, 5);

        spriteRenderer.sprite = bugSprites[randomBugType];
        animation.clip = bugAnimations[randomBugType];
        animator.runtimeAnimatorController = bugAnimatorControllers[randomBugType];
    }

    private void FindPaths()
    {
        GameObject pathsParent = GameObject.FindGameObjectWithTag("PathsParent");

        int pathCount = pathsParent.transform.childCount;

        pathsToFollow = new Transform[pathCount];

        for (int i = 0; i < pathCount; i++)
        {
            pathsToFollow[i] = pathsParent.transform.GetChild(i);
        }

    }

}
