using UnityEngine;

public interface IQuestObjectiveType
{
    public bool CheckCondition(QuestObjective quest);
    public void StartObjetive();
}