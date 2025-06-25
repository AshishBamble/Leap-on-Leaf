using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
public class JumpRing : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField] private int segments = 40;
    [SerializeField] private float yOffset = 0.1f;
    [SerializeField] private float defaultRadius = 0.5f;
    [SerializeField] private Material ringMaterial;

    public float DefaultRadius => defaultRadius;

    private LineRenderer lineRenderer;
    private float currentRadius;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.enabled = false;
        lineRenderer.widthMultiplier = 0.05f;

        if (ringMaterial != null)
            lineRenderer.material = ringMaterial;
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            SetRadius(defaultRadius);
            lineRenderer.widthMultiplier = 0.05f;
        }
    }

    private void OnValidate()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.widthMultiplier = 0.05f;

        if (!Application.isPlaying)
        {
            SetRadius(defaultRadius);
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Force the ring to always project onto Y = 0
        position.y = 0 + yOffset;
        transform.position = position;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Flat on ground
        UpdateRing();
    }

    public void SetRadius(float radius)
    {
        currentRadius = radius;
        UpdateRing();
    }

    public void Show()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = true;
    }

    public void Hide()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    private void UpdateRing()
    {
        if (lineRenderer == null) return;

        lineRenderer.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            float x = Mathf.Cos(angle) * currentRadius;
            float z = Mathf.Sin(angle) * currentRadius;

            Vector3 point = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            lineRenderer.SetPosition(i, point);
        }
    }
}
