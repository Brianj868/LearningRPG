using Ink.Runtime;
using System;
using System.Text;
using TMPro;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] TMP_Text _storyText;
    [SerializeField] Button[] _choiceButtons;
    [SerializeField] Animator _animator;

    Story _story;

    [ContextMenu("Start Dialog")]
    public void StartDialog(TextAsset dialog)
    {
        _story = new Story(dialog.text);
        RefreshView();
    }

    void RefreshView()
    {
        StringBuilder storyTextBuilder = new StringBuilder();
        while (_story.canContinue)
        {
            storyTextBuilder.AppendLine(_story.Continue());
            HandleTags();
        }

        _storyText.SetText(storyTextBuilder);

        for (int i = 0; i < _choiceButtons.Length; i++)
        {
            var button = _choiceButtons[i];
            button.gameObject.SetActive(i < _story.currentChoices.Count);
            button.onClick.RemoveAllListeners();
            if (i < _story.currentChoices.Count)
            {
                var choice = _story.currentChoices[i];
                button.GetComponentInChildren<TMP_Text>().SetText(choice.text);
                button.onClick.AddListener(() =>
                {
                    _story.ChooseChoiceIndex(choice.index);
                    RefreshView();
                });
            }
        }
    }

    void HandleTags()
    {
        foreach (var tag in _story.currentTags)
        {
            Debug.Log(tag);
            if (tag.StartsWith("E."))
            {
                string eventName = tag.Remove(0, 2);
                GameEvent.RaiseEvent(eventName);
            }
        }
    }
}
