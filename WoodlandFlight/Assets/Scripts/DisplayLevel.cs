using TMPro;
using UnityEngine;

public class DisplayLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI experience;
    private string levelText = "Level ";
    private string experienceText = " XP";

    public void UpdateLevel(int level, int experience, int maxLevel) {
        this.level.SetText(levelText + level);
        if (level == maxLevel)
            this.experience.SetText("");
        else
            this.experience.SetText(experience + experienceText);
    }
}
