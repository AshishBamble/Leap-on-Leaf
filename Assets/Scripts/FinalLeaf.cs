using UnityEngine;

public class FinalLeaf : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Level Complete - Resetting for now");

            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.ResetPlayer();
        }
    }
}
