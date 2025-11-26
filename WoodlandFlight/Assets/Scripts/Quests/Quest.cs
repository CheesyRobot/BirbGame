using UnityEngine;

public class Quest : MonoBehaviour
{
    private int currentQuestStage;
    public QuestObjective[] objectives;

    void Start()
    {
        currentQuestStage = 0;
    }
}
