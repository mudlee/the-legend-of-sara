using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private CommandPromptHandler _commandPromptHandler;

	void Start ()
    {
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
            _commandPromptHandler.Deactivate();
        }
    }
}
