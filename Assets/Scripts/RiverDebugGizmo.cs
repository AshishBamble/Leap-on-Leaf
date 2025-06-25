using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class RiverDebugGizmo : MonoBehaviour
{
    [Header("Debug Visual Settings")]
    [SerializeField] private Color debugColor = new Color(0f, 0.5f, 1f, 0.25f);
    [SerializeField] private bool showInGameView = true;

    private GameObject debugVisualizer;
    private Material debugMaterial;

    private void OnEnable()
    {
        CreateOrUpdateVisualizer();
    }

    private void OnDisable()
    {
        DestroyVisualizer();
    }

    private void OnValidate()
    {
        CreateOrUpdateVisualizer();
    }

    private void CreateOrUpdateVisualizer()
    {
        // Destroy old visualizer
        DestroyVisualizer();

        // Create cube for visualizer
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;

        debugVisualizer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        debugVisualizer.name = "[Debug] River Collider Visualizer";
        debugVisualizer.transform.SetParent(transform, false);
        debugVisualizer.transform.localPosition = box.center;
        debugVisualizer.transform.localRotation = Quaternion.identity;
        debugVisualizer.transform.localScale = box.size;

        // Remove unnecessary collider
        DestroyImmediate(debugVisualizer.GetComponent<Collider>());

        // Create and apply transparent material
        Shader shader = Shader.Find("Unlit/Color");
        debugMaterial = new Material(shader);
        debugMaterial.color = debugColor;

        MeshRenderer renderer = debugVisualizer.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = debugMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        // Hide in play mode if desired
        debugVisualizer.hideFlags = HideFlags.DontSave;
        debugVisualizer.SetActive(showInGameView);
    }

    private void DestroyVisualizer()
    {
        if (debugVisualizer == null) return;

#if UNITY_EDITOR
        if (!Application.isPlaying)
            DestroyImmediate(debugVisualizer);
        else
            Destroy(debugVisualizer);
#else
        Destroy(debugVisualizer);
#endif
    }
}
