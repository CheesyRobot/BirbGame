using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsEvents : MonoBehaviour
{
    private UIDocument _document;
    private ToggleButtonGroup _buttons;
    private TemplateContainer _settings;
    private VisualElement _game;
    private VisualElement _video;
    private VisualElement _audio;
    private VisualElement _controls;
    void Start()
    {
        _document = GetComponent<UIDocument>();

        _settings = _document.rootVisualElement.Q<TemplateContainer>("SettingsMenu");
        _buttons = _settings.Q<ToggleButtonGroup>("Buttons");

        // Getting panels
        _game = _settings.Q<VisualElement>("Game");
        _video = _settings.Q<VisualElement>("Video");
        _audio = _settings.Q<VisualElement>("Audio");
        _controls = _settings.Q<VisualElement>("Controls");

        //Sets default button to 0 (Game button in UI)
        ulong mask = 0UL;
        mask |= (1UL << 0);
        _buttons.SetValueWithoutNotify(new ToggleButtonGroupState(mask, 4));

        _buttons.RegisterValueChangedCallback(OnToggles);
    }

    private void OnToggles(ChangeEvent<ToggleButtonGroupState> evt)
    {
        var value = evt.previousValue;
        var options = value.GetActiveOptions(stackalloc int[value.length]);
        switch (options[0])
            {
                case 0:
                    _game.style.display = DisplayStyle.None;
                    break;
                case 1:
                    _video.style.display = DisplayStyle.None;
                    break;
                case 2:
                    _audio.style.display = DisplayStyle.None;
                    break;
                default:
                    _controls.style.display = DisplayStyle.None;
                    break;
            }
        value = evt.newValue;
        options = value.GetActiveOptions(stackalloc int[value.length]);
        switch (options[0])
        {
            case 0:
                _game.style.display = DisplayStyle.Flex;
                break;
            case 1:
                _video.style.display = DisplayStyle.Flex;
                break;
            case 2:
                _audio.style.display = DisplayStyle.Flex;
                break;
            default:
                _controls.style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void OnDisable()
    {
        _buttons.UnregisterValueChangedCallback(OnToggles);
    }

}
