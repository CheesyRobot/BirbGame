using System;
using UnityEngine;

public class BonkObjective : MonoBehaviour, IQuestObjectiveType
{
    [SerializeField] private Bonk enemy;
    private bool completed;
    
    void Awake() {
        completed = false;
    }
    
    public bool CheckCondition()
    {
        return completed;
    }

    public void StartObjective() {
        completed = false;
        enemy.OnBonked += Receive_OnBonked;
    }

    public void CompleteObjective() {
        completed = true;
    }

    private void Receive_OnBonked(object sender, EventArgs e) {
        completed = true;
        enemy.OnBonked -= Receive_OnBonked;
    }
}
