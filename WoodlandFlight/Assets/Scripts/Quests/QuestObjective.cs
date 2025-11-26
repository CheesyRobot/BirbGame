using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public string hintText;
    public string journalEntryActive;
    public string journalEntryCompleted;
    public GameObject objective;
    private IQuestObjectiveType questObjectiveType;

    void Start() {
        questObjectiveType = objective.GetComponent<IQuestObjectiveType>();
    }

    public bool IsCompleted() {
        return questObjectiveType.CheckCondition(this);
    }
    public void StartObjetive() {
        questObjectiveType.StartObjetive();
    }
}
