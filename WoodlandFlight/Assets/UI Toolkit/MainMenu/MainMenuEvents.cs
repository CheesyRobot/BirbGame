using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private Button _settingsButton;
    private Button _quitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _document = GetComponent<UIDocument>();

        _playButton = _document.rootVisualElement.Q("PlayButton") as Button;
        _settingsButton = _document.rootVisualElement.Q("SettingsButton") as Button;
        _quitButton = _document.rootVisualElement.Q("QuitButton") as Button;

        _playButton.RegisterCallback<ClickEvent>(OnPlayClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        _quitButton.RegisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        _quitButton.UnregisterCallback<ClickEvent>(OnQuitClick);
    }

    private void OnPlayClick(ClickEvent evt)
    {
        //Debug.Log("Play");
        SceneManager.LoadScene(1);
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        Debug.Log("Settings");
    }

    private void OnQuitClick(ClickEvent evt)
    {
        //Debug.Log("Quit");
        Application.Quit();
    }
}
