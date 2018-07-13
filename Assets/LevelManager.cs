using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private enum SceneIndex
    {
        LOADING = 0,
        MENU = 1,
        LEVEL_1 = 2
    }

    [SerializeField] private bool _autoLoadNextLevel;
    private static int MENU_AMBIENT_SOUND_ID;

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
                soundPlayer.Stop(LevelManager.MENU_AMBIENT_SOUND_ID, 3);
                soundPlayer.Play(Sound.GAME_AMBIENT, 3);
                soundPlayer.Play(Sound.HEARTBEAT_SLOW);
                break;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
