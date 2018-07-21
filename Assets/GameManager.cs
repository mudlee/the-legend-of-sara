using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public enum RoomBorderTrigger { TRIGGER1, TRIGGER2, TRIGGER3, TRIGGER4, TRIGGER5, TRIGGER6 }

    private enum GameState { DEFAULT, CONSOLE_OPEN, MENU_ACTIVE, GAME_OVER_ACTIVE, WON_ACTIVE }

    private const int GAME_TIME = 600;
    private float _timeAtGameStart;

    [SerializeField] Transform _playerSpawnPosition;
    [SerializeField] private Slider _health;
    [SerializeField] private Text _score;
    [SerializeField] private GameObject _treasures;

    private GameObject _player;
    private PlayerController _playerController;
    private UIHandler _uIHandler;
    private SoundPlayer _soundPlayer;
    private int _currentRoom = 0;
    private int _heartbeatSoundID;
    private GameState _state;
    private GameObject _treasuresInstance;

    public void Restart()
    {
        _timeAtGameStart = Time.timeSinceLevelLoad;
        if(_treasuresInstance != null)
        {
            Destroy(_treasuresInstance);
        }

        _treasuresInstance = Instantiate(_treasures) as GameObject;

        EventManager.TriggerEvent(EventManager.Event.RESET_GAME);
        _state = GameState.DEFAULT;
        Cursor.visible = false;

        _player.transform.position = _playerSpawnPosition.position;
        _playerController.EnableMovement();
        
        _uIHandler.CommandPrompt.Deactivate();
        _uIHandler.Menu.SetActive(false);
        _uIHandler.EndScreen.HideAll();
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
        _state = GameState.DEFAULT;
        _playerController.EnableMovement();

        /*if (_currentRoom == 2)
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
        }*/
    }

    private void Awake ()
    {
        EventManager.StartListening(EventManager.Event.PLAYER_REACHED_EXIT, () => {
            _state = GameState.WON_ACTIVE;
            _uIHandler.CommandPrompt.Deactivate();
            _uIHandler.Menu.SetActive(false);
            _playerController.DisableMovement();
            _uIHandler.EndScreen.ShowWon();
            Cursor.visible = true;
        });

        EventManager.StartListening(EventManager.Event.PLAYER_DIED, () => {
            _state = GameState.GAME_OVER_ACTIVE;
            _uIHandler.CommandPrompt.Deactivate();
            _uIHandler.Menu.SetActive(false);
            _playerController.DisableMovement();
            _uIHandler.EndScreen.ShowLost();
            Cursor.visible = true;
        });

        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _soundPlayer = FindObjectOfType<SoundPlayer>();

        _uIHandler = FindObjectOfType<UIHandler>();
        _uIHandler.Tutorial.SetActiveFor(5);
        RenderSettings.ambientLight = Color.black;
    }

    private void Start()
    {
        Restart();
        _soundPlayer?.Play(Sound.GAME_AMBIENT, 3);

        EventManager.StartListening(EventManager.Event.ENEMY_ATTENTION_LOST, () => {
            //_soundPlayer.Stop(_heartbeatSoundID);
        });
        EventManager.StartListening(EventManager.Event.ENEMY_ATTENTION_LOW, () => {
            //_soundPlayer.Stop(_heartbeatSoundID);
            //_heartbeatSoundID = _soundPlayer.Play(Sound.HEARTBEAT_SLOW);
        });
        EventManager.StartListening(EventManager.Event.ENEMY_ATTENTION_HIGH, () => {
            //_soundPlayer.Stop(_heartbeatSoundID);
            //_heartbeatSoundID = _soundPlayer.Play(Sound.HEARTBEAT_FAST);
        });
    }

    private void Update()
    {
        float diff = Time.timeSinceLevelLoad - _timeAtGameStart;
        _uIHandler.HealthAndScore.UpdateRemainingTime(GAME_TIME - (int)diff);

        if (Input.GetKeyUp(KeyCode.Alpha0) && !_uIHandler.CommandPrompt.IsActive())
        {
            _uIHandler.CommandPrompt.Activate();
            _state = GameState.CONSOLE_OPEN;
            _playerController.DisableMovement();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_uIHandler.CommandPrompt.IsActive())
            {
                _state = GameState.DEFAULT;
                _uIHandler.CommandPrompt.Deactivate();
                _playerController.EnableMovement();
            }
            else
            {
                if(_uIHandler.EndScreen.IsActive())
                {
                    return;
                }

                _uIHandler.Menu.SetActive(!_uIHandler.Menu.IsActive());

                if(_uIHandler.Menu.IsActive())
                {
                    _state = GameState.MENU_ACTIVE;
                    _playerController.DisableMovement();
                    Cursor.visible = true;
                }
                else
                {
                    _state = GameState.DEFAULT;
                    _playerController.EnableMovement();
                    Cursor.visible = false;
                }
            }
        }
    }

    private void SetCurrenTroom(int newRoom)
    {
        _currentRoom = newRoom;
    }
}
