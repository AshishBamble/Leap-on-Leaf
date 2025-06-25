using UnityEngine;

public class FrogStickToLeaf : MonoBehaviour
{
    [Header("Tag Settings")]
    [Tooltip("Tag used for the frog/player GameObject")]
    [SerializeField] private string frogTag = "Player";

    [Tooltip("Tag used for all leaves that the frog should stick to")]
    [SerializeField] private string leafTag = "Leaf";

    private Transform frogTransform;
    private Transform currentLeaf;
    private Vector3 offset;

    private void FixedUpdate()
    {
        if (frogTransform == null)
        {
            FindFrog();
            return;
        }

        if (currentLeaf != null)
        {
            frogTransform.position = currentLeaf.position + offset;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(frogTag))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.otherCollider.CompareTag(leafTag))
                {
                    frogTransform = collision.transform;
                    currentLeaf = contact.otherCollider.transform;
                    offset = frogTransform.position - currentLeaf.position;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(frogTag))
        {
            if (currentLeaf != null)
            {
                currentLeaf = null;
            }
        }
    }

    private void FindFrog()
    {
        GameObject frog = GameObject.FindGameObjectWithTag(frogTag);
        if (frog != null)
        {
            frogTransform = frog.transform;
        }
    }

    // 🔁 Call this from FrogJump to allow free movement
    public void DetachFromLeaf()
    {
        currentLeaf = null;
    }
}
