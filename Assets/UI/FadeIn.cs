using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeIn : MonoBehaviour {
    public enum Logic { FADE_IN, FADE_IN_OUT};

    [SerializeField] private Logic _logic = Logic.FADE_IN;
    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _waitBeforeFadeOutTime;
    [SerializeField] private float _fadeOutTime;
    [SerializeField] private bool _destroyOnCompletion;
    private Image _image;
    private Color _color=Color.black;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = _color;
    }

    private void Start ()
    {
        
	}
	
	private void Update ()
    {
        if(Time.timeSinceLevelLoad < _fadeInTime)
        {
            float alpha = Time.deltaTime / _fadeInTime;
            _color.a -= alpha;
            _image.color = _color;
        }
        else
        {
            if(_logic == Logic.FADE_IN)
            {
                if (_destroyOnCompletion)
                {
                    Destroy(gameObject);
                }
            }
            else if(_logic == Logic.FADE_IN_OUT)
            {
                float waitStart = _fadeInTime;
                float waitEnd = _fadeInTime + _waitBeforeFadeOutTime;
                float fadeOutEnd = waitEnd + _fadeOutTime;

                if (Time.timeSinceLevelLoad >= waitStart && Time.timeSinceLevelLoad <= waitEnd)
                {
                    // do nothing
                }
                else if (Time.timeSinceLevelLoad < fadeOutEnd)
                {
                    float alpha = Time.deltaTime / _fadeOutTime;
                    _color.a += alpha;
                    _image.color = _color;
                }
                else
                {
                    if (_destroyOnCompletion)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
	}
}
