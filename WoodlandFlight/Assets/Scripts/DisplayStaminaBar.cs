using UnityEngine;

public class DisplayStaminaBar : MonoBehaviour
{
    public float width, height, padding;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform stamina;
    [SerializeField] private Player player;
    void Start()
    {
        
    }

    void Update()
    {
        float newBarWidth = width * (1 + player.staminaLimit / 1000);
        float newStaminaWidth = (player.currentStamina / player.staminaLimit) * newBarWidth;
        stamina.sizeDelta = new Vector2(newStaminaWidth - padding, height - padding);
        bar.sizeDelta = new Vector2(newBarWidth, height);
    }
}
