using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour {
    [SerializeField] private NPCInfo _npcInfo;
    private SoundPlayer _soundPlayer;


    private void Start ()
    {
        _soundPlayer = FindObjectOfType<SoundPlayer>();
        InvokeRepeating("Scream",Random.Range(4f,10f),Random.Range(2f,5f));
    }
	
	private void Scream()
    {
        int numOfSounds = _npcInfo.sounds.Length;
        SoundInfo sound = _npcInfo.sounds[Random.Range(0, numOfSounds - 1)];
        _soundPlayer.Play(sound.sound);
    }
}
