using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _playButton;
    private Button _settingsButton;
    private Button _quitButton;
    private TemplateContainer _main;
    private Button _returnButton;
    private Button _gameButton;
    private Button _videoButton;
    private Button _audioButton;
    private Button _controlsButton;
    private TemplateContainer _settings;
    void Start()
    {
        _document = GetComponent<UIDocument>();

        InitiateMainMenu();
        InitiateSettingsMenu();
        
    }

    private void InitiateMainMenu()
    {
        _main = _document.rootVisualElement.Q<TemplateContainer>("MainMenu");
        _playButton = _main.Q<Button>("PlayButton");
        _settingsButton = _main.Q<Button>("SettingsButton");
        _quitButton = _main.Q<Button>("QuitButton");

        _playButton.RegisterCallback<ClickEvent>(OnPlayClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        _quitButton.RegisterCallback<ClickEvent>(OnQuitClick);
    }

    private void InitiateSettingsMenu()
    {
        _settings = _document.rootVisualElement.Q<TemplateContainer>("SettingsMenu");
        _returnButton = _settings.Q<Button>("ReturnButton");

        _returnButton.RegisterCallback<ClickEvent>(OnSettingsReturnClick);
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(OnPlayClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        _quitButton.UnregisterCallback<ClickEvent>(OnQuitClick);
        _returnButton.UnregisterCallback<ClickEvent>(OnSettingsReturnClick);
    }

    private void OnPlayClick(ClickEvent evt)
    {
        SceneManager.LoadScene(1);
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        _settings.style.display = DisplayStyle.Flex;
        _main.style.display = DisplayStyle.None;
    }

    private void OnSettingsReturnClick(ClickEvent evt)
    {
        _main.style.display = DisplayStyle.Flex;
        _settings.style.display = DisplayStyle.None;
    }

    private void OnQuitClick(ClickEvent evt)
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
