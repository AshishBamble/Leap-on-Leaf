using UnityEngine;

public class SinkingLeaf : MonoBehaviour
{
    [Header("Sinking Settings")]
    [SerializeField] private float sinkDelay = 1f;
    [SerializeField] private float sinkSpeed = 1f;

    [Tooltip("Target Y position after sinking (e.g., -0.78)")]
    [SerializeField] private float sinkTargetY = -0.78f;

    [SerializeField] private string frogTag = "Player";

    private bool hasFrogLanded = false;
    private bool isSinking = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasFrogLanded && collision.collider.CompareTag(frogTag))
        {
            hasFrogLanded = true;
            Invoke(nameof(BeginSinking), sinkDelay);
        }
    }

    private void BeginSinking()
    {
        isSinking = true;
    }

    private void Update()
    {
        if (isSinking)
        {
            Vector3 pos = transform.position;

            // Smooth sink using Lerp toward sinkTargetY
            float newY = Mathf.Lerp(pos.y, sinkTargetY, sinkSpeed * Time.deltaTime);
            transform.position = new Vector3(pos.x, newY, pos.z);

            if (Mathf.Abs(pos.y - sinkTargetY) < 0.01f)
            {
                transform.position = new Vector3(pos.x, sinkTargetY, pos.z);
                isSinking = false;
            }
        }
    }
}
