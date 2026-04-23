using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RandomPathGenerator : MonoBehaviour
{
    public static RandomPathGenerator instance;

    private Vector2[] sides;

    private Vector2 TopLeft = new Vector2(-10f, 5f);
    private Vector2 BottomLeft = new Vector2(-10f, -5f);
    private Vector2 TopRight = new Vector2(10f, 5f);
    private Vector2 BottomRight = new Vector2(10f, -5f);

    private Vector2 startPoint = new Vector2();
    private Vector2 secondPoint = new Vector2();
    private Vector2 thirdPoint = new Vector2();
    private Vector2 endPoint = new Vector2();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        sides = new Vector2[4] { TopLeft, TopRight, BottomRight, BottomLeft };
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void generatePath()
        {
            int randomSide = Random.Range(0, 3);
            int goalSide = randomSide + 2;
            if (goalSide > 3)
            {
                goalSide -= 4;
            }
            // To reach a random point on a side, begin in the corner and add a randomly maginified vector in the direction of the side (limited at max length).

            //Keep second point with start and third point with end.!!
            startPoint.x = sides[randomSide].x + Random.Range(0f, 20f) * Mathf.Cos(randomSide * Mathf.PI / 2);
            startPoint.y = sides[randomSide].y + Random.Range(0f, 5f) * Mathf.Sin(randomSide * Mathf.PI / 2);
            
            secondPoint.x = sides[randomSide].x + Random.Range(0f, 20f) * Mathf.Cos(randomSide * Mathf.PI / 2);
            secondPoint.y = sides[randomSide].y + Random.Range(0f, 5f) * Mathf.Sin(randomSide * Mathf.PI / 2);

            endPoint.x = sides[goalSide].x + Random.Range(0f, 20f) * Mathf.Cos(goalSide * Mathf.PI / 2);
            endPoint.y = sides[goalSide].y + Random.Range(0f, 5f) * Mathf.Sin(goalSide * Mathf.PI / 2);

            thirdPoint.x = sides[goalSide].x + Random.Range(0f, 20f) * Mathf.Cos(goalSide * Mathf.PI / 2);
            thirdPoint.y = sides[goalSide].y + Random.Range(0f, 5f) * Mathf.Sin(goalSide * Mathf.PI / 2);

        applyPath();
        }

    public void applyPath()
    {
        GameObject.Find("Path").transform.GetChild(0).position = startPoint;
        GameObject.Find("Path").transform.GetChild(1).position = secondPoint;
        GameObject.Find("Path").transform.GetChild(2).position = thirdPoint;
        GameObject.Find("Path").transform.GetChild(3).position = endPoint;
    }
    

    void OnDrawGizmos()
    {
            Gizmos.DrawLine(startPoint, endPoint);
    }
}
