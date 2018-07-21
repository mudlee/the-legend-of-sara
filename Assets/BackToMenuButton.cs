using UnityEngine;
using UnityEngine.UI;

public class BackToMenuButton : MonoBehaviour {
	private void Start ()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            LevelManager.LoadMenu();
        });
    }
}
