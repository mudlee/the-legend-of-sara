using UnityEngine;
using UnityEngine.SceneManagement;

// TODO
// https://www.dafont.com/windows-command-prompt.font?text=The+Legend+of+Sara&back=bitmap
public class LevelManager : MonoBehaviour
{
    private static bool created = false;

    void Start ()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
    }
}
