using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    [SerializeField] private string dialogue;
    [SerializeField] private string prompt;
    [SerializeField] private DialogueEvents de;
    [SerializeField] private GameObject UICanvas;
    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        if (!de.isEnabled()) {
            de.SetNameText(npcName);
            de.SetDialogueText(dialogue);
            de.Show(true);
            UICanvas.SetActive(false);
        }
        else {
            de.Show(false);
            UICanvas.SetActive(true);
        }
        return true;
    }
}
