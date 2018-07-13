using UnityEngine;

public class UIHandler : MonoBehaviour {
    [SerializeField] private NotificationHandler _notificationHandler;
    public NotificationHandler Notification { get { return _notificationHandler; } }

    [SerializeField] private CommandPromptHandler _commandPromptHandler;
    public CommandPromptHandler CommandPrompt { get { return _commandPromptHandler; } }

    [SerializeField] private MenuHandler _menuHandler;
    public MenuHandler Menu { get { return _menuHandler; } }

    [SerializeField] private EndScreenHandler _endScreenHandler;
    public EndScreenHandler EndScreen { get { return _endScreenHandler; } }

    [SerializeField] private TutorialHandler _tutorialHandler;
    public TutorialHandler Tutorial { get { return _tutorialHandler; } }
}
