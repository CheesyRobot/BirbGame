using UnityEngine;

public interface IQuestObjectiveType
{
    public bool CheckCondition();
    public void StartObjective();
    
    // For loading quest save data
    public void CompleteObjective();
}