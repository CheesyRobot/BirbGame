using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsEvents : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    private UIDocument _document;
    private ToggleButtonGroup _buttons;
    private TemplateContainer _settings;
    private VisualElement _game;
    private VisualElement _video;
    private VisualElement _audio;
    private VisualElement _controls;
    private Slider _master;
    private Slider _music;
    private Slider _SFX;
    private Slider _ambient;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
        _settings = _document.rootVisualElement.Q<TemplateContainer>("SettingsMenu");

        // Getting panels
        _game = _settings.Q<VisualElement>("Game");
        _video = _settings.Q<VisualElement>("Video");
        _audio = _settings.Q<VisualElement>("Audio");
        _controls = _settings.Q<VisualElement>("Controls");

        // Audio sliders
        _master = _settings.Q<Slider>("Master");
        _music = _settings.Q<Slider>("Music");
        _SFX = _settings.Q<Slider>("SFX");
        _ambient = _settings.Q<Slider>("Ambient");

        // Settings toggle buttons: Game, Video, Audio, Controls
        _buttons = _settings.Q<ToggleButtonGroup>("Buttons");

        //Sets the default settings button to 0 (the Game button in UI)
        ulong mask = 0UL;
        mask |= (1UL << 0);
        _buttons.SetValueWithoutNotify(new ToggleButtonGroupState(mask, 4));
        // Making panels other than the Game settings panel invisible
        _video.style.display = DisplayStyle.None;
        _audio.style.display = DisplayStyle.None;
        _controls.style.display = DisplayStyle.None;

        // Connecting functions to elements
        _buttons.RegisterValueChangedCallback(OnToggles);

        _master.RegisterValueChangedCallback(MasterAudioChanged);
        _music.RegisterValueChangedCallback(MusicAudioChanged);
        _SFX.RegisterValueChangedCallback(SFXAudioChanged);
        _ambient.RegisterValueChangedCallback(AmbientAudioChanged);
        SetDefaultMusicSliderValues();
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

    public void SetDefaultMusicSliderValues()
    {
        float audioValue;
        audioMixer.GetFloat("MasterVolume", out audioValue);
        _master.value = MixerToSlider(audioValue);
        audioMixer.GetFloat("MusicVolume", out audioValue);
        _music.value = MixerToSlider(audioValue);
        audioMixer.GetFloat("SFXVolume", out audioValue);
        _SFX.value = MixerToSlider(audioValue);
        audioMixer.GetFloat("AmbientVolume", out audioValue);
        _ambient.value = MixerToSlider(audioValue);
    }

    private void MasterAudioChanged(ChangeEvent<float> evt)
    {

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(_master.value)*20f);
    }

    private void MusicAudioChanged(ChangeEvent<float> evt)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(_music.value)*20f);
    }

    private void SFXAudioChanged(ChangeEvent<float> evt)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(_SFX.value) * 20f);
    }

    private void AmbientAudioChanged(ChangeEvent<float> evt)
    {
        audioMixer.SetFloat("AmbientVolume", Mathf.Log10(_ambient.value) * 20f);
    }

    private float MixerToSlider(float mixerValue)
    {
        //Mathf.InverseLerp(-80, 0, mixerValue);
        return Mathf.Pow(10, mixerValue / 20); 
    }

    private void OnDisable()
    {
        _buttons.UnregisterValueChangedCallback(OnToggles);
    }

    private void OnEnable()
    {
        _buttons.RegisterValueChangedCallback(OnToggles);
    }

}
