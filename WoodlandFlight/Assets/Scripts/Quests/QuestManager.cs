using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Runtime.Versioning;
using System;

public class QuestManager : MonoBehaviour
{
    public List<Quest> inactiveQuests;
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;
    public Quest trackedQuest;
    public QuestTrackEvents qte;
    // public QuestManagerData questManagerData;

    void Start()
    {
        inactiveQuests = Resources.FindObjectsOfTypeAll<Quest>().ToList();
        inactiveQuests.RemoveAll(q => activeQuests.Contains(q) || completedQuests.Contains(q));

        if (true || !File.Exists(SaveHandler.SaveFileName())) {
            foreach (Quest quest in activeQuests) {
                quest.StartQuest();
                Debug.Log(quest.questName + " STARTED");
                MarkQuestTracked(quest);
            }
        }
        
        DisableNonActiveQuests();
    }
    public void MarkQuestActive(Quest item) {
        inactiveQuests.Remove(item);
        activeQuests.Add(item);
        item.enabled = true;
        item.StartQuest();
        MarkQuestTracked(item);
        Debug.Log(item.questName + " STARTED");
    }

    public void MarkQuestCompleted(Quest item) {
        activeQuests.Remove(item);
        completedQuests.Add(item);
        if (trackedQuest == item)
            MarkQuestTracked(activeQuests.FirstOrDefault());
        item.enabled = false;
        Debug.Log(item.questName + " COMPLETED");
    }

    public void MarkQuestTracked(Quest item) {
        if (item != null && trackedQuest != item) {
            UpdateTrackedQuest(item);
            trackedQuest = item;
            qte.Show(true);
        }
        else {
            trackedQuest = null;
            qte.Show(false);
        }
    }

    public void UpdateTrackedQuest(Quest item) {
        if (trackedQuest != null && trackedQuest == item) {
            // qte.SetTitleText(trackedQuest.questName);
            // qte.SetDescriptionText(trackedQuest.GetQuestHint());
            StartCoroutine(FadeTrackedQuestDescription(0.5f, 1f));
        }
        else {
            trackedQuest = item;
            StartCoroutine(FadeTrackedQuestTitle(0.5f, 1f));
            StartCoroutine(FadeTrackedQuestDescription(0.5f, 1f));
        }
        Debug.Log(trackedQuest);
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
        Quest trackedQuest = quests.FirstOrDefault(q => q.ID == questManagerData.trackedQuest);
        this.trackedQuest = null;

        DisableNonActiveQuests();
        
        foreach (Quest activeQuest in activeQuests) {
            foreach (QuestData questData in questManagerData.activeQuests) {
                if (activeQuest.ID == questData.ID) {
                    activeQuest.enabled = true;
                    activeQuest.SetQuestStage(questData.stage);
                }
            }
        }
        MarkQuestTracked(trackedQuest);
    }

    IEnumerator FadeTrackedQuestTitle(float fadeOut, float fadeIn) {
        float timer = 0;
        while (timer < fadeOut) {
            qte.SetTitleOpacity(Math.Clamp(1 - timer / fadeOut, 0, 1));
            timer += Time.deltaTime;
            yield return null;
        }
        qte.SetTitleText(trackedQuest.questName);
        timer = 0;
        while (timer < fadeIn) {
            qte.SetTitleOpacity(Math.Clamp(timer / fadeIn, 0, 1));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeTrackedQuestDescription(float fadeOut, float fadeIn) {
        float timer = 0;
        while (timer < fadeOut) {
            qte.SetDescriptionOpacity(Math.Clamp(1 - timer / fadeOut, 0, 1));
            timer += Time.deltaTime;
            yield return null;
        }
        qte.SetDescriptionText(trackedQuest.GetQuestHint());
        timer = 0;
        while (timer < fadeIn) {
            qte.SetDescriptionOpacity(Math.Clamp(timer / fadeIn, 0, 1));
            timer += Time.deltaTime;
            yield return null;
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
