using UnityEngine;

public class AudioManager : MonoBehaviour {
    public enum Source { PRIMARY, SECONDARY }
    public enum EffectType
    {
        STEP
    }

    [SerializeField] private AudioClip _menuClip;
    [SerializeField] private AudioClip _ambientClip;
    [SerializeField] private AudioClip _stepClip;
    [SerializeField] private AudioSource _sourceMainMusic;
    [SerializeField] private AudioSource _sourceEffectsPrimary;
    private static bool CREATED = false;
    private float _originalVolume;
    private const float FADE_TIME = 2f;
    private float _fadeStart;
    private bool _fadingToAmbient;
    private EffectType _clipOnPrimary;
    private EffectType _clipOnSecondary;

    public void StartAmbientMusic()
    {
        _fadeStart = Time.timeSinceLevelLoad;
        _fadingToAmbient = true;
    }

    public void Play(EffectType type, Source source)
    {
        
        if (
            (source == Source.PRIMARY &&_clipOnPrimary == type) ||
            (source == Source.SECONDARY && _clipOnSecondary == type)
        )
        {
            // TODO: that does not work somehow
            return;
        }
        
        if (source == Source.PRIMARY)
        {
            _sourceEffectsPrimary.Stop();
            _clipOnPrimary = type;
            _sourceEffectsPrimary.clip = _stepClip; // TODO
            _sourceEffectsPrimary.Play();
        }
    }

    public void Stop(Source source)
    {
        _sourceEffectsPrimary.Stop();
    }

    private void Awake()
    {
        if (!CREATED)
        {
            DontDestroyOnLoad(this.gameObject);
            CREATED = true;
        }
    }

    private void Start()
    {
        _originalVolume = _sourceMainMusic.volume;
        _sourceMainMusic.clip = _menuClip;
        _sourceMainMusic.Play();
    }

    private void Update()
    {
        if (_fadingToAmbient)
        {
            FadeMusic();
        }
    }

    void FadeMusic()
    {
        
        if((_fadeStart + FADE_TIME) > Time.timeSinceLevelLoad)
        {
            print(_sourceMainMusic.volume);
            _sourceMainMusic.volume -= 0.2f * Time.deltaTime;
        }
        if (_sourceMainMusic.volume <= 0 || (_fadeStart + FADE_TIME)<Time.timeSinceLevelLoad)
        {
            _fadingToAmbient = false;
            _sourceMainMusic.Stop();
            _sourceMainMusic.volume = _originalVolume;
            _sourceMainMusic.clip = _ambientClip;
            _sourceMainMusic.loop = true;
            _sourceMainMusic.Play();
        }
    }
}
