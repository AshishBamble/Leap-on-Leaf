using UnityEngine;
using System.Collections.Generic;

public class LeafResetManager : MonoBehaviour
{
    [Header("Leaf Tags To Reset")]
    [SerializeField] private List<string> leafTags = new List<string> { "StaticLeaf", "MovingLeaf", "SinkingLeaf", "SubmergedLeaf" };

    private List<Transform> leafObjects = new List<Transform>();
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();

    private void Start()
    {
        foreach (string tag in leafTags)
        {
            GameObject[] leaves = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject leaf in leaves)
            {
                Transform t = leaf.transform;
                if (!originalPositions.ContainsKey(t))
                {
                    leafObjects.Add(t);
                    originalPositions[t] = t.position;
                    originalRotations[t] = t.rotation;
                }
            }
        }
    }

    public void ResetLeaves()
    {
        foreach (Transform leaf in leafObjects)
        {
            if (leaf != null)
            {
                leaf.position = originalPositions[leaf];
                leaf.rotation = originalRotations[leaf];

                Rigidbody rb = leaf.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                leaf.gameObject.SetActive(true); // Reactivate if deactivated (e.g. sinking leaf)
            }
        }

        Debug.Log("All leaves reset.");
    }
}
