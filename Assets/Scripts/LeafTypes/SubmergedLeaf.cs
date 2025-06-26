using UnityEngine;

public class SubmergedLeaf : MonoBehaviour
{
    [Header("Sinking Settings")]
    [SerializeField] private float sinkDelay = 1f;
    [SerializeField] private float sinkSpeed = 1f;

    [Tooltip("Target Y position after sinking (e.g., -0.78)")]
    [SerializeField] private float sinkTargetY = -0.78f;

    [SerializeField] private string frogTag = "Player";

    private bool isSinking = false;
    private bool isRising = false;
    private float originalY;
    private Transform frogTransform;

    private void Start()
    {
        originalY = transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(frogTag))
        {
            frogTransform = collision.collider.transform;
            CancelInvoke(nameof(BeginRising)); // in case it's rising
            Invoke(nameof(BeginSinking), sinkDelay);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(frogTag) && collision.collider.transform == frogTransform)
        {
            frogTransform = null;
            CancelInvoke(nameof(BeginSinking)); // Cancel sinking if frog leaves early
            Invoke(nameof(BeginRising), sinkDelay);
        }
    }

    private void BeginSinking()
    {
        if (frogTransform != null) // ensure frog is still there
        {
            isSinking = true;
            isRising = false;
        }
    }

    private void BeginRising()
    {
        isRising = true;
        isSinking = false;
    }

    private void Update()
    {
        if (isSinking)
        {
            Vector3 pos = transform.position;
            float newY = Mathf.Lerp(pos.y, sinkTargetY, sinkSpeed * Time.deltaTime);
            transform.position = new Vector3(pos.x, newY, pos.z);

            if (Mathf.Abs(pos.y - sinkTargetY) < 0.01f)
            {
                transform.position = new Vector3(pos.x, sinkTargetY, pos.z);
                isSinking = false;
            }
        }
        else if (isRising)
        {
            Vector3 pos = transform.position;
            float newY = Mathf.Lerp(pos.y, originalY, sinkSpeed * Time.deltaTime);
            transform.position = new Vector3(pos.x, newY, pos.z);

            if (Mathf.Abs(pos.y - originalY) < 0.01f)
            {
                transform.position = new Vector3(pos.x, originalY, pos.z);
                isRising = false;
            }
        }
    }
}
