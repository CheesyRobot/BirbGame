using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuEvents : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private UIDocument _document;
    private SettingsEvents Settings;
    private VisualElement _menusContainer;
    private TemplateContainer _pause;
    private TemplateContainer _settings;
    private Button _continueButton;
    private Button _journalButton;
    private Button _settingsButton;
    private Button _quitToMenuButton;
    private Button _returnButton;
    void Awake()
    {
        Settings = GetComponent<SettingsEvents>();
        _document = GetComponent<UIDocument>();
        _menusContainer = _document.rootVisualElement.Q<VisualElement>("PauseMenuScreens");
        _pause = _document.rootVisualElement.Q<TemplateContainer>("PauseMenu");
        _continueButton = _pause.Q<Button>("ContinueButton");
        _journalButton = _pause.Q<Button>("JournalButton");
        _settingsButton = _pause.Q<Button>("SettingsButton");
        _quitToMenuButton = _pause.Q<Button>("QuitToMenuButton");
        _settings = _document.rootVisualElement.Q<TemplateContainer>("SettingsMenu");
        _returnButton = _settings.Q<Button>("ReturnButton");

        _menusContainer.style.display = DisplayStyle.None;
        //Debug.Log("Awake");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Continue();
            }
            else
                Pause();
        }
    }
    private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(OnContinueClick);
        _journalButton.UnregisterCallback<ClickEvent>(OnJournalClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        _quitToMenuButton.UnregisterCallback<ClickEvent>(OnQuitMenuClick);
        _returnButton.UnregisterCallback<ClickEvent>(OnSettingsReturnClick);
    }

    private void OnEnable()
    {
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        _continueButton.RegisterCallback<ClickEvent>(OnContinueClick);
        _journalButton.RegisterCallback<ClickEvent>(OnJournalClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        _quitToMenuButton.RegisterCallback<ClickEvent>(OnQuitMenuClick);
        _returnButton.RegisterCallback<ClickEvent>(OnSettingsReturnClick);
    }

    private void Pause()
    {
        _menusContainer.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    private void Continue()
    {
        OnContinueClick(ClickEvent.GetPooled());
    }
    private void OnContinueClick(ClickEvent evt)
    {
        _menusContainer.style.display = DisplayStyle.None;
        Time.timeScale = 1f;
        GameIsPaused = false;
        ResetDisplays();
    }

    private void ResetDisplays()
    {
        OnSettingsReturnClick(ClickEvent.GetPooled());
    }

    private void OnJournalClick(ClickEvent evt)
    {
        Debug.Log("Journal");
        //_settings.style.display = DisplayStyle.Flex;
        //_pause.style.display = DisplayStyle.None;
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        Settings.enabled = true;
        _settings.style.display = DisplayStyle.Flex;
        _pause.style.display = DisplayStyle.None;
    }

    private void OnSettingsReturnClick(ClickEvent evt)
    {
        Settings.enabled = false;
        _pause.style.display = DisplayStyle.Flex;
        _settings.style.display = DisplayStyle.None;
    }

    private void OnQuitMenuClick(ClickEvent evt)
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
