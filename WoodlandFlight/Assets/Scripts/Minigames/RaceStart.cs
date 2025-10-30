using UnityEngine;

public class RaceStart : MonoBehaviour, IInteractable
{
    [SerializeField] private Race race;
    [SerializeField] private string prompt;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor) {
        race.StartMinigame();
        return true;
    }
}
