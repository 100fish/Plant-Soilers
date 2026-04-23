using UnityEngine;

public class PathGizmos : MonoBehaviour
{
    [SerializeField]
    private Transform[] bezierPoints;

    private Vector2 gizmosPosition;

    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * bezierPoints[0].position + 3 * Mathf.Pow(1 - t, 2) * t * bezierPoints[1].position + 3 * (1 - t) * Mathf.Pow(t, 2) * bezierPoints[2].position + Mathf.Pow(t, 3) * bezierPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }

        Gizmos.DrawLine(new Vector2(bezierPoints[0].position.x, bezierPoints[0].position.y), new Vector2(bezierPoints[1].position.x, bezierPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(bezierPoints[2].position.x, bezierPoints[2].position.y), new Vector2(bezierPoints[3].position.x, bezierPoints[3].position.y));

    }
}
