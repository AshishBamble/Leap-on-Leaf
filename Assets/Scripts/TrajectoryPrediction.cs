using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPrediction : MonoBehaviour
{
    [Header("Trajectory Settings")]
    [SerializeField] private int resolution = 30;
    [SerializeField] private float arcLength = 1.5f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    public void ShowTrajectory(Vector3 startPos, Vector3 force, float mass, float maxTime)
    {
        lineRenderer.positionCount = resolution;
        float timeStep = arcLength / resolution;

        Vector3 velocity = force / mass;

        for (int i = 0; i < resolution; i++)
        {
            float t = timeStep * i;
            Vector3 point = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.enabled = true;
    }

    public void Hide()
    {
        lineRenderer.enabled = false;
    }

    public Vector3 GetEndPoint(Vector3 startPos, Vector3 force, float mass, float maxTime)
    {
        int steps = resolution;
        float timeStep = arcLength / steps;

        Vector3 velocity = force / mass;
        Vector3 previousPoint = startPos;

        for (int i = 1; i < steps; i++)
        {
            float t = timeStep * i;
            Vector3 currentPoint = startPos + velocity * t + 0.5f * Physics.gravity * t * t;

            // Check for crossing the ground (y=0)
            if (previousPoint.y > 0f && currentPoint.y <= 0f)
            {
                float tFactor = previousPoint.y / (previousPoint.y - currentPoint.y);
                Vector3 interpolated = Vector3.Lerp(previousPoint, currentPoint, tFactor);
                return interpolated;
            }

            previousPoint = currentPoint;
        }

        // Fallback: return last calculated point
        return previousPoint;
    }
}
