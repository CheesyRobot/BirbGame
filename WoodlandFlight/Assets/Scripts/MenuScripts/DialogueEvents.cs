using UnityEngine;
using UnityEngine.UIElements;

public class DialogueEvents : MonoBehaviour
{
    private UIDocument _document;
    private VisualElement _dialogueContainer;   // Contains all elements. On default invisible
    private TemplateContainer _optionsButtons;  // Contains option buttons. On default invisible
    private TemplateContainer _dialogueBox;     // Contains dialogue box and its labels
    private Button _option1;
    private Button _option2;
    private Button _option3;
    private Label _nameLabel;
    private Label _dialogueLabel;
    private Label _pressEnterLabel;

    void Awake()
    {
        // Gets the document that has the UI elements
        _document = GetComponent<UIDocument>();
        
        // Getting containers
        _dialogueContainer = _document.rootVisualElement.Q<VisualElement>("DialogueContainer");
        _optionsButtons = _document.rootVisualElement.Q<TemplateContainer>("OptionsButtons");
        _dialogueBox = _document.rootVisualElement.Q<TemplateContainer>("DialogueBox");

        // Getting buttons
        _option1 = _optionsButtons.Q<Button>("OptionButton1");
        _option2 = _optionsButtons.Q<Button>("OptionButton2");
        _option3 = _optionsButtons.Q<Button>("OptionButton3");

        // Getting labels
        _nameLabel = _dialogueBox.Q<Label>("Name");
        _dialogueLabel = _dialogueBox.Q<Label>("Dialogue");
        _pressEnterLabel = _dialogueBox.Q<Label>("EnterContinue");

        /// Mygtukams priskirti funkcijas naudojamas RegisterCallback, pvz:
        /// _continueButton.RegisterCallback<ClickEvent>(OnContinueClick);
        /// Galima atskirti (atjungti) funkcijas su UnregisterCallback
        /// _continueButton.UnregisterCallback<ClickEvent>(OnContinueClick);

        /// Toliau, veikia visiems UI elementams ( Button, Label, etc.), išskyrus pačiam UIDocument:
        /// Elementams paslėpti, kad būtų nematomi:
        //_dialogueContainer.style.display = DisplayStyle.None;
        /// Elementams parodyti:
        //_dialogueContainer.style.display = DisplayStyle.Flex;
    }
}
