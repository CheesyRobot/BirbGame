using System;
using UnityEngine;

public class TalkObjective : MonoBehaviour, IQuestObjectiveType
{
    public NPC npc;
    public string[] dialogueLines;
    private int currentDialogueLine;
    private bool completed;
    
    void Start() {
        currentDialogueLine = 0;
        completed = false;
    }
    
    public bool CheckCondition()
    {
        return completed;
    }

    public string SayOneLine() {
        if (currentDialogueLine < dialogueLines.Length)
            currentDialogueLine++;
        else
            completed = true;
        return dialogueLines[currentDialogueLine - 1];
    }
    public bool HasLinesLeft() {
        return !(currentDialogueLine == dialogueLines.Length);
    }

    public void StartObjective() {
        currentDialogueLine = 0;
        completed = false;
        npc.SetTalkObjective(this);
    }

    public void CompleteObjective() {
        npc.SetTalkObjective(this);
        completed = true;
        currentDialogueLine = dialogueLines.Length;
    }

    // public void SetNPC(NPC npc) {
    //     this.npc = npc;
    // }
}
