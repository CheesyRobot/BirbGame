using System;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;

public class Quest : MonoBehaviourID
{
    private int currentQuestStage;
    private bool isCompleted;
    public string questName;
    public QuestManager manager;
    // [SerializeField] public QuestObjective[] objectives;
    [SerializeField] public QuestObjectiveData[] questStages;
    // private IQuestObjectiveType[] iobjectives;
    public Quest nextQuest;
    public event EventHandler<OnStageChangedEventArgs> OnStageChanged;
    public class OnStageChangedEventArgs : EventArgs {
        public int stage;
    }

    void Start() {
        // iobjectives = new IQuestObjectiveType[questData.Length];
        isCompleted = false;
        // for (int i = 0; i < questData.Length; i++) {
        //     iobjectives[i] = questData[i].objective.GetComponent<IQuestObjectiveType>();
        // }
        // for (int i = 0; i < objectives.Length; i++) {
        //     objectives[i].enabled = false;
        // }
        // StartQuest();
    }

    void Update() {
        if (!isCompleted && questStages[currentQuestStage].objective.Value.CheckCondition()) {
            // objectives[currentQuestStage].enabled = false;
            currentQuestStage++;
            OnStageChanged?.Invoke(this, new OnStageChangedEventArgs{stage = currentQuestStage});

            if (currentQuestStage < questStages.Length)
                questStages[currentQuestStage].objective.Value.StartObjective();
            else {
                isCompleted = true;
                ResetNPCs();
                manager.MarkQuestCompleted(this);
                if (nextQuest != null)
                    manager.MarkQuestActive(nextQuest);
            }
        }
        // if (!isCompleted && objectives[currentQuestStage].IsCompleted()) {
        //     objectives[currentQuestStage].enabled = false;
        //     currentQuestStage++;
        //     if (currentQuestStage < objectives.Length)
        //         objectives[currentQuestStage].StartObjetive();
        //     else {
        //         isCompleted = true;
        //         Debug.Log("Quest completed");
        //     }
        // }
    }

    public void StartQuest() {
        isCompleted = false;
        currentQuestStage = 0;
        OnStageChanged?.Invoke(this, new OnStageChangedEventArgs{stage = currentQuestStage});
        // objectives[currentQuestStage].StartObjetive();
        questStages[currentQuestStage].objective.Value.StartObjective();
    }

    public void TrackQuest(bool value) {
        // Should show objective hint text on HUD
    }

    public int GetQuestStage() {
        return currentQuestStage;
    }

    public void SetQuestStage(int stage) {
        isCompleted = false;
        for (int i = 0; i < stage; i++) {
            questStages[i].objective.Value.CompleteObjective();
        }
        currentQuestStage = stage;
        questStages[currentQuestStage].objective.Value.StartObjective();
    }

    public string GetQuestDescription() {
        string text = "";
        int i = 0;
        while (i < currentQuestStage) {
            text += questStages[i].journalEntryActive + "\n";
            text += questStages[i].journalEntryCompleted + "\n";
            i++;
        }
        if (isCompleted) {
            //text += questStages[i].journalEntryCompleted;
        }
        else
            text += questStages[i].journalEntryActive;
        return text;
    }

    // Set NPC dialogue to default
    private void ResetNPCs() {
        for (int i = 0; i < questStages.Length; i++) {
            if (questStages[i].objective.Value is TalkObjective objective) {
                objective.npc.SetTalkObjective(null);
            }
        }
    }
}
