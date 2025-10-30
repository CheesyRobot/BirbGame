using System;
using TMPro;
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    public TextMeshProUGUI textField;
    void Start()
    {
        textField.gameObject.SetActive(false);
    }

    public void EnableTimer(bool value) {
        textField.gameObject.SetActive(value);
    }

    public void UpdateTime(float timeSeconds) {
        TimeSpan ts = System.TimeSpan.FromSeconds(timeSeconds);
        // string text = string.Format("{0}:{1}", ts.Minutes.ToString(), ts.Seconds.ToString());
        // textField.SetText(text);
        string text = ts.ToString(@"mm\:ss");
        textField.SetText(text);
    }
}
