using UnityEngine;

public class PlayerController : MonoBehaviour {
    private const float SPEED = 2f;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _moveEnabled = true;
    private AudioManager _audioManager;

    private void Start ()
    {
        EventManager.StartListening(EventManager.Event.GAME_PAUSED, DisableMovement);
        EventManager.StartListening(EventManager.Event.GAME_RESUMED, () => _moveEnabled = true);
        EventManager.StartListening(EventManager.Event.PLAYER_REACHED_EXIT, DisableMovement);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void Update ()
    {
        if(!_moveEnabled)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _animator.SetBool("WalkRight", horizontal > 0);
        _animator.SetBool("WalkLeft", horizontal < 0);
        _animator.SetBool("WalkTop", vertical > 0);
        _animator.SetBool("WalkBottom", vertical < 0);

        _rigidbody.velocity = new Vector2(horizontal * SPEED, vertical * SPEED);

        if(horizontal != 0 || vertical != 0)
        {
            _audioManager?.Play(AudioManager.EffectType.STEP, AudioManager.Source.PRIMARY, true);
        }
        else
        {
            _audioManager?.Stop(AudioManager.Source.PRIMARY);
        }
	}

    private void DisableMovement()
    {
        _moveEnabled = false;
        _animator.SetBool("WalkRight", false);
        _animator.SetBool("WalkLeft", false);
        _animator.SetBool("WalkTop", false);
        _animator.SetBool("WalkBottom", false);
    }
}
