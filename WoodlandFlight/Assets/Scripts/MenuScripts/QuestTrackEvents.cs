using UnityEngine;
using UnityEngine.UIElements;

public class QuestTrackEvents : MonoBehaviour
{
    private UIDocument _document;
    private TemplateContainer _questContainer;   // Contains all elements
    private Label _titleLabel;
    private Label _textLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Gets the document that has the UI elements
        _document = GetComponent<UIDocument>();

        // Getting container
        _questContainer = _document.rootVisualElement.Q<TemplateContainer>("QuestContainer");

        // Getting labels
        _titleLabel = _questContainer.Q<Label>("QuestTitle");
        _textLabel = _questContainer.Q<Label>("QuestText");
    }

    /// <summary>
    /// Sets title text
    /// </summary>
    /// <param name="text"></param>
    public void SetTitleText(string text)
    {
        _titleLabel.text = text;
    }

    /// <summary>
    /// Sets description text
    /// </summary>
    /// <param name="text"></param>
    public void SetDescriptionText(string text)
    {
        _textLabel.text = text;
    }

    /// <summary>
    /// Sets title text opacity
    /// </summary>
    /// <param name="value">1 - visible, 0 - invisible</param>
    public void SetTitleOpacity(float value)
    {
        _titleLabel.style.color = new Color(1f, 1f, 1f, value);
    }

    /// <summary>
    /// Sets description text opacity
    /// </summary>
    /// <param name="value">1 - visible, 0 - invisible</param>
    public void SetDescriptionOpacity(float value)
    {
        _textLabel.style.color = new Color(1f, 1f, 1f, value);
    }

    /// <summary>
    /// Hides or shows UI
    /// </summary>
    /// <param name="isEnabled"></param>
    public void Show(bool isEnabled)
    {
        if (isEnabled)
        {
            _questContainer.style.display = DisplayStyle.Flex;
        }
        else
            _questContainer.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Returns if UI is enabled (showing) or not
    /// </summary>
    /// <returns></returns>
    public bool isEnabled()
    {
        if (_questContainer.resolvedStyle.display == DisplayStyle.Flex)
        {
            return true;
        }
        else return false;
    }
}
