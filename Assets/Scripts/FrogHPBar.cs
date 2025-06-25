using UnityEngine;
using UnityEngine.UI;

public class FrogHPBar : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private Image hpBarFill;
    [SerializeField] private float drainRate = 0.1f;

    private float currentHP = 1f;
    private bool isDraining = false;

    private void Update()
    {
        if (isDraining)
        {
            currentHP -= drainRate * Time.deltaTime;
            currentHP = Mathf.Clamp01(currentHP);
            UpdateUI();
        }
    }

    public void StartDraining()
    {
        isDraining = true;
    }

    public void StopDraining()
    {
        isDraining = false;
    }

    public void SetHP(float value)
    {
        currentHP = Mathf.Clamp01(value);
        UpdateUI();
    }

    public void AddHealth(float amount)
    {
        currentHP = Mathf.Clamp(currentHP + amount, 0f, 1f);
        UpdateUI();
    }

    public void ReduceHealth(float amount)
    {
        currentHP = Mathf.Clamp(currentHP - amount, 0f, 1f);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = currentHP;
        }
    }
}
