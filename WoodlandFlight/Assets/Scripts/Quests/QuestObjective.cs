using UnityEngine;

[System.Serializable]
public class QuestObjective : MonoBehaviour
{
    public string hintText;
    public string journalEntryActive;
    public string journalEntryCompleted;
    public GameObject objective;
    public IQuestObjectiveType questObjectiveType;

    void Start() {
        questObjectiveType = objective.GetComponent<IQuestObjectiveType>();
    }

    public bool IsCompleted() {
        return questObjectiveType.CheckCondition();
    }
    public void StartObjetive() {
        questObjectiveType.StartObjective();
    }
}