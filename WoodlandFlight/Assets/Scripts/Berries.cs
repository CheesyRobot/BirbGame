using UnityEngine;

public class Berries : MonoBehaviour, IInteractable
{
    [SerializeField] private float staminaAmount;
    [SerializeField] private int experienceAmount;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public void Start() {
        InvokeRepeating("Respawn", 1.0f, 2.0f);
    }
    public bool Interact(Interactor interactor) {
        interactor.GetComponent<Player>().IncreaseMaxStamina(staminaAmount);
        interactor.GetComponent<Player>().AddStamina(staminaAmount);
        interactor.GetComponent<Player>().AddExperience(experienceAmount);
        this.gameObject.SetActive(false);
        return true;
    }

    private void Respawn() {
        if (Random.Range(0f, 1f) <= 0.5f)
        this.gameObject.SetActive(true);
    }
}
