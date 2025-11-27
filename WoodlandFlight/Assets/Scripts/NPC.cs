using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName;
    [SerializeField] private string[] dialogue;
    [SerializeField] private string prompt;
    [SerializeField] private float exitDistance;
    [SerializeField] private DialogueEvents de;
    [SerializeField] private GameObject UICanvas;
    private TalkObjective questDialogue;
    private int currentDialogueLine;
    private Interactor lastInteractor;
    public string InteractionPrompt => prompt;

    void Start() {
        currentDialogueLine = 0;
    }

    void Update() {
        if (lastInteractor != null && Vector3.Distance(lastInteractor.transform.position, transform.position) > exitDistance) {
            de.Show(false);
            UICanvas.SetActive(true);
            lastInteractor = null;
        }
    }
    public bool Interact(Interactor interactor) {
        if (questDialogue == null)
            TalkIdle();
        else
            TalkQuest();
        lastInteractor = interactor;
        return true;
    }

    private void TalkIdle() {
        if (!de.isEnabled()) {
            de.SetNameText(npcName);
            de.SetDialogueText(dialogue[currentDialogueLine]);
            de.Show(true);
            UICanvas.SetActive(false);
        }
        else if (currentDialogueLine < dialogue.Length - 1) {
            currentDialogueLine++;
            de.SetDialogueText(dialogue[currentDialogueLine]);
        }
        else {
            de.Show(false);
            UICanvas.SetActive(true);
        }
    }

    public void TalkQuest() {
        if (!de.isEnabled()) {
            de.SetNameText(npcName);
            de.SetDialogueText(questDialogue.SayOneLine());
            de.Show(true);
            UICanvas.SetActive(false);
        }
        else if (questDialogue.HasLinesLeft()) {
            de.SetDialogueText(questDialogue.SayOneLine());
        }
        else {
            questDialogue.SayOneLine();
            de.Show(false);
            UICanvas.SetActive(true);
        }
    }

    public void SetTalkObjective(TalkObjective to) {
        questDialogue = to;
    }
}