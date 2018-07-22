using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int _score = 0;

    private const float SPEED = 2f;
    private const float FIRE_RATE_LIMIT = 0.5f;
    [SerializeField] private Animator _bleedAnimator;
    [SerializeField] private GameObject _projectile;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _moveEnabled = true;
    private SoundPlayer _soundPlayer;
    private Vector2 _velocity = new Vector2();
    private int _stepSoundAudioSource;
    private bool _moving;
    private UIHandler _uIHandler;
    private int _health = 100;
    private int _slowDownModifier = 0;
    private float _lastFireTime = 0;

    public void NPCKilled(int point)
    {
        _score += point;
        _uIHandler.HealthAndScore.UpdateScore(_score);
    }

    public void TreasureFound(int point, int heal)
    {
        _score += point;
        _uIHandler.HealthAndScore.UpdateScore(_score);

        if(heal>0)
        {
            _health += heal;
            if (_health > 100)
            {
                _health = 100;
            }

            _uIHandler.HealthAndScore.UpdateHealth(_health);
        }
    }

    public void Damage(int amount, int slowDown)
    {
        _bleedAnimator.SetTrigger("Bleed");
        _health -= amount;
        _uIHandler.HealthAndScore.UpdateHealth(_health);
        _slowDownModifier = slowDown;

        if(slowDown>0)
        {
            Invoke("StopSlowDown",2f);
        }

        if(_health<=0)
        {
            EventManager.TriggerEvent(EventManager.Event.PLAYER_DIED);
        }
    }

    public void StopSlowDown()
    {
        _slowDownModifier = 0;
    }

    public void EnableMovement()
    {
        _moveEnabled = true;
    }

    public void DisableMovement()
    {
        _moveEnabled = false;

        if (_moving)
        {
            _moving = false;
            _animator.SetBool("WalkRight", false);
            _animator.SetBool("WalkLeft", false);
            _animator.SetBool("WalkTop", false);
            _animator.SetBool("WalkBottom", false);

            _velocity.Set(0, 0);
            _rigidbody.velocity = _velocity;
            _soundPlayer?.Stop(_stepSoundAudioSource);
        }
    }

    private void Start ()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _soundPlayer = FindObjectOfType<SoundPlayer>();
        _uIHandler = GameObject.FindObjectOfType<UIHandler>();

        EventManager.StartListening(EventManager.Event.RESET_GAME, () => {
            _health = 100;
            _score = 0;
            _uIHandler.HealthAndScore.UpdateHealth(_health);
            _uIHandler.HealthAndScore.UpdateScore(_score);
        });
    }

    private void Update ()
    {
        if(!_moveEnabled)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.timeSinceLevelLoad > _lastFireTime + FIRE_RATE_LIMIT)
        {
            Fire();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _animator.SetBool("WalkRight", horizontal > 0);
        _animator.SetBool("WalkLeft", horizontal < 0);
        _animator.SetBool("WalkTop", vertical > 0);
        _animator.SetBool("WalkBottom", vertical < 0);

        float speed = SPEED - SPEED * _slowDownModifier/100;

        _velocity.Set(horizontal * speed, vertical * speed);
        _rigidbody.velocity = _velocity;


        if(horizontal != 0f || vertical != 0f)
        {
            if(!_moving)
            {
                _moving = true;
                if(_soundPlayer!=null)
                {
                    _stepSoundAudioSource = _soundPlayer.Play(Sound.PLAYER_STEP);    
                }
            }
        }
        else if (_moving)
        {
            _moving = false;
            _soundPlayer?.Stop(_stepSoundAudioSource);
        }
	}

    private void Fire()
    {
        if(_rigidbody.velocity.x == 0 && _rigidbody.velocity.y == 0)
        {
            return;
        }

        _lastFireTime = Time.timeSinceLevelLoad;

        GameObject projectile = Instantiate(_projectile, transform.forward * 20, transform.rotation) as GameObject;
        projectile.transform.position = transform.position;
        projectile.GetComponent<Rigidbody2D>().velocity = _velocity.normalized * 10;
    }
}
