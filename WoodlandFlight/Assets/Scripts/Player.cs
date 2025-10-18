using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float health;
    [SerializeField] private float stamina;
    [SerializeField] private float staminaLimit;
    [SerializeField] private int experience;
    public float weightLimit { get; private set; }
    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }
    public float recoveryRateStamina { get; private set; }
    public float recoveryRateHealth { get; private set; }
    void Start()
    {
        currentHealth = health;
        currentStamina = stamina;
    }

    // Update is called once per frame
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
