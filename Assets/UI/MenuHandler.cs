﻿using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    public enum Logic { USE_START, USE_RESTART }
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Logic _logic = Logic.USE_START;
    private LevelManager _levelManager;
    private GameManager _gameManager;

    private void Awake()
    {
        switch (_logic)
        {
            case Logic.USE_START:
                _restartButton.gameObject.SetActive(false);
                break;
            case Logic.USE_RESTART:
                _startButton.gameObject.SetActive(false);
                break;
        }
    }

    private void Start()
    {
        _levelManager = GameObject.FindObjectOfType<LevelManager>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _quitButton.onClick.AddListener(Quit);
        _startButton.onClick.AddListener(() => _levelManager.LoadNextLevel());
        _restartButton.onClick.AddListener(() => {
            SetActive(false);
            _gameManager.Restart();
        });
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
