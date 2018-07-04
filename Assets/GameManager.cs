using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum RoomBorderTrigger { TRIGGER1, TRIGGER2, TRIGGER3, TRIGGER4, TRIGGER5, TRIGGER6 }

    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _playerSpawnPosition;
    private GameObject _player;
    private CommandPromptHandler _commandPromptHandler;
    private MenuHandler _menuHandler;
    private NotificationHandler _notificationHandler;
    private int _currentRoom = 0;

    public void Restart()
    {
        _player.transform.position = _playerSpawnPosition.position;
        EventManager.TriggerEvent(EventManager.Event.GAME_RESUMED);
    }

    public void RoomBorderCrossed(RoomBorderTrigger trigger)
    {
        switch (trigger)
        {
            case RoomBorderTrigger.TRIGGER1:
                SetCurrenTroom(_currentRoom==1?0:1);
                break;
            case RoomBorderTrigger.TRIGGER2:
                SetCurrenTroom(_currentRoom == 2 ? 1 : 2);
                break;
            case RoomBorderTrigger.TRIGGER3:
                SetCurrenTroom(_currentRoom == 3 ? 2 : 3);
                break;
            case RoomBorderTrigger.TRIGGER4:
                SetCurrenTroom(_currentRoom == 4 ? 3 : 4);
                break;
            case RoomBorderTrigger.TRIGGER5:
                SetCurrenTroom(_currentRoom == 5 ? 4 : 5);
                break;
            case RoomBorderTrigger.TRIGGER6:
                SetCurrenTroom(_currentRoom == 6 ? 5 : 6);
                break;
            default:
                Debug.LogError(string.Format("RoomBorderTrigger {0} is not handled",trigger));
                break;
        }
    }

    public void RunCommand(string command)
    {
        _commandPromptHandler.Deactivate();
        EventManager.TriggerEvent(EventManager.Event.GAME_RESUMED);

        if (_currentRoom == 2)
        {
            if(command == "42")
            {
                EventManager.TriggerEvent(EventManager.Event.ROOM2_DOOR_QUESTION_ANSWERED);
            }
            else
            {
                _notificationHandler.ShowNotification("Wrong answer.");
            }
        }
        else
        {
            _notificationHandler.ShowNotification("Commands are available at specific locations.");
        }
    }

    private void Awake ()
    {
        EventManager.StartListening(EventManager.Event.PLAYER_REACHED_EXIT, () => {
            _menuHandler.ShowWon();
        });

        _notificationHandler = GameObject.FindGameObjectWithTag("NotificationHandler").GetComponent<NotificationHandler>();
        _commandPromptHandler = GameObject.FindObjectOfType<CommandPromptHandler>();
        _menuHandler = GameObject.FindObjectOfType<MenuHandler>();
        _menuHandler.SetActive(false);
        RenderSettings.ambientLight = Color.black;

        _player = GameObject.Instantiate(_playerPrefab,_playerSpawnPosition) as GameObject;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0) && !_commandPromptHandler.IsActive())
        {
            _commandPromptHandler.Activate();
            EventManager.TriggerEvent(EventManager.Event.GAME_PAUSED);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_commandPromptHandler.IsActive())
            {
                _commandPromptHandler.Deactivate();
            }
            else
            {
                _menuHandler.SetActive(!_menuHandler.IsActive());
            }

            EventManager.Event gameStateEvent = _menuHandler.IsActive() ? EventManager.Event.GAME_PAUSED : EventManager.Event.GAME_RESUMED;
            EventManager.TriggerEvent(gameStateEvent);
        }
    }

    private void SetCurrenTroom(int newRoom)
    {
        Debug.Log(string.Format("Room changed: {0}->{1}",_currentRoom,newRoom));
        _currentRoom = newRoom;
    }
}
