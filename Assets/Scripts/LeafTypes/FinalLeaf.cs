using UnityEngine;

public class FinalLeaf : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Level Complete - Resetting for now");

            // Reset Player
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.ResetPlayer();

            // Reset Leaves
            LeafResetManager leafReset = FindFirstObjectByType<LeafResetManager>();
            if (leafReset != null)
                leafReset.ResetLeaves();

            // Reset all IResettable objects (Tornado, InsectSpawner, etc.)
            MonoBehaviour[] allMonoBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour mono in allMonoBehaviours)
            {
                if (mono is IResettable resettable)
                {
                    resettable.ResetObject();
                }
            }

            Debug.Log("All IResettable objects reset.");
        }
    }
}
