using System.Collections.Generic;
using UnityEngine;

public class MovingLeaf : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float waitTimeAtEnds = 1f;

    private int currentIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool forward = true;

    private void Update()
    {
        if (waypoints == null || waypoints.Count < 2)
            return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
                isWaiting = false;
            else
                return;
        }

        Transform targetPoint = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            // Wait only at first and last points
            if (currentIndex == 0 || currentIndex == waypoints.Count - 1)
            {
                isWaiting = true;
                waitTimer = waitTimeAtEnds;
            }

            // Ping-pong logic
            if (forward)
            {
                if (currentIndex < waypoints.Count - 1)
                    currentIndex++;
                else
                {
                    forward = false;
                    currentIndex--;
                }
            }
            else
            {
                if (currentIndex > 0)
                    currentIndex--;
                else
                {
                    forward = true;
                    currentIndex++;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}
