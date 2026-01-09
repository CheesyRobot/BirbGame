using System;
using UnityEngine;

public class VisitObjective : MonoBehaviour, IQuestObjectiveType
{
    //public Collider dropZone;
    private bool completed;
    
    void Awake() {
        completed = false;
        //GetComponent<Collider>().enabled = false;
    }
    
    public bool CheckCondition()
    {
        return completed;
    }

    void OnTriggerEnter(Collider col) {
        if (col.GetComponent<Player>() != null)
        {

            GetComponent<Collider>().enabled = false;
            // questItem.GetComponent<Grabbable>().enabled = false;
            completed = true;
        }
    }
    public void StartObjective() {
        completed = false;
        GetComponent<Collider>().enabled = true;
    }

    public void CompleteObjective() {
        GetComponent<Collider>().enabled = false;
        completed = true;
    }
}
