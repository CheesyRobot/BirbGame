using UnityEngine;

public class BehaviourStartQuest : QuestStageBehaviour {
    [SerializeField] private Quest newQuest;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        quest.manager.MarkQuestActive(newQuest);
    }
}
