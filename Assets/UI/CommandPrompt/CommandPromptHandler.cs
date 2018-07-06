using UnityEngine;
using UnityEngine.UI;

public class CommandPromptHandler : MonoBehaviour {
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
        Debug.Log("Activating command prompt");
        _active = true;
        _text.text = "_";
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        Debug.Log("Deactivating command prompt");
        _text.text = "_";
        _active = false;
        gameObject.SetActive(false);
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
