using UnityEngine;

public class BehaviourHideItem : QuestStageBehaviour {
    [SerializeField] private GameObject item;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        item.SetActive(false);
    }
}
