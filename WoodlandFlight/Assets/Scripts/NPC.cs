using UnityEngine;
using System.Collections;

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
    private bool continueTalking;
    private bool blockNextInteraction;
    public string InteractionPrompt => prompt;

    void Start() {
        currentDialogueLine = 0;
        continueTalking = false;
        blockNextInteraction = false;
    }

    void Update() {
        if (lastInteractor != null && Vector3.Distance(lastInteractor.transform.position, transform.position) > exitDistance) {
            StopTalk();
            blockNextInteraction = false;
        }
        if (lastInteractor != null && Input.GetKeyDown(KeyCode.E)) {
            if (!continueTalking)
                continueTalking = true;
            else if (questDialogue == null)
                ContinueTalkIdle();
            else
                ContinueTalkQuest();
        }
    }
    public bool Interact(Interactor interactor) {
        if (blockNextInteraction) {
            blockNextInteraction = false;
            return true;
        }
        if (continueTalking) {
            return true;
        }
        if (questDialogue == null)
            StartTalkIdle();
        else
            StartTalkQuest();
        lastInteractor = interactor;
        continueTalking = false;
        return true;
    }
    
    private void StartTalkIdle() {
        if (!de.isEnabled()) {
            de.SetNameText(npcName);
            de.SetDialogueText(dialogue[currentDialogueLine]);
            de.Show(true);
            UICanvas.SetActive(false);
        }
    }

    public void StartTalkQuest() {
        if (!de.isEnabled()) {
            de.SetNameText(npcName);
            de.SetDialogueText(questDialogue.SayOneLine());
            de.Show(true);
            UICanvas.SetActive(false);
        }
    }

    private void ContinueTalkIdle() {
        if (currentDialogueLine < dialogue.Length - 1) {
            currentDialogueLine++;
            de.SetDialogueText(dialogue[currentDialogueLine]);
        }
        else {
            StopTalk();
        }
    }

    private void ContinueTalkQuest() {
        if (questDialogue.HasLinesLeft()) {
            de.SetDialogueText(questDialogue.SayOneLine());
        }
        else {
            questDialogue.SayOneLine();
            StopTalk();
        }
    }

    private void StopTalk() {
        de.Show(false);
        UICanvas.SetActive(true);
        lastInteractor = null;
        continueTalking = false;
        blockNextInteraction = true;
        StartCoroutine(UnblockInteraction(0.2f));
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

    IEnumerator UnblockInteraction(float seconds) {
        yield return new WaitForSeconds(seconds);
        blockNextInteraction = false;
    }

    IEnumerator DelayTalk(float seconds) {
        yield return new WaitForSeconds(seconds);
        if (questDialogue == null)
            StartTalkIdle();
        else
            StartTalkQuest();
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