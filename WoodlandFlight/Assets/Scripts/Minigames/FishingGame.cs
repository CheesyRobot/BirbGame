using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FishingGame : MonoBehaviour
{
    private bool active;
    private bool completed;
    private float score;
    private float timer;
    private float originalStaminaRate;
    private Collider boundary;
    public DisplayTime scoreDisplay;
    public FishingStart gameStart;
    public Transform startPosition;
    public int startingFishCount;
    public int scoreGoal;
    public float timeLimit;
    public int xpReward;
    public float spawnTime;
    public float despawnTime;
    public Player player;
    public AudioSource musicSource;
    public AudioClip musicClip;
    public FishSpawner spawner;
    void Start()
    {
        active = false;
        boundary = GetComponent<Collider>();
        if (boundary != null)
            boundary.enabled = false;
    }

    void Update()
    {
        if (active) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 0;
                EndMinigame();
            }
            scoreDisplay.UpdateTime(timer);
            scoreDisplay.AppendText("\nScore: " + score + " (Goal: " + scoreGoal + ")");
        }
    }

    public void StartMinigame() {
        if (active)
            return;

        gameStart.gameObject.SetActive(false);
        if (boundary != null)
            boundary.enabled = true;
        if (startPosition != null)
            player.transform.position = startPosition.position;
        score = 0;
        timer = timeLimit;
        scoreDisplay.EnableTimer(true);
        spawner.SetSpawnTime(spawnTime);
        spawner.SetDespawnTime(despawnTime);
        spawner.SetMinigame(this, true, startingFishCount);
        originalStaminaRate = player.SetStaminaConsumptionRate(0);

        active = true;
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void AddScore() {
        score += 1;
    }

    public void EndMinigame() {
        active = false;
        gameStart.gameObject.SetActive(true);
        if (boundary != null)
            boundary.enabled = false;
        spawner.ResetSpawnTime();
        spawner.ResetDespawnTime();
        spawner.SetMinigame(this, false, 0);
        scoreDisplay.EnableTimer(false);
        player.SetStaminaConsumptionRate(originalStaminaRate);
        player.transform.position = gameStart.transform.position;
        if (!completed && score >= scoreGoal) {
            player.AddExperience(xpReward);
            completed = true;
        }
        musicSource.Stop();
    }
}
