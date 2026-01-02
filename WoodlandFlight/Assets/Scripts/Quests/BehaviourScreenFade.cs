using UnityEngine;

public class BehaviourScreenFade : QuestStageBehaviour {
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float holdTime;
    [SerializeField] private float fadeInTime;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        ScreenFade.Instance.FadeOutHoldFadeIn(fadeOutTime, holdTime, fadeInTime);
    }
}
