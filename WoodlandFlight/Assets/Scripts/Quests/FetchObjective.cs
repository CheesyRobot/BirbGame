using System;
using UnityEngine;

public class FetchObjective : MonoBehaviour, IQuestObjectiveType
{
    public GameObject questItem;
    //public Collider dropZone;
    private bool completed;
    
    void Start() {
        completed = false;
        GetComponent<Collider>().enabled = false;
    }
    
    public bool CheckCondition()
    {
        return completed;
    }

    void OnTriggerEnter(Collider col) {
        if(col.name == questItem.name)
        {
            Debug.Log("Quest item detected");
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
