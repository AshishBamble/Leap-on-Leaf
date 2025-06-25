using UnityEngine;
using System.Collections.Generic;

public class HealingInsect : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();
    [SerializeField] private float speed = 2f;
    [SerializeField] private float stopDistance = 0.1f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool faceDirection = true;
    [Tooltip("Optional rotation offset to match the visual orientation (e.g., Y = 90 if the model faces right)")]
    [SerializeField] private Vector3 rotationOffset;

    [Header("Healing")]
    [SerializeField] private float healingAmount = 0.3f;
    [SerializeField] private string frogTag = "Player";

    private int currentPoint = 0;
    private bool forward = true;

    private void Update()
    {
        if (pathPoints == null || pathPoints.Count == 0) return;

        MoveAlongPath();
        RotateTowardNextPoint();
    }

    private void MoveAlongPath()
    {
        if (pathPoints.Count == 0) return;

        Transform targetPoint = pathPoints[currentPoint];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetPoint.position);
        if (distance < stopDistance)
        {
            if (forward)
            {
                currentPoint++;
                if (currentPoint >= pathPoints.Count)
                {
                    currentPoint = pathPoints.Count - 2;
                    forward = false;
                }
            }
            else
            {
                currentPoint--;
                if (currentPoint < 0)
                {
                    currentPoint = 1;
                    forward = true;
                }
            }
        }
    }

    private void RotateTowardNextPoint()
    {
        if (!faceDirection || pathPoints == null || pathPoints.Count < 2) return;

        Vector3 direction = pathPoints[currentPoint].position - transform.position;
        direction.y = 0f; // Prevent tilting
        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation *= Quaternion.Euler(rotationOffset); // Apply user-defined offset
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frogTag))
        {
            FrogHPBar frogHP = other.GetComponent<FrogHPBar>();
            if (frogHP != null)
            {
                frogHP.AddHealth(healingAmount);
            }
            Destroy(gameObject);
        }
    }
}
