using UnityEngine;

public interface IInteractableModifier
{
    public string InteractionPrompt { get; }
    public bool Interact(Interactor interactor);
}
