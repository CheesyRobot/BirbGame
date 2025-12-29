using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> inactiveQuests;
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;
    public Quest trackedQuest;
    // public QuestManagerData questManagerData;

    void Start()
    {
        if (!File.Exists(SaveHandler.SaveFileName()))
            foreach (Quest quest in activeQuests) {
                quest.StartQuest();
                Debug.Log(quest.questName + " STARTED");
            }
        DisableNonActiveQuests();
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
        if (trackedQuest != null) {
            trackedQuest.TrackQuest(false);
            if (trackedQuest != item)
            {
                trackedQuest = item;
                trackedQuest.TrackQuest(true);
            }
        }
        else {
            trackedQuest = item;
            trackedQuest.TrackQuest(true);
        }
    }

    private void DisableNonActiveQuests() {
        foreach (Quest quest in inactiveQuests) {
            quest.enabled = false;
        }
        foreach (Quest quest in completedQuests) {
            quest.enabled = false;
        }
    }

    // public QuestManagerData Save() {
    //     QuestManagerData questManagerData = new();
    //     questManagerData.inactiveQuests = inactiveQuests;
    //     questManagerData.activeQuests = activeQuests;
    //     questManagerData.completedQuests = completedQuests;
    //     questManagerData.trackedQuest = trackedQuest;
    //     return questManagerData;
    // }

    // public void Load(QuestManagerData questManagerData) {
    //     inactiveQuests = questManagerData.inactiveQuests;
    //     activeQuests = questManagerData.activeQuests;
    //     completedQuests = questManagerData.completedQuests;
    //     trackedQuest = questManagerData.trackedQuest;
    // }

    public QuestManagerData Save() {
        QuestManagerData questManagerData = new();
        questManagerData.inactiveQuests = inactiveQuests.Select(q => q.ID).ToList();
        questManagerData.activeQuests = activeQuests.Select(q => new QuestData(q.ID, q.GetQuestStage())).ToList();
        questManagerData.completedQuests = completedQuests.Select(q => q.ID).ToList();
        if (trackedQuest != null)
            questManagerData.trackedQuest = trackedQuest.ID;
        return questManagerData;
    }

    public void Load(QuestManagerData questManagerData) {
        Quest[] quests = Resources.FindObjectsOfTypeAll<Quest>();
        inactiveQuests = quests.Where(q => questManagerData.inactiveQuests.Contains(q.ID)).ToList();
        activeQuests = quests.Where(q => questManagerData.activeQuests.Select(a => a.ID).Contains(q.ID)).ToList();
        completedQuests = quests.Where(q => questManagerData.completedQuests.Contains(q.ID)).ToList();
        if (questManagerData.trackedQuest != null)
            trackedQuest = quests.FirstOrDefault(q => q.ID == questManagerData.trackedQuest);
        else
            trackedQuest = null;

        DisableNonActiveQuests();
        
        foreach (Quest activeQuest in activeQuests) {
            foreach (QuestData questData in questManagerData.activeQuests) {
                if (activeQuest.ID == questData.ID) {
                    activeQuest.enabled = true;
                    activeQuest.SetQuestStage(questData.stage);
                }
            }
        }
    }
}

[System.Serializable]
public struct QuestManagerData {
    public List<string> inactiveQuests;
    public List<QuestData> activeQuests;
    public List<string> completedQuests;
    public string trackedQuest;
}

[System.Serializable]
public struct QuestData {
    public string ID;
    public int stage;
    public QuestData(string ID, int stage)
    {
        this.ID = ID;
        this.stage = stage;
    }
}
