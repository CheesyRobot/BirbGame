using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JournalQuestButton
{
    public Quest quest;
    public TemplateContainer _newQuest;
    Button _button;
    VisualElement _line;
    public bool selected = false;
    bool completed;
    JournalEvents events;

    public JournalQuestButton(Quest quest, bool isCompleted, VisualTreeAsset _questButton, JournalEvents evts)
    {
        _newQuest = _questButton.Instantiate();
        this.quest = quest;
        _button = _newQuest.Q<Button>("QuestButton");
        _line = _newQuest.Q<VisualElement>("Line");
        _button.text = quest.questName;
        _button.RegisterCallback<ClickEvent>(OnClick);
        events = evts;
        completed = isCompleted;
        if(events.selectedButton != null &&
            events.selectedButton.quest.questName == quest.questName)
        {
            SetAsUnderlined();
            events.selectedButton = this;
            if (isCompleted) events.FollowQuestButtonEnabled(false);
        }
        if (isCompleted)
        {
            //_button.SetEnabled(false);
            _button.style.color = new Color(1f, 1f, 1f, 0.5f);
            _line.style.backgroundColor = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void OnClick(ClickEvent evt)
    {
        if (events.selectedButton != null)
        { events.selectedButton.OffClick(); }
        _line.style.display = DisplayStyle.Flex;
        selected = true;
        events.SetTitle(quest.questName);
        events.selectedButton = this;
        if (completed) events.FollowQuestButtonEnabled(false);
        else events.FollowQuestButtonEnabled(true);
    }

    public void OffClick()
    {
        _line.style.display = DisplayStyle.None;
        selected = false;
    }

    public void SetAsUnderlined()
    {
        _line.style.display = DisplayStyle.Flex;
        selected = true;
    }
}
