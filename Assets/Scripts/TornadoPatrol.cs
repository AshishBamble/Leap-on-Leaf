using UnityEngine;
using System.Collections.Generic;

public class TornadoPatrol : MonoBehaviour
{
    [Header("Patrol Waypoints")]
    [Tooltip("Empty GameObjects that define the tornado's path")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAtEnds = 0.5f;

    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 360f; // degrees per second
    [Tooltip("Local axis to spin around (default: Y)")]
    [SerializeField] private Vector3 spinAxis = Vector3.up;

    private int currentIndex = 0;
    private bool movingForward = true;
    private bool waiting = false;
    private float waitTimer = 0f;

    private void Update()
    {
        SpinTornado();

        if (waypoints.Count < 2) return;

        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
                waiting = false;
            else
                return;
        }

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            if (movingForward)
            {
                if (currentIndex >= waypoints.Count - 1)
                {
                    movingForward = false;
                    waitTimer = waitTimeAtEnds;
                    waiting = true;
                }
                else
                {
                    currentIndex++;
                }
            }
            else
            {
                if (currentIndex <= 0)
                {
                    movingForward = true;
                    waitTimer = waitTimeAtEnds;
                    waiting = true;
                }
                else
                {
                    currentIndex--;
                }
            }
        }
    }

    private void SpinTornado()
    {
        transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.Self);
    }
}
