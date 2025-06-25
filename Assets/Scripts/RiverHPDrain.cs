using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RiverHPDrain : MonoBehaviour
{
    [Header("Frog Detection")]
    [SerializeField] private string frogTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(frogTag))
        {
            FrogHPBar hpBar = other.GetComponent<FrogHPBar>();
            if (hpBar != null)
            {
                hpBar.StartDraining();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(frogTag))
        {
            FrogHPBar hpBar = other.GetComponent<FrogHPBar>();
            if (hpBar != null)
            {
                hpBar.StopDraining();
            }
        }
    }
}
