using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float detectionDistance;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float timeUntilDetection;
    [SerializeField] private float detectionDecayPerSecond;
    [SerializeField] private Transform rayOriginPoint;
    [SerializeField] private Transform target;
    private float timer;
    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (timer > timeUntilDetection) {
            Debug.Log("Detected");
            timer = 0f;
        }
        else if (IsInSight()) {
            timer += Time.deltaTime;
        }
        else if (timer > 0f) {
            timer -= Time.deltaTime * detectionDecayPerSecond;
        }
    }

    private bool IsInSight() {
        Vector3 targetDistance = target.position - rayOriginPoint.position;
        if (targetDistance.magnitude > detectionDistance || Vector3.Angle(rayOriginPoint.forward, targetDistance) > detectionAngle)
            return false;

        RaycastHit hit;
        Ray rayDirection = new Ray(rayOriginPoint.position, Vector3.Normalize(targetDistance));
        Debug.DrawRay(rayOriginPoint.position, targetDistance, Color.red);
        
        bool hasHit = Physics.Raycast(rayDirection, out hit, detectionDistance);
        if (hasHit && hit.transform == target)
            return true;
        else
            return false;
    }
}
