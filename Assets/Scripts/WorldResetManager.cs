using UnityEngine;

public class WorldResetManager : MonoBehaviour
{
    public void ResetWorldObjects()
    {
        IResettable[] resettableObjects = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        ) as IResettable[];

        foreach (IResettable obj in resettableObjects)
        {
            obj.ResetObject();
        }

        Debug.Log("All resettable world objects have been reset.");
    }
}
