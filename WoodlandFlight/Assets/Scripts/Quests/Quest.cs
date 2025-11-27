using Unity.VisualScripting;
using UnityEngine;

public class Quest : MonoBehaviour
{
    private int currentQuestStage;
    private bool isCompleted;
    public string questName;
    public QuestManager manager;
    // [SerializeField] public QuestObjective[] objectives;
    [SerializeField] public QuestObjectiveData[] questStages;
    // private IQuestObjectiveType[] iobjectives;
    public Quest nextQuest;

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
            if (currentQuestStage < questStages.Length)
                questStages[currentQuestStage].objective.Value.StartObjective();
            else {
                isCompleted = true;
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
        currentQuestStage = 0;
        // objectives[currentQuestStage].StartObjetive();
        questStages[currentQuestStage].objective.Value.StartObjective();
    }

    public void TrackQuest(bool value) {
        // Should show objective hint text on HUD
    }
}
