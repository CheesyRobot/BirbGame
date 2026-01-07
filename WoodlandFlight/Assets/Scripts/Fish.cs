using UnityEngine;

public class Fish : MonoBehaviour, IInteractableModifier
{
    public bool cought;
    public Grabbable grabbable;
    public FishMovement fishMovement;
    [SerializeField] private int experienceAmount;
    [SerializeField] private float staminaAmount;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    private bool minigame = false;
    private FishingGame game;

    void Start() {
        cought = false;
        grabbable.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    public void Catch(Transform grabPoint) {
        if (cought)
            return;
        cought = true;
        grabbable.enabled = true;
        fishMovement.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        grabbable.Grab(grabPoint);
    }

    public bool Interact(Interactor interactor)
    {
        if (!minigame) {
            Player player = interactor.GetComponent<Player>();
            player.AddExperience(experienceAmount);
            player.AddStamina(staminaAmount);
        }
        else {
            game?.AddScore();
        }
        GetComponent<Grabbable>()?.Drop();
        Destroy(gameObject);

        return true;
    }

    public void SetMinigame(FishingGame game, bool active) {
        this.game = game;
        minigame = active;
    }
}