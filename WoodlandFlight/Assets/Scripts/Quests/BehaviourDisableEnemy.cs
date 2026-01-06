using UnityEngine;

public class BehaviourDisableEnemy : QuestStageBehaviour {
    [SerializeField] private EnemyAI enemy;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        enemy.DisableEnemy();
    }
}
