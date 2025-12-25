using UnityEngine;

public class QuestStageBehaviour : MonoBehaviour
{
    [SerializeField] protected Quest quest;
    [SerializeField] protected int stage;
    void Start()
    {
        quest.OnStageChanged += Receive_OnStageChanged;
    }

    private void Receive_OnStageChanged(object sender, Quest.OnStageChangedEventArgs e) {
        if (e.stage == stage)
            DoAction(sender, e);
    }

    public virtual void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        Debug.Log(sender + " " + e.stage);
    }
}
