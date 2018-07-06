using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public enum RoomBorderTrigger { TRIGGER1, TRIGGER2, TRIGGER3, TRIGGER4, TRIGGER5, TRIGGER6 }

    [SerializeField] Transform _playerSpawnPosition;
    [SerializeField] private Slider _health;
    [SerializeField] private Text _score;
    private GameObject _player;
    private UIHandler _uIHandler;
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
        _uIHandler.CommandPrompt.Deactivate();
        EventManager.TriggerEvent(EventManager.Event.GAME_RESUMED);

        if (_currentRoom == 2)
        {
            if(command == "42")
            {
                EventManager.TriggerEvent(EventManager.Event.ROOM2_DOOR_QUESTION_ANSWERED);
            }
            else
            {
                _uIHandler.Notification.ShowNotification("Wrong answer.");
            }
        }
        else
        {
            _uIHandler.Notification.ShowNotification("Commands are available at specific locations.");
        }
    }

    private void Awake ()
    {
        EventManager.StartListening(EventManager.Event.PLAYER_REACHED_EXIT, () => {
            _uIHandler.EndScreen.ShowWon();
        });

        _player = GameObject.FindGameObjectWithTag("Player");

        _uIHandler = GameObject.FindObjectOfType<UIHandler>();
        _uIHandler.Menu.SetActive(false);
        RenderSettings.ambientLight = Color.black;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0) && !_uIHandler.CommandPrompt.IsActive())
        {
            _uIHandler.CommandPrompt.Activate();
            EventManager.TriggerEvent(EventManager.Event.GAME_PAUSED);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uIHandler.CommandPrompt.IsActive())
            {
                _uIHandler.CommandPrompt.Deactivate();
            }
            else
            {
                _uIHandler.Menu.SetActive(!_uIHandler.Menu.IsActive());
            }

            EventManager.Event gameStateEvent = _uIHandler.Menu.IsActive() ? EventManager.Event.GAME_PAUSED : EventManager.Event.GAME_RESUMED;
            EventManager.TriggerEvent(gameStateEvent);
        }
    }

    private void SetCurrenTroom(int newRoom)
    {
        Debug.Log(string.Format("Room changed: {0}->{1}",_currentRoom,newRoom));
        _currentRoom = newRoom;
    }
}
