using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundPlayer : MonoBehaviour {
    private static bool CREATED = false;

    [SerializeField] private SoundInfo[] _sounds;
    [SerializeField] [Range(1,20)] private int _audioSourceQueueSize=10;
    private Dictionary<Sound, SoundInfo> _map = new Dictionary<Sound, SoundInfo>();
    private Dictionary<int, AudioSource> _queue=new Dictionary<int, AudioSource>();

    public int Play(Sound sound)
    {
        SoundInfo info;
        _map.TryGetValue(sound, out info);

        if(info == null)
        {
            Debug.LogError(string.Format("Sound {0} cannot be played. No SoundInfo found", sound.ToString()));
            return 0;
        }

        AudioSource source = _queue.Values.FirstOrDefault(AudioSource => !AudioSource.isPlaying);
        if( source == null )
        {
            Debug.LogError("No available AudioSource");
            return 0;
        }

        source.loop = info.loop;
        source.clip = info.clip;
        source.volume = info.volume;
        source.Play();
        return source.GetInstanceID();
    }

    public void Stop(int audioSourceID)
    {
        AudioSource source;
        _queue.TryGetValue(audioSourceID, out source);

        if(source == null)
        {
            Debug.LogError(string.Format("AudioSource with ID:{0} not found", audioSourceID));
            return;
        }

        source.Stop();
    }

    private void Awake()
    {
        if (!CREATED)
        {
            DontDestroyOnLoad(this.gameObject);
            CREATED = true;
        }
    }

	private void Start ()
    {
        foreach (SoundInfo info in _sounds) 
        {
            _map.Add(info.sound, info);
        }

        for (int i = 0; i < _audioSourceQueueSize;i++){
            AudioSource source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            _queue.Add(source.GetInstanceID(), source);
        }
	}
}
