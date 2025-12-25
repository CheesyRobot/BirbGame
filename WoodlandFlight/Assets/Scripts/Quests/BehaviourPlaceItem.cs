using UnityEngine;

public class BehaviourPlaceItem : QuestStageBehaviour {
    [SerializeField] private GameObject item;
    [SerializeField] private Transform position;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        item.SetActive(true);
        if (position != null) {
            item.transform.position = position.position;
            item.transform.rotation = position.rotation;
        }
    }
}
