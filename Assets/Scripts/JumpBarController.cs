using UnityEngine;
using UnityEngine.UI;

public class JumpBarController : MonoBehaviour
{
    [Header("Jump Bar UI")]
    [SerializeField] private Image jumpBarFill;

    [Header("Refill Settings")]
    [SerializeField] private float NormalrefillRate = 0.25f;
    [SerializeField] private float RiverrefillRate = 0.5f;

    [SerializeField] private float NormalrefillCooldown = 1.5f;
    [SerializeField] private float RiverrefillCooldown = 1f;

    [Header("Energy Multipliers")]
    [SerializeField] private float normalEnergyMultiplier = 0.5f;
    [SerializeField] private float riverEnergyMultiplier = 0.8f;

    private float energyMultiplier;
    private float refillRate;
    private float refillCooldown;

    private float currentEnergy = 1f;
    private bool isRefilling = false;
    private float lastConsumeTime = -999f;

    private void Awake()
    {
        energyMultiplier = normalEnergyMultiplier;
        refillRate = NormalrefillRate;
        refillCooldown = NormalrefillCooldown;
    }

    private void Update()
    {
        // Trigger refill if <1
        if (currentEnergy > 0f && currentEnergy < 1f)
        {
            isRefilling = true;
        }

        // Trigger cooldown-based refill if empty
        if (currentEnergy <= 0f && !isRefilling && Time.time - lastConsumeTime >= refillCooldown)
        {
            isRefilling = true;
        }

        if (isRefilling)
        {
            currentEnergy += refillRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp01(currentEnergy);
            UpdateUI();

            if (currentEnergy >= 1f)
            {
                isRefilling = false;
            }
        }
    }

    public bool HasEnoughEnergy(float percent)
    {
        float required = percent * energyMultiplier;
        return currentEnergy >= required;
    }

    public void ConsumeEnergy(float percent)
    {
        float required = percent * energyMultiplier;
        required = Mathf.Min(required, currentEnergy);

        currentEnergy -= required;
        currentEnergy = Mathf.Clamp01(currentEnergy);
        lastConsumeTime = Time.time;

        if (currentEnergy <= 0f)
        {
            isRefilling = false;
        }

        UpdateUI();
    }

    public void SetRiverState(bool inRiver)
    {
        energyMultiplier = inRiver ? riverEnergyMultiplier : normalEnergyMultiplier;
        refillRate = inRiver ? RiverrefillRate : NormalrefillRate;
        refillCooldown = inRiver ? RiverrefillCooldown : NormalrefillCooldown;
    }

    private void UpdateUI()
    {
        if (jumpBarFill != null)
        {
            jumpBarFill.fillAmount = currentEnergy;
        }
    }
}
