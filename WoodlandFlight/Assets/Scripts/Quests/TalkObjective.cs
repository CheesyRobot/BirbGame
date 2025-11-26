using UnityEngine;

public class TalkObjective : MonoBehaviour, IQuestObjectiveType
{
    public NPC npc;
    public bool CheckCondition(Quest quest)
    {

        return true;
    }
}
