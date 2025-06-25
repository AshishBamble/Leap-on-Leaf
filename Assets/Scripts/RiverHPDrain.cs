using UnityEngine;

public class RiverHPDrain : MonoBehaviour
{
    [SerializeField] private string riverBodyTag = "RiverBody";

    private FrogHPBar frogHPBar;

    private void Awake()
    {
        frogHPBar = Object.FindFirstObjectByType<FrogHPBar>();
        if (frogHPBar == null)
        {
            Debug.LogWarning("RiverHPDrain: No FrogHPBar found in scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(riverBodyTag))
        {
            frogHPBar?.StartDraining();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(riverBodyTag))
        {
            frogHPBar?.StopDraining();
        }
    }
}
