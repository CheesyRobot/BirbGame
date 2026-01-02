using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private float detectionDistance;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float timeUntilDetection;
    [SerializeField] private float detectionDecayPerSecond;
    [SerializeField] private Transform rayOriginPoint;
    [SerializeField] private Transform target;
    [SerializeField] private RawImage indicator;
    private float timer;
    public event EventHandler<OnPlayerDetectedEventArgs> OnPlayerDetected;
    public class OnPlayerDetectedEventArgs : EventArgs {
        public Transform player;
    }
    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        if (timer > timeUntilDetection) {
            // Debug.Log("Detected");
            OnPlayerDetected?.Invoke(this, new OnPlayerDetectedEventArgs{player = this.target});
            timer = 0f;
        }
        else if (IsInSight()) {
            timer += Time.deltaTime;
        }
        else if (timer > 0f) {
            timer -= Time.deltaTime * detectionDecayPerSecond;
        }
        if (indicator != null)
            indicator.color = new Color(1,1,1, GetDetectionProgress());
    }

    public bool IsInSight() {
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

    public float GetDetectionProgress() {
        if (timer <= 0)
            return 0;
        return 1 - (timeUntilDetection - timer) / timeUntilDetection;
    }
}
