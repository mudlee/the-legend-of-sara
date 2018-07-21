using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    public enum Logic { USE_START, USE_RESTART }
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _storyButton;
    [SerializeField] private Logic _logic = Logic.USE_START;
    private LevelManager _levelManager;
    private GameManager _gameManager;

    public void SetActive(bool active)
    {
        Cursor.visible = active;
        gameObject.SetActive(active);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void Awake()
    {
        switch (_logic)
        {
            case Logic.USE_START:
                _restartButton.gameObject.SetActive(false);
                _storyButton.gameObject.SetActive(true);
                _creditsButton.gameObject.SetActive(true);
                break;
            case Logic.USE_RESTART:
                _startButton.gameObject.SetActive(false);
                _storyButton.gameObject.SetActive(false);
                _creditsButton.gameObject.SetActive(false);
                break;
        }
    }

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();
        _gameManager = FindObjectOfType<GameManager>();

        _quitButton.onClick.AddListener(Quit);

        _startButton.onClick.AddListener(() => {
            _levelManager.LoadNextLevel();
        });

        _restartButton.onClick.AddListener(() => {
            _gameManager.Restart();
        });

        _creditsButton.onClick.AddListener(() => {
            _levelManager.LoadCredits();
        });

        _storyButton.onClick.AddListener(() => {
            _levelManager.LoadStory();
        });
    }
}
