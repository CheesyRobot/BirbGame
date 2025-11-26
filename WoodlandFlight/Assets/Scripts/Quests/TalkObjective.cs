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
    
    public bool CheckCondition(QuestObjective quest)
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
        return !completed;
    }

    public void StartObjetive() {
        npc.SetTalkObjective(this);
    }

    // public void SetNPC(NPC npc) {
    //     this.npc = npc;
    // }
}
