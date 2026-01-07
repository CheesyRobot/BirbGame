//using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field:SerializeField] public float health { get; private set; }
    [field:SerializeField] public float stamina { get; private set; }
    [field:SerializeField] public float staminaLimit { get; private set; }
    [field:SerializeField] public int experience { get; private set; }
    public int currentLevel { get; private set; }
    public float weightLimit { get; private set; }
    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }
    [field:SerializeField] public float staminaRecoveryRate { get; private set; }
    [field:SerializeField] public float staminaRecoveryRateGliding { get; private set; }
    [field:SerializeField] public float staminaRecoveryDelay { get; private set; }
    [field:SerializeField] public float healthRecoveryRate { get; private set; }
    [field:SerializeField] public float staminaConsumptionRate { get; private set; }
    [field:SerializeField] public Transform respawnPoint { get; private set; }
    
    [System.Serializable] private struct PlayerLevel {
        public int requiredXP;
        public float health;
        public float staminaLimit;
        public float weightLimit;
    }

    [SerializeField] private PlayerLevel[] levels;
    [SerializeField] private DisplayLevel levelDisplay;
    private Movement movement;
    void Start()
    {
        movement = GetComponent<Movement>();
        currentLevel = 1;
        UpdateStats();
        levelDisplay.UpdateLevel(currentLevel, experience, levels.Length);
        currentHealth = health;
        currentStamina = stamina;
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
        while (currentLevel < levels.Length && experience >= levels[currentLevel].requiredXP) {
            experience -= levels[currentLevel].requiredXP;
            currentLevel++;
            UpdateStats();
        }
            levelDisplay.UpdateLevel(currentLevel, experience, levels.Length);
    }

    private void UpdateStats() {
        health = levels[currentLevel - 1].health;
        staminaLimit = levels[currentLevel - 1].staminaLimit;
        weightLimit = levels[currentLevel - 1].weightLimit;
    }

    public float SetStaminaConsumptionRate(float rate) {
        float currentRate = staminaConsumptionRate;
        staminaConsumptionRate = rate;
        return currentRate;
    }

    public void Death() {
        ScreenFade.Instance.FadeOutHoldFadeIn(0f, 1f, 1f);
        Grabbable grabbedObject = GetComponent<GrabControl>().GetGrabbedObject();
        if (grabbedObject != null)
            grabbedObject.Drop();
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        currentStamina = stamina;
        experience = 0;
    }

    public PlayerData Save() {
        PlayerData playerData = new();
        playerData.position = transform.position;
        playerData.rotation = transform.rotation;
        playerData.cameraPosition = movement.Camera.position;
        playerData.cameraRotation = movement.Camera.rotation;
        playerData.currentLevel = currentLevel;
        playerData.experience = experience;
        playerData.currentStamina = currentStamina;
        playerData.currentHealth = currentHealth;
        return playerData;
    }

    public void Load(PlayerData playerData) {
        transform.position = playerData.position;
        transform.rotation = playerData.rotation;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        movement.Camera.position = playerData.cameraPosition;
        movement.Camera.rotation = playerData.cameraRotation;
        currentLevel = playerData.currentLevel;
        experience = playerData.experience;
        UpdateStats();
        levelDisplay.UpdateLevel(currentLevel, experience, levels.Length);
        currentStamina = playerData.currentStamina;
        currentHealth = playerData.currentHealth;
    }
}

[System.Serializable]
public struct PlayerData {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public int currentLevel;
    public int experience;
    public float currentStamina;
    public float currentHealth;
}
