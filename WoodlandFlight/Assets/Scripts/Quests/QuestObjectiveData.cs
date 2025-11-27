using UnityEngine;

[System.Serializable]
public class QuestObjectiveData {
    public string hintText;
    public string journalEntryActive;
    public string journalEntryCompleted;
    public InterfaceReference<IQuestObjectiveType> objective;
}
