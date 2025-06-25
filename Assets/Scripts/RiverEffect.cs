using UnityEngine;

public class RiverEffect : MonoBehaviour
{
    [Header("River Jump Settings")]
    [SerializeField] private float riverJumpForce = 2f;
    [SerializeField] private float riverUpwardForce = 4.65f;
    [SerializeField] private float riverMaxJumpDistance = 2f;
    [SerializeField] private float riverDragCancelThreshold = 0.5f;
    [SerializeField] private float riverJumpCooldown = 2f;

    private FrogJump frogJump;
    private JumpBarController jumpBar;

    // Backup of original values
    private float originalJumpForce;
    private float originalUpwardForce;
    private float originalMaxJumpDistance;
    private float originalDragCancelThreshold;
    private float originalJumpCooldown;

    private bool isInRiver = false;

    private void Awake()
    {
        frogJump = GetComponent<FrogJump>();
        if (frogJump == null)
        {
            Debug.LogError("RiverEffect requires a FrogJump component on the same GameObject.");
            enabled = false;
            return;
        }

        jumpBar = Object.FindFirstObjectByType<JumpBarController>();
        if (jumpBar == null)
        {
            Debug.LogWarning("RiverEffect: No JumpBarController found in scene.");
        }

        // Cache original values
        originalJumpForce = GetPrivateField("jumpForce");
        originalUpwardForce = GetPrivateField("upwardForce");
        originalMaxJumpDistance = GetPrivateField("maxJumpDistance");
        originalDragCancelThreshold = GetPrivateField("dragCancelThreshold");
        originalJumpCooldown = GetPrivateField("jumpCooldown");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("River") && !isInRiver)
        {
            ApplyRiverValues();
            isInRiver = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("River") && isInRiver)
        {
            RestoreOriginalValues();
            isInRiver = false;
        }
    }

    private void ApplyRiverValues()
    {
        SetPrivateField("jumpForce", riverJumpForce);
        SetPrivateField("upwardForce", riverUpwardForce);
        SetPrivateField("maxJumpDistance", riverMaxJumpDistance);
        SetPrivateField("dragCancelThreshold", riverDragCancelThreshold);
        SetPrivateField("jumpCooldown", riverJumpCooldown);

        if (jumpBar != null) jumpBar.SetRiverState(true);

        Debug.Log("RiverEffect: Applied river settings.");
    }

    private void RestoreOriginalValues()
    {
        SetPrivateField("jumpForce", originalJumpForce);
        SetPrivateField("upwardForce", originalUpwardForce);
        SetPrivateField("maxJumpDistance", originalMaxJumpDistance);
        SetPrivateField("dragCancelThreshold", originalDragCancelThreshold);
        SetPrivateField("jumpCooldown", originalJumpCooldown);

        if (jumpBar != null) jumpBar.SetRiverState(false);

        Debug.Log("RiverEffect: Restored original jump settings.");
    }

    private float GetPrivateField(string fieldName)
    {
        var field = typeof(FrogJump).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(frogJump);
    }

    private void SetPrivateField(string fieldName, float value)
    {
        var field = typeof(FrogJump).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(frogJump, value);
    }
}
