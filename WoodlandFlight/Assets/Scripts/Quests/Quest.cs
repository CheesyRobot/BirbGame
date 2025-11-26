using Unity.VisualScripting;
using UnityEngine;

public class Quest : MonoBehaviour
{
    private int currentQuestStage;
    private bool isCompleted;
    public QuestObjective[] objectives;

    void Start() {
        isCompleted = false;
        StartQuest();
        // for (int i = 0; i < objectives.Length; i++) {
        //     objectives[i].enabled = false;
        // }
    }

    void Update() {
        if (!isCompleted && objectives[currentQuestStage].IsCompleted()) {
            objectives[currentQuestStage].enabled = false;
            currentQuestStage++;
            if (currentQuestStage < objectives.Length)
                objectives[currentQuestStage].StartObjetive();
            else {
                isCompleted = true;
                Debug.Log("Quest completed");
            }
        }
    }

    public void StartQuest() {
        currentQuestStage = 0;
        objectives[currentQuestStage].StartObjetive();
    }
}
