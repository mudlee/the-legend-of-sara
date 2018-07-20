using UnityEngine;
using UnityEngine.UI;

public class EndScreenHandler : MonoBehaviour {
    [SerializeField] private GameObject _won;
    [SerializeField] private GameObject _lost;
    [SerializeField] private Button[] _restartButtons;
    [SerializeField] private Button[] _quitButtons;
    
    private void Awake()
    {
        GameManager _gameManager = GameObject.FindObjectOfType<GameManager>(); ;

        foreach (Button button in _restartButtons)
        {
            button.onClick.AddListener(()=> {
                _gameManager.Restart();
            });
        }

        foreach (Button button in _quitButtons)
        {
            button.onClick.AddListener(() => {
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            });
        }
    }

    public bool IsActive()
    {
        return _won.activeSelf || _lost.activeSelf;
    }

    public void HideAll()
    {
        _won.SetActive(false);
        _lost.SetActive(false);
    }

    public void ShowWon()
    {
        _won.SetActive(true);
    }

    public void ShowLost()
    {
        _lost.SetActive(true);
    }
}
