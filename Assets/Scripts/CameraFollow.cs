using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;              // The frog
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);
    [SerializeField] private float followSpeed = 5f;

    [Header("Drag Settings")]
    [SerializeField] private bool followDragDirection = true;
    [SerializeField] private float dragInfluence = 2f;

    [Header("FrogJump Reference")]
    [SerializeField] private FrogJump frogJump;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Apply drag direction offset during drag
        if (followDragDirection && frogJump != null && frogJump.IsDragging)
        {
            Vector3 dragDir = frogJump.DragDirection;
            desiredPosition += dragDir * dragInfluence;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFrogJump(FrogJump jumpScript)
    {
        frogJump = jumpScript;
    }
}
