using UnityEngine;

public class LoadingSoundInitialiser : MonoBehaviour {
    public static int MenuAmbientSoundID;

	private void Start ()
    {
        SoundPlayer soundPlayer = FindObjectOfType<SoundPlayer>();
        MenuAmbientSoundID = soundPlayer.Play(Sound.MENU_AMBIENT);
	}
}
