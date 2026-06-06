using UnityEngine;

public class TouchTimeout : MonoBehaviour
{
    public bool canCatch { get; private set; } = true;  
    public bool caughtSomething = false;
    public float timeoutSeconds;
    private float timeOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (caughtSomething)
        {
            Debug.Log("Caught something, starting timeout.");
            canCatch = false;  
            timeOut -= Time.deltaTime;
            if (timeOut <= 0)
            {
                Debug.Log("Timeout expired, can catch again.");
                canCatch = true;
                caughtSomething = false;
                timeOut = timeoutSeconds;
            }
        }
    }
}
