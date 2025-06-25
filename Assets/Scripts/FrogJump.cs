using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrogJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float upwardForce = 5f;
    [SerializeField] private float maxJumpDistance = 7f;
    [SerializeField] private float dragCancelThreshold = 0.5f;
    [SerializeField] private float jumpCooldown = 1f;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private JumpRing jumpRing;
    [SerializeField] private TrajectoryPrediction trajectoryPrediction;
    [SerializeField] private JumpBarController jumpBar;

    private Rigidbody rb;
    private bool isDragging = false;
    private bool canJump = true;

    private Vector3 startDragPosition;
    private Vector3 currentDragPosition;

    public bool IsDragging => isDragging;
    public Vector3 DragDirection => (currentDragPosition - startDragPosition).normalized;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrag(GetWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateDrag(GetWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPos = GetWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartDrag(touchWorldPos);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                        UpdateDrag(touchWorldPos);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging)
                        EndDrag();
                    break;
            }
        }
    }

    private void StartDrag(Vector3 worldPosition)
    {
        if (!canJump) return;

        isDragging = true;
        startDragPosition = worldPosition;
        currentDragPosition = worldPosition;
    }

    private void UpdateDrag(Vector3 worldPosition)
    {
        currentDragPosition = worldPosition;

        Vector3 rawVector = worldPosition - startDragPosition;
        Vector3 clampedVector = Vector3.ClampMagnitude(rawVector, maxJumpDistance);
        currentDragPosition = startDragPosition + clampedVector;

        Vector3 dragVector = new Vector3(-clampedVector.x, 0f, -clampedVector.z); // Inverted slingshot

        float dragMagnitude = dragVector.magnitude;

        if (dragMagnitude <= dragCancelThreshold)
        {
            jumpRing.SetRadius(jumpRing.DefaultRadius);
            jumpRing.Hide();
            trajectoryPrediction.Hide();
            return;
        }

        float clampedMagnitude = Mathf.Min(dragMagnitude, maxJumpDistance);
        float dragPercent = clampedMagnitude / maxJumpDistance;

        Vector3 jumpDirection = dragVector.normalized;
        Vector3 appliedForce = (jumpDirection * dragPercent * jumpForce) + (Vector3.up * upwardForce);

        Vector3 predictedPosition = trajectoryPrediction.GetEndPoint(transform.position, appliedForce, rb.mass, maxJumpDistance);

        predictedPosition.y = 0f;

        jumpRing.SetRadius(jumpRing.DefaultRadius);
        jumpRing.SetPosition(predictedPosition);
        jumpRing.Show();

        trajectoryPrediction.ShowTrajectory(transform.position, appliedForce, rb.mass, maxJumpDistance);

        transform.rotation = Quaternion.LookRotation(jumpDirection);
    }

    private void EndDrag()
    {
        isDragging = false;

        Vector3 rawVector = currentDragPosition - startDragPosition;
        Vector3 dragVector = new Vector3(-rawVector.x, 0f, -rawVector.z);

        float dragMagnitude = dragVector.magnitude;

        if (dragMagnitude <= dragCancelThreshold)
        {
            jumpRing.Hide();
            trajectoryPrediction.Hide();
            return;
        }

        float clampedMagnitude = Mathf.Min(dragMagnitude, maxJumpDistance);
        float dragPercent = clampedMagnitude / maxJumpDistance;

        if (jumpBar != null && !jumpBar.HasEnoughEnergy(dragPercent))
        {
            jumpRing.Hide();
            trajectoryPrediction.Hide();
            Debug.Log("Not enough energy to jump.");
            return;
        }

        Vector3 jumpDirection = dragVector.normalized;
        Vector3 appliedForce = (jumpDirection * dragPercent * jumpForce) + (Vector3.up * upwardForce);

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(appliedForce, ForceMode.Impulse);

        if (jumpBar != null)
        {
            jumpBar.ConsumeEnergy(dragPercent);
        }

        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);

        jumpRing.Hide();
        trajectoryPrediction.Hide();
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private Vector3 GetWorldPoint(Vector3 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }

    public void SetJumpEnabled(bool enabled)
    {
        canJump = enabled;
    }
}
