using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject fish;
    public Collider spawnBox;
    public float spawnTime;
    public float despawnTime;
    void Start()
    {
        InvokeRepeating("Spawn", 1.0f, spawnTime);
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
            movement.SetDespawnTime(despawnTime);
            point.y = pos.y + b.y;
            movement.SetTargetPosition(point);
        }

    }
}
