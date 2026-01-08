using System;
using System.Collections;
using UnityEngine;

public class FetchFishObjective : MonoBehaviour, IQuestObjectiveType
{
    public float deleteObjectAfterSeconds = -1;
    //public Collider dropZone;
    private bool completed;
    private float timer;
    
    void Awake() {
        completed = false;
        GetComponent<Collider>().enabled = false;
        timer = deleteObjectAfterSeconds;
    }

    void Update() {
        if (GetComponent<Collider>().enabled == true && timer <= deleteObjectAfterSeconds)
            timer += Time.deltaTime;
    }
    
    public bool CheckCondition()
    {
        return completed;
    }

    void OnTriggerStay(Collider col) {
        if(col.GetComponent<Fish>() != null && timer >= deleteObjectAfterSeconds)
        {
            if (col.GetComponent<Grabbable>() != null && col.GetComponent<Grabbable>().IsGrabbed())
                return;
            if (deleteObjectAfterSeconds >= 0)
                Destroy(col.gameObject, deleteObjectAfterSeconds);
            timer = 0;
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
