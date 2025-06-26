using UnityEngine;
using System.Collections;

public class TornadoZone : MonoBehaviour
{
    [Header("Tornado Settings")]
    [SerializeField] private string frogTag = "Player";

    [Tooltip("Min and Max duration the frog will be spun before thrown")]
    [SerializeField] private float minSpinDuration = 1f;
    [SerializeField] private float maxSpinDuration = 2f;

    [SerializeField] private float throwForce = 4f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private float hpDamage = 0.2f;
    [SerializeField] private float disableControlTime = 1.5f;

    [Tooltip("Cooldown before the tornado can affect the frog again")]
    [SerializeField] private float reactivationCooldown = 1f;

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
            isActive = false;
            Invoke(nameof(Reactivate), reactivationCooldown);
        }
    }

    private IEnumerator SpinAndThrowFrog(Rigidbody frogRb, FrogHPBar hpBar, FrogJump frogJump)
    {
        if (frogJump != null)
            frogJump.SetJumpEnabled(false);

        float spinDuration = Random.Range(minSpinDuration, maxSpinDuration);
        float timer = 0f;
        Transform frogTransform = frogRb.transform;

        while (timer < spinDuration)
        {
            frogTransform.Rotate(Vector3.up, 720f * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 force = (randomDirection * throwForce) + (Vector3.up * upwardForce);

        frogRb.linearVelocity = Vector3.zero;
        frogRb.AddForce(force, ForceMode.Impulse);

        if (hpBar != null)
        {
            hpBar.ReduceHealth(hpDamage);
        }

        yield return new WaitForSeconds(disableControlTime);

        if (frogJump != null)
            frogJump.SetJumpEnabled(true);
    }

    private void Reactivate()
    {
        isActive = true;
    }
}
