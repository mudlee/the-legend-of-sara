using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum RoomBorderTrigger { TRIGGER1, TRIGGER2, TRIGGER3, TRIGGER4, TRIGGER5, TRIGGER6 }

    private CommandPromptHandler _commandPromptHandler;
    private MenuHandler _menu;
    private bool _menuActive;
    private int _currentRoom = 0;

    public void Restart()
    {
        print("RESTART");
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

    private void Awake ()
    {
        _commandPromptHandler = GameObject.FindObjectOfType<CommandPromptHandler>();
        _menu = GameObject.FindObjectOfType<MenuHandler>();
        _menu.SetActive(false);
        RenderSettings.ambientLight = Color.black;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _commandPromptHandler.Activate();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_commandPromptHandler.IsActive())
            {
                _commandPromptHandler.Deactivate();
            }
            else
            {
                _menuActive = !_menuActive;
                _menu.SetActive(_menuActive);
            }
        }
    }

    private void SetCurrenTroom(int newRoom)
    {
        Debug.Log(string.Format("Room changed: {0}->{1}",_currentRoom,newRoom));
        _currentRoom = newRoom;
    }
}
