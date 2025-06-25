using UnityEngine;

public class TornadoZone : MonoBehaviour
{
    [Header("Tornado Settings")]
    [SerializeField] private string frogTag = "Player";
    [SerializeField] private float spinDuration = 1.5f;
    [SerializeField] private float throwForce = 4f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private float hpDamage = 0.2f;
    [SerializeField] private float disableControlTime = 1.5f;

    private bool isActive = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || !other.CompareTag(frogTag)) return;

        Rigidbody frogRb = other.attachedRigidbody;
        FrogHPBar hpBar = other.GetComponent<FrogHPBar>();
        FrogJump frogJump = other.GetComponent<FrogJump>();

        if (frogRb != null)
        {
            StartCoroutine(SpinAndThrowFrog(frogRb, hpBar, frogJump));
            isActive = false; // Prevent re-trigger
        }
    }

    private System.Collections.IEnumerator SpinAndThrowFrog(Rigidbody frogRb, FrogHPBar hpBar, FrogJump frogJump)
    {
        if (frogJump != null)
            frogJump.SetJumpEnabled(false);

        float timer = 0f;

        // Cache transform
        Transform frogTransform = frogRb.transform;

        while (timer < spinDuration)
        {
            frogTransform.Rotate(Vector3.up, 720f * Time.deltaTime); // Fast spin
            timer += Time.deltaTime;
            yield return null;
        }

        // Apply short toss force
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 force = (randomDirection * throwForce) + (Vector3.up * upwardForce);
        frogRb.linearVelocity = Vector3.zero;
        frogRb.AddForce(force, ForceMode.Impulse);

        // Damage HP
        if (hpBar != null)
        {
            hpBar.ReduceHealth(hpDamage);
        }

        // Wait before enabling jump again
        yield return new WaitForSeconds(disableControlTime);

        if (frogJump != null)
            frogJump.SetJumpEnabled(true);
    }
}
