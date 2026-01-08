using UnityEngine;

public class BehaviourAnimatorTrigger : QuestStageBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e)
    {
        animator.SetTrigger(triggerName);
    }
}