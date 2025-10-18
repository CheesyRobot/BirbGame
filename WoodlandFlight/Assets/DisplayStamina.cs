using TMPro;
using UnityEngine;

public class DisplayStamina : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("Stamina: " + player.currentStamina);
    }
}
