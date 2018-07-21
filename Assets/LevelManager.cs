using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int MENU_AMBIENT_SOUND_ID;

    private enum SceneIndex
    {
        LOADING = 0,
        MENU = 1,
        LEVEL_1 = 2
    }

    [SerializeField] private bool _autoLoadNextLevel;

    void Start ()
    {
        if (_autoLoadNextLevel)
        {
            Invoke("LoadNextLevel", 10);
        }

        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        Scene scene = SceneManager.GetActiveScene();

        switch (scene.buildIndex)
        {
            case (int)SceneIndex.LOADING:
                LevelManager.MENU_AMBIENT_SOUND_ID = soundPlayer.Play(Sound.MENU_AMBIENT);
                break;
            case (int)SceneIndex.MENU:
                break;
            case (int)SceneIndex.LEVEL_1:
                soundPlayer?.Stop(LevelManager.MENU_AMBIENT_SOUND_ID, 3);
                break;
        }
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadStory()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
