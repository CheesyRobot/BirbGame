using UnityEngine;
using System.Collections;

public class QuestStageBehaviour : MonoBehaviour
{
    protected Quest quest;
    [SerializeField] protected int stage;
    [SerializeField] protected float delaySeconds;
    void Awake()
    {
        quest = GetComponent<Quest>();
        quest.OnStageChanged += Receive_OnStageChanged;
    }

    private void Receive_OnStageChanged(object sender, Quest.OnStageChangedEventArgs e) {
        if (e.stage == stage)
            StartCoroutine(DoActionAfterDelay(sender, e, delaySeconds));
    }

    public virtual void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        Debug.Log(sender + " " + e.stage);
    }

    IEnumerator DoActionAfterDelay(object sender, Quest.OnStageChangedEventArgs e, float duration) {
        yield return new WaitForSeconds(duration);
        DoAction(sender, e);
    }
}
