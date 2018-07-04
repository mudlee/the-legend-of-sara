using UnityEngine;
using UnityEngine.UI;

public class CommandPromptHandler : MonoBehaviour {
    [SerializeField] private GameObject _textGO;
    [SerializeField] private GameObject _panelGO;
    [SerializeField] private Text _text;
    private GameManager _gameManager;
    private bool _active;

    private void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public bool IsActive()
    {
        return _active;
    }

    public void Activate()
    {
        _active = true;
        _text.text = "_";
        _textGO.SetActive(true);
        _panelGO.SetActive(true);
    }

    public void Deactivate()
    {
        _text.text = "_";
        _active = false;
        _textGO.SetActive(false);
        _panelGO.SetActive(false);
    }

    void Update ()
    {
        if (!_active)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Return))
        {
            RunCommand();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if(_text.text.Length > 1)
            {
                _text.text = _text.text.Substring(0,_text.text.Length-2)+"_";
                return;
            }

            return;
        }

        if (Input.anyKeyDown)
        {
            if (_text.text.Length > 50)
            {
                return;
            }

            _text.text = _text.text.Substring(0, _text.text.Length-1) + Input.inputString + "_";
        }
	}

    private void RunCommand()
    {
        string command = _text.text.Substring(0,_text.text.Length-1);
        if (command.Length == 0)
        {
            return;
        }
        print(string.Format("Running command: '{0}'",command));
        _gameManager.RunCommand(command);
    }
}
