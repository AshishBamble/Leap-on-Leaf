using UnityEngine;
using System.Collections.Generic;

public class HealingInsectSpawner : MonoBehaviour, IResettable
{
    [Header("Healing Insect Settings")]
    [SerializeField] private GameObject healingInsectPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Transform> pathPoints;

    private GameObject currentInsect;

    private void Start()
    {
        // Only spawn if nothing exists
        if (currentInsect == null)
        {
            SpawnInsect();
        }
    }

    private void SpawnInsect()
    {
        if (healingInsectPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Spawner missing prefab or spawn point.");
            return;
        }

        // Double-check no duplicates
        if (currentInsect != null)
        {
            Destroy(currentInsect);
        }

        currentInsect = Instantiate(healingInsectPrefab, spawnPoint.position, spawnPoint.rotation);

        HealingInsect insectScript = currentInsect.GetComponent<HealingInsect>();
        if (insectScript != null)
        {
            insectScript.SetPathPoints(pathPoints);
        }
    }

    public void ResetObject()
    {
        SpawnInsect(); // replaces the old one (if any)
    }
}
