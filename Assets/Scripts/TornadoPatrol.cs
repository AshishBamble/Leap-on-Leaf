using UnityEngine;
using System.Collections.Generic;

public class TornadoPatrol : MonoBehaviour, IResettable
{
    [Header("Patrol Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTimeAtEnds = 0.5f;

    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private Vector3 spinAxis = Vector3.up;

    private int currentIndex = 0;
    private bool movingForward = true;
    private bool waiting = false;
    private float waitTimer = 0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

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

    public void ResetObject()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentIndex = 0;
        movingForward = true;
        waiting = false;
        waitTimer = 0f;
    }
}
