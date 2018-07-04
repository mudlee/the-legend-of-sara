using UnityEngine;

public class AudioManager : MonoBehaviour {
    public enum Source { PRIMARY, SECONDARY }
    public enum EffectType { _NONE, STEP, DOOR_OPEN }

    [SerializeField] private AudioClip _menuClip;
    [SerializeField] private AudioClip _ambientClip;
    [SerializeField] private AudioClip _stepClip;
    [SerializeField] private AudioClip _doorOpenClip;
    [SerializeField] private AudioSource _sourceMainMusic;
    [SerializeField] private AudioSource _sourceEffectsPrimary;
    [SerializeField] private AudioSource _sourceEffectsSecondary;
    private static bool CREATED = false;
    private float _originalVolume;
    private const float FADE_TIME = 2f;
    private float _fadeStart;
    private bool _fadingToAmbient;
    private EffectType _activeClipPrimary;
    private EffectType _activeClipSecondary;

    public void StartAmbientMusic()
    {
        _fadeStart = Time.timeSinceLevelLoad;
        _fadingToAmbient = true;
    }

    public void Play(EffectType type, Source source, bool loop)
    {
        if (
            (source == Source.PRIMARY &&_activeClipPrimary == type) ||
            (source == Source.SECONDARY && _activeClipSecondary == type)
        )
        {
            return;
        }

        AudioClip clip;
        switch (type)
        {
            case EffectType.STEP:
                clip = _stepClip;
                break;
            case EffectType.DOOR_OPEN:
                clip = _doorOpenClip;
                break;
            default:
                Debug.LogError(string.Format("Unknown effect type: {0}",type));
                return;
        }

        AudioSource player = source == Source.PRIMARY ? _sourceEffectsPrimary : _sourceEffectsSecondary;
        player.Stop();
        player.loop = loop;
        player.clip = clip;
        player.Play();

        switch (source)
        {
            case Source.PRIMARY:
                _activeClipPrimary = type;
                break;
            case Source.SECONDARY:
                _activeClipSecondary = type;
                break;
        }
    }

    public void Stop(Source source)
    {
        // TODO
        _sourceEffectsPrimary.Stop();
        _activeClipPrimary = EffectType._NONE;
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
