using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalEvents : MonoBehaviour
{
    private UIDocument _document;
    private TemplateContainer _journal;
    [SerializeField] VisualTreeAsset _questButton;
    private Button _followQuest;
    private Label _titleLabel;
    private Label _descriptionLabel;
    private ScrollView _questScrollView;
    [SerializeField] QuestManager questManager;
    public JournalQuestButton selectedButton;
    //private List<JournalQuestButton> questButtons;
    private int questCount;
    void Awake()
    {
        questCount = 0;
        //questButtons = new List<JournalQuestButton>();
        _document = GetComponent<UIDocument>();
        _journal = _document.rootVisualElement.Q<TemplateContainer>("JournalMenu");

        // Follow Quest button
        _followQuest = _journal.Q<Button>("FollowQuestButton");

        // Getting labels
        _titleLabel = _journal.Q<Label>("QuestName");
        _descriptionLabel = _journal.Q<Label>("Description");

        // Quest list container:
        _questScrollView = _journal.Q<ScrollView>("QuestScrollView");
        //InstantiateQuests();
        InstantiateQuests();
    }

    private void OnEnable()
    {
        RefreshQuests();
    }

    void RefreshQuests()
    {
        //selectedButton = null;
        for(int i = 0; i < questCount; i++)
        {
            _questScrollView.RemoveAt(1);
        }
        /*foreach (JournalQuestButton entry in questButtons)
        {
            _questScrollView.RemoveAt(2);
        }*/
        //questButtons = new List<JournalQuestButton>();
        questCount = 0;
        InstantiateQuests();
    }

    void InstantiateQuests()
    {

        foreach(Quest quest in questManager.activeQuests)
        {
            JournalQuestButton button = new JournalQuestButton(quest, false, _questButton, this);
            questCount++;
            //questButtons.Add(button);
            _questScrollView.Add(button._newQuest);
        }
        foreach (Quest quest in questManager.completedQuests)
        {
            JournalQuestButton button = new JournalQuestButton(quest, true, _questButton, this);
            questCount++;
            //questButtons.Add(button);
            _questScrollView.Add(button._newQuest);
        }
    }

    public void SetTitle(string title)
    {
        _titleLabel.text = title;
    }

    public void FollowQuestButtonEnabled(bool enabled)
    {
        if(enabled)
            _followQuest.style.display = DisplayStyle.Flex;
        else
            _followQuest.style.display = DisplayStyle.None;
    }

    public void SetDescription(string description)
    {
        _descriptionLabel.text = description;
    }

}
