using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public float health { get; private set; }
    [field:SerializeField] public float stamina { get; private set; }
    [field:SerializeField] public float staminaLimit { get; private set; }
    [field:SerializeField] public int experience { get; private set; }
    public float weightLimit { get; private set; }
    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }
    [field:SerializeField] public float staminaRecoveryRate { get; private set; }
    [field:SerializeField] public float staminaRecoveryRateGliding { get; private set; }
    [field:SerializeField] public float staminaRecoveryDelay { get; private set; }
    [field:SerializeField] public float healthRecoveryRate { get; private set; }
    [field:SerializeField] public float staminaConsumptionRate { get; private set; }
    void Start()
    {
        currentHealth = health;
        currentStamina = stamina;
    }

    void Update()
    {
        
    }

    public void AddHealth(float amount) {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
    }

    public void AddStamina(float amount) {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, stamina);
    }

    public void IncreaseMaxStamina(float amount) {
        stamina = Mathf.Clamp(stamina + amount, 0, staminaLimit);
    }
    public void IncreaseWeightLimit(float amount) {
        weightLimit += amount;
    }

    public void AddExperience(int amount) {
        experience += amount;
    }
}
