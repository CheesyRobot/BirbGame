using UnityEngine;
using TMPro;

public class WorldBorder : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private int warningDistance;
    [SerializeField] private int maximumDistance;
    void Update()
    {
        float distance = Vector3.Distance(centerPoint.position, player.transform.position);
        if (distance > maximumDistance)
            player.TeleportToSpawn();
        else if (distance > warningDistance)
            warningText.alpha = 1;
        else
            warningText.alpha = 0;
    }
}
