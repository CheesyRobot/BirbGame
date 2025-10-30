using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Race : MonoBehaviour
{
    private bool active;
    private float timer;
    private int currentHoopIndex;
    // public List<Hoop> hoops;
    public Hoop[] hoops;
    public DisplayTime timeDisplay;
    public RaceStart raceStart;
    public int timeGoal;
    void Start()
    {
        active = false;
        foreach (Hoop hoop in hoops) {
            hoop.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (active) {
            timer += Time.deltaTime;
            timeDisplay.UpdateTime(timer);

            if (hoops[currentHoopIndex].completed == true) {
                hoops[currentHoopIndex].SetTarget(false);
                hoops[currentHoopIndex].gameObject.SetActive(false);
                
                currentHoopIndex++;
                if (currentHoopIndex == hoops.Length)
                    EndMinigame();
                else
                {
                    // always show one hoop in advance
                    if (currentHoopIndex < hoops.Length - 1) {
                        hoops[currentHoopIndex + 1].gameObject.SetActive(true);
                    }
                    
                    hoops[currentHoopIndex].SetTarget(true);
                }
            }
        }
    }

    public void StartMinigame() {
        if (active)
            return;

        raceStart.gameObject.SetActive(false);
        timer = 0f;

        if (hoops.Length >= 2) {
            currentHoopIndex = 0;
            hoops[0].gameObject.SetActive(true);
            hoops[1].gameObject.SetActive(true);
            hoops[0].SetTarget(true);
            hoops[0].SetVisible(true);
            hoops[1].SetVisible(true);
            timeDisplay.EnableTimer(true);
        }
        else
            Debug.Log("Race must have at least 2 hoops");
        active = true;
    }

    public void EndMinigame() {
        active = false;
        raceStart.gameObject.SetActive(true);
        foreach (Hoop hoop in hoops) {
            hoop.gameObject.SetActive(false);
        }
        timeDisplay.EnableTimer(false);
    }
}
