using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> inactiveQuests;
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;
    public Quest trackedQuest;

    void Start()
    {
        foreach (Quest quest in activeQuests) {
            quest.StartQuest();
            Debug.Log(quest.questName + " STARTED");
        }
    }
    public void MarkQuestActive(Quest item) {
        inactiveQuests.Remove(item);
        activeQuests.Add(item);
        item.enabled = true;
        item.StartQuest();
        Debug.Log(item.questName + " STARTED");
    }

    public void MarkQuestCompleted(Quest item) {
        activeQuests.Remove(item);
        completedQuests.Add(item);
        if (trackedQuest == item)
            trackedQuest.TrackQuest(false);
        item.enabled = false;
        Debug.Log(item.questName + " COMPLETED");
    }

    public void MarkQuestTracked(Quest item) {
        trackedQuest.TrackQuest(false);
        if (trackedQuest != item) {
            trackedQuest = item;
            trackedQuest.TrackQuest(true);
        }
    }
}
