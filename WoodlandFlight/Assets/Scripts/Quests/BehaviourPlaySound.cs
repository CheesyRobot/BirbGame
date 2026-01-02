using UnityEngine;

public class BehaviourPlaySound : QuestStageBehaviour {
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioClip sound;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e) {
        SFXSource.clip = sound;
        SFXSource.Play();
    }
}
