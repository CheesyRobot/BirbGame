using UnityEngine;

public class FishingStart : MonoBehaviour, IInteractable
{
    [SerializeField] private FishingGame minigame;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor) {
        minigame.StartMinigame();
        return true;
    }
}
