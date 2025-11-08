using UnityEngine;
using UnityEngine.UIElements;

public class FishMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private Collider movementVolume;
    private bool enableMovement;
    private bool despawning;
    private float despawnTime;
    private float changeDirectionTime;
    private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        targetPosition = transform.position;
        enableMovement = false;
        despawning = false;
        despawnTime = 10f;
        changeDirectionTime = 2f;
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!despawning && despawnTime <= 0) {
            despawning = true;
            Despawn();
        }
        despawnTime -= Time.deltaTime;
        changeDirectionTime -= Time.deltaTime;
        if (Move() || changeDirectionTime < 0) {
            SetTargetPosition(PointInBox());
        }
    }

    public void SetTargetPosition(Vector3 position) {
        targetPosition = position;
        changeDirectionTime = Random.Range(3, 10);
    }

    public void SetMovementVolume(Collider volume) {
        movementVolume = volume;
        enableMovement = true;
    }

    public void SetDespawnTime(float time) {
        despawnTime = time;
    }

    private bool Move() {
        if (!enableMovement)
            return false;
        if (Vector3.Distance(targetPosition, transform.position) <= 0.1)
            return true;
        Vector3 direction = Vector3.Normalize(targetPosition - transform.position);
        direction.x = Mathf.Lerp(transform.forward.x, direction.x, Time.deltaTime * 0.5f);
        direction.z = Mathf.Lerp(transform.forward.z, direction.z, Time.deltaTime * 0.5f);
        transform.position += direction * speed * Time.deltaTime;
        transform.forward = direction;
        return false;
    }

    public void Despawn() {
        changeDirectionTime = 10f;
        Destroy(gameObject, 1f);
        targetPosition = PointInBox();
        targetPosition.y = transform.position.y - 2;
    }

    private Vector3 PointInBox() {
        if (movementVolume == null)
            return transform.position;
        Vector3 pos = movementVolume.transform.position;
        Vector3 b = movementVolume.bounds.size / 2;
        float pointX = Random.Range(pos.x - b.x, pos.x + b.x);
        float pointY = Random.Range(pos.y + b.y, pos.y + b.y);
        float pointZ = Random.Range(pos.z - b.z, pos.z + b.z);
        return new Vector3(pointX, pointY, pointZ);
    }
}
