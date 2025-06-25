using UnityEngine;

public class FrogCaughtInTornado : MonoBehaviour
{
    [Header("Frog Detection")]
    [SerializeField] private string frogTag = "Player";

    [Header("Orbit Settings")]
    [SerializeField] private float spinRadius = 1f;
    [SerializeField] private float orbitSpeed = 360f;
    [SerializeField] private float heightOffset = 0.5f;

    [Header("Throw Settings")]
    [SerializeField] private float spinDuration = 1.5f;
    [SerializeField] private float throwForce = 4f;
    [SerializeField] private Vector3 throwDirection = new Vector3(1, 1, 0);
    [SerializeField] private float catchCooldown = 1.5f;

    private Transform frogTransform;
    private Rigidbody frogRb;

    private float orbitAngle;
    private float spinTimer;
    private bool isCaught;
    private float lastThrowTime = -999f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isCaught && Time.time - lastThrowTime >= catchCooldown && other.CompareTag(frogTag))
        {
            frogTransform = other.transform;
            frogRb = frogTransform.GetComponent<Rigidbody>();
            if (frogRb == null) return;

            frogRb.isKinematic = true; // Disable physics during spin
            orbitAngle = 0f;
            spinTimer = 0f;
            isCaught = true;
        }
    }

    private void Update()
    {
        if (isCaught && frogTransform != null)
        {
            spinTimer += Time.deltaTime;
            orbitAngle += orbitSpeed * Time.deltaTime;
            float rad = orbitAngle * Mathf.Deg2Rad;

            // Orbit around tornado (this GameObject's position)
            Vector3 orbitPos = transform.position + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * spinRadius;
            orbitPos.y += heightOffset;

            frogTransform.position = orbitPos;

            if (spinTimer >= spinDuration)
            {
                ReleaseFrog();
            }
        }
    }

    private void ReleaseFrog()
    {
        if (frogTransform != null)
        {
            frogRb.isKinematic = false;

            // Apply force in world space
            Vector3 forceDir = throwDirection.normalized;
            frogRb.AddForce(forceDir * throwForce, ForceMode.Impulse);

            // Reset
            frogTransform = null;
            frogRb = null;
            isCaught = false;
            lastThrowTime = Time.time;
        }
    }
}
