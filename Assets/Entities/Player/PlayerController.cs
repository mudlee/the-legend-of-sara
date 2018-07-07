﻿using UnityEngine;

public class PlayerController : MonoBehaviour {
    private const float SPEED = 2f;
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private bool _moveEnabled = true;
    private SoundPlayer _soundPlayer;
    private Vector2 _velocity = new Vector2();
    private int _stepSoundAudioSource;
    private bool _moving;

    private void Start ()
    {
        EventManager.StartListening(EventManager.Event.GAME_PAUSED, DisableMovement);
        EventManager.StartListening(EventManager.Event.GAME_RESUMED, () => _moveEnabled = true);
        EventManager.StartListening(EventManager.Event.PLAYER_REACHED_EXIT, DisableMovement);
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _soundPlayer = FindObjectOfType<SoundPlayer>();
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

        _velocity.Set(horizontal * SPEED, vertical * SPEED);
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

    private void DisableMovement()
    {
        _moveEnabled = false;

        if(_moving)
        {
            _moving = false;
            _animator.SetBool("WalkRight", false);
            _animator.SetBool("WalkLeft", false);
            _animator.SetBool("WalkTop", false);
            _animator.SetBool("WalkBottom", false);

            _velocity.Set(0,0);
            _rigidbody.velocity = _velocity;
            _soundPlayer?.Stop(_stepSoundAudioSource);
        }
    }
}
