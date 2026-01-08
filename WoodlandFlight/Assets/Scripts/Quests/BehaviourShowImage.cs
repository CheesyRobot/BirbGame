using UnityEngine;
using UnityEngine.UI;

public class BehaviourShowImage : QuestStageBehaviour
{
    [SerializeField] private float holdTime;
    [SerializeField] private Texture2D image;
    public override void DoAction(object sender, Quest.OnStageChangedEventArgs e)
    {
        ScreenImage.Instance.ShowForAmountOfTime(holdTime, image);
    }
}