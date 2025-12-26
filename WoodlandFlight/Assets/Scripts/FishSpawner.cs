using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fish;
    public Collider spawnBox;
    public float spawnTime;
    public float despawnTime;
    private float spawnTimeMod;
    private float despawnTimeMod;
    private FishingGame game;
    private bool gameActive;
    void Start()
    {
        spawnTimeMod = spawnTime;
        despawnTimeMod = despawnTime;
        InvokeRepeating("Spawn", 1.0f, spawnTimeMod);
    }


    public void Spawn() {
        Vector3 pos = spawnBox.transform.position;
        Vector3 b = spawnBox.bounds.size / 2;
        float pointX = Random.Range(pos.x - b.x, pos.x + b.x);
        float pointY = Random.Range(pos.y - b.y, pos.y - b.y);
        float pointZ = Random.Range(pos.z - b.z, pos.z + b.z);
        Vector3 point = new Vector3(pointX, pointY, pointZ);

        GameObject fishIntance = Instantiate(fish, point, Quaternion.identity);
        FishMovement movement = fishIntance.GetComponent<FishMovement>();
        if (movement != null) {
            movement.SetMovementVolume(spawnBox);
            movement.SetDespawnTime(despawnTimeMod);
            point.y = pos.y + b.y;
            movement.SetTargetPosition(point);
        }
        if (gameActive) {
            Fish fish = fishIntance.GetComponent<Fish>();
            fish.SetMinigame(game, gameActive);
        }
    }

    public void SetSpawnTime(float seconds) {
        spawnTimeMod = seconds;
    }

    public void SetDespawnTime(float seconds) {
        despawnTimeMod = seconds;
    }

    public void ResetSpawnTime() {
        spawnTimeMod = spawnTime;
    }

    public void ResetDespawnTime() {
        despawnTimeMod = despawnTime;
    }

    public void SetMinigame(FishingGame game, bool active, int startingAmount) {
        this.game = game;
        gameActive = active;
        FishMovement[] fishes = Resources.FindObjectsOfTypeAll<FishMovement>();
        foreach (FishMovement fish in fishes) {
            // Check if object is not a prefab
            if (fish.gameObject.scene.name != null)
                Destroy(fish.gameObject);
        }
            
        for (int i = 0; i < startingAmount; i++)
            Spawn();
    }
}
