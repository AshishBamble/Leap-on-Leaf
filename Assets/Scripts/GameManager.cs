using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform startPoint;    // Assign in Inspector
    public GameObject player;       // Assign frog/player here

    public void ResetPlayer()
    {
        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.position;
            player.transform.rotation = Quaternion.identity; // ✅ Reset rotation
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            Debug.Log("Player reset to start point.");
        }
        else
        {
            Debug.LogWarning("GameManager: StartPoint or Player not assigned.");
        }
    }
}
