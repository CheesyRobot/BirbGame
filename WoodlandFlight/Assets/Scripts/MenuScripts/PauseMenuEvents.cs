using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuEvents : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private UIDocument _document;
    private SettingsEvents Settings;
    private JournalEvents Journal;
    private VisualElement _menusContainer;
    private TemplateContainer _pause;
    private TemplateContainer _settings;
    private TemplateContainer _journal;
    private Button _continueButton;
    private Button _journalButton;
    private Button _settingsButton;
    private Button _quitToMenuButton;
    private Button _settingsReturnButton;
    private Button _journalReturnButton;
    void Awake()
    {
        Settings = GetComponent<SettingsEvents>();
        Journal = GetComponent<JournalEvents>();
        _document = GetComponent<UIDocument>();
        _menusContainer = _document.rootVisualElement.Q<VisualElement>("PauseMenuScreens");
        _pause = _document.rootVisualElement.Q<TemplateContainer>("PauseMenu");
        _continueButton = _pause.Q<Button>("ContinueButton");
        _journalButton = _pause.Q<Button>("JournalButton");
        _settingsButton = _pause.Q<Button>("SettingsButton");
        _quitToMenuButton = _pause.Q<Button>("QuitToMenuButton");
        _settings = _document.rootVisualElement.Q<TemplateContainer>("SettingsMenu");
        _settingsReturnButton = _settings.Q<Button>("ReturnButton");
        _journal = _document.rootVisualElement.Q<TemplateContainer>("JournalMenu");
        _journalReturnButton = _journal.Q<Button>("ReturnButton");
        RegisterCallbacks(); // Remove this line when not testing mid play mode anymore
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

    // Remove comment symbols on functions when not testing mid play mode anymore
    /*private void OnDisable()
    {
        _continueButton.UnregisterCallback<ClickEvent>(OnContinueClick);
        _journalButton.UnregisterCallback<ClickEvent>(OnJournalClick);
        _settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        _quitToMenuButton.UnregisterCallback<ClickEvent>(OnQuitMenuClick);
        _settingsReturnButton.UnregisterCallback<ClickEvent>(OnReturnClick);
        _journalReturnButton.UnregisterCallback<ClickEvent>(OnReturnClick);
    }

    private void OnEnable()
    {
        RegisterCallbacks();
    }*/

    private void RegisterCallbacks()
    {
        _continueButton.RegisterCallback<ClickEvent>(OnContinueClick);
        _journalButton.RegisterCallback<ClickEvent>(OnJournalClick);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        _quitToMenuButton.RegisterCallback<ClickEvent>(OnQuitMenuClick);
        _settingsReturnButton.RegisterCallback<ClickEvent>(OnReturnClick);
        _journalReturnButton.RegisterCallback<ClickEvent>(OnReturnClick);
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
        OnReturnClick(ClickEvent.GetPooled());
    }

    private void OnJournalClick(ClickEvent evt)
    {
        Debug.Log("Journal");
        Journal.enabled = true;
        _journal.style.display = DisplayStyle.Flex;
        _pause.style.display = DisplayStyle.None;
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        Settings.enabled = true;
        _settings.style.display = DisplayStyle.Flex;
        _pause.style.display = DisplayStyle.None;
    }

    private void OnReturnClick(ClickEvent evt)
    {
        Settings.enabled = false;
        Journal.enabled = false;
        _pause.style.display = DisplayStyle.Flex;
        _settings.style.display = DisplayStyle.None;
        _journal.style.display = DisplayStyle.None;
    }

    private void OnQuitMenuClick(ClickEvent evt)
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
