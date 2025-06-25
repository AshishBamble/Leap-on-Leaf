using UnityEngine;

public class MovingLeafStick : MonoBehaviour
{
    [Tooltip("Tag of the frog/player GameObject")]
    [SerializeField] private string frogTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(frogTag))
        {
            // Parent the frog to the leaf
            collision.collider.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(frogTag))
        {
            // Unparent the frog from the leaf
            if (collision.collider.transform.parent == transform)
            {
                collision.collider.transform.SetParent(null, true);
            }
        }
    }
}
