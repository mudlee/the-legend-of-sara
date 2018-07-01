using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool _autoLoadNextLevel;

    void Start ()
    {
        if (_autoLoadNextLevel)
        {
            Invoke("LoadNextLevel", 6);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
